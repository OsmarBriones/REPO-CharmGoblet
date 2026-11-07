## User Scenarios

### User Story 1 - Healing Chalice (Priority: P1)
A player stores the Goblet in a pocket and gets this power while it stays there: when an extraction point completes, you and nearby allies are healed.
**Acceptance**: with the valuable pocketed the power works; with it out of the pocket it does not; each extraction in which it healed someone wears the valuable (no cooldown).
Behaviour: on the host, after an extraction point completes, each Goblet carrier and the living players within RadiusMeters of them are healed for HealPercentOfMaxHealth of max health (at least 1 point); wear applies once per carrier, only if somebody was healed.

## Requirements
- FR-001: Registers a charm with CharmCore for the prefab Valuable Manor Goblet.
- FR-002: Settings through CharmCore: Enabled, WearPercent, CooldownSeconds, DebugMode; host-authoritative except DebugMode.
- FR-003: All players install it (Category 2); depends on Pockets and CharmCore.