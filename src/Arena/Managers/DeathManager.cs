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
        Start();
    }

    protected override void StartListening()
    {
        On.RoR2.CharacterBody.OnDeathStart += OnDeathStart;

        Log.Info($"Started watching deaths.");
    }

    protected override void StopListening()
    {
        On.RoR2.CharacterBody.OnDeathStart -= OnDeathStart;

        Log.Info($"Stopped watching deaths.");
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
            Log.Info("Only the Champion is alive: " + championName);

            Stop();
            _onChampionWon(championName);
        }
        else
        {
            Log.Info("There are still multiple fighters alive.");
        }

        orig(self);
    }
}
