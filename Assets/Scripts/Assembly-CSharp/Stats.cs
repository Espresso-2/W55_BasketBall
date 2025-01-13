using System;
using UnityEngine;

[Serializable]
public class Stats : MonoBehaviour
{
	public static int numShots;

	public static int numMakes;

	public static int num3PtShots;

	public static int num3PtMakes;

	public static int numPoints;

	public static int numRebounds;

	public static int numBlocks;

	public static int numSteals;

	public static int numSeconds;

	public static bool forfeited;

	public static int p1Score;

	public static int p2Score;

	public virtual void Start()
	{
	}

	public static void NewGame()
	{
		numShots = 0;
		numMakes = 0;
		num3PtShots = 0;
		num3PtMakes = 0;
		numPoints = 0;
		numRebounds = 0;
		numBlocks = 0;
		numSteals = 0;
		numSeconds = 0;
	}

	public static void SaveGame(bool won)
	{
		if (won)
		{
			SetNumWins(GetNumWins() + 1);
			AddGameToWinStreak();
		}
		else
		{
			SetNumLosses(GetNumLosses() + 1);
			ResetWinStreak();
		}
		SetNumShots(GetNumShots() + numShots);
		SetNumMakes(GetNumMakes() + numMakes);
		SetNum3PtShots(GetNum3PtShots() + num3PtShots);
		SetNum3PtMakes(GetNum3PtMakes() + num3PtMakes);
		SetNumPoints(GetNumPoints() + numPoints);
		SetNumRebounds(GetNumRebounds() + numRebounds);
		SetNumBlocks(GetNumBlocks() + numBlocks);
		SetNumSteals(GetNumSteals() + numSteals);
		SetNumSeconds(GetNumSeconds() + numSeconds);
	}

	public static void DebugPrintStats()
	{
		Debug.Log("GetNumWins: " + GetNumWins());
		Debug.Log("GetNumLosses: " + GetNumLosses());
		Debug.Log("GetNumShots: " + GetNumShots());
		Debug.Log("GetNumMakes: " + GetNumMakes());
		Debug.Log("GetNum3PtMakes: " + GetNum3PtMakes());
		Debug.Log("GetNum3PtShots: " + GetNum3PtShots());
		Debug.Log("GetNumRebounds: " + GetNumRebounds());
		Debug.Log("GetNumBlocks: " + GetNumBlocks());
		Debug.Log("GetNumSteals: " + GetNumSteals());
	}

	public static int GetNumSessions()
	{
		return PlayerPrefs.GetInt("NUM_SESSIONS");
	}

	public static void SetNumSessions(int num)
	{
		PlayerPrefsHelper.SetInt("NUM_SESSIONS", num, true);
	}

	public static int GetNumWins()
	{
		return PlayerPrefs.GetInt("NUM_WINS");
	}

	public static void SetNumWins(int num)
	{
		PlayerPrefsHelper.SetInt("NUM_WINS", num, true);
	}

	public static int GetNumWinsMilestone()
	{
		int result = 0;
		int numWins = GetNumWins();
		if (numWins >= 1000)
		{
			result = 1000;
		}
		else if (numWins >= 200)
		{
			result = 200;
		}
		else if (numWins >= 100)
		{
			result = 100;
		}
		else if (numWins >= 50)
		{
			result = 50;
		}
		else if (numWins >= 10)
		{
			result = 10;
		}
		else if (numWins >= 5)
		{
			result = 5;
		}
		else if (numWins >= 1)
		{
			result = 1;
		}
		return result;
	}

	public static int GetNumLosses()
	{
		return PlayerPrefs.GetInt("NUM_LOSSES");
	}

	private static void SetNumLosses(int num)
	{
		PlayerPrefsHelper.SetInt("NUM_LOSSES", num, true);
	}

	public static int GetWinStreak()
	{
		return PlayerPrefs.GetInt("WIN_STREAK");
	}

	public static int GetBestWinStreak()
	{
		return PlayerPrefs.GetInt("WIN_STREAK_BEST");
	}

	private static void ResetWinStreak()
	{
		PlayerPrefsHelper.SetInt("WIN_STREAK", 0, true);
	}

	private static void AddGameToWinStreak()
	{
		int num = GetWinStreak() + 1;
		PlayerPrefsHelper.SetInt("WIN_STREAK", num, true);
		if (num > GetBestWinStreak())
		{
			PlayerPrefsHelper.SetInt("WIN_STREAK_BEST", num, true);
		}
	}

	public static int GetNumShots()
	{
		return PlayerPrefs.GetInt("NUM_SHOTS");
	}

	private static void SetNumShots(int num)
	{
		PlayerPrefsHelper.SetInt("NUM_SHOTS", num, true);
	}

	public static int GetNumMakes()
	{
		return PlayerPrefs.GetInt("NUM_MAKES");
	}

	private static void SetNumMakes(int num)
	{
		PlayerPrefsHelper.SetInt("NUM_MAKES", num, true);
	}

	public static int GetNum3PtShots()
	{
		return PlayerPrefs.GetInt("NUM_3PTSHOTS");
	}

	private static void SetNum3PtShots(int num)
	{
		PlayerPrefsHelper.SetInt("NUM_3PTSHOTS", num, true);
	}

	public static int GetNum3PtMakes()
	{
		return PlayerPrefs.GetInt("NUM_3PTMAKES");
	}

	private static void SetNum3PtMakes(int num)
	{
		PlayerPrefsHelper.SetInt("NUM_3PTMAKES", num, true);
	}

	public static int GetNumPoints()
	{
		return PlayerPrefs.GetInt("NUM_POINTS");
	}

	private static void SetNumPoints(int num)
	{
		PlayerPrefsHelper.SetInt("NUM_POINTS", num, true);
	}

	public static int GetNumRebounds()
	{
		return PlayerPrefs.GetInt("NUM_REBOUNDS");
	}

	private static void SetNumRebounds(int num)
	{
		PlayerPrefsHelper.SetInt("NUM_REBOUNDS", num, true);
	}

	public static int GetNumBlocks()
	{
		return PlayerPrefs.GetInt("NUM_BLOCKS");
	}

	private static void SetNumBlocks(int num)
	{
		PlayerPrefsHelper.SetInt("NUM_BLOCKS", num, true);
	}

	public static int GetNumSteals()
	{
		return PlayerPrefs.GetInt("NUM_STEALS");
	}

	private static void SetNumSteals(int num)
	{
		PlayerPrefsHelper.SetInt("NUM_STEALS", num, true);
	}

	public static int GetNumSeconds()
	{
		return PlayerPrefs.GetInt("NUM_SECONDS");
	}

	private static void SetNumSeconds(int num)
	{
		PlayerPrefsHelper.SetInt("NUM_SECONDS", num, true);
		PlayerPrefsHelper.SetInt("NUM_GAME_SECONDS_RECORDED", PlayerPrefs.GetInt("NUM_GAME_SECONDS_RECORDED") + 1, true);
	}

	public static void PlayedLiveEvent()
	{
		PlayerPrefsHelper.SetInt("NUM_LIVE_EVENTS", PlayerPrefs.GetInt("NUM_LIVE_EVENTS") + 1, true);
	}
}
