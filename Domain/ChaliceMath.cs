using System;

namespace CharmGoblet.Domain
{
	/// <summary>Game-free rules of the Healing Chalice: heal size, who is in range and who still needs healing. Kept free of Unity and game types so it is unit tested.</summary>
	internal static class ChaliceMath
	{
		/// <summary>
		/// Health points restored to a player: <paramref name="percent"/> of <paramref name="maxHealth"/>, rounded to the nearest
		/// whole point (halves go up), and at least 1 when the percent and the max health are above 0. Returns 0 otherwise.
		/// </summary>
		internal static int HealAmount(int maxHealth, float percent)
		{
			if (maxHealth <= 0 || percent <= 0f)
			{
				return 0;
			}

			int amount = (int)Math.Round(maxHealth * (double)percent / 100.0, MidpointRounding.AwayFromZero);
			return amount < 1 ? 1 : amount;
		}

		/// <summary>True when two positions are at most <paramref name="radius"/> metres apart (straight-line distance). A negative radius reaches nobody.</summary>
		internal static bool IsWithinRange(float ax, float ay, float az, float bx, float by, float bz, float radius)
		{
			if (radius < 0f)
			{
				return false;
			}

			double dx = ax - bx;
			double dy = ay - by;
			double dz = az - bz;
			return dx * dx + dy * dy + dz * dz <= (double)radius * radius;
		}

		/// <summary>
		/// True when a living player (health above 0) would still have missing health after <paramref name="pendingHeal"/> points
		/// already sent this completion (other chalices heal separately and their effect is not visible yet on the host).
		/// </summary>
		internal static bool NeedsHealing(int health, int maxHealth, int pendingHeal)
		{
			return health > 0 && maxHealth > 0 && health + Math.Max(0, pendingHeal) < maxHealth;
		}

		/// <summary>
		/// True when a completion event repeats the one handled just before: same completed-count and less than <paramref name="windowSeconds"/> apart.
		/// A safety net against the game running the completion hook twice for one extraction.
		/// </summary>
		internal static bool IsDuplicateCompletion(int lastCount, float lastTime, int count, float now, float windowSeconds)
		{
			return lastCount == count && now >= lastTime && now - lastTime < windowSeconds;
		}
	}
}
