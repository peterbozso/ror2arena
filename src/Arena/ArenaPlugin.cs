using Arena.Managers;
using BepInEx;
using R2API.Utils;
using RoR2;

namespace Arena;

[BepInDependency(R2API.R2API.PluginGUID)]
[BepInPlugin(PluginGUID, PluginName, PluginVersion)]
public class ArenaPlugin : BaseUnityPlugin
{
    public const string PluginGUID = PluginAuthor + "." + PluginName;
    public const string PluginAuthor = "peterbozso";
    public const string PluginName = "Arena";
    public const string PluginVersion = "0.0.1";

    private readonly Champion _champion = new();
    private readonly Clock _clock = new();
    private readonly FriendlyFire _friendlyFire = new();
    private readonly Portals _portals = new();

    private bool _isEventInProgress;

    public void Awake()
    {
        Log.Init(Logger);

        On.RoR2.Run.Awake += Run_Awake;
        On.RoR2.Run.OnDestroy += Run_OnDestroy;
    }

    public void OnDestroy()
    {
        On.RoR2.Run.Awake -= Run_Awake;
        On.RoR2.Run.OnDestroy -= Run_OnDestroy;
    }

    private void Run_Awake(On.RoR2.Run.orig_Awake orig, Run self)
    {
        orig(self);

        TeleporterInteraction.onTeleporterChargedGlobal += TeleporterInteraction_onTeleporterChargedGlobal;
        On.RoR2.Stage.Start += Stage_Start;

        Log.LogMessage("Arena plugin hooked.");
    }

    private void Run_OnDestroy(On.RoR2.Run.orig_OnDestroy orig, Run self)
    {
        orig(self);

        TeleporterInteraction.onTeleporterChargedGlobal -= TeleporterInteraction_onTeleporterChargedGlobal;
        On.RoR2.Stage.Start -= Stage_Start;

        Log.LogMessage("Arena plugin unhooked.");
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

    private void Stage_Start(On.RoR2.Stage.orig_Start orig, Stage self)
    {
        orig(self);

        if (_isEventInProgress)
        {
            _clock.Resume();
            _friendlyFire.Disable();

            _isEventInProgress = false;
            Log.LogMessage("Arena event ended.");
        }
    }

    private void CharacterBody_OnDeathStart(On.RoR2.CharacterBody.orig_OnDeathStart orig, CharacterBody self)
    {
        orig(self);

        if (!self.isPlayerControlled)
        {
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
    }
}
