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
			// Extra host-synced numbers for this power go here, for example:
			// Amount = AddSetting("Amount", 30f, 1f, 100f, "Description for the config file.");
		}
	}
}
