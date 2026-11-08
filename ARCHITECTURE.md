# CharmGoblet Architecture

A Charm mod (layer 3 of the PocketCharms family): one valuable, one power. It depends on **Pockets** (storage) and **CharmCore** (registry, wear, host settings, debug mode).

## The power: Healing Chalice

**Charging (changed after the first design).** The Goblet is a *charging* charm (`Charm.EnableCharge`): its charge starts at 0 when it is stored, grows linearly with time while the valuable stays in a pocket (`ChargeSeconds`, default 240 s), is kept when the valuable is taken out (it stops growing), continues for whoever stores it next, and is reset to 0 after it heals. Without it the chalice could be pocketed only at the moment an extraction completes. The heal at an extraction is `HealPercentOfMaxHealth` (default 100%) times the carrier's charge fraction; a chalice with no charge does nothing and is not worn, and one that heals nobody keeps its charge. The default 240 s is an estimate: the game has no fixed duration for an extraction point (the fixed phases are a few seconds: about 2 s of success, then warning, extraction and about 3 s of complete; the real time is the gathering of the loot, which depends on the quota and the distance to the loot), so it is a setting.

When an extraction point completes, each player carrying the Goblet in a pocket heals themselves and every living player within `RadiusMeters` of them by `HealPercentOfMaxHealth` x charge percent of that player's max health. This is a **host effect**: everything runs on the host (or alone).

- `GobletCharm : Charm` registers the prefab name `Valuable Manor Goblet`, wear 20% (no cooldown) and the host-synced settings below.
- `Patches/RoundDirector_ExtractionCompleted_Patch.cs`: Harmony postfix on `RoundDirector.ExtractionCompleted()`. It returns unless `SemiFunc.IsMasterClientOrSingleplayer()`, then calls `HealingChalice.OnExtractionCompleted`.
- `HealingChalice` (static class) loops over carriers with `CharmsApi.ForEachCarrier`, heals and wears with `charm.WearValuable`.
- `Domain/ChaliceMath.cs` holds the game-free maths (`HealAmount`, `IsWithinRange`, `NeedsHealing`, `IsDuplicateCompletion`), unit tested in `ChaliceMathTests`.

### Flow
1. The game runs the extraction state machine on every machine (`ExtractionPoint.StateSetRPC` is sent to all). The completion calls `RoundDirector.ExtractionCompleted()` from two places in `ExtractionPoint`: `StateTaxReturn` (on state start, only reached when `taxReturn` is true) and `StateComplete` (on state start, inside `if (!taxReturn)`). They are mutually exclusive: a tax-return extraction goes TaxReturn then Complete, and Complete skips the call because `taxReturn` is still true. So one completion calls the method once per machine. Hooking the method (not the two call sites) fires once per completion per machine; the non-host machines return at the host guard.
2. On the host the postfix reads the Goblet settings, then for each carrier (`ForEachCarrier` skips dead players and a disabled charm) heals the carrier plus every living player (not `deadSet`, not `isDisabled`) whose `transform.position` is within `RadiusMeters` of the carrier's, using `PlayerHealth.HealOther(amount, true)`. `HealOther` heals directly when the target is owned by this machine (or in singleplayer); otherwise it sends `HealOtherRPC`, which the master is allowed to send.
3. A player is skipped when `health + pendingHeal >= maxHealth` (already full). The wear is `charm.WearValuable(valuable, charm.WearPercent)` once per carrier, only if that carrier's chalice healed at least one player.

### Settings
| Key | Type | Range | Default | Meaning |
|---|---|---|---|---|
| `ChargeSeconds` (CharmCore) | float, seconds | 5 to 1800 | 240 | Time in a pocket for a full charge. |
| `HealPercentOfMaxHealth` | float | 0 to 100 | 100 | Percent of each player's max health restored at FULL charge; partial charge scales it. 0 turns healing off (no wear). Result rounded to the nearest point, at least 1. |
| `RadiusMeters` | float | 1 to 50 | 10 | Straight-line distance (between avatar transform positions) from the carrier. The carrier is always healed. |
| `WearPercent` (CharmCore) | float | 0 to 100 | 20 | Wear per carrier per extraction in which somebody was healed. |

