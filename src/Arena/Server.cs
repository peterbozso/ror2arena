using RoR2;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Networking;

namespace Arena;

internal static class Server
{
    public static bool IsRunning
    {
        get
        {
#if DEBUG
            return true;
#endif
#pragma warning disable CS0162 // Unreachable code detected
            return NetworkServer.active;
#pragma warning restore CS0162 // Unreachable code detected
        }
    }

    public static IEnumerable<CharacterMaster> AllPlayers
    {
        get
        {
#if DEBUG
            return PlayerCharacterMasterController.instances.Select(pcmc => pcmc.master);
#endif
#pragma warning disable CS0162 // Unreachable code detected
            return NetworkUser.readOnlyInstancesList.Select(nu => nu.master);
#pragma warning restore CS0162 // Unreachable code detected
        }
    }
}
