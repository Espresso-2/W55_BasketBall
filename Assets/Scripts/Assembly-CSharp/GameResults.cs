using System;
using System.Collections;
using I2.Loc;
using UnityEngine;
using UnityEngine.UI;
using W_Log;

[Serializable]
public class GameResults : MonoBehaviour
{
	public GameObject replayBox;

	public GameObject msgBox;

	public GameObject continueButton;

	/*public RemoveAdsMsgBox removeAdsMsgBox;*/

	public GameObject claimButtonsHolder;

	public ChampAward_C champAward;

	public BonusReward bonusReward;

	public LevelUpScreen levelUpScreen;

	public Localize heading;

	public Text p1Name;

	public Localize p1NameLocalize;

	public Text p1Score;

	public Text p2Name;

	public Localize p2NameLocalize;

	public Text p2Score;

	public Text shootingStat;

	public Text shootingPrize;

	private int shootingPrizeNum;

	public Text reboundsStat;

	public Text reboundsPrize;

	private int reboundsPrizeNum;

	public Text blocksStat;

	public Text blocksPrize;

	private int blocksPrizeNum;

	public Text stealsStat;

	public Text stealsPrize;

	private int stealsPrizeNum;

	public Text secondsStat;

	public Text secondsPrize;

	private int secondsPrizeNum;

	public GameObject winMarginRow;

	public Text winMarginStat;

	public Text winMarginPrize;

	private int winMarginInt;

	private int winMarginPrizeNum;

	public GameObject rowPrize;

	public Text championXpPrize;

	private int championXpPrizeNum;

	public Text championCashPrize;

	private int championCashPrizeNum;

	public Text totalXp;

	private int totalXpNum;

	public Text totalCash;

	private int totalCashNum;

	public Text totalGold;

	private int totalGoldNum;

	public GameObject totalGoldIcon;

	public GameObject cashDoubledMessageBox;

	/*public DoubleRewardsBox doubleRewardsBox;*/

	public GameObject xpDoubledBox;

	public Text xpDoubledNum;

	public GameObject cashDoubledBox;

	public Text cashDoubledNum;

	public CurrencyAnim[] currencyAnims;

	public Tournaments tournaments;

	private bool leveledUp;

	private bool giveBagBonusReward;

	private bool giveCashBonusReward;

	/*private bool showIntAd;*/

	private GameSounds gameSounds;

	private SessionVars sessionVars;

	private Music music;

	/*private bool intCompleted;*/

	private bool isLiveEvent;

	private bool isChampionship;

	private bool won;

	private bool gameEligibleForAchievements;

	private int roundThatWeJustPlayed;

