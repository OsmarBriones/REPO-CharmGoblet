# Charm Goblet

Healing Chalice: the Goblet charges slowly while it stays in a pocket, and when an extraction point completes it heals you and nearby allies in proportion to its charge, up to all of your health.

> **Everyone in the lobby must install this mod, together with Pockets and CharmCore.** The power changes what your own character can do and its state shows on your inventory slot, so friends without the mods would see an empty slot and be unable to use the pocket.

## How to Use

1. Find the **Goblet** valuable and store it in a free inventory slot with [Pockets](https://thunderstore.io/c/repo/p/OsmarBriones/Pockets/) (grab it, press the slot key).
2. Keep it there. It **charges slowly** while it stays in a pocket (about four minutes for a full charge by default, roughly what a team takes to gather the loot for an extraction point). The slot shows the charge as a percentage, with a veil that lifts as it fills. When an extraction point completes, you and every teammate within about 10 metres recover health in proportion to the charge: at 100% all of their health, at 50% half of their maximum health, and so on. Players who are dead are not revived, and players already at full health are not affected.
3. After healing, the Goblet starts charging again from zero.
4. Take the valuable out of the pocket and the power stops, and its charge is lost.

## Features

- **Healing Chalice:** carry the Goblet for a while, finish an extraction, and the whole group near you gets back health. It cannot be pocketed at the last second: a fresh Goblet heals little, a charged one heals everything. If several players carry a Goblet, each one heals separately, so the healing stacks.
- **It costs the valuable:** each extraction in which the Goblet actually healed someone wears away a share of its value, and a worn-out valuable breaks. If everybody nearby was already at full health, nothing is healed and nothing is worn.
- **Everything is configurable:** wear, how much health is restored and how far the healing reaches. The host's choices apply to everyone.

## Requirements
- [BepInEx Pack for R.E.P.O.](https://thunderstore.io/c/repo/p/BepInEx/BepInExPack/)
- [Pockets](https://thunderstore.io/c/repo/p/OsmarBriones/Pockets/)
- [CharmCore](https://thunderstore.io/c/repo/p/OsmarBriones/CharmCore/)

## Installation
1. Install through r2modman / Thunderstore Mod Manager (Pockets and CharmCore come with it), or place `CharmGoblet.dll` next to `Pockets.dll` and `CharmCore.dll` in `BepInEx/plugins`. **All players need it.**
2. Launch the game once; the configuration file is created inside `BepInEx/config`.

## Configuration
Settings are in `com.osmar.CharmGoblet.cfg` inside `BepInEx/config`, section `Charm.Goblet`. The host's choices apply to everyone.

- `Enabled` (default `true`): turn this charm on or off.
- `WearPercent` (default `20`, 0 to 100): share of the valuable's original value lost each time the Goblet heals at an extraction.
- `ChargeSeconds` (default `240`, 5 to 1800): seconds the Goblet must stay in a pocket to be fully charged. The game has no fixed duration for an extraction point (it depends on the loot), so this is an estimate: raise it if the Goblet charges too fast for your group.
- `HealPercentOfMaxHealth` (default `100`, 0 to 100): share of each player's maximum health restored at FULL charge; a partial charge restores that share in proportion. `0` turns the healing off.
- `RadiusMeters` (default `10`, 1 to 50): how far from the Goblet carrier other players still get healed. The carrier is always healed.

Section `Debug`: `DebugMode` (default `false`) places a Goblet on the floor in front of you when a level starts (host or singleplayer) and writes extra log lines. For testing. `SpawnNow` (default `false`, needs `DebugMode`): set it to `true` while a level is running to place one right away; it switches itself back off.

## Issues & Bug Reports
The official way to report issues, suggest improvements, or submit feedback is by opening an issue on the official GitHub repository:
👉 [GitHub Issues](https://github.com/OsmarBriones/REPO-CharmGoblet/issues)

## Credits
Developed by **Osmar Briones**
