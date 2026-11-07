using HarmonyLib;

namespace CharmGoblet.Patches
{
	/// <summary>
	/// Runs after an extraction point completes. The game calls <c>RoundDirector.ExtractionCompleted</c> on every machine
	/// (the extraction state machine runs everywhere), so only the host (or a singleplayer game) heals.
	/// </summary>
	[HarmonyPatch(typeof(RoundDirector), "ExtractionCompleted")]
	internal static class RoundDirector_ExtractionCompleted_Patch
	{
		[HarmonyPostfix]
		private static void Postfix(RoundDirector __instance)
		{
			if (!SemiFunc.IsMasterClientOrSingleplayer())
			{
				return;
			}

			HealingChalice.OnExtractionCompleted(CharmGobletPlugin.Charm, __instance.extractionPointsCompleted);
		}
	}
}
