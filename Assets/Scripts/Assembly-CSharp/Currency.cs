using System;
using UnityEngine;

[Serializable]
public class Currency : MonoBehaviour
{
	public static int MIN_XP_AFTER_FAL;

	static Currency()
	{
		MIN_XP_AFTER_FAL = 250;
	}

	public virtual void Start()
	{
	}

	public static int GetCurrentCash()
	{
		return PlayerPrefs.GetInt("CASH");
	}

	public static int AddCash(int amount)
	{
		int num = GetCurrentCash() + amount;
		PlayerPrefsHelper.SetInt("CASH", num, true);
		/*RevenueTracker.AddedCash(amount, method);*/
		return num;
	}

	public static bool SpendCash(int amount)
	{
		int currentCash = GetCurrentCash();
		if (currentCash >= amount)
		{
			PlayerPrefsHelper.SetInt("CASH", currentCash - amount, true);
			return true;
		}
		return false;
	}

	public static int GetCurrentGold()
	{
		return PlayerPrefs.GetInt("GOLD");
	}

	public static int AddGold(int amount)
	{
		int num = GetCurrentGold() + amount;
		PlayerPrefsHelper.SetInt("GOLD", num, true);
		/*ConversionTracker.AddedGold(amount, method);
		RevenueTracker.AddedGold(amount, method);*/
		return num;
	}

	public static bool SpendGold(int amount, string itemType)
	{
		int currentGold = GetCurrentGold();
		if (currentGold >= amount)
		{
			PlayerPrefsHelper.SetInt("GOLD", currentGold - amount, true);
			/*ConversionTracker.SpentGold(amount, itemType);*/
			if (currentGold >= 20000)
			{
				LogFraudulentUser("SpendGold");
			}
			return true;
		}
		return false;
	}

	public static int GetNumPrizeBalls()
	{
		return PlayerPrefs.GetInt("PRIZE_BALLS");
	}

	public static int AddPrizeBalls(int amount, string method)
	{
		int num = GetNumPrizeBalls() + amount;
		PlayerPrefsHelper.SetInt("PRIZE_BALLS", num, true);
		/*RevenueTracker.AddedPrizeBalls(amount, method);*/
		
		return num;
	}

	public static bool UsePrizeBall()
	{
		int numPrizeBalls = GetNumPrizeBalls();
		if (numPrizeBalls >= 1)
		{
			PlayerPrefsHelper.SetInt("PRIZE_BALLS", numPrizeBalls - 1, true);
			return true;
		}
		return false;
	}

	public static int GetCurrentXp()
	{
		return PlayerPrefs.GetInt("XP");
	}

	public static int GetCurrentXpLevel()
	{
		return GetXpLevelForXp(GetCurrentXp());
	}

	public static int GetXpLevelForXp(int xp)
	{
		return (int)Mathf.Round(xp / 500) + 1;
	}

	public static int GetCurrentXpLevelGoldReward()
	{
		return 0;
	}

	public static int GetCurrentXpLevelCashReward()
	{
		return 0;
	}

	public static int GetCurrentXpLevelPrizeBallsReward()
	{
		return 1;
	}

	public static float GetCurrentXpLevelProgress()
	{
		int currentXp = GetCurrentXp();
		Debug.Log("xp: " + currentXp);
		return (float)(currentXp % 500) / 500f;
	}

	public static int AddXp(int amount)
	{
		int num = GetCurrentXp() + amount;
		PlayerPrefsHelper.SetInt("XP", num, true);
		return num;
	}

	public static bool SpendXp(int amount)
	{
		int currentXp = GetCurrentXp();
		if (currentXp >= amount)
		{
			PlayerPrefsHelper.SetInt("XP", currentXp - amount, true);
			return true;
		}
		return false;
	}

	public static int GetCashGoldPrice(int cash, int maxGoldPrice)
	{
		int num = (int)Mathf.Round((float)cash / 400f);
		if (num == 0)
		{
			num = 1;
		}
		if (maxGoldPrice > 0 && num > maxGoldPrice)
		{
			num = maxGoldPrice;
		}
		return num;
	}

	public static void SetStartingAmounts()
	{
		AddXp(225);
		AddCash(500);
		AddGold(6);
	}

	public static void LogFraudulentUser(string detectedBy)
	{
		if (PlayerPrefs.GetInt("IS_FRAUDULENT_USER") == 0)
		{
			PlayerPrefsHelper.SetInt("IS_FRAUDULENT_USER", 1, true);
			//FlurryAnalytics.Instance().LogEvent("IS_FRAUDULENT_USER", new string[1] { "detectedBy:" + detectedBy + string.Empty }, false);
			//PlayFabManager.Instance().UpdateDisplayName(PlayerPrefs.GetString("TEAM_NAME") + " FR1");
		}
	}
}
