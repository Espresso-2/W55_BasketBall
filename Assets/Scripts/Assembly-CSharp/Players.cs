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
            AddPlayer(false, false, "托尼·诺埃尔", 0, -299, -299, 6f, 10f, 10f, 10f, 18f);
            AddPlayer(false, false, "艾杰·斯宾纳", (int)(2500f * num), 35, 10, 15f, 15f, 10f, 10f, 17f);
            AddPlayer(false, false, "比利·瑞", (int)(16500f * num), -36000, 20, 13f, 20f, 13f, 14f, 19f);
            AddPlayer(false, false, "斯科特·西姆斯", (int)(8000f * num), 85, 30, 15f, 30f, 18f, 30f, 15f);
            AddPlayer(false, false, "阿克·拜伦", (int)(37500f * num), -79800, 50, 28f, 24f, 21f, 20f, 32f);
            AddPlayer(false, false, "乔·詹姆斯", (int)(8000f * num), 199, 100, 31f, 27f, 29f, 25f, 38f);
            AddPlayer(false, false, "凯尔·克莱", (int)(62500f * num), -270000, 200, 29f, 45f, 20f, 50f, 30f);
            AddPlayer(false, false, "皮·邓金", (int)(41500f * num), 350, 999, 45f, 35f, 45f, 40f, 50f);
            AddPlayer(false, true, "乔·霍普金斯", 0, -199, -199, 2f, 14f, 6f, 6f, 14f);
            AddPlayer(false, true, "布莱恩·巴凯特", (int)(1500f * num), 29, 10, 8f, 19f, 6f, 6f, 13f);
            AddPlayer(false, true, "汤姆·梅里特", (int)(12500f * num), -33000, 20, 7f, 24f, 9f, 10f, 15f);
            AddPlayer(false, true, "布罗克·弗莱尔", (int)(10000f * num), 59, 30, 11f, 25f, 14f, 11f, 26f);
            AddPlayer(false, true, "亚历克斯·阿彻", (int)(25000f * num), -66000, 40, 17f, 28f, 17f, 16f, 28f);
            AddPlayer(false, true, "马克·特拉普", (int)(20500f * num), 95, 100, 24f, 29f, 31f, 11f, 38f);
            AddPlayer(false, true, "乔尔·杰特", (int)(33000f * num), -186000, 180, 20f, 49f, 16f, 46f, 26f);
            AddPlayer(false, true, "比尔·霍克", (int)(25000f * num), 249, 995, 32f, 41f, 41f, 37f, 46f);
            AddPlayer(true, false, "莎拉·丝克", 0, 9, 4, 5f, 10f, 10f, 10f, 18f);
            AddPlayer(true, false, "安·弗莱尔", 0, -29400, 10, 14f, 15f, 12f, 11f, 17f);
            AddPlayer(true, false, "杰丝·巴斯", (int)(1500f * num), 45, 20, 12f, 20f, 14f, 15f, 19f);
            AddPlayer(true, false, "艾丽·奥尼尔", (int)(8000f * num), -48000, 30, 18f, 21f, 20f, 16f, 30f);
            AddPlayer(true, false, "凯莉·凯泽", (int)(10000f * num), 99, 50, 27f, 24f, 22f, 22f, 32f);
            AddPlayer(true, false, "莉亚·利特尔", (int)(25000f * num), -114000, 100, 31f, 25f, 37f, 16f, 42f);
            AddPlayer(true, false, "托里·泰勒", (int)(33000f * num), 189, 200, 28f, 45f, 22f, 55f, 30f);
            AddPlayer(true, false, "阿里尔·阿普尔", (int)(50000f * num), -299400, 999, 43f, 35f, 46f, 47f, 50f);
            AddPlayer(true, true, "蒂娜·克拉克", 0, -199, -199, 2f, 16f, 6f, 6f, 14f);
            AddPlayer(true, true, "克里斯·贾", (int)(2500f * num), 19, 10, 7f, 20f, 9f, 8f, 15f);
            AddPlayer(true, true, "卡拉·胡珀", (int)(8000f * num), -30000, 20, 6f, 24f, 10f, 12f, 16f);
            AddPlayer(true, true, "莫妮卡·琼斯", (int)(8000f * num), 29, 30, 10f, 26f, 15f, 15f, 25f);
            AddPlayer(true, true, "珊莎·阿彻", (int)(25000f * num), -66000, 40, 16f, 28f, 19f, 17f, 25f);
            AddPlayer(true, true, "阿里尔·阿尔斯滕", (int)(20500f * num), 89, 100, 23f, 32f, 32f, 12f, 36f);
            AddPlayer(true, true, "凯拉·奎因", (int)(50000f * num), -189000, 180, 19f, 51f, 18f, 49f, 26f);
            AddPlayer(true, true, "珀尔莎·佩恩", (int)(50000f * num), 199, 995, 26f, 45f, 42f, 39f, 46f);
        }
        else
        {
            AddPlayer(false, false, "托尼·诺埃尔", 0, -299, -299, 6f, 10f, 10f, 10f, 18f);
            AddPlayer(false, false, "艾杰·斯宾纳", 0, 35, 10, 15f, 15f, 10f, 10f, 17f);
            AddPlayer(false, false, "比利·瑞", 0, 49, 20, 13f, 20f, 13f, 14f, 19f);
            AddPlayer(false, false, "斯科特·西姆斯", 0, 85, 30, 15f, 30f, 18f, 30f, 15f);
            AddPlayer(false, false, "阿克·拜伦", 2500, 125, 50, 28f, 24f, 21f, 20f, 32f);
            AddPlayer(false, false, "乔·詹姆斯", 0, 199, 100, 31f, 27f, 29f, 25f, 38f);
            AddPlayer(false, false, "凯尔·克莱", 7500, 399, 200, 29f, 45f, 20f, 50f, 30f);
            AddPlayer(false, false, "皮·邓金", 10000, 999, 999, 45f, 35f, 45f, 40f, 50f);
            AddPlayer(false, true, "乔·霍普金斯", 0, -199, -199, 2f, 14f, 6f, 6f, 14f);
            AddPlayer(false, true, "布莱恩·巴凯特", 0, 29, 10, 8f, 19f, 6f, 6f, 13f);
            AddPlayer(false, true, "汤姆·梅里特", 0, 45, 20, 7f, 24f, 9f, 10f, 15f);
            AddPlayer(false, true, "布罗克·弗莱尔", 0, 59, 30, 11f, 25f, 14f, 11f, 26f);
            AddPlayer(false, true, "亚历克斯·阿彻", 2500, 99, 40, 17f, 28f, 17f, 16f, 28f);
            AddPlayer(false, true, "马克·特拉普", 4000, 199, 100, 24f, 29f, 31f, 11f, 38f);
            AddPlayer(false, true, "乔尔·杰特", 7500, 299, 180, 20f, 49f, 16f, 46f, 26f);
            AddPlayer(false, true, "比尔·霍克", 10000, 749, 995, 32f, 41f, 41f, 37f, 46f);
            AddPlayer(true, false, "莎拉·丝克", 0, 2, 4, 5f, 10f, 10f, 10f, 18f);
            AddPlayer(true, false, "安·弗莱尔", 0, 34, 10, 14f, 15f, 12f, 11f, 17f);
            AddPlayer(true, false, "杰丝·巴斯", 0, 45, 20, 12f, 20f, 14f, 15f, 19f);
            AddPlayer(true, false, "艾丽·奥尼尔", 0, 69, 30, 18f, 21f, 20f, 16f, 30f);
            AddPlayer(true, false, "凯莉·凯泽", 2500, 129, 50, 27f, 24f, 22f, 22f, 32f);
            AddPlayer(true, false, "莉亚·利特尔", 000, 179, 100, 31f, 25f, 37f, 16f, 42f);
            AddPlayer(true, false, "托里·泰勒", 7500, 349, 200, 28f, 45f, 22f, 55f, 30f);
            AddPlayer(true, false, "阿里尔·阿普尔", 10000, 949, 999, 43f, 35f, 46f, 47f, 50f);
            AddPlayer(true, true, "蒂娜·克拉克", 0, -199, -199, 2f, 16f, 6f, 6f, 14f);
            AddPlayer(true, true, "克里斯·贾", 0, 30, 10, 7f, 20f, 9f, 8f, 15f);
            AddPlayer(true, true, "卡拉·胡珀", 0, 40, 20, 6f, 24f, 10f, 12f, 16f);
            AddPlayer(true, true, "莫妮卡·琼斯", 0, 64, 30, 10f, 26f, 15f, 15f, 25f);
            AddPlayer(true, true, "珊莎·阿彻", 2500, 97, 40, 16f, 28f, 19f, 17f, 25f);
            AddPlayer(true, true, "阿里尔·阿尔斯滕", 4000, 185, 100, 23f, 32f, 32f, 12f, 36f);
            AddPlayer(true, true, "凯拉·奎因", 7500, 315, 180, 19f, 51f, 18f, 49f, 26f);
            AddPlayer(true, true, "珀尔莎·佩恩", 10000, 700, 995, 26f, 45f, 42f, 39f, 46f);
        }
    }

    private void AddPlayer(bool isFemale, bool isBackup, string fullNameKey, int reqXP, int goldPrice, int goldSellReward, float size, float speed,
        float jump, float shooting, float defense)
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