## Game members used

- `RoundDirector.ExtractionCompleted()` (patched, postfix) and `RoundDirector.extractionPointsCompleted` (read, for the duplicate guard).
- `SemiFunc.IsMasterClientOrSingleplayer()`.
- `GameDirector.PlayerList`; `PlayerAvatar.deadSet`, `isDisabled`, `playerHealth`.
- `PlayerHealth.health`, `maxHealth` (read), `PlayerHealth.HealOther(int, bool)`.
- `ValuableObject.dollarValueCurrent` (wear, done by CharmCore).

All are checked in `CharmGoblet.Tests/GameContractTests.cs`.

## Debug mode

Provided by CharmCore: the `Debug.DebugMode` setting (off by default) places the valuable on the floor at the start of each level and enables `Charm.LogDebug` output. This mod logs each completion, each carrier and each heal with it.

## Decisions

- **Host does everything.** `PlayerHealth.Heal` is owner-only and `HealOtherRPC` accepts only the master or the target's owner, so the host heals allies and itself through `HealOther`. Carriers on clients need no message.
- **Stacking.** Each carrier's chalice heals separately; two carriers near the same ally heal it twice (the game clamps at max health). A second chalice skips an ally the first already filled (`pendingHeal`), so no wear is spent on a full player.
- **Wear.** Once per carrier per completion, only if at least one player (the carrier included) was actually healed. Full health everywhere means no heal and no wear. Only the first Goblet of a carrier counts (`ForEachCarrier` hands over one valuable per player).
- **Range** is plain distance between avatar `transform.position` values, not `PlayerGetAllPlayerAvatarWithinRange` (that one measures from the vision target and has a raycast option; plain distance is simpler to test and explain). No line-of-sight check: walls do not block the healing.
- **Dead players** (`deadSet`, `isDisabled`, health 0) are skipped; the chalice never revives.
- **Duplicate guard.** Beyond the analysis above, a repeated event with the same `extractionPointsCompleted` within 2 s is ignored. Two different extraction points always have different counts.
- **Rounding:** nearest point, halves up, minimum 1 when the percent is above 0.
- **Heal effect flag** is `true` (the game's heal flash).

## Known limits

- Looked up by exact prefab name; a game update that renames the prefab needs a mod update.
- Not verified in game: the build and unit tests prove the logic and that the game members exist.
- Unverified: that `ExtractionCompleted` runs once per completion on the host (derived from reading the decompiled code, with the duplicate guard as a safety net).
- Unverified: that the host sees up-to-date `health` and `maxHealth` of remote players (the owner sends them to the others after a change). A stale-high value could skip a hurt player; a stale-low value could charge wear for a heal the game ignores.
- Unverified: `HealOtherRPC` from the host to a remote target in a real lobby, and the heal effect shown to the target.
- Unverified: that avatar positions on the host are current for remote players.

## Runtime hygiene
Checked against `RepoKit/references/runtime-hygiene.md`:
1. Lifetime of references: fixed. The only remembered state is the duplicate-completion memory (`lastCount`, `lastTime`; plain numbers, no game objects). `GobletCharm` now overrides `OnLevelEnded` and calls `HealingChalice.Reset()`, so it cannot suppress an extraction in the next level. The per-extraction `Dictionary<PlayerAvatar,int>` is a local and is dropped when the call ends.
2. Work frequency: ok. Everything is event-driven (one patch on `RoundDirector.ExtractionCompleted`, host only); nothing runs per frame.
3. Allocations: accepted. Each extraction allocates one dictionary, a closure and a `PlayerList.ToArray()` per carrier; extractions are rare, so this is not worth a shared buffer (a static buffer would hold players). Debug lines are now guarded with `if (charm.DebugMode)`. No `Find` calls.
4. UI: not applicable.
5. Patches: ok. The postfix exits at once when not the host; there is no per-frame patch.
Not verified in game (build and unit tests only).
