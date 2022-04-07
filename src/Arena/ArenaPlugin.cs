using BepInEx;
using R2API;
using R2API.Utils;

namespace Arena;

// This attribute specifies that we have a dependency on R2API, as we're using it to add our item to the game.
[BepInDependency(R2API.R2API.PluginGUID)]
// We will be using 2 modules from R2API: ItemAPI to add our item and LanguageAPI to add our language tokens.
[R2APISubmoduleDependency(nameof(ItemAPI), nameof(LanguageAPI))]
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

        Log.LogInfo("Good people of the Imperial City, welcome to the Arena!");
    }

    // The Update() method is run on every frame of the game.
    private void Update()
    {
    }
}
