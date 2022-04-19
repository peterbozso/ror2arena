using Arena.Logging;
using Arena.Managers;
using BepInEx;
using R2API.Utils;
using RoR2;
using UnityEngine;

namespace Arena;

[NetworkCompatibility(CompatibilityLevel.NoNeedForSync)]
[R2APISubmoduleDependency(nameof(CommandHelper))]
[BepInDependency(R2API.R2API.PluginGUID)]
[BepInPlugin(PluginGUID, PluginName, PluginVersion)]
public class ArenaPlugin : BaseUnityPlugin
{
    public const string PluginGUID = PluginAuthor + "." + PluginName;
    public const string PluginAuthor = "peterbozso";
    public const string PluginName = "Arena";
    public const string PluginVersion = "0.2.0";

    private readonly StatusLogger _statusLogger = new();

    public void Awake()
    {
        Init();

        On.RoR2.Run.Awake += OnRunAwake;
        On.RoR2.Run.OnDestroy += OnRunOnDestroy;

        Log.Debug("Plugin hooked.");
    }

    public void OnDestroy()
    {
        On.RoR2.Run.Awake -= OnRunAwake;
        On.RoR2.Run.OnDestroy -= OnRunOnDestroy;

        Log.Debug("Plugin unhooked.");
    }

    public void Update()
    {
        if (Server.IsRunning && Input.GetKeyDown(KeyCode.F2))
        {
            _statusLogger.LogStatus();
        }
    }

    private void Init()
    {
        Log.Init(Logger);
        CommandHelper.AddToConsoleWhenReady();
    }

    private void OnRunAwake(On.RoR2.Run.orig_Awake orig, Run self)
    {
        orig(self);

        if (!Server.IsRunning)
        {
            return;
        }

        Store.Instance.Get<ArenaManager>().WatchTeleporter();

        Log.Info("Run started.");
    }

    private void OnRunOnDestroy(On.RoR2.Run.orig_OnDestroy orig, Run self)
    {
        orig(self);

        if (!Server.IsRunning)
        {
            return;
        }

        Store.Instance.CleanUp();

        Log.Info("Run ended.");
    }
}
