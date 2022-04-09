using Arena.Managers;
using BepInEx;
using RoR2;

namespace Arena;

[BepInDependency(R2API.R2API.PluginGUID)]
[BepInPlugin(PluginGUID, PluginName, PluginVersion)]
public class ArenaPlugin : BaseUnityPlugin
{
    public const string PluginGUID = PluginAuthor + "." + PluginName;
    public const string PluginAuthor = "peterbozso";
    public const string PluginName = "Arena";
    public const string PluginVersion = "0.1.1";

    private readonly ArenaManager _arenaManager = new();

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

        _arenaManager.Start();
        Log.LogMessage("Arena plugin hooked.");
    }

    private void Run_OnDestroy(On.RoR2.Run.orig_OnDestroy orig, Run self)
    {
        orig(self);

        _arenaManager.Stop();
        Log.LogMessage("Arena plugin unhooked.");
    }
}