	public virtual void Start()
	{
		Time.timeScale = 1f;
		gameSounds = GameSounds.GetInstance();
		sessionVars = SessionVars.GetInstance();
		GameObject gameObject = GameObject.Find("Music");
		if (gameObject != null)
		{
			music = (Music)gameObject.GetComponent(typeof(Music));
		}
		sessionVars.justCompletedMatch = true;
		/*doubleRewardsBox.gameObject.SetActive(false);
		doubleRewardsBox.claimButton.SetActive(true);*/
		xpDoubledBox.SetActive(false);
		cashDoubledBox.SetActive(false);
		Tournament tournament = tournaments.GetTournament(Tournaments.GetCurrentTournamentNum());
		bool goToTutorial = sessionVars.goToTutorial;
		isLiveEvent = tournament.type == tournamentTypeEnum.LiveEvent && !sessionVars.goToTutorial && !goToTutorial && !sessionVars.goToPractice;
		roundThatWeJustPlayed = tournament.GetCurrentRound();
		if (sessionVars.goToPractice)
		{
			p1Name.text = string.Empty;
			p1Score.text = string.Empty;
			p2Name.text = string.Empty;
			p2Score.text = string.Empty;
		}
		else if (goToTutorial)
		{
			roundThatWeJustPlayed = 1;
			p1NameLocalize.SetTerm("YOU", null);
			p1Score.text = Stats.p1Score.ToString("n0");
			p2NameLocalize.SetTerm("OPPONENT", null);
			p2Score.text = Stats.p2Score.ToString("n0");
		}
		else if (isLiveEvent)
		{
			p1Name.text = "SCORE";
			p1Score.text = Stats.p1Score.ToString("n0");
			p2Name.text = ((Stats.p1Score <= PlayFabLeaderboard.currentEntry) ? "YOUR BEST ENTRY" : string.Empty);
			p2Score.text = ((Stats.p1Score <= PlayFabLeaderboard.currentEntry) ? PlayFabLeaderboard.currentEntry.ToString() : "NEW BEST!");
			if (Stats.p1Score > PlayFabLeaderboard.currentEntry)
			{
				PlayFabLeaderboard.currentEntry = Stats.p1Score;
			}
		}
		else
		{
			p1Name.text = TeamDetails.GetTeamName();
			p1Score.text = Stats.p1Score.ToString("n0");
			p2Name.text = tournament.GetCurrentOpponentName();
			p2Score.text = Stats.p2Score.ToString("n0");
		}
		isChampionship = roundThatWeJustPlayed == 3 && !sessionVars.goToPractice && !isLiveEvent;
		won = Stats.p1Score > Stats.p2Score && !Stats.forfeited && !isLiveEvent;
		if (isChampionship)
		{
			msgBox.SetActive(false);
		}
		sessionVars.wonLastGame = won;
		if (isLiveEvent)
		{
			heading.SetTerm("LIVE EVENT", null);
		}
		else if (sessionVars.goToPractice)
		{
			heading.SetTerm("PRACTICE", null);
		}
		else if (won)
		{
			heading.SetTerm("YOU WON", null);
		}
		else if (Stats.forfeited)
		{
			heading.SetTerm("FORFEITED", null);
		}
		else
		{
			heading.SetTerm("GOOD GAME", null);
		}
		if (!Stats.forfeited && !sessionVars.goToPractice)
		{
			if (Stats.numShots > 0)
			{
				shootingPrizeNum = (int)(6f * Mathf.Round((float)Stats.numMakes * 1f / (float)Stats.numShots * 1f * 25f));
			}
			else
			{
				shootingPrizeNum = 0;
			}
			reboundsPrizeNum = Stats.numRebounds * 40;
			blocksPrizeNum = Stats.numBlocks * 30;
			stealsPrizeNum = Stats.numSteals * 45;
			if (Stats.numSeconds < 50)
			{
				secondsPrizeNum = 90;
			}
			else if (Stats.numSeconds < 90)
			{
				secondsPrizeNum = 70;
			}
			else
			{
				secondsPrizeNum = 50;
			}
			if (won && isChampionship)
			{
				championCashPrizeNum = tournament.cashPrize;
				championXpPrizeNum = tournament.xpPrize;
				totalGoldNum = tournament.goldPrize;
				totalXpNum = tournament.xpPrize;
				TabChanger.bottomNavSlideInDelay = 9.5f;
			}
			else
			{
				if (isLiveEvent)
				{
					totalXpNum = 100;
				}
				else
				{
					totalXpNum = roundThatWeJustPlayed * 35;
				}
				if (!goToTutorial && !sessionVars.goToPractice && !isLiveEvent)
				{
					TabChanger.bottomNavSlideInDelay = 4f;
				}
			}
			if (won)
			{
				winMarginInt = Stats.p1Score - Stats.p2Score;
				winMarginPrizeNum = winMarginInt * 20;
			}
			else
			{
				winMarginRow.SetActive(false);
			}
			/*if (((won && !isChampionship) || isLiveEvent) /*&& AdMediation.IsVidAvail()#1#)
			{
				doubleRewardsBox.gameObject.SetActive(true);
				/*FlurryAnalytics.Instance().LogEvent("SHOW_2X_REW_VID_BTN", new string[2]
				{
					"type:vungle",
					"num_wins_milestone:" + Stats.GetNumWinsMilestone() + string.Empty
				}, false);#1#
			}*/
		}
		else if (sessionVars.goToPractice)
		{
			if (Stats.numShots > 0 && !Stats.forfeited)
			{
				shootingPrizeNum = (int)(6f * Mathf.Round((float)Stats.numMakes * 1f / (float)Stats.numShots * 1f * 25f));
			}
			else
			{
				shootingPrizeNum = 0;
			}
			if (!Stats.forfeited)
			{
				if (Stats.numSeconds < 50)
				{
					secondsPrizeNum = 50;
				}
				else if (Stats.numSeconds < 90)
				{
					secondsPrizeNum = 25;
				}
				else
				{
					secondsPrizeNum = 25;
				}
				totalXpNum = 20;
			}
			else
			{
				secondsPrizeNum = 0;
			}
			winMarginInt = 0;
			winMarginPrizeNum = 0;
			winMarginRow.SetActive(false);
		}
		else
		{
			winMarginRow.SetActive(false);
		}
		totalCashNum = shootingPrizeNum + reboundsPrizeNum + blocksPrizeNum + championCashPrizeNum + stealsPrizeNum + secondsPrizeNum + winMarginPrizeNum;
		if (isLiveEvent)
		{
			TournamentView.showLeaderboardPanel = true;
		}
		else if (totalCashNum > 0 && !goToTutorial && !sessionVars.goToPractice)
		{
			TournamentView.showCashAnim = true;
		}
		EarnedXP(totalXpNum);
		Currency.AddCash(totalCashNum);
		if (Stats.numShots > 0)
		{
			string text = "3PT:" + Stats.num3PtMakes + "/" + Stats.num3PtShots + "       " + Stats.numMakes + "/" + Stats.numShots + string.Empty;
			float num = Mathf.Round((float)Stats.numMakes * 1f / (float)Stats.numShots * 1f * 100f);
			shootingStat.text = text;
			shootingPrize.text = shootingPrizeNum.ToString("n0");
		}
		else
		{
			shootingStat.text = "0/0";
			shootingPrize.text = "0";
		}
		reboundsStat.text = Stats.numRebounds.ToString("n0");
		reboundsPrize.text = reboundsPrizeNum.ToString("n0");
		blocksStat.text = Stats.numBlocks.ToString("n0");
		blocksPrize.text = blocksPrizeNum.ToString("n0");
		stealsStat.text = Stats.numSteals.ToString("n0");
		stealsPrize.text = stealsPrizeNum.ToString("n0");
		secondsStat.text = DoubleTapUtils.GetTimeFromSeconds(Stats.numSeconds) + string.Empty;
		secondsPrize.text = secondsPrizeNum.ToString("n0");
		winMarginStat.text = "+" + winMarginInt.ToString("n0");
		winMarginPrize.text = winMarginPrizeNum.ToString("n0");
		if (championCashPrizeNum > 0)
		{
			championCashPrize.text = championCashPrizeNum.ToString("n0");
			championXpPrize.text = championXpPrizeNum.ToString("n0");
		}
		else
		{
			rowPrize.SetActive(false);
		}
		if (totalGoldNum > 0)
		{
			totalGold.text = totalGoldNum.ToString("n0");
		}
		else
		{
			totalGoldIcon.gameObject.SetActive(false);
			totalGold.gameObject.SetActive(false);
		}
		if (totalCashNum == 0)
		{
			HideCurrencyAnims();
		}
		if (!isLiveEvent && !goToTutorial && !sessionVars.goToPractice)
		{
			gameEligibleForAchievements = true;
			tournament.CompleteRound(Stats.p1Score, Stats.p2Score, Stats.forfeited);
		}
		if (music != null)
		{
			music.StartMusic();
		}
		CalculateBonusRewardAndIntAd(tournament);
		replayBox.SetActive(false);
		msgBox.SetActive(false);
		if (won && GameObject.Find("RecordCamera") != null)
		{
			replayBox.SetActive(true);
		}
		else
		{
			StartCoroutine(ShowResultsBox());
		}
		int num2 = Stats.GetNumWins() + Stats.GetNumLosses();
		if (num2 == 3 || num2 == 6 || num2 == 9 || num2 == 18 || num2 == 50 || num2 == 100)
		{
			string eventId = "PLAYED_" + num2 + "_GAMES";
			/*FlurryAnalytics.Instance().LogEvent(eventId, new string[5]
			{
				"sessions:" + Stats.GetNumSessions() + string.Empty,
				"wins:" + Stats.GetNumWins() + string.Empty,
				"losses:" + Stats.GetNumLosses() + string.Empty,
				"gold:" + Currency.GetCurrentGold() + string.Empty,
				"num_iap:" + PlayerPrefs.GetInt("NUM_PURCHASES") + string.Empty
			}, false);*/
			/*AdMediation.TrackEventInTj(eventId, Stats.GetNumWins());
			switch (num2)
			{
			case 6:
				AdMediation.ActionCompleteInTj("7c50afa1-5898-4d23-a0e3-99db6ce3860f");
				break;
			case 9:
				AdMediation.ActionCompleteInTj("ea4d590c-c6e5-44e8-93c6-5ade32ddd0cb");
				break;
			case 18:
				AdMediation.ActionCompleteInTj("af5ea749-f777-41c6-9785-fe9892aac7f1");
				break;
			}*/
		}
		/*PlayFabManager.Instance().SetUserDataCall1();*/
		//PlayFabManager.Instance().SetUserDataCall2();
		/*PlayFabManager.Instance().UpdateStat("NUM_TROPHIES", Tournaments.numTrophies);*/
		//AdMediation.HideTopBanner();
	}

