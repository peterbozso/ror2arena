using MonoMod.RuntimeDetour;
using RoR2;
using System.Linq;
using System.Reflection;

namespace Arena;

internal static class ArenaManager
{
    public static class Clock
    {
        public static void Pause()
        {
            Stage.instance.sceneDef.sceneType = SceneType.Intermission;
            Log.LogMessage("Clock paused.");
        }

        public static void Resume()
        {
            Stage.instance.sceneDef.sceneType = SceneType.Stage;
            Log.LogMessage("Clock resumed.");
        }
    }

    public static class FriendlyFire
    {
        public static void Enable()
        {
            RunArtifactManager.instance.SetArtifactEnabledServer(RoR2Content.Artifacts.FriendlyFire, true);
            Log.LogMessage("Friendly fire enabled.");
        }

        public static void Disable()
        {
            RunArtifactManager.instance.SetArtifactEnabledServer(RoR2Content.Artifacts.FriendlyFire, false);
            Log.LogMessage("Friendly fire disabled.");
        }
    }

    public class Teleporter
    {
        public delegate Interactability orig_GetInteractability(GenericInteraction self, Interactor activator);
        public Hook hook_GetInteractability;

        public void Disable()
        {
            On.RoR2.TeleporterInteraction.GetInteractability += TeleporterInteraction_GetInteractability;

            hook_GetInteractability = new Hook(typeof(GenericInteraction).GetMethod("RoR2.IInteractable.GetInteractability", BindingFlags.NonPublic | BindingFlags.Instance), typeof(Teleporter).GetMethod("GenericInteraction_GetInteractability"), this, new HookConfig());

            Log.LogMessage("Portals disabled.");
        }

        public void Enable()
        {
            On.RoR2.TeleporterInteraction.GetInteractability -= TeleporterInteraction_GetInteractability;
            hook_GetInteractability.Dispose();
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

            Log.LogInfo("Interaction while portals are disabled: " + interactionName);

            if (interactionName.Contains("portal"))
            {
                return Interactability.ConditionsNotMet;
            }

            return orig(self, activator);
        }
    }

    public static class Champion
    {
        public static string Name
        {
            get
            {
                var players = PlayerCharacterMasterController.instances;
                var alivePlayers = players.Where(player => IsAlive(player.master)).ToArray();

                if (alivePlayers.Length == 1)
                {
                    return alivePlayers[0].master.GetBody().GetUserName();
                }

                return string.Empty;
            }
        }

        private static bool IsAlive(CharacterMaster player)
        {
            var body = player.GetBody();
            return (body && body.healthComponent.alive)
                || player.inventory.GetItemCount(RoR2Content.Items.ExtraLife) > 0
                || player.inventory.GetItemCount(DLC1Content.Items.ExtraLifeVoid) > 0;
        }
    }
}
