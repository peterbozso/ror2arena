using System.Collections.Generic;
using Arena.Logging;
using Arena.Managers;
using BepInEx;
using BepInEx.Configuration;
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

    public static int minAlivePlayerCount;
    public static int maxArenaSeconds;
    public static int maxStageCount;

    public static bool voteEndEnabled;
    public static float votePercentNeeded;

    private readonly StatusLogger _statusLogger = new();

    public void Awake()
    {
        Init();

        On.RoR2.Run.Awake += OnRunAwake;
        On.RoR2.Run.OnDestroy += OnRunOnDestroy;

        Log.Debug("Plugin hooked.");

        //TODO: Make this is a percent of alive players instead.
        minAlivePlayerCount = Config.Bind<int>(new ConfigDefinition("Main", "Minimum Alive Players"), 2, new ConfigDescription("Minimum Alive Players required to start an an Arena.")).Value;
        maxStageCount = Config.Bind<int>(new ConfigDefinition("Main", "Maximum Stage Count"), 4, new ConfigDescription("After this number of stages, Arena events will cease. 0 Means infinite.")).Value;
        maxArenaSeconds = Config.Bind<int>(new ConfigDefinition("Main", "Maximum Fighting Seconds"), 120, new ConfigDescription("Maximum seconds before an Arena even will end in a draw. 0 Means infinite.")).Value;

        voteEndEnabled = Config.Bind<bool>(new ConfigDefinition("Main", "Arena voting"), true, new ConfigDescription("If true, players can vote to end the Arena event.")).Value;
        votePercentNeeded = Config.Bind<float>(new ConfigDefinition("Main", "Arena end voting percentage"), 0.5f, new ConfigDescription("The percentage of players that need to vote to end the Arena event.")).Value;

        Log.Debug($"minAlivePlayerCount: {minAlivePlayerCount}");
        Log.Debug($"maxStageCount: {maxStageCount}");
        Log.Debug($"maxArenaSeconds: {maxArenaSeconds}");
        Log.Debug($"voteEndEnabled: {voteEndEnabled}");
        Log.Debug($"votePercentNeeded: {votePercentNeeded}");
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
        ArenaManager.ArenaEnabled = true;

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
