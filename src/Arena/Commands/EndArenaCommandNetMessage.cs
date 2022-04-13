using Arena.Logging;
using Arena.Managers;
using R2API.Networking.Interfaces;
using UnityEngine.Networking;

namespace Arena.Commands;

internal class EndArenaCommandNetMessage : INetMessage
{
    public void Serialize(NetworkWriter writer) { }

    public void Deserialize(NetworkReader reader) { }

    public void OnReceived()
    {
        Log.Debug($"{ConCommands.EndArenaCommandName}: Ending arena event in response to the server's console command...");

        Store.Instance.Get<ArenaManager>().EndArenaEvent();
    }
}
