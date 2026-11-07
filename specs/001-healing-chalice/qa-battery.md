# QA battery: Healing Chalice (CharmGoblet)

Deploy: Pockets, CharmCore and CharmGoblet are in the r2modman `Debug` profile (compare hashes with the builds). Restart the game first.

## Quick start with debug mode
Set `DebugMode = true` in the `Debug` section of `BepInEx/config/com.osmar.CharmGoblet.cfg` (off by default). When a level starts, a Goblet is placed on the floor about 2.5 m in front of you; the log then also shows the detailed lines. Any level works.

## Single player
- [ ] Log shows `Charm registered: Goblet - Healing Chalice` and `CharmGoblet 1.0.0 loaded!`.
- [ ] Store the valuable in a slot: the slot shows its picture and value.
- [ ] Get hurt to about half health, then complete an extraction point with the Goblet in a slot: you recover 30% of your max health (30 of 100), with the heal flash. The Goblet loses 20% of its original value. Log (DebugMode): `Healing Chalice: ... healed by 30`.
- [ ] Complete an extraction at full health: no heal, no wear.
- [ ] A tax-return extraction (surplus above the goal, not the last point) heals exactly once, not twice.
- [ ] No cooldown veil appears on the slot.
- [ ] Goblet carried in hand or lying on the floor: no heal.
- [ ] Take the valuable out: the power stops.
- [ ] Use it until the valuable breaks: the slot empties, the power stops, no errors in the log.

## Settings
- [ ] `WearPercent = 0`: heals, no wear.
- [ ] `HealPercentOfMaxHealth = 100`: full heal. `0`: no heal, no wear. `1` with max health 100: heals 1.
- [ ] `RadiusMeters = 1`: only the carrier (and anyone within 1 m) is healed. `50`: the whole group.
- [ ] Charm `Enabled = false`: no effect and no wear. CharmCore `Enabled = false`: nothing works.

## Multiplayer (host and client)
- [ ] Host carries the Goblet: a hurt client within 10 m is healed (health updates on both screens), a hurt client far away is not.
- [ ] Client carries the Goblet: the client and nearby hurt players (host included) are healed; the Goblet value drops on both machines.
- [ ] Dead players are not revived or healed.
- [ ] Two carriers near one hurt ally: healed twice (stacking), each Goblet worn; if the first heal fills the ally, the second Goblet is not worn.
- [ ] Host settings override the client's.
- [ ] Client leaves and rejoins: no stuck veil.

Log patterns: `Charm registered`, `threw`, `Exception`.
