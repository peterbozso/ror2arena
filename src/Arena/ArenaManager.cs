using RoR2;

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
}
