# QA battery: Healing Chalice (CharmGoblet)

Deploy: Pockets, CharmCore and CharmGoblet are in the r2modman `Debug` profile (compare hashes with the builds). Restart the game first.

## Quick start with debug mode
Set `DebugMode = true` in the `Debug` section of `BepInEx/config/com.osmar.CharmGoblet.cfg` (off by default). When a level starts, a Goblet is placed on the floor about 2.5 m in front of you; the log then also shows the detailed lines. Any level works. To get another one at any moment, set `SpawnNow = true` in the same section while a level is running: the valuable is placed near you and the switch turns itself back off (the file is re-read about once a second).

## Single player
- [ ] Log shows `Charm registered: Goblet - Healing Chalice` and `CharmGoblet 1.0.0 loaded!`.
- [ ] Store the valuable in a slot: the slot shows its picture and value.
- [ ] With the Goblet in a slot the slot shows a percentage that rises over time (a veil lifts as it charges); about four minutes (`ChargeSeconds`, set it to 20 to test quickly) to reach 100%.
- [ ] Get hurt to about half health, wait for a full charge, then complete an extraction point: you recover all of your health, with the heal flash. The Goblet loses 20% of its original value and its charge goes back to 0. Log (DebugMode): `Healing Chalice: ... charge 100%, healed 1 player(s)`.
- [ ] Same, but complete the extraction at about 50% charge: you recover about half of your max health.
- [ ] Store the Goblet only just before the extraction completes: nothing (or almost nothing) is healed and it is not worn.
- [ ] After a heal the slot shows the charge starting again from 0%.
- [ ] Take the Goblet out at about 50%: the slot is free, and when you store it again it continues from about 50% (it did not charge while outside). Let another player (or a second slot) store it: it continues from the charge it had.
- [ ] Complete an extraction at full health: no heal, no wear, and the charge is kept.
- [ ] A tax-return extraction (surplus above the goal, not the last point) heals exactly once, not twice.
- [ ] Goblet carried in hand or lying on the floor: no heal.
- [ ] Take the valuable out: the power stops.
- [ ] Use it until the valuable breaks: the slot empties, the power stops, no errors in the log.

## Settings
- [ ] `WearPercent = 0`: heals, no wear.
- [ ] `ChargeSeconds = 5`: charges almost at once; `1800`: very slow. `HealPercentOfMaxHealth = 50`: a full charge heals half. `0`: no heal, no wear.
- [ ] `RadiusMeters = 1`: only the carrier (and anyone within 1 m) is healed. `50`: the whole group.
- [ ] Charm `Enabled = false`: no effect and no wear. CharmCore `Enabled = false`: nothing works.

## Multiplayer (host and client)
- [ ] The charge percentage shown to a client carrying the Goblet is consistent with the host's (the host decides the heal from its own tracking of when the Goblet was stored); after a heal the client's slot restarts from 0% too.
- [ ] Host carries the Goblet: a hurt client within 10 m is healed (health updates on both screens), a hurt client far away is not.
- [ ] Client carries the Goblet: the client and nearby hurt players (host included) are healed; the Goblet value drops on both machines.
- [ ] Dead players are not revived or healed.
- [ ] Two carriers near one hurt ally: healed twice (stacking), each Goblet worn; if the first heal fills the ally, the second Goblet is not worn.
- [ ] Host settings override the client's.
- [ ] Client leaves and rejoins: no stuck veil.

Log patterns: `Charm registered`, `threw`, `Exception`.
