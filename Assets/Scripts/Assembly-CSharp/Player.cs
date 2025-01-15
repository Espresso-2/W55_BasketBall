using System;
using UnityEngine;

[Serializable]
public class Player
{
	public static string PLAYER_OWNED_KEY;

	public static string PLAYER_SOLD_KEY;

	public static string PLAYER_COLOR_SKIN_KEY;

	public static string PLAYER_COLOR_HAIR_KEY;

	public static string PLAYER_COLOR_EYES_KEY;

	public static string PLAYER_COLOR_BROWS_KEY;

	public static string UPGRADE_SPEED_KEY;

	public static string UPGRADE_JUMP_KEY;

	public static string UPGRADE_SHOOTING_KEY;

	public static string UPGRADE_DEFENSE_KEY;

	public static string UPGRADE_TIMESTAMP_KEYTAG;

	public static bool playerNamesMayHaveChanged;

	public int num;

	public int playerPrefNum;

	public bool isBackup;

	public bool isFemale;

	public bool isP2;

	public string fullNameKey;

	public string fullName;

	public int goldPrice;

	public int goldSellReward;

	public int reqXP;

	public float size;

	public float speed;

	public float jump;

	public float shooting;

	public float defense;

	public SessionVars sessionVars;

	public Player()
	{
		fullNameKey = string.Empty;
		fullName = string.Empty;
		size = 10f;
		speed = 10f;
		jump = 10f;
		shooting = 10f;
		defense = 10f;
	}

	static Player()
	{
		PLAYER_OWNED_KEY = "PLAYER_OWNED_";
		PLAYER_SOLD_KEY = "PLAYER_SOLD_";
		PLAYER_COLOR_SKIN_KEY = "PLAYER_COLOR_SKIN_";
		PLAYER_COLOR_HAIR_KEY = "PLAYER_COLOR_HAIR_";
		PLAYER_COLOR_EYES_KEY = "PLAYER_COLOR_EYES_";
		PLAYER_COLOR_BROWS_KEY = "PLAYER_COLOR_BROWS_";
		UPGRADE_SPEED_KEY = "PLAYER_SPEED_";
		UPGRADE_JUMP_KEY = "PLAYER_JUMP_";
		UPGRADE_SHOOTING_KEY = "PLAYER_SHOOTING_";
		UPGRADE_DEFENSE_KEY = "PLAYER_DEFENSE_";
		UPGRADE_TIMESTAMP_KEYTAG = "_UPGRADE_TS";
	}

	public virtual bool IsOwned()
	{
		bool result = false;
		if ((PlayerPrefs.GetInt(PLAYER_OWNED_KEY + playerPrefNum) == 1 || goldPrice == -199) && PlayerPrefs.GetInt(PLAYER_SOLD_KEY + playerPrefNum) == 0)
		{
			result = true;
		}
		return result;
	}

	public virtual int GetUpgradeLevelByNum(int num)
	{
		string upgradeKey = GetUpgradeKey(num);
		int num2 = PlayerPrefs.GetInt(upgradeKey);
		int @int = PlayerPrefs.GetInt(upgradeKey + UPGRADE_TIMESTAMP_KEYTAG);
		int currentTimestamp = sessionVars.currentTimestamp;
		if (@int > 0 && currentTimestamp >= 0 && currentTimestamp > @int + Players.GetUpgradeLength(num2))
		{
			num2++;
			PlayerPrefsHelper.SetInt(upgradeKey, num2, true);
			PlayerPrefsHelper.SetInt(upgradeKey + UPGRADE_TIMESTAMP_KEYTAG, 0, true);
		}
		return num2;
	}

	public virtual bool IsStatBeingUpgraded(int num)
	{
		string upgradeKey = GetUpgradeKey(num);
		int @int = PlayerPrefs.GetInt(upgradeKey + UPGRADE_TIMESTAMP_KEYTAG);
		return @int > 0;
	}

	public virtual int GetStatUpgradeTimeStampByNum(int num)
	{
		string upgradeKey = GetUpgradeKey(num);
		return PlayerPrefs.GetInt(upgradeKey + UPGRADE_TIMESTAMP_KEYTAG);
	}

