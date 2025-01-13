using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class TournamentView : MonoBehaviour
{
	public Tournaments tournaments;

	public Players players;

	public Map map;

	public BracketRoundLabels bracketRoundLabels;

	public GameObject reloadingBox;

	public GameObject[] warningMsgs;

	public GameObject[] playerDetails;

	public GameObject[] upgradeButtons;

	public Image headingBoxImage;

	public Color lightBlueColor;

	public Color redColor;

	public Text tName;

	public GameObject tWomenIcon;

	public GameObject tLiveEventIcon;

	public GameObject tBracketBox;

	public GameObject tLiveEventBox;

	public Text tCashPrize;

	public Text tXpPrize;

	public Text tGoldPrize;

	public GameObject tGoldIcon;

	public Text numWins;

	public GameObject numWinsIcon;

	public Text[] slotTexts;

	public Text slot1a;

	public Text slot1aScore;

	public Text slot1b;

	public Text slot1bScore;

	public Text slot1c;

	public Text slot1cScore;

	public Text slot2a;

	public Text slot2aScore;

	public Text slot2b;

	public Text slot2bScore;

	public Text slot2c;

	public Text slot2cScore;

	public Text slot3a;

	public Text slot3aScore;

	public Text slot3b;

	public Text slot3bScore;

	public Text slot4a;

	public Text slot4aScore;

	public Text slot4b;

	public Text slot4bScore;

	public Text slot5a;

	public Text slot5aScore;

	public Text slot6a;

	public Text slot6aScore;

	public Text slot7a;

	public Text slot7aScore;

	public Text slot8a;

	public Text slot8aScore;

	public GameObject[] highLights;

	public GameObject[] winnerHighlights;

	public PlayButton playButton;

	public AbilityReqBox[] abilityReqBoxes;

	public CoachMsgBox coachMsgBox;

	public TournamentWaitBox tournamentWaitBox;

	public LeaderboardEntries leaderboardEntries;

	public static bool showLeaderboardPanel;

	public LeaderboardPanel leaderboardPanel;

	public static bool showCashAnim;

	public static bool showGoldAnim;

	public CurrencyCollectionAnim cashAnim;

	public CurrencyCollectionAnim goldAnim;

	public CurrencyCollectionAnim xpAnim;

	public TopNavBar topNavBar;

	public GameObject collectCashAnimPrefab;

	public GameObject collectGoldAnimPrefab;

	public GameObject collectXpAnimPrefab;

	private Tournament t;

	private SessionVars sessionVars;

	private GameSounds gameSounds;

	private bool loadedWithoutStarter;

	public virtual void Awake()
	{
		sessionVars = SessionVars.GetInstance();
		gameSounds = GameSounds.GetInstance();
		players.InstantiatePlayers();
	}

	public virtual void OnEnable()
	{
		if (t != null)
		{
			SetViewToTournament(t.num);
		}
	}

	public virtual void Start()
	{
		if (PlayerPrefs.GetInt(MuteButton.SOUND_OFF_PREF_KEY) == 0 && AudioListener.volume == 0f)
		{
			AudioListener.volume = MuteButton.AUDIO_VOLUME;
		}
		HideTournament();
		Tournament tournament = tournaments.GetTournament(Tournaments.GetCurrentTournamentNum());
		if (tournament != null && tournament.currentRound == 4 && !tournament.ReigningChamp() && !Tournaments.TournamentIsCompleted(tournament.num + 1))
		{
			map.MakePinHidden(tournament.num + 1);
		}
	}

	private void HideTournament()
	{
		tBracketBox.SetActive(false);
		tLiveEventBox.SetActive(false);
		base.gameObject.SetActive(false);
	}

	public virtual void OnApplicationPause(bool pause)
	{
		if (!pause)
		{
			CloseTournament();
		}
	}

	public virtual void SetViewToTournament(int tournamentNum)
	{
		t = tournaments.GetTournament(tournamentNum);
		if (!base.gameObject.activeInHierarchy)
		{
			return;
		}
		loadedWithoutStarter = Players.GetActiveStarterNum(t.isFemale) == -1;
		GameObject[] array = this.playerDetails;
		foreach (GameObject gameObject in array)
		{
			PlayerDetails playerDetails = (PlayerDetails)gameObject.GetComponent(typeof(PlayerDetails));
			if (playerDetails != null)
			{
				playerDetails.showingFemales = t.isFemale;
				playerDetails.ShowCorrectPlayer();
			}
		}
		tWomenIcon.SetActive(t.isFemale);
		tName.text = t.name;
		SetupTournament(t.type);
		if (Stats.GetNumWins() >= 1)
		{
			GameObject[] array2 = upgradeButtons;
			foreach (GameObject gameObject2 in array2)
			{
				gameObject2.SetActive(true);
			}
		}
		AdMediation.SetTjLevel(tournamentNum);
	}

	private void SetupTournament(tournamentTypeEnum type)
	{
		tLiveEventIcon.SetActive(type == tournamentTypeEnum.LiveEvent);
		headingBoxImage.color = ((type != tournamentTypeEnum.LiveEvent) ? lightBlueColor : redColor);
		tBracketBox.SetActive(type == tournamentTypeEnum.ThreeRound);
		tLiveEventBox.SetActive(type == tournamentTypeEnum.LiveEvent);
		AbilityReqBox[] array = abilityReqBoxes;
		foreach (AbilityReqBox abilityReqBox in array)
		{
			abilityReqBox.gameObject.SetActive(false);
		}
		GameObject[] array2 = warningMsgs;
		foreach (GameObject gameObject in array2)
		{
			gameObject.SetActive(false);
		}
		tournamentWaitBox.gameObject.SetActive(false);
		if (type == tournamentTypeEnum.ThreeRound)
		{
			SetupThreeRoundBracket();
		}
		else
		{
			StartCoroutine(SetupLiveEvent());
		}
	}

	private IEnumerator SetupLiveEvent()
	{
		StartCoroutine(SetRibbonNumDisplay(0, false));
		playButton.SetToEntriesRemaining(PlayerPrefs.GetInt("ENTRIES_REMAINING"));
		if (!PlayFabManager.Instance().IsClientLoggedIn())
		{
			PlayFabManager.Instance().LoginAsGuest(true);
		}
		else if (showLeaderboardPanel)
		{
			showLeaderboardPanel = false;
			yield return new WaitForSeconds(0.15f);
			leaderboardPanel.gameObject.SetActive(true);
		}
		else
		{
			PlayFabLeaderboard.GetEventLeaderboard(leaderboardEntries, false, false);
		}
	}

	private void SetupThreeRoundBracket()
	{
		int cashPrize = t.cashPrize;
		int num = t.xpPrize;
		int goldPrize = t.goldPrize;
		int num2 = Tournaments.GetNumCompletions(t.num);
		StartCoroutine(ShowBracketProgress(t));
		StartCoroutine(ShowCurrencyAnim());
		if (t.LostLastAttempt())
		{
			playButton.EnableButton();
			playButton.SetToTryAgain();
		}
		else if (t.currentRound == 4)
		{
			if (t.ReigningChamp())
			{
				playButton.EnableButton();
				playButton.SetToPlayAgain();
			}
			else
			{
				GameObject[] array = upgradeButtons;
				foreach (GameObject gameObject in array)
				{
					gameObject.SetActive(false);
				}
				playButton.DisableButton();
				num2--;
			}
			if (Tournaments.GetNumCompletions(t.num) == 1)
			{
				num = t.xpPrizeForFirstWin;
			}
		}
		else if (!loadedWithoutStarter)
		{
			playButton.EnableButton();
			playButton.SetToPlay();
		}
		tCashPrize.text = cashPrize.ToString("n0");
		tXpPrize.text = num.ToString("n0");
		if (goldPrize > 0)
		{
			tGoldIcon.SetActive(true);
			tGoldPrize.text = goldPrize.ToString("n0");
		}
		else
		{
			tGoldIcon.SetActive(false);
			tGoldPrize.text = string.Empty;
		}
		StartCoroutine(SetRibbonNumDisplay(num2, false));
		if (!loadedWithoutStarter)
		{
			Player starter = players.GetStarter(t.isFemale, Players.GetActiveStarterNum(t.isFemale));
			float statTotal = starter.GetStatTotal();
			if (t.GetReqAbility(0) > statTotal)
			{
				abilityReqBoxes[0].gameObject.SetActive(true);
				abilityReqBoxes[0].SetText((int)statTotal, (int)t.GetReqAbility(0));
				playButton.DisableButton();
				warningMsgs[1].SetActive(true);
				if (PlayerPrefs.GetInt("SHOWED_COACHMSGBOX_UPGRADEPLAYER") == 0)
				{
					coachMsgBox.UpgradePlayer();
					coachMsgBox.gameObject.SetActive(true);
				}
			}
			Player backup = players.GetBackup(t.isFemale, Players.GetActiveBackupNum(t.isFemale));
			float statTotal2 = backup.GetStatTotal();
			if (t.GetReqAbility(1) > statTotal2)
			{
				abilityReqBoxes[1].gameObject.SetActive(true);
				abilityReqBoxes[1].SetText((int)statTotal2, (int)t.GetReqAbility(1));
				playButton.DisableButton();
				warningMsgs[1].SetActive(true);
			}
			if (t != null && sessionVars != null && !t.IsCompleted() && t.GetSecondsToWait() > (float)t.GetSecondsWaited(sessionVars.currentTimestamp))
			{
				t.StartWaiting(sessionVars.currentTimestamp);
				tournamentWaitBox.gameObject.SetActive(true);
				tournamentWaitBox.SetSecondsToWait((int)(t.GetSecondsToWait() - (float)t.GetSecondsWaited(sessionVars.currentTimestamp)));
				playButton.DisableButton();
				Debug.Log("t.numt.numt.numt.numt.numt.numt.numt.numt.numt.numt.numt.numt.numt.numt.num: " + t.num);
				if (t.num == 2 || t.num == 5)
				{
					coachMsgBox.WaitForTournament();
					coachMsgBox.gameObject.SetActive(true);
				}
			}
		}
		else
		{
			warningMsgs[0].SetActive(true);
			coachMsgBox.gameObject.SetActive(true);
			if (t.isFemale)
			{
				coachMsgBox.SignFemalePlayer();
			}
			else
			{
				coachMsgBox.SignPlayer();
			}
			playButton.DisableButton();
		}
	}

	private IEnumerator ShowBracketProgress(Tournament t)
	{
		slot1a.text = t.GetSlotTeam(0);
		slot1aScore.text = t.GetSlotScore(0);
		slot1b.text = t.GetSlotTeam(1);
		slot1bScore.text = t.GetSlotScore(1);
		slot1c.text = t.GetSlotTeam(2);
		slot1cScore.text = string.Empty;
		slot2a.text = t.GetSlotTeam(3);
		slot2aScore.text = t.GetSlotScore(3);
		slot2b.text = t.GetSlotTeam(4);
		slot2bScore.text = t.GetSlotScore(4);
		slot2c.text = t.GetSlotTeam(5);
		slot2cScore.text = string.Empty;
		slot3a.text = t.GetSlotTeam(6);
		slot3aScore.text = t.GetSlotScore(6);
		slot3b.text = t.GetSlotTeam(7);
		slot3bScore.text = t.GetSlotScore(7);
		slot4a.text = t.GetSlotTeam(8);
		slot4aScore.text = t.GetSlotScore(8);
		slot4b.text = t.GetSlotTeam(9);
		slot4bScore.text = t.GetSlotScore(9);
		slot5a.text = t.GetSlotTeam(10);
		slot5aScore.text = t.GetSlotScore(10);
		slot6a.text = t.GetSlotTeam(11);
		slot6aScore.text = t.GetSlotScore(11);
		slot7a.text = t.GetSlotTeam(12);
		slot7aScore.text = t.GetSlotScore(12);
		slot8a.text = t.GetSlotTeam(13);
		slot8aScore.text = t.GetSlotScore(13);
		for (int i = 0; i < highLights.Length; i++)
		{
			highLights[i].SetActive(i + 1 <= t.currentRound);
		}
		GameObject[] array = winnerHighlights;
		foreach (GameObject gameObject in array)
		{
			gameObject.SetActive(false);
		}
		tBracketBox.SetActive(true);
		if (this.t.ReigningChamp())
		{
			slot1cScore.text = t.GetSlotScore(2);
			slot2cScore.text = t.GetSlotScore(5);
			winnerHighlights[0].SetActive(true);
			bracketRoundLabels.SetCurrentRound(t.currentRound - 1, this.t.LostLastAttempt(), false);
			yield break;
		}
		if (this.t.currentRound != 4)
		{
			slot1cScore.text = t.GetSlotScore(2);
			slot2cScore.text = t.GetSlotScore(5);
			if (showCashAnim && !this.t.LostLastAttempt())
			{
				bracketRoundLabels.SetCurrentRound(t.currentRound - 1, this.t.LostLastAttempt(), t.currentRound == 3);
				yield return new WaitForSeconds(4.25f);
				gameSounds.Play_light_click_2();
			}
			bracketRoundLabels.SetCurrentRound(t.currentRound, this.t.LostLastAttempt(), false);
			yield break;
		}
		bracketRoundLabels.SetCurrentRound(t.currentRound, this.t.LostLastAttempt(), false);
		yield return new WaitForSeconds(4f);
		gameSounds.Play_unselect();
		slot1cScore.text = t.GetSlotScore(2);
		yield return new WaitForSeconds(0.35f);
		gameSounds.Play_unselect();
		slot2cScore.text = t.GetSlotScore(5);
		yield return new WaitForSeconds(0.35f);
		gameSounds.Play_chime_shimmer();
		yield return new WaitForSeconds(0.15f);
		winnerHighlights[0].SetActive(true);
		StartCoroutine(SetRibbonNumDisplay(Tournaments.GetNumCompletions(this.t.num), true));
		yield return new WaitForSeconds(2.5f);
		t.SetReigningChamp(true);
		int nextTournamentNum = t.num + 1;
		if (t.num == 3)
		{
			nextTournamentNum = 1;
		}
		else if (t.num == 2)
		{
			nextTournamentNum = 4;
		}
		Tournament nextTournament = tournaments.GetTournament(nextTournamentNum);
		if (nextTournament.IsVisible())
		{
			map.gameObject.SendMessage("SegueToPin", nextTournamentNum);
		}
		HideTournament();
	}

	private IEnumerator ShowCurrencyAnim()
	{
		yield return new WaitForSeconds(0.25f);
		if (showCashAnim)
		{
			showCashAnim = false;
			ShowCurrencyAnimPrefab(collectCashAnimPrefab, topNavBar.cashIcon.transform);
			yield return new WaitForSeconds(1.25f);
			ShowCurrencyAnimPrefab(collectXpAnimPrefab, topNavBar.xpLevelNum.transform);
			yield return new WaitForSeconds(1.25f);
		}
		if (showGoldAnim)
		{
			showGoldAnim = false;
			ShowCurrencyAnimPrefab(collectGoldAnimPrefab, topNavBar.goldIcon.transform);
		}
	}

	private void ShowCurrencyAnimPrefab(GameObject prefab, Transform dest)
	{
		Vector3 vector = default(Vector3);
		vector = ((t.currentRound == 4) ? slot1c.transform.position : ((t.currentRound != 3) ? slot7a.transform.position : slot3b.transform.position));
		vector += new Vector3(1.9f, 1.2f, 0f);
		CurrencyCollectionAnim currencyCollectionAnim = (CurrencyCollectionAnim)UnityEngine.Object.Instantiate(prefab, prefab.transform.position, prefab.transform.rotation).GetComponent(typeof(CurrencyCollectionAnim));
		currencyCollectionAnim.transform.position = vector;
		currencyCollectionAnim.destination = dest;
		currencyCollectionAnim.transform.parent = topNavBar.transform;
		gameSounds.Play_coin_glow_2();
	}

	public virtual IEnumerator ReloadBracket()
	{
		reloadingBox.SetActive(true);
		playButton.DisableButton();
		t.ReloadBracket();
		yield return new WaitForSeconds(1f);
		SetViewToTournament(t.num);
		reloadingBox.SetActive(false);
		playButton.SetToPlay();
		playButton.EnableButton();
	}

	public virtual void CloseTournament()
	{
		if (gameSounds != null)
		{
			gameSounds.Play_select();
		}
		if (coachMsgBox.hintArrows != null && coachMsgBox.hintArrows.Length >= 3)
		{
			coachMsgBox.hintArrows[1].SetActive(false);
			coachMsgBox.hintArrows[2].SetActive(false);
		}
		playButton.SetToPlay();
		playButton.EnableButton();
		tBracketBox.SetActive(false);
		tLiveEventBox.SetActive(false);
		base.gameObject.SetActive(false);
	}

	private IEnumerator SetRibbonNumDisplay(int numWins, bool animate)
	{
		if (numWins > 0)
		{
			if (numWins > 3)
			{
				this.numWins.text = "3/3 + " + (numWins - 3);
			}
			else
			{
				this.numWins.text = string.Empty + numWins + "/3";
			}
			numWinsIcon.SetActive(true);
			if (animate)
			{
				LeanTween.scale(numWinsIcon, new Vector3(1.2f, 1.2f, 1f), 0.3f).setEase(LeanTweenType.easeOutQuad);
				LeanTween.scale(this.numWins.gameObject, new Vector3(1.2f, 1.2f, 1f), 0.3f).setEase(LeanTweenType.easeOutQuad);
				yield return new WaitForSeconds(0.35f);
				LeanTween.scale(numWinsIcon, new Vector3(1f, 1f, 1f), 0.3f).setEase(LeanTweenType.easeInQuad);
				LeanTween.scale(this.numWins.gameObject, new Vector3(1f, 1f, 1f), 0.3f).setEase(LeanTweenType.easeInQuad);
			}
		}
		else
		{
			this.numWins.text = string.Empty;
			numWinsIcon.SetActive(false);
		}
	}

	public virtual void UpgradeWasCompleted()
	{
		SetViewToTournament(t.num);
		GameObject[] array = playerDetails;
		foreach (GameObject gameObject in array)
		{
			gameObject.SetActive(false);
			gameObject.SetActive(true);
		}
	}

	public virtual IEnumerator AnimateUpgradeReq()
	{
		AbilityReqBox[] array = abilityReqBoxes;
		foreach (AbilityReqBox box in array)
		{
			Vector2 curScale = new Vector2(box.gameObject.transform.localScale.x, box.gameObject.transform.localScale.y);
			LeanTween.scale(box.gameObject, new Vector3(curScale.x * 1.25f, curScale.y * 1.25f), 0.25f).setEase(LeanTweenType.easeOutQuad);
			yield return new WaitForSeconds(0.25f);
			LeanTween.scale(box.gameObject, new Vector3(curScale.x, curScale.y), 0.25f).setEase(LeanTweenType.easeOutQuad);
		}
	}

	public virtual void Update()
	{
	}

	public virtual void FixedUpdate()
	{
		if (Input.GetKeyUp(KeyCode.Escape))
		{
			CloseTournament();
		}
	}
}
