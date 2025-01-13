using System;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

public class PlayFabLeaderboard : MonoBehaviour
{
	public static int leaderBoardVersion;

	public static DateTime leaderboardNextReset;

	public static int currentEntry;

	public static bool DidWeReachNextResetTimeSinceLogin()
	{
		float num = 10f;
		return PlayFabManager.Instance().GetCurrentTime() > leaderboardNextReset.AddSeconds(num);
	}

	public static void GetEventLeaderboard(LeaderboardEntries lbEntries, bool prevVersion, bool useGameOn)
	{
		Debug.Log("GetEventLeaderboard: " + lbEntries);
		if (!useGameOn)
		{
			if (lbEntries != null)
			{
				lbEntries.HideCurrentUserCell();
			}
			string statName = "EVENT_SCORE_" + PlayerPrefs.GetInt("LEAGUE_NUM");
			GetLeaderboard(false, statName, 0, 100, prevVersion, lbEntries);
		}
	}

	public static void GetPrevEventTopThree(LeaderboardEntries lbEntries, bool prevVersion, bool useGameOn)
	{
		Debug.Log("GetEventLeaderboard: " + lbEntries);
		if (!useGameOn)
		{
			string statName = "EVENT_SCORE_" + PlayerPrefs.GetInt("LEAGUE_NUM");
			GetLeaderboard(false, statName, 0, 3, prevVersion, lbEntries);
		}
	}

	private static void EventLeaderBoardResultHandler(string[] myNameResults, int[] myScoreResults, LeaderboardEntries lbEntries)
	{
		if (!(lbEntries != null))
		{
			return;
		}
		lbEntries.HideCells();
		lbEntries.HideLoadingIcon();
		int num = 0;
		for (int i = 0; i < myNameResults.Length; i++)
		{
			if (lbEntries.entries.Length > i)
			{
				LeaderboardEntry leaderboardEntry = lbEntries.entries[num];
				leaderboardEntry.UpdateDisplay(i, myNameResults[i], myScoreResults[i], "pfId", "PlayFabId", false);
			}
			num++;
		}
	}

	private static void GetLeaderboard(bool friends, string statName, int startPos, int maxCount, bool prevVersion, LeaderboardEntries lbEntries)
	{
		if (!PlayFabClientAPI.IsClientLoggedIn())
		{
			Debug.Log("ERROR calling GetLeaderboard(): PlayFabClientAPI.IsClientLoggedIn() = false");
			return;
		}
		if (friends)
		{
			GetFriendLeaderboardRequest getFriendLeaderboardRequest = new GetFriendLeaderboardRequest();
			getFriendLeaderboardRequest.StatisticName = statName;
			getFriendLeaderboardRequest.StartPosition = startPos;
			getFriendLeaderboardRequest.MaxResultsCount = maxCount;
			getFriendLeaderboardRequest.IncludeFacebookFriends = true;
			GetFriendLeaderboardRequest request2 = getFriendLeaderboardRequest;
			PlayFabClientAPI.GetFriendLeaderboard(request2, delegate(GetLeaderboardResult result)
			{
				LeaderBoardResultHandler(result, friends, lbEntries, prevVersion);
			}, delegate(PlayFabError error)
			{
				Debug.Log("Got error getting friends leaderboard:");
				Debug.Log(error.ErrorMessage);
			});
			return;
		}
		GetLeaderboardRequest request = new GetLeaderboardRequest
		{
			StatisticName = statName,
			StartPosition = startPos,
			MaxResultsCount = maxCount
		};
		if (prevVersion)
		{
			request.Version = leaderBoardVersion - 1;
		}
		PlayFabClientAPI.GetLeaderboard(request, delegate(GetLeaderboardResult result)
		{
			LeaderBoardResultHandler(result, friends, lbEntries, prevVersion);
		}, delegate(PlayFabError error)
		{
			Debug.Log("Error getting global leaderboard (Version = " + request.Version + " + MaxResultsCount = " + request.MaxResultsCount + "): ");
			Debug.Log(error.ErrorMessage);
		});
	}

	private static void LeaderBoardResultHandler(GetLeaderboardResult result, bool friends, LeaderboardEntries lbEntries, bool prevVersion)
	{
		if (!(lbEntries != null))
		{
			return;
		}
		lbEntries.HideCells();
		lbEntries.HideLoadingIcon();
		int num = 0;
		string @string = PlayerPrefs.GetString("PLAYFAB_RESULT_ID");
		int num2 = 999999;
		foreach (PlayerLeaderboardEntry item in result.Leaderboard)
		{
			if (lbEntries.entries.Length > num)
			{
				LeaderboardEntry leaderboardEntry = lbEntries.entries[num];
				leaderboardEntry.UpdateDisplay(item.Position, item.DisplayName, item.StatValue, item.PlayFabId, @string, friends);
				if (item.PlayFabId == @string)
				{
					num2 = item.Position;
					if (lbEntries.currentUserEntry != null)
					{
						lbEntries.currentUserEntry.gameObject.transform.parent.gameObject.SetActive(true);
						lbEntries.currentUserEntry.UpdateDisplay(num2, item.DisplayName, item.StatValue, item.PlayFabId, @string, friends);
					}
				}
			}
			num++;
		}
		if (num2 > 100)
		{
			string statName = "EVENT_SCORE_" + PlayerPrefs.GetInt("LEAGUE_NUM");
			GetCurrentUserLeaderBoardRank(statName, prevVersion, lbEntries);
		}
	}

