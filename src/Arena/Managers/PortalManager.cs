using Arena.Logging;
using Arena.Managers.Bases;
using MonoMod.RuntimeDetour;
using RoR2;
using System.Collections.Generic;
using System.Reflection;

namespace Arena.Managers;

internal class PortalManager : ListeningManagerBase
{
    public delegate Interactability orig_GetInteractability(GenericInteraction self, Interactor activator);

    private Hook _hook_GetInteractability;

    public override IEnumerable<string> GetStatus() => new string[]
    {
        $"Portals are { (IsListening ? "disabled" : "enabled") }."
    };

    public void DisableAllPortals() => Start();

    public void EnableAllPortals() => Stop();

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

        Log.Info("Portals disabled.");
    }

    protected override void StopListening()
    {
        On.RoR2.TeleporterInteraction.GetInteractability -= OnTeleporterInteractionGetInteractability;

        _hook_GetInteractability.Dispose();

        Log.Info("Portals enabled.");
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
