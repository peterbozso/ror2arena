using RoR2;

namespace Arena.Managers;

internal class ClockManager : ManagerBase
{
    public void PauseClock()
    {
        Stage.instance.sceneDef.sceneType = SceneType.Intermission;
        Log.LogMessage("Clock paused.");
    }

    public void ResumeClock()
    {
        Stage.instance.sceneDef.sceneType = SceneType.Stage;
        Log.LogMessage("Clock resumed.");
    }
}
