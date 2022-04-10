using Arena.Managers.Bases;
using MonoMod.RuntimeDetour;
using RoR2;
using System.Reflection;

namespace Arena.Managers;

internal class PortalManager : ListeningManagerBase
{
    public delegate Interactability orig_GetInteractability(GenericInteraction self, Interactor activator);

    private Hook _hook_GetInteractability;

    public void DisableAllPortals() => StartListening();

    public void EnableAllPortals() => StopListening();

    protected override void StartListening()
    {
        On.RoR2.TeleporterInteraction.GetInteractability += OnTeleporterInteractionGetInteractability;

        _hook_GetInteractability = new Hook(
            typeof(GenericInteraction).GetMethod(
                "RoR2.IInteractable.GetInteractability",
                BindingFlags.NonPublic | BindingFlags.Instance),
            typeof(PortalManager).GetMethod("OnGenericInteractionGetInteractability"),
            this,
            new HookConfig());

        Log.LogDebug("Portals disabled.");

        base.StartListening();
    }

    protected override void StopListening()
    {
        On.RoR2.TeleporterInteraction.GetInteractability -= OnTeleporterInteractionGetInteractability;

        _hook_GetInteractability.Dispose();

        Log.LogDebug("Portals enabled.");

        base.StopListening();
    }

    private static Interactability OnTeleporterInteractionGetInteractability(
        On.RoR2.TeleporterInteraction.orig_GetInteractability orig,
        TeleporterInteraction self,
        Interactor activator) =>
            Interactability.ConditionsNotMet;

    public Interactability OnGenericInteractionGetInteractability(
        orig_GetInteractability orig,
        GenericInteraction self,
        Interactor activator) =>
            self.name.ToLower().Contains("portal")
            ? Interactability.ConditionsNotMet
            : orig(self, activator);
}