	public virtual void BuyStatUpgradeByNum(int num, bool instant, bool instantComplete)
	{
		int upgradeLevelByNum = GetUpgradeLevelByNum(num);
		string upgradeKey = GetUpgradeKey(num);
		if (instantComplete || instant)
		{
			PlayerPrefsHelper.SetInt(upgradeKey, upgradeLevelByNum + 1, true);
			PlayerPrefsHelper.SetInt(upgradeKey + UPGRADE_TIMESTAMP_KEYTAG, 0, true);
		}
		else
		{
			int currentTimestamp = sessionVars.currentTimestamp;
			PlayerPrefsHelper.SetInt(upgradeKey + UPGRADE_TIMESTAMP_KEYTAG, currentTimestamp, true);
		}
		if (instantComplete)
		{
			/*FlurryAnalytics.Instance().LogEvent("buy_upgrade_instant_complete", new string[6]
			{
				"playernum:" + playerPrefNum + string.Empty,
				"statnum:" + num + string.Empty,
				"sessions:" + Stats.GetNumSessions() + string.Empty,
				"wins:" + Stats.GetNumWins() + string.Empty,
				"losses:" + Stats.GetNumLosses() + string.Empty,
				"currentTournament:" + Tournaments.GetCurrentTournamentNum() + string.Empty
			}, false);*/
		}
		else
		{
			/*FlurryAnalytics.Instance().LogEvent("buy_upgrade", new string[7]
			{
				"playernum:" + playerPrefNum + string.Empty,
				"statnum:" + num + string.Empty,
				"sessions:" + Stats.GetNumSessions() + string.Empty,
				"wins:" + Stats.GetNumWins() + string.Empty,
				"losses:" + Stats.GetNumLosses() + string.Empty,
				"currentTournament:" + Tournaments.GetCurrentTournamentNum() + string.Empty,
				"currency:unknown"
			}, false);*/
			/*AdMediation.TrackEventInTj("BUY_UPGRADE", playerPrefNum);*/
		}
	}

	private string GetUpgradeKey(int num)
	{
		string result = string.Empty;
		switch (num)
		{
		case 0:
			result = UPGRADE_SPEED_KEY + playerPrefNum;
			break;
		case 1:
			result = UPGRADE_JUMP_KEY + playerPrefNum;
			break;
		case 2:
			result = UPGRADE_SHOOTING_KEY + playerPrefNum;
			break;
		case 3:
			result = UPGRADE_DEFENSE_KEY + playerPrefNum;
			break;
		}
		return result;
	}

	public virtual float GetStatByNum(int num)
	{
		int num2 = 0;
		int num3 = 0;
		if (num == Players.SIZE)
		{
			num2 = (int)size;
		}
		else if (num == Players.SPEED)
		{
			num2 = (int)speed;
			num3 = GetUpgradeLevelByNum(0);
		}
		else if (num == Players.JUMP)
		{
			num2 = (int)jump;
			num3 = GetUpgradeLevelByNum(1);
		}
		else if (num == Players.SHOOTING)
		{
			num2 = (int)shooting;
			num3 = GetUpgradeLevelByNum(2);
		}
		else if (num == Players.DEFENSE)
		{
			num2 = (int)defense;
			num3 = GetUpgradeLevelByNum(3);
		}
		int num4 = num3;
		if (num3 >= 1)
		{
			num4++;
		}
		if (num3 >= 3)
		{
			num4++;
		}
		if (num3 >= 4)
		{
			num4++;
		}
		if (num3 >= 5)
		{
			num4 += 2;
		}
		return num2 + num4;
	}

	public virtual float GetMaxStatByNum(int num)
	{
		if (num == Players.SIZE)
		{
			return size;
		}
		if (num == Players.SPEED)
		{
			return speed + 5f + 1f + 1f + 1f + 2f;
		}
		if (num == Players.JUMP)
		{
			return jump + 5f + 1f + 1f + 1f + 2f;
		}
		if (num == Players.SHOOTING)
		{
			return shooting + 5f + 1f + 1f + 1f + 2f;
		}
		if (num == Players.DEFENSE)
		{
			return defense + 5f + 1f + 1f + 1f + 2f;
		}
		return 0f;
	}

	public virtual float GetStarValue()
	{
		float num = 40f;
		float num2 = size + speed + jump + shooting + defense;
		float num3 = num2 / num % 1f;
		if (num3 < 0.5f && num3 > 0.12f)
		{
			return Mathf.Round(num2 / num) + 0.5f;
		}
		return Mathf.Round(num2 / num);
	}

