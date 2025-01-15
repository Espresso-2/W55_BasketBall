using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Tournament
{
	public static string TOURNAMENT_COMPLETED_KEY;

	public static string TOURNAMENT_CURRENT_ROUND_KEY;

	public static string TOURNAMENT_GAME_SCORE_KEY;

	public static string TOURNAMENT_LOST_LAST_ATTEMPT;

	public static string TOURNAMENT_REIGNING_CHAMP;

	public static string TOURNAMENT_WAIT_TIMESTAMP;

	public bool isFemale;

	public int num;

	public int prereq;

	public tournamentTypeEnum type;

	public string name;

	public int xpPrize;

	public int xpPrizeForFirstWin;

	public int cashPrize;

	public int goldPrize;

	public int arena;

	public bool completed;

	public bool locked;

	public int currentRound;

	public List<string> opponentNames = new List<string>();

	public List<int> opponentWins = new List<int>();

	public List<int> opponentLosses = new List<int>();

	public string googleAchievementId;

	public string googleLeaderboardId;

	public string gamecenterAchievementId;

	public string gamecenterLeaderboardId;

	private int lastRandomScore;

	static Tournament()
	{
		TOURNAMENT_COMPLETED_KEY = "TOURNAMENT_COMPLETED_";
		TOURNAMENT_CURRENT_ROUND_KEY = "TOURNAMENT_CURRENT_ROUND_";
		TOURNAMENT_GAME_SCORE_KEY = "TOURNAMENT_TEAM_SCORE_";
		TOURNAMENT_LOST_LAST_ATTEMPT = "TOURNAMENT_LOST_LAST_ATTEMPT_";
		TOURNAMENT_REIGNING_CHAMP = "TOURNAMENT_REIGNING_CHAMP_";
		TOURNAMENT_WAIT_TIMESTAMP = "TOURNAMENT_WAIT_TIMESTAMP_";
	}

	public virtual bool IsCompleted()
	{
		return Tournaments.TournamentIsCompleted(num);
	}

	public virtual bool LostLastAttempt()
	{
		int @int = PlayerPrefs.GetInt(TOURNAMENT_LOST_LAST_ATTEMPT + num);
		return @int == 1;
	}

	public virtual void SetLostLastAttempt(bool lostLastAttempt)
	{
		int val = (lostLastAttempt ? 1 : 0);
		PlayerPrefsHelper.SetInt(TOURNAMENT_LOST_LAST_ATTEMPT + num, val, true);
	}

	public virtual bool ReigningChamp()
	{
		int @int = PlayerPrefs.GetInt(TOURNAMENT_REIGNING_CHAMP + num);
		return @int == 1;
	}

	public virtual void SetReigningChamp(bool isReigning)
	{
		int val = (isReigning ? 1 : 0);
		PlayerPrefsHelper.SetInt(TOURNAMENT_REIGNING_CHAMP + num, val, true);
	}

	public virtual void SetCurrentRound(int round)
	{
		PlayerPrefsHelper.SetInt(TOURNAMENT_CURRENT_ROUND_KEY + num, round, true);
		currentRound = round;
	}

	public virtual int GetCurrentRound()
	{
		int num = PlayerPrefs.GetInt(TOURNAMENT_CURRENT_ROUND_KEY + this.num);
		if (num == 0)
		{
			num = 1;
		}
		return num;
	}

	public virtual string GetCurrentRoundName()
	{
		if (type == tournamentTypeEnum.LiveEvent)
		{
			return "POINT STREAK CHALLENGE";
		}
		switch (GetCurrentRound())
		{
		case 1:
			return "QUARTERFINALS";
		case 2:
			return "SEMIFINALS";
		case 3:
			return "CHAMPIONSHIP";
		default:
			return string.Empty;
		}
	}

	public virtual string GetCurrentOpponentName()
	{
		if (type == tournamentTypeEnum.LiveEvent)
		{
			int @int = PlayerPrefs.GetInt("LB_VERSION", 4);
			Debug.Log("LB_VERSION: " + @int);
			return opponentNames[@int % opponentNames.Count];
		}
		switch (GetCurrentRound())
		{
		case 1:
			return GetSlotTeam(12);
		case 2:
			return GetSlotTeam(7);
		case 3:
			return GetSlotTeam(2);
		default:
			return string.Empty;
		}
	}

	public virtual string GetSlotTeam(int slot)
	{
		if (currentRound == 1 && !LostLastAttempt())
		{
			if (slot == 1 || slot == 2 || slot == 4 || slot == 5 || slot == 7 || slot == 9)
			{
				return string.Empty;
			}
		}
		else if (currentRound == 2 && !LostLastAttempt() && (slot == 2 || slot == 5))
		{
			return string.Empty;
		}
		string result = "OPPONENT";
		if (opponentNames == null || opponentNames.Count == 0)
		{
			return result;
		}
		switch (slot)
		{
		case 0:
			result = opponentNames[0];
			break;
		case 3:
			result = opponentNames[1];
			break;
		case 6:
			result = opponentNames[2];
			break;
		case 8:
			result = opponentNames[3];
			break;
		case 10:
			result = opponentNames[4];
			break;
		case 11:
			result = opponentNames[5];
			break;
		case 12:
			result = opponentNames[6];
			break;
		case 13:
			result = TeamDetails.GetTeamName();
			break;
		case 1:
			result = ((GetSlotScoreInt(0) <= GetSlotScoreInt(3)) ? opponentNames[1] : opponentNames[0]);
			break;
		case 4:
			result = ((GetSlotScoreInt(6) <= GetSlotScoreInt(8)) ? opponentNames[3] : opponentNames[2]);
			break;
		case 7:
			result = ((GetSlotScoreInt(10) <= GetSlotScoreInt(11)) ? opponentNames[5] : opponentNames[4]);
			break;
		case 9:
			result = ((GetSlotScoreInt(12) <= GetSlotScoreInt(13)) ? TeamDetails.GetTeamName() : opponentNames[6]);
			break;
		case 2:
			result = ((GetSlotScoreInt(1) <= GetSlotScoreInt(4)) ? GetSlotTeam(4) : GetSlotTeam(1));
			break;
		case 5:
			result = ((GetSlotScoreInt(7) <= GetSlotScoreInt(9)) ? GetSlotTeam(9) : GetSlotTeam(7));
			break;
		}
		return result;
	}

	public virtual int GetSlotScoreInt(int slot)
	{
		return PlayerPrefs.GetInt(TOURNAMENT_GAME_SCORE_KEY + num + "_" + slot);
	}

	public virtual string GetSlotScore(int slot)
	{
		int @int = PlayerPrefs.GetInt(TOURNAMENT_GAME_SCORE_KEY + num + "_" + slot);
		if (@int < 0 || (currentRound == 1 && !LostLastAttempt()))
		{
			return string.Empty;
		}
		return @int.ToString();
	}

	public virtual float GetReqAbility(int playerNum)
	{
		if (PlayerPrefs.GetInt("UPGRADE_REQ_OFF") == 1 && this.num >= 2)
		{
			return 1f;
		}
		if (this.num == 0 || this.num == 2 || this.num == 3 || this.num == 4 || this.num == 5 || this.num == 7 || this.num == 9 || this.num == 12 || this.num == 125)
		{
			return 1f;
		}
		if (IsCompleted() || currentRound > 1)
		{
			return 1f;
		}
		if (this.num == 6)
		{
			if (playerNum == 0)
			{
				return 62f;
			}
			return 45f;
		}
		if (this.num == 13)
		{
			if (playerNum == 0)
			{
				return 60f;
			}
			return 50f;
		}
		int num = 0;
		if (playerNum == 0)
		{
			if ((this.num + 1) % 3 == 0 && this.num > 4)
			{
				num = -8;
			}
			if ((this.num + 1) % 5 == 0 && this.num > 6)
			{
				num = -12;
			}
			if (this.num == 4 || this.num == 6)
			{
				num = -8;
			}
			return Mathf.Round(53f + (float)this.num * 1.6f + (float)num);
		}
		if ((this.num + 1) % 5 == 0 && this.num > 6)
		{
			num = -12;
		}
		if (this.num == 6)
		{
			num = -3;
		}
		return Mathf.Round(36f + (float)this.num * 1.6f + (float)num);
	}

	public virtual float GetSecondsToWait()
	{
		float num = 0f;
		if (this.num == 2)
		{
			num = 540f;
		}
		else if (this.num == 5)
		{
			num = 2640f;
		}
		else if (this.num == 9)
		{
			num = 5220f;
		}
		else if (this.num == 12)
		{
			num = 8820f;
		}
		else if (this.num == 7)
		{
			num = 3240f;
		}
		else if (this.num >= 15 && this.num % 6 == 0)
		{
			num = 5220f;
		}
		if (num > 0f)
		{
			num += 7f;
		}
		return num;
	}

	public virtual void StartWaiting(int ts)
	{
		if (GetSecondsWaited(ts) < 1)
		{
			Debug.Log("StartWaiting========================================");
			Debug.Log("ts: " + ts);
			PlayerPrefsHelper.SetInt(TOURNAMENT_WAIT_TIMESTAMP + num, ts, true);
			NotificationQueue notificationQueue = null;
			GameObject gameObject = GameObject.Find("NotificationQueue");
			if (gameObject != null)
			{
				notificationQueue = (NotificationQueue)gameObject.GetComponent(typeof(NotificationQueue));
				Debug.Log("this.GetSecondsToWait() = " + GetSecondsToWait());
				Debug.Log("Notification time: " + ((float)ts + GetSecondsToWait()));
				notificationQueue.Add((int)((float)ts + GetSecondsToWait()), Notification.TAB_TOUR, 0);
			}
		}
	}

	public virtual int GetSecondsWaited(int ts)
	{
		int @int = PlayerPrefs.GetInt(TOURNAMENT_WAIT_TIMESTAMP + this.num);
		int num = 0;
		if (@int > 1)
		{
			return ts - @int;
		}
		return 0;
	}

	public virtual string GetCurrentOpponentRecord()
	{
		string result = null;
		int num = UnityEngine.Random.Range(0, 5);
		if (currentRound == 1)
		{
			result = string.Empty + (Stats.GetNumLosses() + 2 + this.num + num) + "-" + (Stats.GetNumWins() + 9) + string.Empty;
		}
		else if (currentRound == 2)
		{
			result = string.Empty + (Stats.GetNumWins() + 1 + this.num + num) + "-" + (Stats.GetNumLosses() + 5) + string.Empty;
		}
		else if (currentRound == 3)
		{
			result = string.Empty + (Stats.GetNumWins() + 5 + this.num + num) + "-" + (Stats.GetNumLosses() + 8) + string.Empty;
		}
		return result;
	}

	public virtual void CompleteRound(int p1Score, int p2Score, bool forfeited)
	{
		bool flag = p1Score >= p2Score && !forfeited;
		/*FlurryAnalytics.Instance().LogEvent("complete_round", new string[3]
		{
			"this.currentRound:" + currentRound + string.Empty,
			"wonGame:" + flag + string.Empty,
			"tournament:" + num + string.Empty
		}, false);*/
		if (flag)
		{
			SetLostLastAttempt(false);
			UpdateTournamentScores(currentRound, p1Score, p2Score, flag);
			if (currentRound >= 3)
			{
				CompleteTournament();
				currentRound = 4;
			}
			else
			{
				currentRound++;
			}
		}
		else
		{
			SetLostLastAttempt(true);
			UpdateTournamentScores(currentRound, p1Score, p2Score, flag);
		}
		SetCurrentRound(currentRound);
	}

	public virtual void ReloadBracket()
	{
		SetLostLastAttempt(false);
		SetReigningChamp(false);
		SetCurrentRound(1);
	}

	public virtual bool IsVisible()
	{
		if (prereq == -1)
		{
			return true;
		}
		return Tournaments.TournamentIsCompleted(prereq);
	}

	private void UpdateTournamentScores(int currentRound, int p1Score, int p2Score, bool won)
	{
		if (currentRound == 3 || (currentRound < 3 && !won))
		{
			if (currentRound < 3 && !won)
			{
				PlayerPrefsHelper.SetInt(TOURNAMENT_GAME_SCORE_KEY + num + "_2", GetRandomScore());
				PlayerPrefsHelper.SetInt(TOURNAMENT_GAME_SCORE_KEY + num + "_5", GetRandomScore());
			}
			else if (Stats.forfeited && currentRound == 3)
			{
				PlayerPrefsHelper.SetInt(TOURNAMENT_GAME_SCORE_KEY + num + "_2", 1);
				PlayerPrefsHelper.SetInt(TOURNAMENT_GAME_SCORE_KEY + num + "_5", 0);
			}
			else
			{
				PlayerPrefsHelper.SetInt(TOURNAMENT_GAME_SCORE_KEY + num + "_2", p2Score, true);
				PlayerPrefsHelper.SetInt(TOURNAMENT_GAME_SCORE_KEY + num + "_5", p1Score, true);
			}
		}
		if (currentRound == 2 || (currentRound < 2 && !won))
		{
			PlayerPrefsHelper.SetInt(TOURNAMENT_GAME_SCORE_KEY + num + "_1", GetRandomScore());
			PlayerPrefsHelper.SetInt(TOURNAMENT_GAME_SCORE_KEY + num + "_4", GetRandomScore());
			if (currentRound < 2 && !won)
			{
				PlayerPrefsHelper.SetInt(TOURNAMENT_GAME_SCORE_KEY + num + "_7", GetRandomScore());
				PlayerPrefsHelper.SetInt(TOURNAMENT_GAME_SCORE_KEY + num + "_9", GetRandomScore());
			}
			else if (Stats.forfeited)
			{
				PlayerPrefsHelper.SetInt(TOURNAMENT_GAME_SCORE_KEY + num + "_7", 1);
				PlayerPrefsHelper.SetInt(TOURNAMENT_GAME_SCORE_KEY + num + "_9", 0);
			}
			else
			{
				PlayerPrefsHelper.SetInt(TOURNAMENT_GAME_SCORE_KEY + num + "_7", p2Score, true);
				PlayerPrefsHelper.SetInt(TOURNAMENT_GAME_SCORE_KEY + num + "_9", p1Score, true);
			}
		}
		if (currentRound == 1)
		{
			PlayerPrefsHelper.SetInt(TOURNAMENT_GAME_SCORE_KEY + num + "_0", GetRandomScore());
			PlayerPrefsHelper.SetInt(TOURNAMENT_GAME_SCORE_KEY + num + "_3", GetRandomScore());
			PlayerPrefsHelper.SetInt(TOURNAMENT_GAME_SCORE_KEY + num + "_6", GetRandomScore());
			PlayerPrefsHelper.SetInt(TOURNAMENT_GAME_SCORE_KEY + num + "_8", GetRandomScore());
			PlayerPrefsHelper.SetInt(TOURNAMENT_GAME_SCORE_KEY + num + "_10", GetRandomScore());
			PlayerPrefsHelper.SetInt(TOURNAMENT_GAME_SCORE_KEY + num + "_11", GetRandomScore());
			if (Stats.forfeited)
			{
				PlayerPrefsHelper.SetInt(TOURNAMENT_GAME_SCORE_KEY + num + "_12", 1);
				PlayerPrefsHelper.SetInt(TOURNAMENT_GAME_SCORE_KEY + num + "_13", 0);
			}
			else
			{
				PlayerPrefsHelper.SetInt(TOURNAMENT_GAME_SCORE_KEY + num + "_12", p2Score, true);
				PlayerPrefsHelper.SetInt(TOURNAMENT_GAME_SCORE_KEY + num + "_13", p1Score, true);
			}
			if (won)
			{
				PlayerPrefsHelper.SetInt(TOURNAMENT_GAME_SCORE_KEY + num + "_1", -1);
				PlayerPrefsHelper.SetInt(TOURNAMENT_GAME_SCORE_KEY + num + "_4", -1);
				PlayerPrefsHelper.SetInt(TOURNAMENT_GAME_SCORE_KEY + num + "_7", -1);
				PlayerPrefsHelper.SetInt(TOURNAMENT_GAME_SCORE_KEY + num + "_9", -1);
				PlayerPrefsHelper.SetInt(TOURNAMENT_GAME_SCORE_KEY + num + "_2", -1);
				PlayerPrefsHelper.SetInt(TOURNAMENT_GAME_SCORE_KEY + num + "_5", -1);
			}
		}
	}

	public virtual void PersistToRoundOne()
	{
		PlayerPrefsHelper.SetInt(TOURNAMENT_CURRENT_ROUND_KEY + num, 1, true);
	}

	private void CompleteTournament()
	{
		completed = true;
		int numCompletions = Tournaments.GetNumCompletions(num);
		numCompletions++;
		PlayerPrefsHelper.SetInt(TOURNAMENT_COMPLETED_KEY + num, numCompletions, true);
		if (numCompletions == 1)
		{
			string text = "COMPLETED_TOURNAMENT_";
			if (num < 10)
			{
				text += "0";
			}
			text = text + string.Empty + num;
			Debug.Log("FLURRY LOG FIRST COMPLETION:" + text);
			/*FlurryAnalytics.Instance().LogEvent(text, new string[7]
			{
				"sessions:" + Stats.GetNumSessions() + string.Empty,
				"wins:" + Stats.GetNumWins() + string.Empty,
				"win_milestone:" + Stats.GetNumWinsMilestone() + string.Empty,
				"vid_milestone:" + WatchVideoButton.GetNumVidMilestone() + string.Empty,
				"losses:" + Stats.GetNumLosses() + string.Empty,
				"gold:" + Currency.GetCurrentGold() + string.Empty,
				"num_iap:" + PlayerPrefs.GetInt("NUM_PURCHASES") + string.Empty
			}, false);*/
			/*AdMediation.TrackEventInTj(text, Stats.GetNumWins());*/
		}
		else
		{
			Debug.Log("FLURRY LOG REPEAT COMPLETION FOR TOURNAMENT: " + num);
			/*FlurryAnalytics.Instance().LogEvent("REPEATED_TOURNAMENT", new string[7]
			{
				"num:" + num,
				"sessions:" + Stats.GetNumSessions() + string.Empty,
				"wins:" + Stats.GetNumWins() + string.Empty,
				"losses:" + Stats.GetNumLosses() + string.Empty,
				"gold:" + Currency.GetCurrentGold() + string.Empty,
				"num_iap:" + PlayerPrefs.GetInt("NUM_PURCHASES") + string.Empty,
				"completions:" + numCompletions
			}, false);*/
		}
	}

	private int GetRandomScore()
	{
		int num = UnityEngine.Random.Range(5, 14);
		return lastRandomScore = ((lastRandomScore < 11) ? UnityEngine.Random.Range(11, 13) : UnityEngine.Random.Range(5, 11));
	}
}
