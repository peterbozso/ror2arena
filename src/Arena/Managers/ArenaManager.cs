using Arena.Logging;
using Arena.Managers.Bases;
using Arena.Models;
using R2API.Utils;
using RoR2;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;

namespace Arena.Managers;

internal class ArenaManager : ListeningManagerBase
{
    // https://es.wikipedia.org/wiki/Arena_(color)
    private static readonly Color ArenaColor = Color.FromArgb(236, 226, 198);

    public static bool ArenaEnabled;
    public bool IsEventInProgress;

    public override IEnumerable<string> GetStatus() => new string[]
    {
        $"{ (IsListening ? "Watching stage events" : "Not watching stage events") }.",
        $"Arena event is { (IsEventInProgress ? "in progress" : "not in progress") }.",
        $"Arena event is { (ArenaEnabled ? "enabled" : "disabled") }."
    };

    public void WatchTeleporter() => Start();

    public void EndArenaEvent()
    {
        Store.Instance.Get<ClockManager>().ResumeClock();
        Store.Instance.Get<FriendlyFireManager>().DisableFriendlyFire();
        Store.Instance.Get<PortalManager>().EnableAllPortals();
        Store.Instance.Get<DeathManager>().Stop();

        IsEventInProgress = false;

        var currentStageCount = Run.instance.stageClearCount;

        if (ArenaPlugin.maxStageCount != 0 && currentStageCount >= ArenaPlugin.maxStageCount)
        {
            Announce("Congratulations! You're free from the arena!");
            Log.Info($"Current stage number: {currentStageCount}. Arena events will cease due to max config.");
            ArenaEnabled = false;
            return;
        }

        Log.Info("Arena event ended.");
    }

    public void ForceEndArenaEvent()
    {
        Announce("The host has skipped the Arena!");

        //Remove this after voting is added
        Announce("There will be no more Anenas in this run!");
        ArenaEnabled = false;

        EndArenaEvent();
    }

    //TODO: Add voting to end the Arena event
    public void VoteEndArenaEvent()
    {
        Announce("The People have spoken! The Arena shall cease!");
        ArenaEnabled = false;

        EndArenaEvent();
    }

    protected override void StartListening()
    {
        TeleporterInteraction.onTeleporterChargedGlobal += OnTeleporterCharged;

        Log.Info($"Started watching the Teleporter.");
    }

    protected override void StopListening()
    {
        TeleporterInteraction.onTeleporterChargedGlobal -= OnTeleporterCharged;

        Log.Info($"Stopped watching the Teleporter.");
    }

    private async void OnTeleporterCharged(TeleporterInteraction tpi)
    {
        var alivePlayerCount = Store.Instance.Get<PlayerManager>().AlivePlayerCount;
        //var alivePlayerPercent = (float)alivePlayerCount / (float)LocalUserManager.readOnlyLocalUsersList.Count;
        var currentStageCount = Run.instance.stageClearCount;

        if (!ArenaEnabled)
        {
            Log.Info("Arena event is disabled.");
            return;
        }

        if (alivePlayerCount < ArenaPlugin.minAlivePlayerCount)
        {
            Announce("Dissapointing! There's not enough players to begin the arena.");
            Log.Info($"Number of alive players: {alivePlayerCount}. Not starting the Arena event.");
            return;
        }

        IsEventInProgress = true;

        Store.Instance.Get<ClockManager>().PauseClock();
        Store.Instance.Get<PortalManager>().DisableAllPortals();

        if (ArenaPlugin.delayArenaSec != 0)
        {
            Announce($"The Arena will begin in {ArenaPlugin.delayArenaSec} seconds!");
            await Task.Delay(ArenaPlugin.delayArenaSec * 1000);
        }

        Announce("Good people of the Imperial City, welcome to the Arena!");

        //We should temporarily take away items that could be lost
        //Power Elixer, Dio's Best Friend, and permenant damaging things like Symbiotic Scorpion
        //Collect all the items in a list and remove them from the player inventory
        //When the Arena event ends, give the items back to the player

        Store.Instance.Get<FriendlyFireManager>().EnableFriendlyFire();
        Store.Instance.Get<DeathManager>().WatchDeaths(OnChampionWon);

        Log.Info("Arena event started.");

        //Arena Event timeout
        if (ArenaPlugin.maxArenaDurationSec == 0) return;
        int arenaDuration = ArenaPlugin.maxArenaDurationSec;
        Log.Info(arenaDuration);

        while (arenaDuration > 0 && IsEventInProgress)
        {
            switch (arenaDuration)
            {
                case 30:
                    Announce("30 seconds left!");
                    break;
                case 10:
                    Announce("10 seconds left!");
                    break;
            }
            await Task.Delay(1000);
            arenaDuration -= 1;
        }

        if (IsEventInProgress) OnArenaTimeout();
    }

    private void OnArenaTimeout()
    {
        Announce("Dissapointing! There is no winner of this Arena.");
        EndArenaEvent();
    }

    private void OnChampionWon(Champion champion)
    {
        Announce($"Good people, we have a winner! All hail the combatant, {champion.Name}! Champion, leave the Arena now and rest! You've earned it!");

        if (ArenaPlugin.protectChampion) champion.Rejuvenate();

        EndArenaEvent();
    }

    private static void Announce(string message) =>
        ChatMessage.SendColored(message, ArenaColor, "Announcer");
}
