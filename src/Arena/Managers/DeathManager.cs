using RoR2;
using System;

namespace Arena.Managers;

internal class DeathManager : ManagerBase
{
    private Action<string> _onAllPlayersDead;

    public bool IsSinglePlayer => Store.Get<ChampionManager>().ChampionName != string.Empty;

    public override void Destroy() => Stop();

    public void Start(Action<string> onAllPlayersDead)
    {
        _onAllPlayersDead = onAllPlayersDead;
        On.RoR2.CharacterBody.OnDeathStart += OnDeathStart;
    }

    private void Stop() => On.RoR2.CharacterBody.OnDeathStart -= OnDeathStart;

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
            Log.LogMessage("Only the Champion is alive: " + championName);
            Stop();
            _onAllPlayersDead(Store.Get<ChampionManager>().ChampionName);
        }
        else
        {
            Log.LogMessage("There are still multiple fighters alive.");
        }

        orig(self);
    }
}
