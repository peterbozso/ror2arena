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
        _characterMaster.GetBody().AddTimedBuff(BuffCatalog.FindBuffIndex("Immune"), 10f);
        //Heal the player to full health
        _characterMaster.GetBody().healthComponent.HealFraction(1f, default);
        //Give them a maximum sheild, just in case
        _characterMaster.GetBody().healthComponent.AddBarrier(1000f);
    }
}
