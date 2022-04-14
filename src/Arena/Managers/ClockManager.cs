using Arena.Logging;
using Arena.Managers.Bases;
using RoR2;
using System.Collections.Generic;

namespace Arena.Managers;

internal class ClockManager : ListeningManagerBase
{
    public override IEnumerable<string> GetStatus() =>
        new[] { $"The clock is { (IsListening ? "running" : "paused") }." };

    public void PauseClock() => Start();

    public void ResumeClock() => Stop();

    protected override void StartListening()
    {
        On.RoR2.Run.ShouldUpdateRunStopwatch += OnShouldUpdateRunStopwatch;

        Log.Info("Clock paused.");
    }

    protected override void StopListening()
    {
        On.RoR2.Run.ShouldUpdateRunStopwatch -= OnShouldUpdateRunStopwatch;

        Log.Info("Clock resumed.");
    }

    private bool OnShouldUpdateRunStopwatch(
        On.RoR2.Run.orig_ShouldUpdateRunStopwatch orig,
        Run self) =>
            false;
}
