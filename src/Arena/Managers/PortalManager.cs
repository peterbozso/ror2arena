using MonoMod.RuntimeDetour;
using RoR2;
using System.Reflection;

namespace Arena.Managers;

internal class PortalManager
{
    public delegate Interactability orig_GetInteractability(GenericInteraction self, Interactor activator);

    private Hook _hook_GetInteractability;

    public void DisableAllPortals()
    {
        On.RoR2.TeleporterInteraction.GetInteractability += TeleporterInteraction_GetInteractability;

        _hook_GetInteractability = new Hook(
            typeof(GenericInteraction).GetMethod(
                "RoR2.IInteractable.GetInteractability",
                BindingFlags.NonPublic | BindingFlags.Instance),
            typeof(PortalManager).GetMethod("GenericInteraction_GetInteractability"),
            this,
            new HookConfig());

        Log.LogMessage("Portals disabled.");
    }

    public void EnableAllPortals()
    {
        On.RoR2.TeleporterInteraction.GetInteractability -= TeleporterInteraction_GetInteractability;
        _hook_GetInteractability.Dispose();
        Log.LogMessage("Portals enabled.");
    }

    private static Interactability TeleporterInteraction_GetInteractability(
        On.RoR2.TeleporterInteraction.orig_GetInteractability orig,
        TeleporterInteraction self,
        Interactor activator) =>
            Interactability.ConditionsNotMet;

    public Interactability GenericInteraction_GetInteractability(
        orig_GetInteractability orig,
        GenericInteraction self,
        Interactor activator)
    {
        var interactionName = self.name.ToLower();

        Log.LogDebug("Interaction while portals are disabled: " + interactionName);

        if (interactionName.Contains("portal"))
        {
            return Interactability.ConditionsNotMet;
        }

        return orig(self, activator);
    }
}
