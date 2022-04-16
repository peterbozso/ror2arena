# Arena

This mod is multiplayer-only. Only the host needs to have it installed.

At the end of each stage, when the Teleporter is fully charged, the following things will happen:

1. Time is frozen.
2. Artifact of Chaos is added.
3. The Teleporter and any other portals are disabled.

In order to progress the game, the players need to kill each other. The last man standing can progress the run to the next stage by using the Teleporter or any other portal. At the beginning of the next stage, everything continues normally.

**Important: this mod is currently in Alpha. There might be rough edges, run- or even game-breaking bugs. If you find any issues, please report them [here.](https://github.com/peterbozso/ror2arena/issues) Your feedback is also very welcome there.**

## Troubleshooting

If the Arena event gets stuck for whatever reason, the player who hosts the game can use the `arena_end` console command to manually end the event.

## Changelog

### 0.2.0

* Make the mod host-only. Fixes [this bug](https://github.com/peterbozso/ror2arena/issues/9) and other issues related to state synchronization.
* Disable Artifact of Chaos and unfreeze time when the Champion is announced, not at the beginning of the next stage.
* Make Arena event announcements fancier.
* Add troubleshooting and debugging tools.

### 0.1.1

* Add missing dependency on R2API.
* Fix Arena event starting with only one survivor being alive, causing run to stuck.

### 0.1.0

* Initial release.
