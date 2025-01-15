using UnityEngine;

public class AchievementsManager
{
	private static AchievementsManager instance;

	public static AchievementsManager Instance
	{
		get
		{
			if (instance == null)
			{
				Debug.Log("AchievementsManager: instance = new AchievementsManager()");
				instance = new AchievementsManager();
			}
			return instance;
		}
	}

	public void CompletedGame(bool won, int round)
	{
		/*if (Stats.numSteals >= 5)
		{
			SocialPlatform.Instance.UnlockAchievement("CgkI_oWvsoANEAIQCA");
		}
		if (Stats.numSteals >= 7)
		{
			SocialPlatform.Instance.UnlockAchievement("CgkI_oWvsoANEAIQDw");
		}
		if (Stats.numRebounds >= 5)
		{
			SocialPlatform.Instance.UnlockAchievement("CgkI_oWvsoANEAIQCQ");
		}
		if (Stats.numRebounds >= 7)
		{
			SocialPlatform.Instance.UnlockAchievement("CgkI_oWvsoANEAIQEA");
		}
		if (Stats.numBlocks >= 5)
		{
			SocialPlatform.Instance.UnlockAchievement("CgkI_oWvsoANEAIQCg");
		}
		if (Stats.numBlocks >= 7)
		{
			SocialPlatform.Instance.UnlockAchievement("CgkI_oWvsoANEAIQEQ");
		}
		if (Stats.p1Score >= 13)
		{
			SocialPlatform.Instance.UnlockAchievement("CgkI_oWvsoANEAIQCw");
		}
		if (GetNumDoubleDigitScoringCategories() == 2)
		{
			SocialPlatform.Instance.UnlockAchievement("CgkI_oWvsoANEAIQDA");
		}
		if (GetNumDoubleDigitScoringCategories() >= 3)
		{
			SocialPlatform.Instance.UnlockAchievement("CgkI_oWvsoANEAIQDQ");
		}
		if (won)
		{
			if (round == 1)
			{
				SocialPlatform.Instance.UnlockAchievement("CgkI_oWvsoANEAIQAw");
			}
			if (round == 2)
			{
				SocialPlatform.Instance.UnlockAchievement("CgkI_oWvsoANEAIQBA");
			}
			if (round == 3)
			{
				SocialPlatform.Instance.UnlockAchievement("CgkI_oWvsoANEAIQBQ");
			}
			if (Stats.p2Score == 0)
			{
				SocialPlatform.Instance.UnlockAchievement("CgkI_oWvsoANEAIQAQ");
			}
			if (Stats.numMakes == Stats.num3PtMakes)
			{
				SocialPlatform.Instance.UnlockAchievement("CgkI_oWvsoANEAIQBg");
			}
			if (Stats.numMakes >= Stats.numShots)
			{
				SocialPlatform.Instance.UnlockAchievement("CgkI_oWvsoANEAIQBw");
			}
			if (Stats.GetWinStreak() >= 10)
			{
				SocialPlatform.Instance.UnlockAchievement("CgkI_oWvsoANEAIQEg");
			}
			if (Stats.GetWinStreak() >= 20)
			{
				SocialPlatform.Instance.UnlockAchievement("CgkI_oWvsoANEAIQFA");
			}
			if (Stats.numSeconds < 40)
			{
				SocialPlatform.Instance.UnlockAchievement("CgkI_oWvsoANEAIQHQ");
			}
			SocialPlatform.Instance.IncrementAchievement("CgkI_oWvsoANEAIQDg", 1);
			SocialPlatform.Instance.IncrementAchievement("CgkI_oWvsoANEAIQFQ", 1);
			SocialPlatform.Instance.IncrementAchievement("CgkI_oWvsoANEAIQFg", 1);
		}
		SocialPlatform.Instance.IncrementAchievement("CgkI_oWvsoANEAIQEw", Stats.p1Score);
		SocialPlatform.Instance.IncrementAchievement("CgkI_oWvsoANEAIQFw", Stats.p1Score);
		SocialPlatform.Instance.IncrementAchievement("CgkI_oWvsoANEAIQGA", Stats.p1Score);*/
	}

	public void OpenedBag()
	{
		Debug.Log("AchievementsManager.OpenedBag()");
		int value = PlayerPrefs.GetInt("NUM_BAGS_OPENED") + 1;
		PlayerPrefs.SetInt("NUM_BAGS_OPENED", value);
		/*SocialPlatform.Instance.IncrementAchievement("CgkI_oWvsoANEAIQGQ", 1);
		SocialPlatform.Instance.IncrementAchievement("CgkI_oWvsoANEAIQGg", 1);*/
	}

	public void DroppedPrizeBall()
	{
		Debug.Log("AchievementsManager.DroppedPrizeBall()");
		int value = PlayerPrefs.GetInt("NUM_PRIZE_BALLS_DROPPED") + 1;
		PlayerPrefs.SetInt("NUM_PRIZE_BALLS_DROPPED", value);
		/*SocialPlatform.Instance.IncrementAchievement("CgkI_oWvsoANEAIQGw", 1);
		SocialPlatform.Instance.IncrementAchievement("CgkI_oWvsoANEAIQHA", 1);*/
	}

	public void FullyUpgradedPlayer()
	{
		Debug.Log("AchievementsManager.FullyUpgradedPlayer()");
		/*SocialPlatform.Instance.UnlockAchievement("CgkI_oWvsoANEAIQHg");*/
	}

	private int GetNumDoubleDigitScoringCategories()
	{
		int num = 0;
		if (Stats.p1Score >= 10)
		{
			num++;
		}
		if (Stats.numBlocks >= 10)
		{
			num++;
		}
		if (Stats.numSteals >= 10)
		{
			num++;
		}
		if (Stats.numRebounds >= 10)
		{
			num++;
		}
		return num;
	}
}
