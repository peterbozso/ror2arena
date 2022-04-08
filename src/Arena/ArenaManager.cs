using RoR2;
using System.Linq;

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

    public static class Teleporter
    {
        public static void Disable()
        {
            // TODO: What about blue/gold/etc. portals?
            On.RoR2.TeleporterInteraction.GetInteractability += TeleporterInteraction_GetInteractability;
            Log.LogMessage("Teleporter disabled.");
        }

        public static void Enable()
        {
            On.RoR2.TeleporterInteraction.GetInteractability -= TeleporterInteraction_GetInteractability;
            Log.LogMessage("Teleporter enabled.");
        }

        private static Interactability TeleporterInteraction_GetInteractability
            (On.RoR2.TeleporterInteraction.orig_GetInteractability orig,
            TeleporterInteraction self,
            Interactor activator) =>
                Interactability.ConditionsNotMet;
    }

    public static class Champion
    {
        public static bool IsAllDead
        {
            get
            {
                var players = PlayerCharacterMasterController.instances;
                var aliveCount = players.Count(player => IsAlive(player.master));
                return aliveCount == 1;
            }
        }

        public static string Name
        {
            get
            {
                var players = PlayerCharacterMasterController.instances;
                var aliveCount = players.Count(player => IsAlive(player.master));

                if (aliveCount == 1)
                {
                    return players
                        .First(player => IsAlive(player.master))
                        .master.GetBody().GetUserName();
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
