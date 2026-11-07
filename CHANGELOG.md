# Changelog

## [Unreleased]

### Added
- Healing Chalice: when an extraction point completes, the player carrying the Goblet and the living players within `RadiusMeters` (default 10 m) are healed for `HealPercentOfMaxHealth` (default 30%) of their max health; the Goblet wears 20% per extraction in which it healed someone.
- Settings `HealPercentOfMaxHealth` (0 to 100) and `RadiusMeters` (1 to 50).
- `Debug.DebugMode` (off by default): puts the Goblet on the floor at the start of each level and writes detailed logs.
