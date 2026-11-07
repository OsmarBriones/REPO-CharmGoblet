using CharmCore.Api;

namespace CharmGoblet
{
	/// <summary>Healing Chalice: when an extraction point completes, you and nearby allies are healed.</summary>
	internal sealed class GobletCharm : Charm
	{
		internal const string PrefabName = "Valuable Manor Goblet";

		internal GobletCharm()
			: base("Goblet", "Goblet - Healing Chalice", defaultWearPercent: 20f, defaultCooldownSeconds: null, PrefabName)
		{
			HealPercentOfMaxHealth = AddSetting("HealPercentOfMaxHealth", 30f, 0f, 100f, "Share of each player's maximum health restored when an extraction point completes (30 = 30%). 0 turns the healing off. The result is rounded to a whole number of health points, never below 1.");
			RadiusMeters = AddSetting("RadiusMeters", 10f, 1f, 50f, "Distance in metres around the Goblet carrier within which other players are healed too. The carrier is always healed.");
		}

		/// <summary>Percent of max health restored per chalice per extraction (0 to 100).</summary>
		internal CharmSetting HealPercentOfMaxHealth { get; }

		/// <summary>Healing radius around the carrier, in metres (1 to 50).</summary>
		internal CharmSetting RadiusMeters { get; }
	}
}
