using System;
using UnityEngine;

[Serializable]
public class Supplies : MonoBehaviour
{
	public static int OXYGEN;

	public static int GRIP;

	public static int CHALK;

	public static int PROTEIN;

	public static int DRINK;

	public static string[] NAMES_FOR_LOGGING;

	public static string COLLECTED_FREE_KEY;

	static Supplies()
	{
		GRIP = 1;
		CHALK = 2;
		PROTEIN = 3;
		DRINK = 4;
		NAMES_FOR_LOGGING = new string[5] { "oxygen", "grip", "chalk", "protein", "drink" };
		COLLECTED_FREE_KEY = "RECEIVED_SAMPLE_";
	}

	public virtual void Start()
	{
		/*FlurryAnalytics.Instance().LogEvent("SCREEN_STORE", new string[4]
		{
			"num_wins:" + Stats.GetNumWins() + string.Empty,
			"num_losses:" + Stats.GetNumLosses() + string.Empty,
			"current_tour:" + Tournaments.GetCurrentTournamentNum() + string.Empty,
			"sessions:" + Stats.GetNumSessions() + string.Empty
		}, false);*/
	}

	public static int GetItemAmount(int type)
	{
		return PlayerPrefs.GetInt("ITEM_" + type);
	}

	public static int GetSupplyByPowerupNum(int num)
	{
		int result = 0;
		switch (num)
		{
		case 0:
			result = GRIP;
			break;
		case 1:
			result = CHALK;
			break;
		case 2:
			result = PROTEIN;
			break;
		}
		return result;
	}

	public static int AddItem(int type, int amount)
	{
		int num = GetItemAmount(type) + amount;
		PlayerPrefsHelper.SetInt("ITEM_" + type, num, true);
		/*FlurryAnalytics.Instance().LogEvent("add_item", new string[6]
		{
			"type:" + type + string.Empty,
			"amount:" + amount + string.Empty,
			"sessions:" + Stats.GetNumSessions() + string.Empty,
			"wins:" + Stats.GetNumWins() + string.Empty,
			"losses:" + Stats.GetNumLosses() + string.Empty,
			"currentTournament:" + Tournaments.GetCurrentTournamentNum() + string.Empty
		}, false);*/
		return num;
	}

	public static bool UseItem(int type, int amount)
	{
		int itemAmount = GetItemAmount(type);
		if (itemAmount >= amount)
		{
			PlayerPrefsHelper.SetInt("ITEM_" + type, itemAmount - amount, true);
			return true;
		}
		return false;
	}

	public static void SetStartingAmounts()
	{
		AddItem(GRIP, 31);
		AddItem(CHALK, 20);
		AddItem(PROTEIN, 43);
	}

	public static void SetStartingAmountsLow()
	{
		AddItem(GRIP, 10);
		AddItem(CHALK, 8);
		AddItem(PROTEIN, 12);
	}

	public static void SetStartingAmountsVeryLow()
	{
		AddItem(GRIP, 6);
		AddItem(CHALK, 4);
		AddItem(PROTEIN, 7);
	}
}