	private void CalculateBonusRewardAndIntAd(Tournament tournament)
	{
		bool flag = false;
		if (won && !sessionVars.goToPractice && !sessionVars.goToTutorial)
		{
			CreateTabNotifications(tournament.num);
			if (Stats.GetNumWins() == 2 || Stats.GetNumWins() == 7)
			{
				giveBagBonusReward = true;
			}
			else if (Tournaments.GetNumCompletions(tournament.num) == 3 && isChampionship)
			{
				giveBagBonusReward = true;
			}
			if (PlayerPrefs.GetInt("GAMES_SINCE_WATCHING_AD") >= 2)
			{
				flag = true;
			}
		}
		/*if ((flag || Stats.forfeited) && PlayerPrefs.GetInt("ADS_OFF") != 1 && (PlayerPrefs.GetInt("NUM_PURCHASES") == 0 || PlayerPrefs.GetInt("IS_FRAUDULENT_USER") == 1))
		{
			showIntAd = UnityEngine.Random.Range(0, 100) >= 50 || Stats.forfeited;
		}*/
		if (sessionVars.goToTutorial && Stats.GetNumWins() < 2)
		{
			giveCashBonusReward = true;
		}
	}

	public virtual void ContinueFromReplayOnClick()
	{
		gameSounds.Play_one_dribble();
		StartCoroutine(ShowResultsBox());
	}

