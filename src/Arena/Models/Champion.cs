using RoR2;

namespace Arena.Models;

internal class Champion
{
    private readonly CharacterMaster _characterMaster;

    public Champion(CharacterMaster characterMaster) => _characterMaster = characterMaster;

    public string Name => _characterMaster.GetBody().GetUserName();

    public void Rejuvenate()
    {
        //TODO: Probably make this better
        _characterMaster.GetBody().healthComponent.HealFraction(1f, default);
        //Give them a maximum barrier
        _characterMaster.GetBody().healthComponent.AddBarrier(1000f);
    }
}
