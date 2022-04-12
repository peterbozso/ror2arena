using Arena.Managers.Bases;
using RoR2;
using System.Collections.Generic;

namespace Arena.Managers;

internal class ItemManager : ManagerBase
{
    public override IEnumerable<string> GetStatus() => new[] { "Not implemented" };

    public void DropRandomItem(CharacterMaster player)
    {
        // TODO!
    }
}
