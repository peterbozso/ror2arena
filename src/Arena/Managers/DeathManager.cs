using Arena.Logging;
using Arena.Managers.Bases;
using Arena.Models;
using RoR2;
using System;
using System.Collections.Generic;

namespace Arena.Managers;

internal class DeathManager : ListeningManagerBase
{
    private Action<Champion> _onChampionWon;

    public override IEnumerable<string> GetStatus() => new string[]
    {
        $"{ (IsListening ? "Watching deaths" : "Not watching deaths") }."
    };

    public void WatchDeaths(Action<Champion> onChampionWon)
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

        var champion = Store.Instance.Get<PlayerManager>().Champion;

        if (champion == null)
        {
            Log.Info("There are still multiple fighters alive.");
        }
        else
        {
            Log.Info("Only the Champion is alive: " + champion);

            Stop();
            _onChampionWon(champion);
        }

        orig(self);
    }
}
