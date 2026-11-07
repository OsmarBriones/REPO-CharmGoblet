using System.Collections.Generic;
using CharmCore.Api;
using CharmGoblet.Domain;
using UnityEngine;

namespace CharmGoblet
{
	/// <summary>
	/// Healing Chalice: when an extraction point completes, every player who carries the Goblet in a pocket heals
	/// themselves and the living players within <c>RadiusMeters</c> by <c>HealPercentOfMaxHealth</c> of max health.
	/// Everything runs on the host (healing another player is only allowed for the master or that player's owner).
	/// </summary>
	internal static class HealingChalice
	{
		/// <summary>A repeated completion event with the same count inside this many seconds is ignored.</summary>
		private const float DuplicateWindowSeconds = 2f;

		private static int lastCount = -1;
		private static float lastTime = -100f;

		/// <summary>Forgets the duplicate-event memory; called when the level ends so nothing carries over to the next level.</summary>
		internal static void Reset()
		{
			lastCount = -1;
			lastTime = -100f;
		}

		/// <summary>Host only. <paramref name="completedCount"/> is the number of extraction points completed so far.</summary>
		internal static void OnExtractionCompleted(GobletCharm charm, int completedCount)
		{
			float now = Time.realtimeSinceStartup;
			if (ChaliceMath.IsDuplicateCompletion(lastCount, lastTime, completedCount, now, DuplicateWindowSeconds))
			{
				if (charm.DebugMode)
				{
					charm.LogDebug($"Healing Chalice: repeated completion event ({completedCount}) ignored");
				}

				return;
			}

			lastCount = completedCount;
			lastTime = now;

			float percent = charm.HealPercentOfMaxHealth.Value;
			float radius = charm.RadiusMeters.Value;
			if (percent <= 0f)
			{
				return;
			}

			// Points already sent this completion per player: the owner's new health is not visible on the host yet,
			// so a second chalice must not count a player that the first one already filled up.
			Dictionary<PlayerAvatar, int> pending = new Dictionary<PlayerAvatar, int>();
			int carriers = 0;
			CharmsApi.ForEachCarrier(charm, (carrier, valuable) =>
			{
				carriers++;
				int healed = HealAround(carrier, percent, radius, pending, charm);
				if (healed > 0)
				{
					charm.WearValuable(valuable, charm.WearPercent);
				}

				if (charm.DebugMode)
				{
					charm.LogDebug($"Healing Chalice: carrier {carrier.name} healed {healed} player(s)");
				}
			});

			if (charm.DebugMode)
			{
				charm.LogDebug($"Healing Chalice: extraction {completedCount} completed, {carriers} carrier(s)");
			}
		}

		private static int HealAround(PlayerAvatar carrier, float percent, float radius, Dictionary<PlayerAvatar, int> pending, GobletCharm charm)
		{
			Vector3 center = carrier.transform.position;
			int healed = 0;
			foreach (PlayerAvatar player in GameDirector.instance.PlayerList.ToArray())
			{
				if (player == null || player.deadSet || player.isDisabled || player.playerHealth == null)
				{
					continue;
				}

				bool isCarrier = player == carrier;
				Vector3 position = player.transform.position;
				if (!isCarrier && !ChaliceMath.IsWithinRange(center.x, center.y, center.z, position.x, position.y, position.z, radius))
				{
					continue;
				}

				PlayerHealth health = player.playerHealth;
				pending.TryGetValue(player, out int already);
				if (!ChaliceMath.NeedsHealing(health.health, health.maxHealth, already))
				{
					continue;
				}

				int amount = ChaliceMath.HealAmount(health.maxHealth, percent);
				if (amount <= 0)
				{
					continue;
				}

				// HealOther heals directly on the owner's machine (or alone); otherwise it sends an RPC the host is allowed to send.
				health.HealOther(amount, true);
				pending[player] = already + amount;
				healed++;
				if (charm.DebugMode)
				{
					charm.LogDebug($"Healing Chalice: {player.name} healed by {amount} (health {health.health}/{health.maxHealth} before this heal)");
				}
			}

			return healed;
		}
	}
}
