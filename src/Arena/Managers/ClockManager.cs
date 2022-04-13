using Arena.Logging;
using Arena.Managers.Bases;
using RoR2;
using System.Collections.Generic;

namespace Arena.Managers;

internal class ClockManager : ManagerBase
{
    private bool _isRunning = true;

    public override IEnumerable<string> GetStatus() =>
        new[] { $"The clock is { (_isRunning ? "running" : "paused") }." };

    public void PauseClock()
    {
        if (!_isRunning)
        {
            return;
        }

        Stage.instance.sceneDef.sceneType = SceneType.Intermission;
        _isRunning = false;

        Log.Info("Clock paused.");
    }

    public void ResumeClock()
    {
        if (_isRunning)
        {
            return;
        }

        Stage.instance.sceneDef.sceneType = SceneType.Stage;
        _isRunning = true;

        Log.Info("Clock resumed.");
    }
}
