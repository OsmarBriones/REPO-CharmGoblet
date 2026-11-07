using CharmGoblet.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CharmGoblet.Tests;

[TestClass]
public class ChaliceMathTests
{
	[DataTestMethod]
	[DataRow(100, 30f, 30)]
	[DataRow(100, 100f, 100)]
	[DataRow(150, 30f, 45)]
	[DataRow(50, 25f, 13)]
	[DataRow(10, 5f, 1)]
	[DataRow(100, 0.1f, 1)]
	public void HealAmount_RoundsAndNeverDropsBelowOne(int maxHealth, float percent, int expected)
	{
		Assert.AreEqual(expected, ChaliceMath.HealAmount(maxHealth, percent));
	}

	[DataTestMethod]
	[DataRow(100, 0f)]
	[DataRow(100, -5f)]
	[DataRow(0, 30f)]
	[DataRow(-10, 30f)]
	public void HealAmount_IsZeroWhenThereIsNothingToHeal(int maxHealth, float percent)
	{
		Assert.AreEqual(0, ChaliceMath.HealAmount(maxHealth, percent));
	}

	[TestMethod]
	public void IsWithinRange_IncludesTheBoundaryAndUsesAllThreeAxes()
	{
		Assert.IsTrue(ChaliceMath.IsWithinRange(0, 0, 0, 10, 0, 0, 10f));
		Assert.IsFalse(ChaliceMath.IsWithinRange(0, 0, 0, 10.01f, 0, 0, 10f));
		Assert.IsTrue(ChaliceMath.IsWithinRange(0, 0, 0, 6, 8, 0, 10f));
		Assert.IsFalse(ChaliceMath.IsWithinRange(0, 0, 0, 6, 8, 1, 10f));
		Assert.IsTrue(ChaliceMath.IsWithinRange(1, 2, 3, 1, 2, 3, 1f));
		Assert.IsFalse(ChaliceMath.IsWithinRange(0, 0, 0, 0, 0, 0, -1f));
	}

	[DataTestMethod]
	[DataRow(50, 100, 0, true)]
	[DataRow(100, 100, 0, false)]
	[DataRow(0, 100, 0, false)]
	[DataRow(50, 100, 50, false)]
	[DataRow(50, 100, 49, true)]
	[DataRow(50, 100, -5, true)]
	public void NeedsHealing_ConsidersHealthAlreadySent(int health, int maxHealth, int pending, bool expected)
	{
		Assert.AreEqual(expected, ChaliceMath.NeedsHealing(health, maxHealth, pending));
	}

	[TestMethod]
	public void IsDuplicateCompletion_OnlySameCountInsideTheWindow()
	{
		Assert.IsTrue(ChaliceMath.IsDuplicateCompletion(1, 10f, 1, 10.5f, 2f));
		Assert.IsFalse(ChaliceMath.IsDuplicateCompletion(1, 10f, 2, 10.5f, 2f));
		Assert.IsFalse(ChaliceMath.IsDuplicateCompletion(1, 10f, 1, 12.5f, 2f));
		Assert.IsFalse(ChaliceMath.IsDuplicateCompletion(-1, -100f, 1, 0f, 2f));
	}
}
