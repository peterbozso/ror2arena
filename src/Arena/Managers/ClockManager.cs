using Arena.Managers.Bases;
using RoR2;

namespace Arena.Managers;

internal class ClockManager : ManagerBase
{
    public void PauseClock()
    {
        Stage.instance.sceneDef.sceneType = SceneType.Intermission;

        Log.LogDebug("Clock paused.");
    }

    public void ResumeClock()
    {
        Stage.instance.sceneDef.sceneType = SceneType.Stage;

        Log.LogDebug("Clock resumed.");
    }
}