	public static void GetCurrentUserLeaderBoardRank(string statName, bool prevVersion, LeaderboardEntries lbEntries)
	{
		if (PlayFabClientAPI.IsClientLoggedIn())
		{
			GetLeaderboardAroundPlayerRequest getLeaderboardAroundPlayerRequest = new GetLeaderboardAroundPlayerRequest();
			getLeaderboardAroundPlayerRequest.PlayFabId = PlayerPrefs.GetString("PLAYFAB_RESULT_ID");
			getLeaderboardAroundPlayerRequest.StatisticName = statName;
			getLeaderboardAroundPlayerRequest.MaxResultsCount = 1;
			GetLeaderboardAroundPlayerRequest getLeaderboardAroundPlayerRequest2 = getLeaderboardAroundPlayerRequest;
			if (prevVersion)
			{
				getLeaderboardAroundPlayerRequest2.Version = leaderBoardVersion - 1;
			}
			PlayFabClientAPI.GetLeaderboardAroundPlayer(getLeaderboardAroundPlayerRequest2, delegate(GetLeaderboardAroundPlayerResult result)
			{
				CurrentUserLeaderBoardRankResultHandler(result, lbEntries, prevVersion, statName);
			}, delegate(PlayFabError error)
			{
				Debug.Log("Got error getting current user leaderboard rank for " + statName + ":");
				Debug.Log(error.ErrorMessage);
			});
		}
	}

	private static void CurrentUserLeaderBoardRankResultHandler(GetLeaderboardAroundPlayerResult result, LeaderboardEntries lbEntries, bool prevVersion, string statName)
	{
		if (prevVersion)
		{
			Debug.Log("we are getting the previous version to show in the prev leaderboard or calculating users seasons results: " + statName);
			if (PlayerPrefs.GetInt("NEED_TO_START_NEW_SEASON") == 1)
			{
				Debug.Log("Need to start the new season...");
				List<PlayerLeaderboardEntry> leaderboard = result.Leaderboard;
				if (leaderboard.Count > 0 && leaderboard[0].StatValue > 0)
				{
					Debug.Log("User played in the season");
					Debug.Log("(Leaderboard Count: " + leaderboard.Count + ")");
					PlayerLeaderboardEntry playerLeaderboardEntry = leaderboard[0];
					PlayerPrefs.SetInt("SEASON_COMPLETED_POS", playerLeaderboardEntry.Position);
					PlayerPrefs.SetInt("SEASON_COMPLETED_SCORE", playerLeaderboardEntry.StatValue);
					PlayerPrefs.SetInt("SHOW_USER_SEASON_RESULTS", 1);
				}
				else
				{
					Debug.Log("User didn't play in the season");
				}
				PlayerPrefs.SetInt("NEED_TO_START_NEW_SEASON", 0);
			}
		}
		else
		{
			leaderBoardVersion = result.Version;
			int @int = PlayerPrefs.GetInt("LB_VERSION");
			Debug.Log("result.Version: " + result.Version + " (storedLeaderboardVersion: " + @int + "): " + statName);
			if (@int != leaderBoardVersion)
			{
				Debug.Log("WE HAVE A NEW LEADERBOARD VERSION! AT THIS POINT USERS EVENT_SCORE HAS PROBABLY BEEN RESET TO 0");
				PlayerPrefs.SetInt("LB_VERSION", leaderBoardVersion);
				PlayerPrefs.SetInt("SHOW_USER_SEASON_RESULTS", 0);
				PlayerPrefs.SetInt("NEED_TO_START_NEW_SEASON", 1);
				PlayerPrefs.SetInt("ENTRIES_REMAINING", 2);
				string statName2 = "EVENT_SCORE_" + PlayerPrefs.GetInt("LEAGUE_NUM");
				GetCurrentUserLeaderBoardRank(statName2, true, null);
				PlayFabManager.Instance().SetUserDataForKey("LB_VERSION", leaderBoardVersion.ToString());
			}
		}
		if (result.NextReset.HasValue)
		{
			leaderboardNextReset = result.NextReset.Value;
			Debug.Log("NextReset: " + leaderboardNextReset);
			if (PlayerPrefs.GetInt("FIX_NEXTRESET") == 1)
			{
				leaderboardNextReset = leaderboardNextReset.AddHours(24.0);
				if ((leaderboardNextReset - PlayFabManager.Instance().GetCurrentTime()).TotalDays > 7.0)
				{
					leaderboardNextReset = leaderboardNextReset.AddDays(-7.0);
				}
				Debug.Log(string.Concat("NextReset Fixed: ", leaderboardNextReset, " : ", statName));
			}
		}
		else
		{
			Debug.Log("NextReset has no value! : " + statName);
		}
		if (result.Leaderboard.Count <= 0)
		{
			return;
		}
		PlayerLeaderboardEntry playerLeaderboardEntry2 = result.Leaderboard[0];
		if (lbEntries != null && lbEntries.currentUserEntry != null)
		{
			string @string = PlayerPrefs.GetString("PLAYFAB_RESULT_ID");
			lbEntries.currentUserEntry.gameObject.transform.parent.gameObject.SetActive(true);
			int num = playerLeaderboardEntry2.Position;
			if (num < 100)
			{
				num = 105;
			}
			if (playerLeaderboardEntry2.StatValue > 0)
			{
				lbEntries.currentUserEntry.UpdateDisplay(num, playerLeaderboardEntry2.DisplayName, playerLeaderboardEntry2.StatValue, playerLeaderboardEntry2.PlayFabId, @string, false);
			}
			else
			{
				lbEntries.HideCurrentUserCell();
			}
		}
		if (!prevVersion)
		{
			currentEntry = playerLeaderboardEntry2.StatValue;
		}
		if (playerLeaderboardEntry2.Profile != null && playerLeaderboardEntry2.Profile.Locations == null)
		{
		}
	}
}