	public virtual float GetStatTotal()
	{
		return GetStatByNum(0) + GetStatByNum(1) + GetStatByNum(2) + GetStatByNum(3) + GetStatByNum(4);
	}

	public virtual float GetStatMaxTotal()
	{
		return size + speed + jump + shooting + defense + 20f + 20f;
	}

	public virtual void Buy()
	{
		PlayerPrefsHelper.SetInt(PLAYER_OWNED_KEY + playerPrefNum, 1, true);
		PlayerPrefsHelper.SetInt(PLAYER_SOLD_KEY + playerPrefNum, 0);
		if (isBackup)
		{
			Players.SetActiveBackupNum(isFemale, num);
		}
		else
		{
			Players.SetActiveStarterNum(isFemale, num);
		}
		/*FlurryAnalytics.Instance().LogEvent("buy_player", new string[5]
		{
			"playernum:" + playerPrefNum + string.Empty,
			"sessions:" + Stats.GetNumSessions() + string.Empty,
			"wins:" + Stats.GetNumWins() + string.Empty,
			"losses:" + Stats.GetNumLosses() + string.Empty,
			"currentTournament:" + Tournaments.GetCurrentTournamentNum() + string.Empty
		}, false);*/
	}

	public virtual void Sell()
	{
		PlayerPrefsHelper.SetInt(PLAYER_SOLD_KEY + playerPrefNum, 1);
		/*FlurryAnalytics.Instance().LogEvent("sell_player", new string[5]
		{
			"playernum:" + playerPrefNum + string.Empty,
			"sessions:" + Stats.GetNumSessions() + string.Empty,
			"wins:" + Stats.GetNumWins() + string.Empty,
			"losses:" + Stats.GetNumLosses() + string.Empty,
			"currentTournament:" + Tournaments.GetCurrentTournamentNum() + string.Empty
		}, false);*/
	}

	public virtual void SetPlayerPrefNum(int num)
	{
		playerPrefNum = num;
	}

	public virtual void SetName(string name)
	{
		fullName = name;
		PlayerPrefsHelper.SetString(fullNameKey, name, true);
		playerNamesMayHaveChanged = true;
		RefreshScreens();
	}

	public virtual Color GetSkinToneColor(CharacterSprites characterSprites)
	{
		Color color = default(Color);
		if (isP2 || GetSkinToneColorNum() == 0)
		{
			return GetDefaultSkinToneColor(characterSprites);
		}
		return characterSprites.customSkinTones[GetSkinToneColorNum()];
	}

	public virtual int GetSkinToneColorNum()
	{
		return PlayerPrefs.GetInt(PLAYER_COLOR_SKIN_KEY + playerPrefNum);
	}

	public virtual Color GetDefaultSkinToneColor(CharacterSprites characterSprites)
	{
		Color color = default(Color);
		if (isP2)
		{
			if (isFemale)
			{
				return characterSprites.p2SkinTonesFemale[num];
			}
			return characterSprites.p2SkinTones[num];
		}
		if (isBackup)
		{
			if (isFemale)
			{
				return characterSprites.p1BackupSkinTonesFemale[num];
			}
			return characterSprites.p1BackupSkinTones[num];
		}
		if (isFemale)
		{
			return characterSprites.p1SkinTonesFemale[num];
		}
		return characterSprites.p1SkinTones[num];
	}

	public virtual Color GetHeadColor(CharacterSprites characterSprites)
	{
		Color color = default(Color);
		if (isP2 || GetSkinToneColorNum() == 0)
		{
			return GetDefaultHeadColor(characterSprites);
		}
		return characterSprites.customSkinTones[GetSkinToneColorNum()];
	}

	public virtual Color GetDefaultHeadColor(CharacterSprites characterSprites)
	{
		Color color = default(Color);
		if (isP2)
		{
			if (isFemale)
			{
				return characterSprites.p2FemaleHeadColors[num];
			}
			return characterSprites.p2HeadColors[num];
		}
		if (isBackup)
		{
			if (isFemale)
			{
				return characterSprites.p1BackupFemaleHeadColors[num];
			}
			return characterSprites.p1BackupHeadColors[num];
		}
		if (isFemale)
		{
			return characterSprites.p1FemaleHeadColors[num];
		}
		return characterSprites.p1HeadColors[num];
	}

