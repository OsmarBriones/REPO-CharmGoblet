using CharmCore.Api;

namespace CharmGoblet
{
	/// <summary>Healing Chalice: it charges while it stays in a pocket; when an extraction point completes, you and nearby allies are healed in proportion to the charge.</summary>
	internal sealed class GobletCharm : Charm
	{
		internal const string PrefabName = "Valuable Manor Goblet";

		internal GobletCharm()
			: base("Goblet", "Goblet - Healing Chalice", defaultWearPercent: 20f, defaultCooldownSeconds: null, PrefabName)
		{
			// The chalice charges slowly while it stays in a pocket, so it cannot be pocketed only when the extraction point completes.
			// The default is about the time players take to gather the loot for an extraction point (the game has no fixed duration).
			EnableCharge(240f);
			HealPercentOfMaxHealth = AddSetting("HealPercentOfMaxHealth", 100f, 0f, 100f, "Share of each player's maximum health restored at FULL charge when an extraction point completes (100 = all of it). A partly charged chalice restores that share in proportion to its charge. 0 turns the healing off. The result is rounded to a whole number of health points, never below 1.");
			RadiusMeters = AddSetting("RadiusMeters", 10f, 1f, 50f, "Distance in metres around the Goblet carrier within which other players are healed too. The carrier is always healed.");
		}

		/// <summary>Percent of max health restored per chalice per extraction (0 to 100).</summary>
		internal CharmSetting HealPercentOfMaxHealth { get; }

		/// <summary>Healing radius around the carrier, in metres (1 to 50).</summary>
		internal CharmSetting RadiusMeters { get; }

		/// <summary>Forgets the duplicate-completion memory so nothing of the level survives it.</summary>
		protected override void OnLevelEnded()
		{
			HealingChalice.Reset();
		}
	}
}
