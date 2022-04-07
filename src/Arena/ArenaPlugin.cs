using BepInEx;
using R2API.Utils;
using RoR2;
using UnityEngine;

namespace Arena;

// This attribute specifies that we have a dependency on R2API, as we're using it to add our item to the game.
[BepInDependency(R2API.R2API.PluginGUID)]
// This attribute is required, and lists metadata for the plugin.
[BepInPlugin(PluginGUID, PluginName, PluginVersion)]

public class ArenaPlugin : BaseUnityPlugin
{
    public const string PluginGUID = PluginAuthor + "." + PluginName;
    public const string PluginAuthor = "peterbozso";
    public const string PluginName = "Arena";
    public const string PluginVersion = "0.0.1";

    // The Awake() method is run at the very start when the game is initialized.
    public void Awake()
    {
        Log.Init(Logger);

        TeleporterInteraction.onTeleporterChargedGlobal += TeleporterInteraction_onTeleporterChargedGlobal;
        TeleporterInteraction.onTeleporterFinishGlobal += TeleporterInteraction_onTeleporterFinishGlobal;
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
    }

    // The Update() method is run on every frame of the game.
    private void Update()
    {
#if DEBUG
        if (Input.GetKeyDown(KeyCode.F11))
        {
            DebugMode.Enable();
        }
        else if (Input.GetKeyDown(KeyCode.F10))
        {
            ArenaManager.Clock.Resume();
            ArenaManager.FriendlyFire.Disable();
            ArenaManager.Teleporter.Enable();
        }
#endif
    }
}
