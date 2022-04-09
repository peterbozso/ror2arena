using R2API.Utils;
using RoR2;

namespace Arena.Managers;

internal class ArenaManager
{
    private readonly ClockManager _clockManager = new();
    private readonly FriendlyFireManager _friendlyFireManager = new();
    private readonly PortalManager _portalManager = new();
    private readonly DeathManager _deathManager = new();

    private bool _isEventInProgress;

    public void Start()
    {
        TeleporterInteraction.onTeleporterChargedGlobal += OnTeleporterCharged;
        On.RoR2.Run.AdvanceStage += OnAdvanceStage;
    }

    public void Stop()
    {
        TeleporterInteraction.onTeleporterChargedGlobal -= OnTeleporterCharged;
        On.RoR2.Run.AdvanceStage -= OnAdvanceStage;
    }

    private void OnTeleporterCharged(TeleporterInteraction tpi)
    {
        if (_deathManager.IsSinglePlayer)
        {
            Log.LogMessage("Only one player is alive. Not starting the event.");
            return;
        }

        ChatMessage.Send("Good people of the Imperial City, welcome to the Arena!");

        _clockManager.PauseClock();
        _friendlyFireManager.EnableFriendlyFire();
        _portalManager.DisableAllPortals();
        _deathManager.Start(OnAllPlayersDead);

        _isEventInProgress = true;
        Log.LogMessage("Arena event started.");
    }

    private void OnAllPlayersDead(string championName)
    {
        _portalManager.EnableAllPortals();

        ChatMessage.Send($"Good people, we have a winner! All hail the combatant, {championName}! Champion, leave the Arena now and rest! You've earned it!");
    }

    private void OnAdvanceStage(On.RoR2.Run.orig_AdvanceStage orig, Run self, SceneDef nextScene)
    {
        if (_isEventInProgress)
        {
            _clockManager.ResumeClock();
            _friendlyFireManager.DisableFriendlyFire();

            _isEventInProgress = false;
            Log.LogMessage("Arena event ended.");
        }

        orig(self, nextScene);
    }
}