	private IEnumerator ShowResultsBox()
	{
		replayBox.SetActive(false);
		DestroyReplayCameras();
		if (isChampionship)
		{
			champAward.gameObject.SetActive(true);
			champAward.ShowAward(won);
			yield return new WaitForSeconds(1f);
			gameSounds.Play_trumpet_chime_2();
			yield return new WaitForSeconds(2.25f);
			champAward.gameObject.SetActive(false);
		}
		else
		{
			yield return new WaitForSeconds(0.5f);
		}
		msgBox.SetActive(true);
		claimButtonsHolder.SetActive(false);
		StartCoroutine(AnimateTextNum(totalXp, totalXpNum, 7, false));
		StartCoroutine(AnimateTextNum(totalCash, totalCashNum, 40, true));
		// if (gameEligibleForAchievements)
		// {
		// 	AchievementsManager.Instance.CompletedGame(won, roundThatWeJustPlayed);
		// }
	}

	public virtual void ClaimOnClick()
	{
		Time.timeScale = 1f;
		gameSounds.Play_one_dribble();
		gameSounds.Play_dunk();
		msgBox.SetActive(false);
		if (giveBagBonusReward || giveCashBonusReward)
		{
			StartCoroutine(ShowBonusReward());
		}
		else
		{
			StartCoroutine(GoToNextScene());
		}
	}

