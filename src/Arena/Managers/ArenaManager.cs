using Arena.Logging;
using Arena.Managers.Bases;
using Arena.Models;
using R2API.Utils;
using RoR2;
using System.Collections.Generic;
using System.Drawing;

namespace Arena.Managers;

internal class ArenaManager : ListeningManagerBase
{
    // https://es.wikipedia.org/wiki/Arena_(color)
    private static readonly Color ArenaColor = Color.FromArgb(236, 226, 198);

    public bool IsEventInProgress;
    public bool ArenaEnabled = true;

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

    private void OnTeleporterCharged(TeleporterInteraction tpi)
    {
        var alivePlayerCount = Store.Instance.Get<PlayerManager>().AlivePlayerCount;
        var currentStageCount = Run.instance.stageClearCount;

        //TODO: Add a config for the minimum number of alive players required to start.
        if (alivePlayerCount < 2)
        {
            Log.Info($"Number of alive players: {alivePlayerCount}. Not starting the Arena event.");
            return;
        }

        //TODO: Add a config for the max stage number before the Arena event stop happening
        if (currentStageCount > 4)
        {
            Log.Info($"Current stage number: {currentStageCount}. Not starting the Arena event.");
            return;
        }

        Announce("Good people of the Imperial City, welcome to the Arena!");

        Store.Instance.Get<ClockManager>().PauseClock();
        Store.Instance.Get<FriendlyFireManager>().EnableFriendlyFire();
        //TODO: Take away single time use items like Power Elixir and Symbotic Scorpion
        //Working out how to return them after might be painful...
        Store.Instance.Get<PortalManager>().DisableAllPortals();
        Store.Instance.Get<DeathManager>().WatchDeaths(OnChampionWon);

        IsEventInProgress = true;

        Log.Info("Arena event started.");
    }

    //Todo: Add a config for a draw timer
    private void OnArenaTimeout()
    {
        Announce("Dissapointing! There is no winner of this Arena event.");

        EndArenaEvent();
    }

    private void OnChampionWon(Champion champion)
    {
        Announce($"Good people, we have a winner! All hail the combatant, {champion.Name}! Champion, leave the Arena now and rest! You've earned it!");

        champion.Rejuvenate();

        EndArenaEvent();
    }

    private static void Announce(string message) =>
        ChatMessage.SendColored(message, ArenaColor, "Arena Mouth");
}
