using RoR2;

namespace Arena.Models;

internal class Champion
{
    private readonly CharacterMaster _characterMaster;

    public Champion(CharacterMaster characterMaster) => _characterMaster = characterMaster;

    public string Name => _characterMaster.GetBody().GetUserName();

    public void Rejuvenate()
    {
        //Invulnerability for 10 seconds
        //I don't know which of these does what so just gonna add both.
        _characterMaster.GetBody().AddTimedBuff(RoR2Content.Buffs.HiddenInvincibility, 10f);
        _characterMaster.GetBody().AddTimedBuff(RoR2Content.Buffs.Immune, 10f);

        //Heal the player to full health
        _characterMaster.GetBody().healthComponent.HealFraction(1f, default);
    }
}
