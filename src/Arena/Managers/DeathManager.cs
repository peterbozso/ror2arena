using Arena.Managers.Bases;
using RoR2;
using System;

namespace Arena.Managers;

internal class DeathManager : ListeningManagerBase
{
    private Action<string> _onChampionWon;

    public bool IsOnePlayerAlive => Store.Get<ChampionManager>().ChampionName != string.Empty;

    public void WatchDeaths(Action<string> onchampionWon)
    {
        _onChampionWon = onchampionWon;
        StartListening();
    }

    protected override void StartListening()
    {
        On.RoR2.CharacterBody.OnDeathStart += OnDeathStart;

        Log.LogDebug($"Started watching deaths.");

        base.StartListening();
    }

    protected override void StopListening()
    {
        On.RoR2.CharacterBody.OnDeathStart -= OnDeathStart;

        Log.LogDebug($"Stopped watching deaths.");

        base.StopListening();
    }

    private void OnDeathStart(On.RoR2.CharacterBody.orig_OnDeathStart orig, CharacterBody self)
    {
        if (!self.isPlayerControlled)
        {
            orig(self);
            return;
        }

        Store.Get<ItemManager>().DropRandomItem(self.master);

        var championName = Store.Get<ChampionManager>().ChampionName;

        if (championName != string.Empty)
        {
            Log.LogDebug("All other players died, only the Champion is alive: " + championName);

            StopListening();
            _onChampionWon(Store.Get<ChampionManager>().ChampionName);
        }
        else
        {
            Log.LogDebug("There are still multiple fighters alive.");
        }

        orig(self);
    }
}
