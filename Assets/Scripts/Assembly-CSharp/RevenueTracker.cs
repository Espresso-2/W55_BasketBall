using UnityEngine;

public class RevenueTracker : MonoBehaviour
{
	public static void AddedGold(int amount, string method)
	{
		Track("GOLD_REV_" + method, amount);
	}

	public static void AddedCash(int amount, string method)
	{
		Track("CASH_REV_" + method, amount);
	}

	public static void AddedPrizeBalls(int amount, string method)
	{
		Track("PRIZE_BALLS_COLLECTED_" + method, amount);
	}

	public static void AddedStandardBags(int amount, string method)
	{
		Track("STANDARD_BAGS_COLLECTED_" + method, amount);
	}

	public static void OpenedPremiumBag()
	{
		Track("PREMIUM_BAGS_OPENED", 1);
	}

	public static void OpenedDailyBag()
	{
		Track("DAILY_BAGS_OPENED", 1);
	}

	public static void PurchasedBag()
	{
		Track("PURCHSED_BAG", 1);
	}

	private static void Track(string key, int amount)
	{
		int val = PlayerPrefs.GetInt(key) + amount;
		PlayerPrefsHelper.SetInt(key, val, true);
	}
}
