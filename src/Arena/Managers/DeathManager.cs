using Arena.Logging;
using Arena.Managers.Bases;
using RoR2;
using System;
using System.Collections.Generic;

namespace Arena.Managers;

internal class DeathManager : ListeningManagerBase
{
    private Action<string> _onChampionWon;

    public override IEnumerable<string> GetStatus() => new string[]
    {
        $"{ (IsListening ? "Watching deaths" : "Not watching deaths") }."
    };

    public bool IsOnePlayerAlive => Store.Instance.Get<ChampionManager>().ChampionName != string.Empty;

    public void WatchDeaths(Action<string> onChampionWon)
    {
        _onChampionWon = onChampionWon;
        StartListening();
    }

    protected override void StartListening()
    {
        On.RoR2.CharacterBody.OnDeathStart += OnDeathStart;

        Log.LogInfo($"Started watching deaths.");

        base.StartListening();
    }

    protected override void StopListening()
    {
        On.RoR2.CharacterBody.OnDeathStart -= OnDeathStart;

        Log.LogInfo($"Stopped watching deaths.");

        base.StopListening();
    }

    private void OnDeathStart(On.RoR2.CharacterBody.orig_OnDeathStart orig, CharacterBody self)
    {
        if (!self.isPlayerControlled)
        {
            orig(self);
            return;
        }

        Store.Instance.Get<ItemManager>().DropRandomItem(self.master);

        var championName = Store.Instance.Get<ChampionManager>().ChampionName;

        if (championName != string.Empty)
        {
            Log.LogInfo("Only the Champion is alive: " + championName);

            StopListening();
            _onChampionWon(championName);
        }
        else
        {
            Log.LogInfo("There are still multiple fighters alive.");
        }

        orig(self);
    }
}
