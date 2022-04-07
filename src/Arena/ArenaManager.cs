using RoR2;

namespace Arena;

internal static class ArenaManager
{
    public static class Clock
    {
        public static void Pause() => Stage.instance.sceneDef.sceneType = SceneType.Intermission;

        public static void Resume() => Stage.instance.sceneDef.sceneType = SceneType.Stage;
    }

    public static class FriendlyFire
    {
        public static void Enable() =>
            RunArtifactManager.instance.SetArtifactEnabledServer(RoR2Content.Artifacts.FriendlyFire, true);

        public static void Disable() =>
            RunArtifactManager.instance.SetArtifactEnabledServer(RoR2Content.Artifacts.FriendlyFire, false);
    }

    public static class Teleporter
    {
        public static void Disable() =>
            On.RoR2.TeleporterInteraction.GetInteractability += TeleporterInteraction_GetInteractability;

        public static void Enable() =>
            On.RoR2.TeleporterInteraction.GetInteractability -= TeleporterInteraction_GetInteractability;

        private static Interactability TeleporterInteraction_GetInteractability
            (On.RoR2.TeleporterInteraction.orig_GetInteractability orig,
            TeleporterInteraction self,
            Interactor activator) =>
                Interactability.ConditionsNotMet;
    }
}