	public virtual void ClaimBonusOnClick()
	{
		bonusReward.gameObject.SetActive(false);
		gameSounds.Play_one_dribble();
		gameSounds.Play_dunk();
		StartCoroutine(GoToNextScene());
	}

	public virtual IEnumerator ShowBonusReward()
	{
		yield return new WaitForSeconds(0.75f);
		bonusReward.gameObject.SetActive(true);
		if (giveBagBonusReward)
		{
			bonusReward.GiveBagReward();
		}
		else
		{
			bonusReward.GiveCashReward();
		}
		yield return new WaitForSeconds(0.75f);
		GameSounds.GetInstance().Play_trumpet_chime_2();
	}

	private void HideCurrencyAnims()
	{
		for (int i = 0; i < currencyAnims.Length; i++)
		{
			currencyAnims[i].gameObject.SetActive(false);
		}
	}

	private IEnumerator GoToNextScene()
	{
		sessionVars.goToTutorial = false;
		yield return new WaitForSeconds(0.15f);
		/*if (showIntAd && AdMediation.IsIntAvail())
		{
			/*FlurryAnalytics.Instance().LogEvent("SHOW_AD", new string[4]
			{
				"num_wins:" + Stats.GetNumWins() + string.Empty,
				"num_losses:" + Stats.GetNumLosses() + string.Empty,
				"current_tour:" + Tournaments.GetCurrentTournamentNum() + string.Empty,
				"num_wins_milestone:" + Stats.GetNumWinsMilestone() + string.Empty
			}, false);#1#
			intCompleted = false;
			yield return new WaitForSeconds(0.95f);
			AdMediation.ShowInt();
			yield return new WaitForSeconds(1.5f);
			if (!intCompleted)
			{
				continueButton.SetActive(true);
				Time.timeScale = 0f;
				yield return new WaitForSeconds(1.5f);
				intCompleted = true;
			}
		}*/
		if (leveledUp)
		{
			yield return new WaitForSeconds(0.25f);
			levelUpScreen.gameObject.SetActive(true);
		}
		else if (Currency.GetNumPrizeBalls() > 0)
		{
			Application.LoadLevel("PrizeBall");
		}
		/*else if (intCompleted && (UnityEngine.Random.Range(0, 100) >= 75 || PlayerPrefs.GetInt("NUM_INT_ADS") == 1))
		{
			removeAdsMsgBox.gameObject.SetActive(true);
		}*/
		else if (PlayerPrefs.GetString("TEAM_NAME") == string.Empty)
		{
			Application.LoadLevel("NameTeam");
		}
		else
		{
			Application.LoadLevel("MainUI");
		}
	}

	public virtual void DoubleRewardsOnClick()
	{
		Time.timeScale = 1f;
		gameSounds.Play_select();
		/*showIntAd = false;*/
		/*FlurryAnalytics.Instance().LogEvent("PLAY_VIDEO_AD", new string[2]
		{
			"type:DOUBLEREWARDS",
			"num_wins_milestone:" + Stats.GetNumWinsMilestone() + string.Empty
		}, false);*/
		//doubleRewardsBox.gameObject.SetActive(false);
		//AdMediation.PlayVid();
	}

	public virtual void AdComplete()
	{
		cashDoubledMessageBox.SetActive(true);
		gameSounds.Play_coin_glow();
		WatchVideoButton.LogVideoView("DOUBLE_REWARD");
	}

