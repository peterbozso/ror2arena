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

    private static ArenaManager.Teleporter Teleporter = new();

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
        TeleporterInteraction.onTeleporterFinishGlobal += TeleporterInteraction_onTeleporterFinishGlobal;

        Log.LogMessage("Arena hooked.");
    }

    private void Run_OnDestroy(On.RoR2.Run.orig_OnDestroy orig, Run self)
    {
        orig(self);

        TeleporterInteraction.onTeleporterChargedGlobal -= TeleporterInteraction_onTeleporterChargedGlobal;
        TeleporterInteraction.onTeleporterFinishGlobal -= TeleporterInteraction_onTeleporterFinishGlobal;

        Log.LogMessage("Arena unhooked.");
    }

    private void TeleporterInteraction_onTeleporterChargedGlobal(TeleporterInteraction tpi)
    {
        if (ArenaManager.Champion.Name != string.Empty)
        {
            Log.LogMessage("Only one player alive. Not starting the event.");
            return;
        }

        ChatMessage.Send("Good people of the Imperial City, welcome to the Arena!");

        ArenaManager.Clock.Pause();
        ArenaManager.FriendlyFire.Enable();
        Teleporter.Disable();

        On.RoR2.CharacterBody.OnDeathStart += CharacterBody_OnDeathStart;
    }

    private void TeleporterInteraction_onTeleporterFinishGlobal(TeleporterInteraction obj)
    {
        ArenaManager.Clock.Resume();
        ArenaManager.FriendlyFire.Disable();
    }

    private void CharacterBody_OnDeathStart(On.RoR2.CharacterBody.orig_OnDeathStart orig, CharacterBody self)
    {
        orig(self);

        if (!self.isPlayerControlled)
        {
            return;
        }

        var championName = ArenaManager.Champion.Name;

        Log.LogMessage(championName == string.Empty
            ? "There are still multiple fighters alive."
            : "Only the Champion is alive: " + championName);

        if (championName != string.Empty)
        {
            On.RoR2.CharacterBody.OnDeathStart -= CharacterBody_OnDeathStart;

            Teleporter.Enable();

            ChatMessage.Send($"Good people, we have a winner! All hail the combatant, {championName}! Champion, leave the Arena now and rest! You've earned it!");
        }
    }
}
