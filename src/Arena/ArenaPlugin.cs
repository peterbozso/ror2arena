using BepInEx;
using R2API.Utils;
using RoR2;
using UnityEngine;

namespace Arena;

[BepInDependency(R2API.R2API.PluginGUID)]
[BepInPlugin(PluginGUID, PluginName, PluginVersion)]
public class ArenaPlugin : BaseUnityPlugin
{
    public const string PluginGUID = PluginAuthor + "." + PluginName;
    public const string PluginAuthor = "peterbozso";
    public const string PluginName = "Arena";
    public const string PluginVersion = "0.0.1";

    public void Awake()
    {
        Log.Init(Logger);

        On.RoR2.Run.Awake += Run_Awake;
        On.RoR2.Run.OnDestroy += Run_OnDestroy;
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
        ChatMessage.Send("Good people of the Imperial City, welcome to the Arena!");

        ArenaManager.Clock.Pause();
        ArenaManager.FriendlyFire.Enable();
        ArenaManager.Teleporter.Disable();
    }

    private void TeleporterInteraction_onTeleporterFinishGlobal(TeleporterInteraction obj)
    {
        ArenaManager.Clock.Resume();
        ArenaManager.FriendlyFire.Disable();
    }

    private void Update()
    {
#if DEBUG
        if (Input.GetKeyDown(KeyCode.F11))
        {
            // TODO: Do this automatically - alongside an announcement - when there's only one player remaining:
            ArenaManager.Teleporter.Enable();
        }
#endif
    }
}
