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
}
