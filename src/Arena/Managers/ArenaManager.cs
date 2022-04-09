using R2API.Utils;
using RoR2;

namespace Arena.Managers;

internal class ArenaManager
{
    private readonly Champion _champion = new();
    private readonly Clock _clock = new();
    private readonly FriendlyFire _friendlyFire = new();
    private readonly Portals _portals = new();

    private bool _isEventInProgress;

    public void Start()
    {
        TeleporterInteraction.onTeleporterChargedGlobal += TeleporterInteraction_onTeleporterChargedGlobal;
        On.RoR2.Run.AdvanceStage += Run_AdvanceStage;
    }

    public void Stop()
    {
        TeleporterInteraction.onTeleporterChargedGlobal -= TeleporterInteraction_onTeleporterChargedGlobal;
        On.RoR2.Run.AdvanceStage -= Run_AdvanceStage;
    }

    private void TeleporterInteraction_onTeleporterChargedGlobal(TeleporterInteraction tpi)
    {
        if (_champion.Name != string.Empty)
        {
            Log.LogMessage("Only one player alive. Not starting the event.");
            return;
        }

        ChatMessage.Send("Good people of the Imperial City, welcome to the Arena!");

        _clock.Pause();
        _friendlyFire.Enable();
        _portals.Disable();

        On.RoR2.CharacterBody.OnDeathStart += CharacterBody_OnDeathStart;

        _isEventInProgress = true;
        Log.LogMessage("Arena event started.");
    }

    private void Run_AdvanceStage(On.RoR2.Run.orig_AdvanceStage orig, Run self, SceneDef nextScene)
    {
        if (_isEventInProgress)
        {
            _clock.Resume();
            _friendlyFire.Disable();

            _isEventInProgress = false;
            Log.LogMessage("Arena event ended.");
        }

        orig(self, nextScene);
    }

    private void CharacterBody_OnDeathStart(On.RoR2.CharacterBody.orig_OnDeathStart orig, CharacterBody self)
    {
        if (!self.isPlayerControlled)
        {
            orig(self);
            return;
        }

        var championName = _champion.Name;

        Log.LogMessage(championName == string.Empty
            ? "There are still multiple fighters alive."
            : "Only the Champion is alive: " + championName);

        if (championName != string.Empty)
        {
            On.RoR2.CharacterBody.OnDeathStart -= CharacterBody_OnDeathStart;

            _portals.Enable();

            ChatMessage.Send($"Good people, we have a winner! All hail the combatant, {championName}! Champion, leave the Arena now and rest! You've earned it!");
        }

        orig(self);
    }
}
