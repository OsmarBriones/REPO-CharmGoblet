# Changelog

## [Unreleased]

### Changed
- The Goblet now charges: it needs to stay in a pocket (about four minutes for a full charge, `ChargeSeconds`) and heals in proportion to its charge, up to all of the players' health (`HealPercentOfMaxHealth` default is now 100, it was 30). The charge belongs to the Goblet: it is kept when the Goblet is taken out of the pocket and continues for another player who stores it. It is spent when it heals and starts charging again, so it cannot be pocketed only at the last second of an extraction.
- Runtime hygiene pass: the duplicate-extraction memory is cleared when a level ends; debug lines are guarded.

### Added
- Healing Chalice: when an extraction point completes, the player carrying the Goblet and the living players within `RadiusMeters` (default 10 m) are healed for `HealPercentOfMaxHealth` (default 30%) of their max health; the Goblet wears 20% per extraction in which it healed someone.
- Settings `HealPercentOfMaxHealth` (0 to 100) and `RadiusMeters` (1 to 50).
- `Debug.DebugMode` (off by default): puts the Goblet on the floor at the start of each level and writes detailed logs.
