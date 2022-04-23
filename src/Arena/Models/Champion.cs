using RoR2;

namespace Arena.Models;

internal class Champion
{
    private readonly CharacterMaster _characterMaster;

    public Champion(CharacterMaster characterMaster) => _characterMaster = characterMaster;

    public string Name => _characterMaster.GetBody().GetUserName();

    public void Rejuvenate()
    {
        // TODO!
    }
}
