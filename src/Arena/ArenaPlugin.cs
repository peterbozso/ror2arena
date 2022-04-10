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

    public void Awake()
    {
        Log.Init(Logger);

        On.RoR2.Run.Awake += OnRunAwake;
        On.RoR2.Run.OnDestroy += OnRunOnDestroy;
    }

    public void OnDestroy()
    {
        On.RoR2.Run.Awake -= OnRunAwake;
        On.RoR2.Run.OnDestroy -= OnRunOnDestroy;
    }

    private void OnRunAwake(On.RoR2.Run.orig_Awake orig, Run self)
    {
        orig(self);

        Store.Get<ArenaManager>().Start();
        Log.LogMessage("Arena plugin hooked.");
    }

    private void OnRunOnDestroy(On.RoR2.Run.orig_OnDestroy orig, Run self)
    {
        orig(self);

        Store.DestroyAll();
        Log.LogMessage("Arena plugin unhooked.");
    }
}