	public virtual Color GetHairColor(CharacterSprites characterSprites)
	{
		Color color = default(Color);
		if (isP2 || GetHairColorNum() == 0)
		{
			return GetDefaultHairColor(characterSprites);
		}
		return characterSprites.customHairColors[GetHairColorNum()];
	}

	public virtual int GetHairColorNum()
	{
		return PlayerPrefs.GetInt(PLAYER_COLOR_HAIR_KEY + playerPrefNum);
	}

	public virtual Color GetDefaultHairColor(CharacterSprites characterSprites)
	{
		Color color = default(Color);
		if (isP2)
		{
			if (isFemale)
			{
				return characterSprites.p2FemaleHairColors[num];
			}
			return characterSprites.p2HairColors[num];
		}
		if (isBackup)
		{
			if (isFemale)
			{
				return characterSprites.p1BackupFemaleHairColors[num];
			}
			return characterSprites.p1BackupHairColors[num];
		}
		if (isFemale)
		{
			return characterSprites.p1FemaleHairColors[num];
		}
		return characterSprites.p1HairColors[num];
	}

	public virtual Color GetBrowsColor(CharacterSprites characterSprites)
	{
		Color color = default(Color);
		if (isP2 || GetBrowsColorNum() == 0)
		{
			return GetDefaultBrowsColor(characterSprites);
		}
		return characterSprites.customHairColors[GetBrowsColorNum()];
	}

	public virtual int GetBrowsColorNum()
	{
		return PlayerPrefs.GetInt(PLAYER_COLOR_BROWS_KEY + playerPrefNum);
	}

	public virtual Color GetDefaultBrowsColor(CharacterSprites characterSprites)
	{
		Color color = default(Color);
		if (isP2)
		{
			if (isFemale)
			{
				return characterSprites.p2FemaleBrowsColors[num];
			}
			return characterSprites.p2BrowsColors[num];
		}
		if (isBackup)
		{
			if (isFemale)
			{
				return characterSprites.p1BackupFemaleBrowsColors[num];
			}
			return characterSprites.p1BackupBrowsColors[num];
		}
		if (isFemale)
		{
			return characterSprites.p1FemaleBrowsColors[num];
		}
		return characterSprites.p1BrowsColors[num];
	}

	public virtual Color GetEyesColor(CharacterSprites characterSprites)
	{
		Color color = default(Color);
		if (isP2 || GetEyesColorNum() == 0)
		{
			return GetDefaultEyesColor(characterSprites);
		}
		return characterSprites.customEyesColors[GetEyesColorNum()];
	}

	public virtual int GetEyesColorNum()
	{
		return PlayerPrefs.GetInt(PLAYER_COLOR_EYES_KEY + playerPrefNum);
	}

	public virtual Color GetDefaultEyesColor(CharacterSprites characterSprites)
	{
		Color color = default(Color);
		if (isP2)
		{
			if (isFemale)
			{
				return characterSprites.p2FemaleEyesColors[num];
			}
			return characterSprites.p2EyesColors[num];
		}
		if (isBackup)
		{
			if (isFemale)
			{
				return characterSprites.p1BackupFemaleEyesColors[num];
			}
			return characterSprites.p1BackupEyesColors[num];
		}
		if (isFemale)
		{
			return characterSprites.p1FemaleEyesColors[num];
		}
		return characterSprites.p1EyesColors[num];
	}

	public virtual void SaveCustomColors(int skin, int hair, int brows, int eyes)
	{
		PlayerPrefsHelper.SetInt(PLAYER_COLOR_SKIN_KEY + playerPrefNum, skin, true);
		PlayerPrefsHelper.SetInt(PLAYER_COLOR_HAIR_KEY + playerPrefNum, hair, true);
		PlayerPrefsHelper.SetInt(PLAYER_COLOR_BROWS_KEY + playerPrefNum, brows, true);
		PlayerPrefsHelper.SetInt(PLAYER_COLOR_EYES_KEY + playerPrefNum, eyes, true);
		RefreshScreens();
	}

	private void RefreshScreens()
	{
		GameObject gameObject = GameObject.Find("Screen_Players");
		if (gameObject != null)
		{
			gameObject.SetActive(false);
			gameObject.SetActive(true);
		}
		GameObject gameObject2 = GameObject.Find("Screen_Upgrade");
		if (gameObject2 != null)
		{
			gameObject2.SetActive(false);
			gameObject2.SetActive(true);
		}
	}
}