	public virtual void AdMessageContinueOnClick()
	{
		Debug.Log("==现金翻倍按钮按下事件==".FL1_HotPink());
		gameSounds.Play_select();
		EarnedXP(totalXpNum);
		Currency.AddCash(totalCashNum);
		cashDoubledMessageBox.SetActive(false);
		//doubleRewardsBox.claimButton.SetActive(true);
		xpDoubledNum.text = (totalXpNum * 2).ToString("n0");
		xpDoubledBox.SetActive(true);
		cashDoubledNum.text = (totalCashNum * 2).ToString("n0");
		cashDoubledBox.SetActive(true);
		gameSounds.Play_coin_glow_2();
	}

	public virtual void ContinueButton()
	{
		/*intCompleted = true;*/
		continueButton.SetActive(false);
		Time.timeScale = 1f;
		gameSounds.Play_one_dribble();
	}

	public virtual void FixedUpdate()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			ContinueButton();
		}
	}

	public virtual void CreateTabNotifications(int tNum)
	{
		NotificationQueue notificationQueue = null;
		GameObject gameObject = GameObject.Find("NotificationQueue");
		if (gameObject != null)
		{
			notificationQueue = (NotificationQueue)gameObject.GetComponent(typeof(NotificationQueue));
			int completionTimeStamp = 100;
			int numWins = Stats.GetNumWins();
			int numCompletions = Tournaments.GetNumCompletions(tNum);
			int currentRound = Tournaments.GetCurrentRound(tNum);
			if (numWins == 4)
			{
				notificationQueue.Add(completionTimeStamp, Notification.TAB_STORE, 0);
			}
		}
	}

	private void EarnedXP(int amount)
	{
		int currentXp = Currency.GetCurrentXp();
		int xpLevelForXp = Currency.GetXpLevelForXp(currentXp);
		int xp = currentXp + amount;
		int xpLevelForXp2 = Currency.GetXpLevelForXp(xp);
		if (xpLevelForXp2 > xpLevelForXp && !leveledUp)
		{
			leveledUp = true;
			Currency.AddGold(Currency.GetCurrentXpLevelGoldReward());
			Currency.AddCash(Currency.GetCurrentXpLevelCashReward());
			Currency.AddPrizeBalls(Currency.GetCurrentXpLevelPrizeBallsReward(), "levelUp");
		}
		Currency.AddXp(amount);
	}

	private void DestroyReplayCameras()
	{
		GameObject gameObject = GameObject.Find("ReplayCamera");
		if (gameObject != null)
		{
			UnityEngine.Object.Destroy(gameObject);
		}
		GameObject gameObject2 = GameObject.Find("RecordCamera");
		if (gameObject2 != null)
		{
			UnityEngine.Object.Destroy(gameObject2);
		}
	}

	private IEnumerator AnimateTextNum(Text t, int num, int speed, bool playSound)
	{
		t.text = "0";
		if (num == 0)
		{
			claimButtonsHolder.SetActive(true);
			yield break;
		}
		yield return new WaitForSeconds(1f);
		bool scaleUp = true;
		for (int i = 0; i <= num; i += speed)
		{
			t.text = i.ToString("n0");
			yield return new WaitForSeconds(0.025f);
			if (playSound)
			{
				gameSounds.Play_light_click_2();
			}
			if (i % 100 == 0)
			{
				if (scaleUp)
				{
					LeanTween.scale(t.gameObject, new Vector3(1.15f, 1.15f, 1f), 0.05f).setEase(LeanTweenType.easeOutExpo);
				}
				else
				{
					LeanTween.scale(t.gameObject, new Vector3(1f, 1f, 1f), 0.05f).setEase(LeanTweenType.easeOutExpo);
				}
				scaleUp = !scaleUp;
			}
		}
		if (playSound)
		{
			gameSounds.Play_light_click_2();
		}
		LeanTween.scale(t.gameObject, new Vector3(1f, 1f, 1f), 1f).setEase(LeanTweenType.easeOutExpo);
		t.text = num.ToString("n0");
		yield return new WaitForSeconds(0.75f);
		claimButtonsHolder.SetActive(true);
	}
}
