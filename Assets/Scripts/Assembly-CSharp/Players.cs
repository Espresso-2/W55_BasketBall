using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Players : MonoBehaviour
{
	public static int SIZE;

	public static int SPEED;

	public static int JUMP;

	public static int SHOOTING;

	public static int DEFENSE;

	private List<Player> starters;

	private List<Player> backups;

	private List<Player> startersFemale;

	private List<Player> backupsFemale;

	private SessionVars sessionVars;

	static Players()
	{
		SPEED = 1;
		JUMP = 2;
		SHOOTING = 3;
		DEFENSE = 4;
	}

	public virtual List<Player> GetStarters(bool female)
	{
		if (female)
		{
			return startersFemale;
		}
		return starters;
	}

	public virtual List<Player> GetBackups(bool female)
	{
		if (female)
		{
			return backupsFemale;
		}
		return backups;
	}

	public virtual Player GetStarter(bool female, int num)
	{
		if (num < 0 || num >= starters.Count)
		{
			return starters[1];
		}
		if (female)
		{
			return startersFemale[num];
		}
		return starters[num];
	}

	public virtual Player GetBackup(bool female, int num)
	{
		if (num < 0 || num >= backups.Count)
		{
			return backups[1];
		}
		if (female)
		{
			return backupsFemale[num];
		}
		return backups[num];
	}

	public virtual Player GetPlayerByPlayerPrefNum(int playerPrefNum)
	{
		bool female = false;
		if (playerPrefNum >= 3000)
		{
			female = true;
			return GetBackup(female, playerPrefNum - 3000);
		}
		if (playerPrefNum >= 2000)
		{
			female = true;
			return GetStarter(female, playerPrefNum - 2000);
		}
		if (playerPrefNum >= 1000)
		{
			return GetBackup(female, playerPrefNum - 1000);
		}
		return GetStarter(female, playerPrefNum);
	}

	public static int GetActiveStarterNum(bool female)
	{
		if (female)
		{
			return PlayerPrefs.GetInt("PLAYER_STARTER_FEMALE");
		}
		return PlayerPrefs.GetInt("PLAYER_STARTER");
	}

	public static void SetActiveStarterNum(bool female, int num)
	{
		if (female)
		{
			PlayerPrefsHelper.SetInt("PLAYER_STARTER_FEMALE", num, true);
		}
		else
		{
			PlayerPrefsHelper.SetInt("PLAYER_STARTER", num, true);
		}
	}

	public static int GetActiveBackupNum(bool female)
	{
		if (female)
		{
			return PlayerPrefs.GetInt("PLAYER_BACKUP_ACTIVE_FEMALE");
		}
		return PlayerPrefs.GetInt("PLAYER_BACKUP_ACTIVE");
	}

	public static void SetActiveBackupNum(bool female, int num)
	{
		if (female)
		{
			PlayerPrefsHelper.SetInt("PLAYER_BACKUP_ACTIVE_FEMALE", num, true);
		}
		else
		{
			PlayerPrefsHelper.SetInt("PLAYER_BACKUP_ACTIVE", num, true);
		}
	}

	public static void RecieveStarterFromDeal(bool female, int num)
	{
		PlayerPrefsHelper.SetInt(Player.PLAYER_OWNED_KEY + num, 1, true);
		SetActiveStarterNum(female, num);
	}

	public static bool IsStarterPurchased(bool female, int num)
	{
		return PlayerPrefs.GetInt(Player.PLAYER_OWNED_KEY + num) == 1;
	}

	public static int GetUpgradeLength(int upgradeLevel)
	{
		int num = 0;
		switch (upgradeLevel)
		{
		case 0:
			num = 5;
			break;
		case 1:
			num = 60;
			break;
		case 2:
			num = 120;
			break;
		case 3:
			num = 240;
			break;
		case 4:
			num = 300;
			break;
		}
		int num2 = UnityEngine.Random.Range(0, 6);
		int @int = PlayerPrefs.GetInt("INCREASE_UPGRADE_TIME");
		int num3 = 0;
		switch (@int)
		{
		case 1:
			num3 = 60;
			break;
		case 2:
			num3 = 300;
			break;
		case 3:
			num3 = 600;
			break;
		case 4:
			num3 = 1800;
			break;
		case 5:
			num3 = 3000;
			break;
		}
		return num + num3 - 2;
	}

	public virtual void InstantiatePlayers()
	{
		sessionVars = SessionVars.GetInstance();
		starters = new List<Player>();
		backups = new List<Player>();
		startersFemale = new List<Player>();
		backupsFemale = new List<Player>();
		int @int = PlayerPrefs.GetInt("PLAYER_PRICING");
		if (@int == 0 || @int == 1 || @int == 2)
		{
			float num = 1f;
			switch (@int)
			{
			case 1:
				num = 0.5f;
				break;
			case 2:
				num = 1.25f;
				break;
			}
			AddPlayer(false, false, "TONY NOEL", 0, -299, -299, 6f, 10f, 10f, 10f, 18f);
			AddPlayer(false, false, "AJ SPINNER", (int)(2500f * num), 35, 10, 15f, 15f, 10f, 10f, 17f);
			AddPlayer(false, false, "BILLY RAE", (int)(16500f * num), -36000, 20, 13f, 20f, 13f, 14f, 19f);
			AddPlayer(false, false, "SCOTT SIMS", (int)(8000f * num), 85, 30, 15f, 30f, 18f, 30f, 15f);
			AddPlayer(false, false, "AK BYRON", (int)(37500f * num), -79800, 50, 28f, 24f, 21f, 20f, 32f);
			AddPlayer(false, false, "JOE JAMES", (int)(8000f * num), 199, 100, 31f, 27f, 29f, 25f, 38f);
			AddPlayer(false, false, "KYLE CLAY", (int)(62500f * num), -270000, 200, 29f, 45f, 20f, 50f, 30f);
			AddPlayer(false, false, "PJ DUNKIN", (int)(41500f * num), 350, 999, 45f, 35f, 45f, 40f, 50f);
			AddPlayer(false, true, "JOE HOPKINS", 0, -199, -199, 2f, 14f, 6f, 6f, 14f);
			AddPlayer(false, true, "BRIAN BUCKET", (int)(1500f * num), 29, 10, 8f, 19f, 6f, 6f, 13f);
			AddPlayer(false, true, "TOM MERIT", (int)(12500f * num), -33000, 20, 7f, 24f, 9f, 10f, 15f);
			AddPlayer(false, true, "BROCK FLAIR", (int)(10000f * num), 59, 30, 11f, 25f, 14f, 11f, 26f);
			AddPlayer(false, true, "ALEX ARCHER", (int)(25000f * num), -66000, 40, 17f, 28f, 17f, 16f, 28f);
			AddPlayer(false, true, "MARK TRAP", (int)(20500f * num), 95, 100, 24f, 29f, 31f, 11f, 38f);
			AddPlayer(false, true, "JOEL JETT", (int)(33000f * num), -186000, 180, 20f, 49f, 16f, 46f, 26f);
			AddPlayer(false, true, "BILL HAWK", (int)(25000f * num), 249, 995, 32f, 41f, 41f, 37f, 46f);
			AddPlayer(true, false, "SARA SILK", 0, 9, 4, 5f, 10f, 10f, 10f, 18f);
			AddPlayer(true, false, "ANN FLYER", 0, -29400, 10, 14f, 15f, 12f, 11f, 17f);
			AddPlayer(true, false, "JESS BASS", (int)(1500f * num), 45, 20, 12f, 20f, 14f, 15f, 19f);
			AddPlayer(true, false, "ALLY ONEILL", (int)(8000f * num), -48000, 30, 18f, 21f, 20f, 16f, 30f);
			AddPlayer(true, false, "KELLY KIZER", (int)(10000f * num), 99, 50, 27f, 24f, 22f, 22f, 32f);
			AddPlayer(true, false, "LIA LITTLE", (int)(25000f * num), -114000, 100, 31f, 25f, 37f, 16f, 42f);
			AddPlayer(true, false, "TORI TAYLOR", (int)(33000f * num), 189, 200, 28f, 45f, 22f, 55f, 30f);
			AddPlayer(true, false, "ARIEL APPLE", (int)(50000f * num), -299400, 999, 43f, 35f, 46f, 47f, 50f);
			AddPlayer(true, true, "TINA CLARK", 0, -199, -199, 2f, 16f, 6f, 6f, 14f);
			AddPlayer(true, true, "KRIS GIA", (int)(2500f * num), 19, 10, 7f, 20f, 9f, 8f, 15f);
			AddPlayer(true, true, "CARLA HOOPER", (int)(8000f * num), -30000, 20, 6f, 24f, 10f, 12f, 16f);
			AddPlayer(true, true, "MONICA JONES", (int)(8000f * num), 29, 30, 10f, 26f, 15f, 15f, 25f);
			AddPlayer(true, true, "SANSA ARCHER", (int)(25000f * num), -66000, 40, 16f, 28f, 19f, 17f, 25f);
			AddPlayer(true, true, "ARIEL ALSTEN", (int)(20500f * num), 89, 100, 23f, 32f, 32f, 12f, 36f);
			AddPlayer(true, true, "KAYLA QUINN", (int)(50000f * num), -189000, 180, 19f, 51f, 18f, 49f, 26f);
			AddPlayer(true, true, "PORSHA PAYNE", (int)(50000f * num), 199, 995, 26f, 45f, 42f, 39f, 46f);
		}
		else
		{
			AddPlayer(false, false, "TONY NOEL", 0, -299, -299, 6f, 10f, 10f, 10f, 18f);
			AddPlayer(false, false, "AJ SPINNER", 0, 35, 10, 15f, 15f, 10f, 10f, 17f);
			AddPlayer(false, false, "BILLY RAE", 0, 49, 20, 13f, 20f, 13f, 14f, 19f);
			AddPlayer(false, false, "SCOTT SIMS", 0, 85, 30, 15f, 30f, 18f, 30f, 15f);
			AddPlayer(false, false, "AK BYRON", 2500, 125, 50, 28f, 24f, 21f, 20f, 32f);
			AddPlayer(false, false, "JOE JAMES", 0, 199, 100, 31f, 27f, 29f, 25f, 38f);
			AddPlayer(false, false, "KYLE CLAY", 7500, 399, 200, 29f, 45f, 20f, 50f, 30f);
			AddPlayer(false, false, "PJ DUNKIN", 10000, 999, 999, 45f, 35f, 45f, 40f, 50f);
			AddPlayer(false, true, "JOE HOPKINS", 0, -199, -199, 2f, 14f, 6f, 6f, 14f);
			AddPlayer(false, true, "BRIAN BUCKET", 0, 29, 10, 8f, 19f, 6f, 6f, 13f);
			AddPlayer(false, true, "TOM MERIT", 0, 45, 20, 7f, 24f, 9f, 10f, 15f);
			AddPlayer(false, true, "BROCK FLAIR", 0, 59, 30, 11f, 25f, 14f, 11f, 26f);
			AddPlayer(false, true, "ALEX ARCHER", 2500, 99, 40, 17f, 28f, 17f, 16f, 28f);
			AddPlayer(false, true, "MARK TRAP", 4000, 199, 100, 24f, 29f, 31f, 11f, 38f);
			AddPlayer(false, true, "JOEL JETT", 7500, 299, 180, 20f, 49f, 16f, 46f, 26f);
			AddPlayer(false, true, "BILL HAWK", 10000, 749, 995, 32f, 41f, 41f, 37f, 46f);
			AddPlayer(true, false, "SARA SILK", 0, 2, 4, 5f, 10f, 10f, 10f, 18f);
			AddPlayer(true, false, "ANN FLYER", 0, 34, 10, 14f, 15f, 12f, 11f, 17f);
			AddPlayer(true, false, "JESS BASS", 0, 45, 20, 12f, 20f, 14f, 15f, 19f);
			AddPlayer(true, false, "ALLY ONEILL", 0, 69, 30, 18f, 21f, 20f, 16f, 30f);
			AddPlayer(true, false, "KELLY KIZER", 2500, 129, 50, 27f, 24f, 22f, 22f, 32f);
			AddPlayer(true, false, "LIA LITTLE", 4000, 179, 100, 31f, 25f, 37f, 16f, 42f);
			AddPlayer(true, false, "TORI TAYLOR", 7500, 349, 200, 28f, 45f, 22f, 55f, 30f);
			AddPlayer(true, false, "ARIEL APPLE", 10000, 949, 999, 43f, 35f, 46f, 47f, 50f);
			AddPlayer(true, true, "TINA CLARK", 0, -199, -199, 2f, 16f, 6f, 6f, 14f);
			AddPlayer(true, true, "KRIS GIA", 0, 30, 10, 7f, 20f, 9f, 8f, 15f);
			AddPlayer(true, true, "CARLA HOOPER", 0, 40, 20, 6f, 24f, 10f, 12f, 16f);
			AddPlayer(true, true, "MONICA JONES", 0, 64, 30, 10f, 26f, 15f, 15f, 25f);
			AddPlayer(true, true, "SANSA ARCHER", 2500, 97, 40, 16f, 28f, 19f, 17f, 25f);
			AddPlayer(true, true, "ARIEL ALSTEN", 4000, 185, 100, 23f, 32f, 32f, 12f, 36f);
			AddPlayer(true, true, "KAYLA QUINN", 7500, 315, 180, 19f, 51f, 18f, 49f, 26f);
			AddPlayer(true, true, "PORSHA PAYNE", 10000, 700, 995, 26f, 45f, 42f, 39f, 46f);
		}
	}

	private void AddPlayer(bool isFemale, bool isBackup, string fullNameKey, int reqXP, int goldPrice, int goldSellReward, float size, float speed, float jump, float shooting, float defense)
	{
		Player player = new Player();
		player.sessionVars = sessionVars;
		player.isBackup = isBackup;
		player.isFemale = isFemale;
		player.isP2 = false;
		player.fullNameKey = fullNameKey;
		player.fullName = ((!(PlayerPrefs.GetString(fullNameKey) != string.Empty)) ? fullNameKey : PlayerPrefs.GetString(fullNameKey));
		player.reqXP = reqXP;
		player.goldPrice = goldPrice;
		player.goldSellReward = goldSellReward;
		player.size = size;
		player.speed = speed;
		player.jump = jump;
		player.shooting = shooting;
		player.defense = defense;
		if (isBackup)
		{
			if (isFemale)
			{
				player.num = backupsFemale.Count;
				player.SetPlayerPrefNum(player.num + 3000);
				backupsFemale.Add(player);
			}
			else
			{
				player.num = backups.Count;
				player.SetPlayerPrefNum(player.num + 1000);
				backups.Add(player);
			}
		}
		else if (isFemale)
		{
			player.num = startersFemale.Count;
			player.SetPlayerPrefNum(player.num + 2000);
			startersFemale.Add(player);
		}
		else
		{
			player.num = starters.Count;
			player.SetPlayerPrefNum(player.num);
			starters.Add(player);
		}
	}
}
