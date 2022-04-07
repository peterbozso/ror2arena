using RoR2;

namespace Arena;

internal static class DebugMode
{
    public static void Enable()
    {
        // Make player invulnerable:
        On.RoR2.HealthComponent.TakeDamage += (orig, self, damageInfo) =>
        {
            var charComponent = self.GetComponent<CharacterBody>();

            if (charComponent != null && charComponent.isPlayerControlled)
            {
                return;
            }

            orig(self, damageInfo);
        };

        // Finish the teleporter event immediately:
        TeleporterInteraction.instance.currentState.outer.SetNextState(
            new TeleporterInteraction.ChargedState());
    }
}
