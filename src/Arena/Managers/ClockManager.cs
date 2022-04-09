using RoR2;

namespace Arena.Managers;

public class ClockManager
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
