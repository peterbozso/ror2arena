using RoR2;

namespace Arena;

internal static class Clock
{
    public static void Pause() => Stage.instance.sceneDef.sceneType = SceneType.Intermission;

    public static void Resume() => Stage.instance.sceneDef.sceneType = SceneType.Stage;
}
