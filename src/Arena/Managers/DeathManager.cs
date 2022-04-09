using RoR2;
using System;

namespace Arena.Managers;

internal class DeathManager
{
    private readonly ChampionManager _championManager = new();
    private readonly ItemManager _itemManager = new();

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

        _itemManager.DropRandomItem(self.master);

        var championName = _championManager.ChampionName;

        if (championName != string.Empty)
        {
            Log.LogMessage("Only the Champion is alive: " + championName);
            Stop();
        }
        else
        {
            Log.LogMessage("There are still multiple fighters alive.");
        }

        orig(self);
    }
}
