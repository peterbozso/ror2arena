using RoR2;
using System;

namespace Arena.Managers;

internal class DeathManager
{
    private readonly ChampionManager _championManager = new();

    private Action<string> _onAllPlayersDead;

    public bool IsSinglePlayer => _championManager.ChampionName != string.Empty;

    public void Start(Action<string> onAllPlayersDead)
    {
        _onAllPlayersDead = onAllPlayersDead;
        On.RoR2.CharacterBody.OnDeathStart += OnDeathStart;
    }

    private void Stop()
    {
        On.RoR2.CharacterBody.OnDeathStart -= OnDeathStart;
        _onAllPlayersDead(_championManager.ChampionName);
    }

    private void OnDeathStart(On.RoR2.CharacterBody.orig_OnDeathStart orig, CharacterBody self)
    {
        if (!self.isPlayerControlled)
        {
            orig(self);
            return;
        }

        var championName = _championManager.ChampionName;

        Log.LogMessage(championName == string.Empty
            ? "There are still multiple fighters alive."
            : "Only the Champion is alive: " + championName);

        if (championName != string.Empty)
        {
            Stop();
        }

        orig(self);
    }
}
