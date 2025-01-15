/*
using GooglePlayGames;
using UnityEngine;

public class SocialPlatform : MonoBehaviour
{
	private static SocialPlatform _instance;

	private bool standby;

	public static SocialPlatform Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new SocialPlatform();
			}
			return _instance;
		}
	}

	public void Activate()
	{
		PlayGamesPlatform.Activate();
	}

	public void LogIn()
	{
		if (Social.localUser.authenticated || standby)
		{
			return;
		}
		standby = true;
		Social.localUser.Authenticate(delegate (bool success)
		{
			standby = false;
			if (success)
			{
				PlayerPrefsHelper.SetInt("LAST_SOCIALPLATFORM_LOGIN_FAILED", 0);
				PlayerPrefsHelper.SetInt("USER_LOGGED_OUT_OF_SOCIALPLATFORM", 0);
			}
			else
			{
				PlayerPrefsHelper.SetInt("LAST_SOCIALPLATFORM_LOGIN_FAILED", 1);
			}
		});
	}

	public void LogOut()
	{
		((PlayGamesPlatform)Social.Active).SignOut();
	}

	public bool IsLoggedIn()
	{
		return Social.localUser.authenticated;
	}

	public string GetUserId()
	{
		if (Social.localUser.authenticated)
		{
			return ((PlayGamesPlatform)Social.Active).GetUserId();
		}
		return null;
	}

	public string GetUserDisplayName()
	{
		if (Social.localUser.authenticated)
		{
			return ((PlayGamesPlatform)Social.Active).GetUserDisplayName();
		}
		return null;
	}

	public void ShowAchievements()
	{
		if (Social.localUser.authenticated)
		{
			Social.ShowAchievementsUI();
		}
		else
		{
			LogIn();
		}
	}

	public void UnlockAchievement(string achievementID)
	{
		if (!Social.localUser.authenticated)
		{
			return;
		}
		Social.ReportProgress(achievementID, 100.0, delegate (bool success)
		{
			if (!success)
			{
			}
		});
	}

	public void IncrementAchievement(string achievementID, int value)
	{
		if (!Social.localUser.authenticated)
		{
			return;
		}
		PlayGamesPlatform.Instance.IncrementAchievement(achievementID, value, delegate (bool success)
		{
			if (!success)
			{
			}
		});
	}

	public void ShowLeaderboards()
	{
		if (Social.localUser.authenticated)
		{
			Social.ShowLeaderboardUI();
		}
		else
		{
			LogIn();
		}
	}

	public void ShowLeaderboard(string leaderboardID)
	{
		if (Social.localUser.authenticated)
		{
			PlayGamesPlatform.Instance.ShowLeaderboardUI(leaderboardID);
		}
		else
		{
			LogIn();
		}
	}

	public void ReportHighScore(long highScore, string leaderboardID)
	{
		if (!Social.localUser.authenticated)
		{
			return;
		}
		Social.ReportScore(highScore, leaderboardID, delegate (bool success)
		{
			if (!success)
			{
			}
		});
	}
}
*/
