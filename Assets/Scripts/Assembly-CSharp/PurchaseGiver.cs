using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class PurchaseGiver : MonoBehaviour
{
	public static int[] goldPackAmounts = new int[8] { 100, 200, 480, 750, 1325, 3200, 80, 500 };

	private static int[] goldPackUsd = new int[8] { 5, 10, 20, 30, 50, 100, 2, 6 };

	public static int[] cashPackAmounts = new int[8] { 100000, 100000, 100000, 100000, 100000, 100000, 100000, 100000 };

	public static int[][] dealAmounts = new int[2][]
	{
		new int[5] { 40, 40, 40, 0, 0 },
		new int[5] { 100, 100, 100, 60, 0 }
	};

	public static int[] dealPlayers = new int[2] { 3, 5 };

	private static int[] dealUsd = new int[2] { 10, 20 };

	public static int[] prizeBallsPackAmounts = new int[2] { 15, 5 };

	private static int[] prizeBallsUsd = new int[2] { 5, 2 };

	public virtual void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(this);
	}

	public virtual void AddGoldPackage(int pkgNum)
	{
		int goldAmountForPackage = GetGoldAmountForPackage(pkgNum);
		Currency.AddGold(goldAmountForPackage);
		LogPurchase("GOLD_PACK", goldAmountForPackage, pkgNum, goldPackUsd[pkgNum]);
		if (Currency.GetCurrentGold() >= 10000)
		{
			Currency.LogFraudulentUser("AddGoldPackage");
		}
	}

	public virtual void AddPrizeBallsPackage(int pkgNum)
	{
		int num = prizeBallsPackAmounts[pkgNum];
		Currency.AddPrizeBalls(num, "iap");
		LogPurchase("PRIZEBALLS_PACK", num, pkgNum, prizeBallsUsd[pkgNum]);
	}

	public virtual void AddGoldAndCashPackage(int pkgNum)
	{
		int goldAmountForPackage = GetGoldAmountForPackage(pkgNum);
		Currency.AddGold(goldAmountForPackage);
		Currency.AddCash(cashPackAmounts[pkgNum]);
		LogPurchase("GOLDANDCASH_PACK", goldAmountForPackage, pkgNum, 2);
	}

	public virtual void RecieveDeal(int dealNum)
	{
		int num = dealPlayers[dealNum];
		Players.RecieveStarterFromDeal(false, num);
		for (int i = 0; i < dealAmounts[dealNum].Length; i++)
		{
			int num2 = dealAmounts[dealNum][i];
			if (i == 0)
			{
				Supplies.AddItem(Supplies.GRIP, num2);
			}
			if (i == 1)
			{
				Supplies.AddItem(Supplies.CHALK, num2);
			}
			if (i == 2)
			{
				Supplies.AddItem(Supplies.PROTEIN, num2);
			}
			if (i == 3)
			{
				Supplies.AddItem(Supplies.DRINK, num2);
			}
			if (i == 4)
			{
				Supplies.AddItem(Supplies.OXYGEN, num2);
			}
			Debug.Log("i: " + i + " amt: " + num2);
		}
		PlayerPrefsHelper.SetInt("PURCHASED_DEAL_" + dealNum, 1, true);
		LogPurchase("DEAL", dealNum, dealNum, dealUsd[dealNum]);
	}

	public virtual void RemoveAds()
	{
		if (PlayerPrefs.GetInt("IAP_ADS_REMOVED") != 1)
		{
			PlayerPrefsHelper.SetInt("IAP_ADS_REMOVED", 1, true);
			Currency.AddGold(100);
			LogPurchase("REMOVE_ADS", -1, -1, 2);
		}
	}

	private void LogPurchase(string eventName, int amt, int pkgNum, int usd)
	{
		int @int = PlayerPrefs.GetInt("NUM_PURCHASES");
		@int++;
		PlayerPrefsHelper.SetInt("NUM_PURCHASES", @int, true);
		string currentScreen = GetCurrentScreen();
	
		RecordSpendingInfoInUsd(usd);
	}

	private void RecordSpendingInfoInUsd(int usd)
	{
		int @int = PlayerPrefs.GetInt("IAP_BIGGEST_IN_USD");
		if (usd > @int)
		{
			PlayerPrefsHelper.SetInt("IAP_BIGGEST_IN_USD", usd, true);
		}
		int int2 = PlayerPrefs.GetInt("IAP_TOTAL_IN_USD");
		PlayerPrefsHelper.SetInt("IAP_TOTAL_IN_USD", int2 + usd, true);
		string @string = PlayerPrefs.GetString("IAP_HISTORY_IN_USD");
		PlayerPrefsHelper.SetString("IAP_HISTORY_IN_USD", @string + string.Empty + usd + ", ", true);
		PlayerPrefsHelper.SetInt("IAP_MOST_RECENT_IN_USD", usd, true);
		PlayerPrefsHelper.SetInt("IAP_MOST_RECENT_TIMESTAMP", SessionVars.GetInstance().currentTimestamp, true);
		PlayerPrefsHelper.SetInt("IAP_MOST_RECENT_NUMGAMES", Stats.GetNumWins() + Stats.GetNumLosses(), true);
	}

	private string GetCurrentScreen()
	{
		string text = SceneManager.GetActiveScene().name;
		if (text == "MainUI")
		{
			text = ((TabChanger.currentScreenNum < screenEnum.Settings) ? (text + "_TAB" + TabChanger.currentTabNum) : (text + "_SCREEN" + TabChanger.currentScreenNum));
		}
		return text;
	}

	public static int GetGoldAmountForPackage(int pkgNum)
	{
		int num = goldPackAmounts[pkgNum];
		int @int = PlayerPrefs.GetInt("TAPJOY_BONUS");
		if (@int > 0)
		{
			float num2 = (float)@int / 100f;
			num = (int)((float)num + Mathf.Round((float)num * num2));
		}
		return num;
	}
}
