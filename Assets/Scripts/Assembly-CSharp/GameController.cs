using System;
using System.Collections;
using System.Collections.Generic;
using I2.Loc;
using Moments;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class GameController : MonoBehaviour
{
	public GameObject[] replayObjects;

	public Recorder recorder;

	public Text replayTopText;

	public Localize tournamentName;

	public Fps fps;

	public Tournaments tournaments;

	private Tournament tournament;

	public GameScore score;

	public GameObject scoreBoard;

	public ScoreboardTimeouts score1Timeouts;

	public GameObject startMsg;

	public GameObject youWonMsg;

	public Text currentHalfLabel;

	public Text debugText;

	private int debugPresses;

	public GameObject[] singlePlayerOnlyObjects;

	public GameObject[] twoPlayerOnlyObjects;

	public GameObject[] twoPlayerWinnerMsgs;

	public GameObject halfTime;

	public GameObject timeoutScreen;

	public GameHintMsg hintMessage;

	public GameHintMsg hintMessageTopOfScreen;

	private bool isShowingHint;

	private float secShowingHint;

	private bool showedEnduranceHint;

	public GameObject[] hintArrows;

	public GameControls gameControls;

	public GameObject earlyR;

	public GameObject lateR;

	public GameObject earlyRP1;

	public GameObject earlyRP2;

	public GameObject lateRP1;

	public GameObject lateRP2;

	public Tutorial tutorialGUI;

	public PauseButton pauseButton;

	public GameObject pauseDialog;

	public TimeoutButton timeoutButton;

	public GameObject[] powerupIcons;

	public bool playingGame;

	public PlayerController playerController;

	public PlayerVisual playerVisual;

	public PlayerController enemyPlayerController;

	public PlayerVisual enemyPlayerVisual;

	public Background background;

	public Background background2;

	public GameRoster gameRoster;

	public GameComputer gameComputer;

	public Tail ballTail;

	public Shadow ballShadow;

	public GameObject ballPrefab;

	public Ball ball;

	public IkBall ikBall;

	private int playToScore;

	public int numConsecutiveEarlyReleases;

	private bool shooting3PT;

	private bool shootingLayup;

	private bool dunkTriggered;

	private bool p1ShotBall;

	private bool p1ScoredLastPoint;

	private bool p2ScoredLastPoint;

	public bool twoPlayerMode;

	public GameObject cameraSinglePlayer;

	public GameObject cameraPlayer1;

	public GameObject cameraPlayer2;

	private bool inTutorial;

	private bool inScrimmage;

	public bool showingMsg;

	public bool gameIsOver;

	public int numTutorialBalls;

	public VoiceOvers voiceOvers;

	public GameNoise gameNoise;

	private GameSounds gameSounds;

	private SessionVars sessionVars;

	private Music music;

	private bool isLiveEvent;

	private int currentHalf = 1;

	private int numPlaysInSecondHalf;

	private int timeouts;

	private float totalSeconds;

	private float gameSeconds;

	private float gamePlayTimeScale = 1f;

	private CustomItems customItems;

	private CharacterSprites characterSprites;

	private bool justStartedRecording;

	private bool settingUpNewPlay;

	private float settingUpNewPlayTime;

	private int arenaNum;

	public bool InTutorial
	{
		get
		{
			return inTutorial;
		}
	}

	public bool InScrimmage
	{
		get
		{
			return inScrimmage;
		}
	}

	public virtual void Awake()
	{
		GameObject gameObject = GameObject.Find("CustomItems");
		if (gameObject != null)
		{
			customItems = (CustomItems)gameObject.GetComponent(typeof(CustomItems));
		}
		characterSprites = (CharacterSprites)GameObject.Find("CharacterSprites").GetComponent(typeof(CharacterSprites));
		if (PlayerPrefs.GetInt("GAMEPLAY_TIMESCALE_INCREASED") == 6)
		{
			gamePlayTimeScale = 0.75f;
		}
		else if (PlayerPrefs.GetInt("GAMEPLAY_TIMESCALE_INCREASED") == 5)
		{
			gamePlayTimeScale = 1f;
		}
		else if (PlayerPrefs.GetInt("GAMEPLAY_TIMESCALE_INCREASED") == 4)
		{
			gamePlayTimeScale = 1.3f;
		}
		else if (PlayerPrefs.GetInt("GAMEPLAY_TIMESCALE_INCREASED") == 3)
		{
			gamePlayTimeScale = 1.2f;
		}
		else if (PlayerPrefs.GetInt("GAMEPLAY_TIMESCALE_INCREASED") == 2)
		{
			gamePlayTimeScale = 1.1f;
		}
		else if (PlayerPrefs.GetInt("GAMEPLAY_TIMESCALE_INCREASED") == 1)
		{
			gamePlayTimeScale = 1.05f;
		}
		else
		{
			gamePlayTimeScale = 1f;
		}
	}

	public virtual void Start()
	{
		sessionVars = SessionVars.GetInstance();
		gameSounds = GameSounds.GetInstance();
		GameObject gameObject = GameObject.Find("Music");
		if (gameObject != null)
		{
			music = (Music)gameObject.GetComponent(typeof(Music));
		}
		twoPlayerMode = sessionVars.twoPlayerMode;
		if (PlayerPrefs.GetInt("RAN_TOURNAMENT") == 1)
		{
			tournament = tournaments.GetTournament(UnityEngine.Random.Range(0, 5));
			gameRoster.SetStarterPlayerNum(UnityEngine.Random.Range(0, 8));
			gameComputer.SetTournament(tournament);
		}
		else
		{
			tournament = tournaments.GetTournament(Tournaments.GetCurrentTournamentNum());
		}
		if (twoPlayerMode)
		{
			GameObject[] array = singlePlayerOnlyObjects;
			foreach (GameObject gameObject2 in array)
			{
				gameObject2.SetActive(false);
			}
			GameObject[] array2 = replayObjects;
			foreach (GameObject gameObject3 in array2)
			{
				if (gameObject3 != null)
				{
					UnityEngine.Object.Destroy(gameObject3);
				}
			}
			if (DtUtils.IsSuperWideScreenDevice())
			{
				RectTransform rectTransform = (RectTransform)scoreBoard.GetComponent(typeof(RectTransform));
				if (rectTransform != null)
				{
					rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y + 205f);
				}
			}
			arenaNum = 69;
		}
		else
		{
			isLiveEvent = tournament.type == tournamentTypeEnum.LiveEvent && !sessionVars.goToTutorial && !inScrimmage && !sessionVars.goToPractice;
			GameObject[] array3 = twoPlayerOnlyObjects;
			foreach (GameObject gameObject4 in array3)
			{
				gameObject4.SetActive(false);
			}
			arenaNum = tournament.arena;
		}
		twoPlayerWinnerMsgs[0].SetActive(false);
		twoPlayerWinnerMsgs[1].SetActive(false);
		earlyR.SetActive(false);
		lateR.SetActive(false);
		earlyRP1.SetActive(false);
		earlyRP2.SetActive(false);
		lateRP1.SetActive(false);
		lateRP2.SetActive(false);
		timeoutScreen.SetActive(false);
		halfTime.SetActive(false);
		for (int l = 0; l < powerupIcons.Length; l++)
		{
			powerupIcons[l].SetActive(sessionVars.usingPowerups[l]);
		}
		debugText.gameObject.SetActive(false);
		if (sessionVars.goToPractice)
		{
			arenaNum = 27;
			StartCoroutine(NewPracticeGame());
		}
		else if (sessionVars.goToTutorial)
		{
			arenaNum = ArenaChooser.GetTutorialArena();
			NewTutorialGame();
		}
		else
		{
			StartCoroutine(NewGame());
		}
		((BallVisual)ikBall.gameObject.GetComponent(typeof(BallVisual))).SetVisual(characterSprites, customItems);
		showedEnduranceHint = PlayerPrefs.GetInt("SHOWED_ENDURANCE") != 0;
		ArenaSkinController.Instance.UpdateArenaSkin(arenaNum);
	}

	public virtual void Update()
	{
		totalSeconds += Time.deltaTime;
		if (playingGame)
		{
			gameSeconds += Time.deltaTime;
		}
		if (isShowingHint)
		{
			secShowingHint += Time.deltaTime;
			if (secShowingHint >= 6.5f)
			{
				CloseHint();
			}
		}
		if (settingUpNewPlay)
		{
			settingUpNewPlayTime += Time.deltaTime;
			if (settingUpNewPlayTime > 6f)
			{
				//FlurryAnalytics.Instance().LogEvent("GAME_FROZE_DURING_TIPOFF", new string[1] { "sessions:" + Stats.GetNumSessions() + string.Empty }, false);
				StartCoroutine(NewPlay(false));
			}
		}
		else
		{
			settingUpNewPlayTime = 0f;
		}
	}

	public virtual IEnumerator NewPracticeGame()
	{
		inTutorial = false;
		StartCoroutine(gameNoise.PlayBackgroundCrowd());
		voiceOvers.Mute();
		float x = 9.08f;
		Vector3 position = enemyPlayerController.gameObject.transform.position;
		position.x = x;
		enemyPlayerController.gameObject.transform.position = position;
		enemyPlayerController.gameObject.SetActive(false);
		//FlurryAnalytics.Instance().LogEvent("start_practice_game");
		SetPlayerStats();
		playerController.Reset();
		timeouts = 3;
		currentHalf = 1;
		currentHalfLabel.text = ((!isLiveEvent) ? "1" : "X");
		if (PlayerPrefs.GetInt("DEVMODE") == 1)
		{
			playToScore = 6;
		}
		else
		{
			playToScore = 11;
		}
		score1Timeouts.SetNumTimeouts(timeouts);
		cameraSinglePlayer.SetActive(true);
		cameraSinglePlayer.SendMessage("AdjustGameControls");
		cameraPlayer1.SetActive(false);
		cameraPlayer2.SetActive(false);
		SetPlayerSprites();
		SetPlayerStats();
		playerController.NewGame();
		if (enemyPlayerController.enabled)
		{
			enemyPlayerController.NewGame();
		}
		Stats.NewGame();
		StartCoroutine(NewPlay(false));
		yield return new WaitForSeconds(0.5f);
		score.Reset();
	}

	public virtual void NewTutorialGame()
	{
		Time.timeScale = gamePlayTimeScale;
		inTutorial = true;
		voiceOvers.Mute();
		tutorialGUI.gameObject.SetActive(true);
		GameObject[] array = singlePlayerOnlyObjects;
		foreach (GameObject gameObject in array)
		{
			gameObject.SetActive(false);
		}
		GameObject[] array2 = twoPlayerOnlyObjects;
		foreach (GameObject gameObject2 in array2)
		{
			gameObject2.SetActive(false);
		}
		float x = 9.08f;
		Vector3 position = enemyPlayerController.gameObject.transform.position;
		position.x = x;
		enemyPlayerController.gameObject.transform.position = position;
		pauseButton.gameObject.SetActive(true);
		pauseButton.quitButton.SetActive(false);
		enemyPlayerController.gameObject.SetActive(false);
		startMsg.SetActive(false);
		//FlurryAnalytics.Instance().LogEvent("start_tut");
		PlayerPrefsHelper.SetInt("STARTED_TUTORIAL", 1);
		PlayFabManager.Instance().SetUserDataForKey("STARTED_TUTORIAL", 1);
		SetPlayerStats();
		playerController.Reset();
		ballTail.trailRenderer.enabled = false;
		playingGame = true;
		if (sessionVars.goToScrimmage)
		{
			gameControls.gameObject.SetActive(false);
			StartCoroutine(tutorialGUI.TutorialCompleted());
		}
	}

	public virtual IEnumerator NewTutorialPlay()
	{
		numTutorialBalls++;
		if (numTutorialBalls >= 7)
		{
			playingGame = false;
			gameControls.gameObject.SetActive(false);
			StartCoroutine(tutorialGUI.TutorialCompleted());
			yield break;
		}
		if (this.ball != null)
		{
			UnityEngine.Object.Destroy(this.ball.gameObject);
		}
		playingGame = true;
		Ball ball = (Ball)UnityEngine.Object.Instantiate(ballPrefab, ballPrefab.transform.position, ballPrefab.transform.rotation).GetComponent(typeof(Ball));
		float x = 9f;
		Vector3 position = ball.gameObject.transform.position;
		position.x = x;
		ball.gameObject.transform.position = position;
		float y = -1.5f;
		Vector3 position2 = ball.gameObject.transform.position;
		position2.y = y;
		ball.gameObject.transform.position = position2;
		Rigidbody2D ballRb2D = ball.gameObject.GetComponent<Rigidbody2D>();
		if (numTutorialBalls % 3 == 0)
		{
			ballRb2D.velocity = new Vector2(-6.5f, 30f);
			ballRb2D.angularVelocity = 1850f;
		}
		else if (numTutorialBalls % 2 == 0)
		{
			ballRb2D.velocity = new Vector2(-6f, 12.5f);
			ballRb2D.angularVelocity = 150f;
		}
		else
		{
			ballRb2D.velocity = new Vector2(-3f, 24f);
			ballRb2D.angularVelocity = 100f;
		}
		this.ball = ball;
		this.ball.gameSounds = gameSounds;
		ballShadow.startingYOffset = 0f;
		ikBall.FixToTarget(ball.gameObject);
		((BallVisual)ikBall.GetComponent(typeof(BallVisual))).SetSortingLayerName("Player");
		yield return new WaitForSeconds(0.15f);
		ballTail.trailRenderer.enabled = true;
	}

	public virtual void NewScrimmageGame()
	{
		arenaNum = ArenaChooser.GetScrimmageArena();
		ArenaSkinController.Instance.UpdateArenaSkin(arenaNum);
		gameSounds.Play_select();
		voiceOvers.UnMute();
		if (music != null)
		{
			music.FadeOutMusic();
		}
		StartCoroutine(gameNoise.PlayBackgroundCrowd());
		voiceOvers.Play_HereIsTheTipoff01();
		tutorialGUI.gameObject.SetActive(false);
		gameControls.gameObject.SetActive(true);
		inTutorial = false;
		inScrimmage = true;
		playToScore = 7;
		GameObject[] array = singlePlayerOnlyObjects;
		foreach (GameObject gameObject in array)
		{
			gameObject.SetActive(false);
		}
		GameObject[] array2 = twoPlayerOnlyObjects;
		foreach (GameObject gameObject2 in array2)
		{
			gameObject2.SetActive(false);
		}
		pauseButton.gameObject.SetActive(true);
		pauseButton.quitButton.SetActive(false);
		currentHalf = 1;
		gameSeconds = 0f;
		currentHalfLabel.text = "1";
		enemyPlayerController.isComputer = true;
		cameraSinglePlayer.SetActive(true);
		cameraSinglePlayer.SendMessage("AdjustGameControls");
		cameraPlayer1.SetActive(false);
		cameraPlayer2.SetActive(false);
		/*FlurryAnalytics.Instance().LogEvent("new_scrimmage_1v1", new string[4]
		{
			"current_tour:" + Tournaments.GetCurrentTournamentNum() + string.Empty,
			"sessions:" + Stats.GetNumSessions() + string.Empty,
			"seconds:" + DoubleTapUtils.GetRoundedSeconds((int)totalSeconds) + string.Empty,
			"fps: " + Fps.GetFpsAverageForAnalytics()
		}, false);*/
		PlayerPrefsHelper.SetInt("STARTED_SCRIM", 1);
		PlayFabManager.Instance().SetUserDataForKey("STARTED_SCRIM", 1);
		Stats.NewGame();
		enemyPlayerController.gameObject.SetActive(true);
		scoreBoard.SetActive(true);
		score.Reset();
		playerController.NewGame();
		if (enemyPlayerController.enabled)
		{
			enemyPlayerController.NewGame();
		}
		StartCoroutine(NewPlay(false));
		PlayFabManager.Instance().SetUserDataForKey("BUILD_NUMBER_AT_FIRST_LAUNCH", PlayerPrefs.GetInt("BUILD_NUMBER_AT_FIRST_LAUNCH"));
	}

	public virtual IEnumerator NewGame()
	{
		StartCoroutine(gameNoise.PlayBackgroundCrowd());
		voiceOvers.Play_HereIsTheTipoff01();
		inTutorial = false;
		timeouts = 3;
		currentHalf = 1;
		currentHalfLabel.text = ((!isLiveEvent) ? "1" : "X");
		if (isLiveEvent)
		{
			playToScore = 600;
			PlayerPrefsHelper.SetInt("ENTRIES_REMAINING", PlayerPrefs.GetInt("ENTRIES_REMAINING") - 1);
		}
		else if (PlayerPrefs.GetInt("DEVMODE") == 1)
		{
			playToScore = 6;
		}
		else
		{
			playToScore = 11;
		}
		if (twoPlayerMode)
		{
			enemyPlayerController.isComputer = false;
			enemyPlayerController.computerAI.enabled = false;
			cameraSinglePlayer.SetActive(false);
			cameraPlayer1.SetActive(true);
			cameraPlayer2.SetActive(true);
			playerController.SetSize(1f);
			enemyPlayerController.SetSize(1f);
			playerController.SetJumpForce(480f);
			enemyPlayerController.SetJumpForce(480f);
			playerController.SetEnergyRegenerationSpeed(1.5f);
			enemyPlayerController.SetEnergyRegenerationSpeed(1.5f);
			playerController.shooter.SetPlayerShootingArch(80f);
			enemyPlayerController.shooter.SetPlayerShootingArch(80f);
			playerController.SetDefendedMultiplier(0.45f);
			enemyPlayerController.SetDefendedMultiplier(0.45f);
			SetPlayerSprites();
			score1Timeouts.SetNumTimeouts(0);
			//FlurryAnalytics.Instance().LogEvent("new_game_1v1_2player");
		}
		else
		{
			enemyPlayerController.isComputer = true;
			score1Timeouts.SetNumTimeouts(timeouts);
			/*FlurryAnalytics.Instance().LogEvent("new_game_1v1", new string[4]
			{
				"num_wins:" + Stats.GetNumWins() + string.Empty,
				"num_losses:" + Stats.GetNumLosses() + string.Empty,
				"current_tour:" + Tournaments.GetCurrentTournamentNum() + string.Empty,
				"sessions:" + Stats.GetNumSessions() + string.Empty
			}, false);*/
			cameraSinglePlayer.SetActive(true);
			cameraSinglePlayer.SendMessage("AdjustGameControls");
			cameraPlayer1.SetActive(false);
			cameraPlayer2.SetActive(false);
			SetPlayerSprites();
			SetPlayerStats();
		}
		Stats.NewGame();
		playerController.NewGame();
		if (enemyPlayerController.enabled)
		{
			enemyPlayerController.NewGame();
		}
		StartCoroutine(NewPlay(false));
		if (PlayerPrefs.GetInt("SCOREBOARD_OFF") == 1)
		{
			scoreBoard.SetActive(false);
		}
		yield return new WaitForSeconds(0.5f);
		score.Reset();
		yield return new WaitForSeconds(2f);
		if (!twoPlayerMode)
		{
			voiceOvers.PlayCurrentRound(tournament.currentRound);
		}
	}

	public virtual IEnumerator NewPlay(bool comingFromTimeout)
	{
		float xDir2 = 0f;
		float yDir2 = 0f;
		/*FlurryAnalytics.Instance().LogEvent("new_play_1v1");
		if (PlayerPrefs.GetInt("RAN_TOURNAMENT_EACH_NEWPLAY") == 1)
		{
			tournament = tournaments.GetTournament(UnityEngine.Random.Range(0, 5));
			gameRoster.SetStarterPlayerNum(UnityEngine.Random.Range(0, 8));
			gameComputer.SetTournament(tournament);
			SetPlayerSprites();
			SetPlayerStats();
		}*/
		int scoreTotal = score.player1Score + score.player2Score;
		bool firstPlayInGame = scoreTotal == 0 && !comingFromTimeout;
		if (!twoPlayerMode)
		{
			int cpuScore = ((!isLiveEvent) ? score.player2Score : 0);
			gameComputer.SetComputerAttributes(score.player1Score, cpuScore, playToScore, inScrimmage, currentHalf);
		}
		SetDebugText();
		timeoutButton.SetHasBall(false);
		SetPlayerSprites();
		SetPlayerStats();
		if (currentHalf == 2)
		{
			numPlaysInSecondHalf++;
		}
		if (!settingUpNewPlay)
		{
			settingUpNewPlay = true;
			if (this.ball != null)
			{
				UnityEngine.Object.Destroy(this.ball.gameObject);
			}
			Ball ball = (Ball)UnityEngine.Object.Instantiate(ballPrefab, ballPrefab.transform.position, ballPrefab.transform.rotation).GetComponent(typeof(Ball));
			this.ball = ball;
		}
		else
		{
			this.ball = (Ball)GameObject.Find("Ball(Clone)").GetComponent(typeof(Ball));
		}
		this.ball.gameSounds = gameSounds;
		this.ball.isTipoff = true;
		ballShadow.startingYOffset = 0f;
		ikBall.FixToTarget(this.ball.gameObject);
		((BallVisual)ikBall.GetComponent(typeof(BallVisual))).SetSortingLayerName("Player");
		Rigidbody2D ballRb2D = this.ball.gameObject.GetComponent<Rigidbody2D>();
		ballRb2D.bodyType = RigidbodyType2D.Static;
		ballRb2D.angularVelocity = 150f;
		bool ballToP1 = false;
		bool ballToP2 = false;
		bool extraHighBall = false;
		bool hardForUserToGet = tournament.num >= 6;
		if ((numPlaysInSecondHalf == 1 && !twoPlayerMode && !inScrimmage && !isLiveEvent) || (firstPlayInGame && !inScrimmage))
		{
			xDir2 = 0f;
			yDir2 = 18f;
		}
		else if (comingFromTimeout || p2ScoredLastPoint)
		{
			xDir2 = -0.95f;
			yDir2 = ((UnityEngine.Random.Range(0, 100) < 50) ? 16f : 11f);
			ballToP1 = true;
		}
		else
		{
			extraHighBall = UnityEngine.Random.Range(0, 100) > 50;
			if (extraHighBall)
			{
				xDir2 = ((!hardForUserToGet) ? 1.3f : 1.7f);
				yDir2 = ((!hardForUserToGet) ? 17f : 12f);
			}
			else
			{
				xDir2 = ((!hardForUserToGet) ? 1.3f : 1.9f);
				yDir2 = ((!hardForUserToGet) ? 13f : 10f);
			}
			ballToP2 = true;
		}
		if (sessionVars.goToPractice && UnityEngine.Random.Range(0, 100) >= 50)
		{
			xDir2 *= -1f;
		}
		playerController.Reset();
		if (enemyPlayerController.gameObject.activeInHierarchy && enemyPlayerController.enabled)
		{
			enemyPlayerController.Reset();
		}
		TurnOnRecordingObjects();
		CheckPerformance();
		if (!isShowingHint && !twoPlayerMode)
		{
			if (scoreTotal >= 2 && ballToP1 && PlayerPrefs.GetInt("SHOWED_THREES") == 0)
			{
				ShowHint(true);
				hintMessageTopOfScreen.Threes();
				PlayerPrefsHelper.SetInt("SHOWED_THREES", 1);
				PlayFabManager.Instance().SetUserDataForKey("SHOWED_THREES", 1);
			}
			else if (score.player1Score >= 4 && ballToP2 && PlayerPrefs.GetInt("SHOWED_STEAL") < 2)
			{
				ShowHint(false);
				if (PlayerPrefs.GetInt("SHOWED_STEAL") == 0)
				{
					hintMessage.Stealing();
				}
				else
				{
					hintMessage.Stealing2();
				}
				PlayerPrefsHelper.SetInt("SHOWED_STEAL", PlayerPrefs.GetInt("SHOWED_STEAL") + 1);
				PlayFabManager.Instance().SetUserDataForKey("SHOWED_STEAL", PlayerPrefs.GetInt("SHOWED_STEAL") + 1);
			}
			else if (scoreTotal >= 2 && ballToP1 && !inScrimmage && PlayerPrefs.GetInt("SHOWED_PUMPFAKE") == 0)
			{
				ShowHint(true);
				hintMessageTopOfScreen.PumpFake();
				PlayerPrefsHelper.SetInt("SHOWED_PUMPFAKE", 1);
			}
		}
		if (firstPlayInGame)
		{
			yield return new WaitForSeconds(1.5f);
		}
		else if (numPlaysInSecondHalf == 1)
		{
			yield return new WaitForSeconds(1f);
			if (!twoPlayerMode && !inScrimmage && !isLiveEvent)
			{
				voiceOvers.PlayStartOfSecHalf(tournament.currentRound);
			}
			yield return new WaitForSeconds(0.5f);
		}
		else
		{
			if (score.player1Score + 2 >= playToScore && score.player2Score + 2 >= playToScore)
			{
				StartCoroutine(voiceOvers.PlayGamePoint());
			}
			else if (ballToP1 && score.player1Score + 2 >= playToScore)
			{
				voiceOvers.Play_CanWinWithABucketHere01();
			}
			else if (ballToP2 && score.player2Score + 2 >= playToScore)
			{
				voiceOvers.Play_CanWinWithABucketHere01();
			}
			else if (ballToP1 && score.player1Score + 3 >= playToScore)
			{
				voiceOvers.Play_AThreeHereWinsTheGame01();
			}
			else if (score.player1Score == score.player2Score && score.player1Score >= 2)
			{
				StartCoroutine(voiceOvers.PlayTied());
			}
			else if (Mathf.Abs(score.player1Score - score.player2Score) <= 3 && (score.player1Score >= 8 || score.player2Score >= 8))
			{
				StartCoroutine(voiceOvers.PlayOnePosGame());
			}
			yield return new WaitForSeconds(0.25f);
			if (justStartedRecording && !comingFromTimeout)
			{
				justStartedRecording = false;
				yield return new WaitForSeconds(1.75f);
			}
		}
		ballTail.trailRenderer.enabled = true;
		playingGame = true;
		Time.timeScale = gamePlayTimeScale;
		ballRb2D.bodyType = RigidbodyType2D.Dynamic;
		ballRb2D.velocity = new Vector2(xDir2, yDir2);
		ballRb2D.angularVelocity = xDir2 * 500f;
		StartCoroutine(gameNoise.PlayBgSqueaks());
		playerController.OnTipOff(ballToP1, ballToP2, extraHighBall, hardForUserToGet);
		if (enemyPlayerController.gameObject.activeInHierarchy)
		{
			enemyPlayerController.OnTipOff(ballToP1, ballToP2, extraHighBall, hardForUserToGet);
		}
		AdMediation.HideTopBanner();
		AdMediation.HideCenterBanner();
		settingUpNewPlay = false;
	}

	private void TurnOnRecordingObjects()
	{
		if (twoPlayerMode || score.player1Score + 3 < playToScore)
		{
			return;
		}
		float num = 48f;
		float fpsTotAverage = fps.GetFpsTotAverage();
		num = 35f;
		num = 10f;
		if (fpsTotAverage >= num && PlayerPrefs.GetInt("DISABLE_REPLAYS") != 1)
		{
			if (recorder.gameObject != null && recorder.enabled)
			{
				recorder.FlushMemory();
				recorder.Record();
			}
			GameObject[] array = replayObjects;
			foreach (GameObject gameObject in array)
			{
				if (gameObject != null)
				{
					gameObject.SetActive(true);
				}
			}
			if (sessionVars.goToPractice)
			{
				replayTopText.text = PlayerPrefs.GetString("TEAM_NAME").ToUpper();
				tournamentName.gameObject.SetActive(false);
			}
			else if (inScrimmage)
			{
				replayTopText.text = "GAME WINNER";
				tournamentName.gameObject.SetActive(false);
			}
			else
			{
				replayTopText.text = PlayerPrefs.GetString("TEAM_NAME").ToUpper() + ": GAME WINNER";
				if (replayTopText.text.Length >= 25 && !DtUtils.IsWideScreenDevice())
				{
					replayTopText.fontSize = 70;
				}
				tournamentName.SetTerm(tournament.GetCurrentRoundName(), null);
			}
			justStartedRecording = true;
			return;
		}
		GameObject[] array2 = replayObjects;
		foreach (GameObject gameObject2 in array2)
		{
			if (gameObject2 != null)
			{
				UnityEngine.Object.Destroy(gameObject2);
			}
		}
	}

	private void CheckPerformance()
	{
		float num = 48f;
		num = 20f;
		num = 10f;
		float fpsTotAverage = fps.GetFpsTotAverage();
		if (fpsTotAverage > 1f && fpsTotAverage < 999f && !float.IsNaN(fpsTotAverage) && fpsTotAverage < num)
		{
			ballTail.trailRenderer.enabled = false;
		}
	}

	public virtual void ForfeitGame()
	{
		Time.timeScale = 1f;
		if (!AdMediation.IsIntAvail() && PlayerPrefs.GetInt("ADS_OFF") != 1 && (PlayerPrefs.GetInt("NUM_PURCHASES") == 0 || PlayerPrefs.GetInt("IS_FRAUDULENT_USER") == 1))
		{
			AdMediation.ReqInt();
		}
		EndGame(true);
	}

	public virtual void EndGame(bool forfeit)
	{
		gameSounds.Play_bball_buzzer();
		GameVibrations.Instance().PlayEndGame();
		playingGame = false;
		/*if (!forfeit)
		{
			if (isLiveEvent)
			{
				FlurryAnalytics.Instance().LogEvent("completed_point_streak_challenge", new string[3]
				{
					"numShots:" + Stats.numShots + string.Empty,
					"score: " + Stats.p1Score,
					"fps: " + Fps.GetFpsAverageForAnalytics()
				}, false);
			}
			else if (twoPlayerMode)
			{
				FlurryAnalytics.Instance().LogEvent("completed_game_2P", new string[2]
				{
					"numShots:" + Stats.numShots + string.Empty,
					"fps: " + Fps.GetFpsAverageForAnalytics()
				}, false);
			}
			else
			{
				FlurryAnalytics.Instance().LogEvent("completed_game_1P", new string[2]
				{
					"numShots:" + Stats.numShots + string.Empty,
					"fps: " + Fps.GetFpsAverageForAnalytics()
				}, false);
			}
		}
		else if (twoPlayerMode)
		{
			FlurryAnalytics.Instance().LogEvent("forfeited_game_2P", new string[2]
			{
				"numShots:" + Stats.numShots + string.Empty,
				"fps: " + Fps.GetFpsAverageForAnalytics()
			}, false);
		}
		else
		{
			FlurryAnalytics.Instance().LogEvent("forfeited_game_1P", new string[2]
			{
				"numShots:" + Stats.numShots + string.Empty,
				"fps: " + Fps.GetFpsAverageForAnalytics()
			}, false);
		}*/
		if (recorder != null)
		{
			recorder.Pause();
		}
		StartCoroutine(ShowGameOver(forfeit));
	}

	private IEnumerator ShowGameOver(bool forfeit)
	{
		Time.timeScale = 1f;
		Stats.numSeconds = (int)(gameSeconds * 1.25f);
		Stats.forfeited = forfeit;
		Stats.p1Score = score.player1Score;
		Stats.p2Score = score.player2Score;
		bool won = score.player1Score > score.player2Score && !forfeit;
		if (isLiveEvent)
		{
			Stats.PlayedLiveEvent();
		}
		else if (!twoPlayerMode && !inScrimmage && !sessionVars.goToPractice && !inTutorial && !forfeit)
		{
			Stats.SaveGame(won);
		}
		else if (inScrimmage)
		{
			/*FlurryAnalytics.Instance().LogEvent("scrimmage_finished", new string[4]
			{
				"won:" + won + string.Empty,
				"sessions:" + Stats.GetNumSessions() + string.Empty,
				"seconds:" + DoubleTapUtils.GetRoundedSeconds((int)totalSeconds) + string.Empty,
				"fps: " + Fps.GetFpsAverageForAnalytics()
			}, false);*/
			PlayerPrefsHelper.SetInt("COMPLETED_SCRIM", 1);
			PlayFabManager.Instance().SetUserDataForKey("COMPLETED_SCRIM", 1);
		}
		if (!forfeit && !sessionVars.goToPractice && !isLiveEvent)
		{
			if (won)
			{
				if (twoPlayerMode)
				{
					twoPlayerWinnerMsgs[0].SetActive(true);
					((Text)twoPlayerWinnerMsgs[0].GetComponent(typeof(Text))).text = "YOU WON!";
					twoPlayerWinnerMsgs[1].SetActive(true);
					((Text)twoPlayerWinnerMsgs[1].GetComponent(typeof(Text))).text = "YOU LOST";
				}
				else
				{
					/*FlurryAnalytics.Instance().LogEvent("won_game_1P", new string[6]
					{
						"num_wins:" + Stats.GetNumWins() + string.Empty,
						"num_losses:" + Stats.GetNumLosses() + string.Empty,
						"current_tour:" + Tournaments.GetCurrentTournamentNum() + string.Empty,
						"sessions:" + Stats.GetNumSessions() + string.Empty,
						"seconds:" + DoubleTapUtils.GetRoundedSeconds((int)totalSeconds) + string.Empty,
						"fps: " + Fps.GetFpsAverageForAnalytics()
					}, false);*/
				}
				gameSounds.Play_crowd_long_cheer_01();
			}
			else if (twoPlayerMode)
			{
				twoPlayerWinnerMsgs[0].SetActive(true);
				((Text)twoPlayerWinnerMsgs[0].GetComponent(typeof(Text))).text = "YOU LOST";
				twoPlayerWinnerMsgs[1].SetActive(true);
				((Text)twoPlayerWinnerMsgs[1].GetComponent(typeof(Text))).text = "YOU WON!";
				gameSounds.Play_crowd_long_cheer_01();
			}
			else
			{
				if (Stats.GetNumLosses() == 1)
				{
					/*FlurryAnalytics.Instance().LogEvent("lost_game_1P_first", new string[6]
					{
						"num_wins:" + Stats.GetNumWins() + string.Empty,
						"num_losses:" + Stats.GetNumLosses() + string.Empty,
						"current_tour:" + Tournaments.GetCurrentTournamentNum() + string.Empty,
						"sessions:" + Stats.GetNumSessions() + string.Empty,
						"seconds:" + DoubleTapUtils.GetRoundedSeconds((int)totalSeconds) + string.Empty,
						"fps: " + Fps.GetFpsAverageForAnalytics()
					}, false);*/
				}
				/*FlurryAnalytics.Instance().LogEvent("lost_game_1P", new string[6]
				{
					"num_wins:" + Stats.GetNumWins() + string.Empty,
					"num_losses:" + Stats.GetNumLosses() + string.Empty,
					"current_tour:" + Tournaments.GetCurrentTournamentNum() + string.Empty,
					"sessions:" + Stats.GetNumSessions() + string.Empty,
					"seconds:" + DoubleTapUtils.GetRoundedSeconds((int)totalSeconds) + string.Empty,
					"fps: " + Fps.GetFpsAverageForAnalytics()
				}, false);*/
			}
		}
		gameIsOver = true;
		if (!isLiveEvent)
		{
			voiceOvers.PlayEndOfGame(won);
		}
		gameControls.gameObject.SetActive(false);
		yield return new WaitForSeconds(0.1f);
		yield return new WaitForSeconds(0.65f);
		yield return new WaitForSeconds(0.5f);
		Time.timeScale = 0f;
		AdMediation.DestroyCenterBanner();
		if (twoPlayerMode)
		{
			Application.LoadLevel("MainUI");
		}
		else
		{
			Application.LoadLevel("GameResults");
		}
	}

	public virtual int BallInBasket(bool isEnemyBasket, int numRimHits, bool wasBlocked)
	{
		if (!playingGame)
		{
			return 0;
		}
		if (!inTutorial)
		{
			playingGame = false;
		}
		gameSounds.Play_swoosh();
		gameNoise.PauseBgSqueaks();
		int num = 0;
		if (shooting3PT && ((p1ShotBall && !isEnemyBasket) || (!p1ShotBall && isEnemyBasket)))
		{
			num = 3;
			StartCoroutine(voiceOvers.PlayMadeThree(numRimHits));
			GameVibrations.Instance().PlayMadeThree();
		}
		else
		{
			num = 2;
			if (dunkTriggered)
			{
				voiceOvers.PlayMadeDunk();
				GameVibrations.Instance().PlayMadeDunk();
			}
			else if (shootingLayup)
			{
				voiceOvers.PlayMadeLayup();
				GameVibrations.Instance().PlayMadeLayup();
			}
			else
			{
				StartCoroutine(voiceOvers.PlayMadeTwo(numRimHits, wasBlocked));
				GameVibrations.Instance().PlayMadeTwo();
			}
		}
		StartCoroutine(BallInBasketComplete(isEnemyBasket, num));
		return num;
	}

	private IEnumerator BallInBasketComplete(bool isEnemyBasket, int points)
	{
		if (!isEnemyBasket)
		{
			Stats.numMakes++;
			if (points == 3)
			{
				Stats.num3PtMakes++;
			}
			Stats.numPoints += points;
		}
		if (inTutorial)
		{
			StartCoroutine(tutorialGUI.MadeShot(shootingLayup));
			StartCoroutine(score.AddShot(isEnemyBasket, points));
			yield return new WaitForSeconds(0.25f);
			ballTail.trailRenderer.enabled = false;
			yield return new WaitForSeconds(0.5f);
			StartCoroutine(NewTutorialPlay());
			yield break;
		}
		StartCoroutine(score.AddShot(isEnemyBasket, points));
		p1ScoredLastPoint = !isEnemyBasket;
		p2ScoredLastPoint = isEnemyBasket;
		if (isLiveEvent)
		{
			if (PlayerPrefs.GetInt("LEAGUE_NUM") == 0)
			{
				int val = UnityEngine.Random.Range(1, 6);
				PlayerPrefsHelper.SetInt("LEAGUE_NUM", val);
			}
			string text = "EVENT_SCORE_" + PlayerPrefs.GetInt("LEAGUE_NUM");
			PlayFabManager.Instance().UpdateStat(text, score.player1Score);
		}
		if (!twoPlayerMode && !inScrimmage && !inTutorial)
		{
			float amountUsed = (isEnemyBasket ? ((UnityEngine.Random.Range(0, 100) < 50) ? (0.15f * (float)points) : (0.05f * (float)points)) : ((UnityEngine.Random.Range(0, 100) < 50) ? (0.25f * (float)points) : (0.2f * (float)points)));
			gameRoster.UsePlayerHydration(amountUsed);
		}
		bool isHalfWayThroughGame = ((float)score.player1Score >= (float)playToScore / 2f || (float)score.player2Score >= (float)playToScore / 2f || (score.player1Score >= 10 && playToScore >= 100)) && currentHalf == 1;
		bool isEndOfGame = IsEndOfGame();
		bool userWon = score.player1Score >= playToScore;
		if (isEndOfGame)
		{
			voiceOvers.PlayEndOfGame(userWon);
		}
		yield return new WaitForSeconds(0.95f);
		if (isHalfWayThroughGame && !isLiveEvent && !twoPlayerMode && !inScrimmage)
		{
			gameSounds.Play_bball_buzzer();
		}
		yield return new WaitForSeconds(0.1f);
		ballTail.trailRenderer.enabled = false;
		yield return new WaitForSeconds(0.5f);
		if (isEndOfGame && userWon && !twoPlayerMode && !sessionVars.goToPractice && !isLiveEvent)
		{
			youWonMsg.SetActive(true);
			gameSounds.Play_ascend_chime_bright_2();
		}
		if (isHalfWayThroughGame)
		{
			currentHalf = 2;
			currentHalfLabel.text = ((!isLiveEvent) ? "2" : "X");
			if (isLiveEvent)
			{
				playerController.sprintBar.Reset();
				enemyPlayerController.sprintBar.Reset();
				StartCoroutine(NewPlay(false));
			}
			else if (!twoPlayerMode && !inScrimmage)
			{
				StartCoroutine(EnterHalfTime());
			}
			else
			{
				StartCoroutine(NewPlay(false));
			}
		}
		else if (isEndOfGame)
		{
			EndGame(false);
			if (userWon)
			{
				StartCoroutine(playerController.CelebrateWin());
			}
		}
		else
		{
			StartCoroutine(NewPlay(false));
		}
	}

	public virtual bool IsEndOfGame()
	{
		return score.player1Score >= playToScore || score.player2Score >= playToScore || (isLiveEvent && score.player2Score > 0);
	}

	public virtual void PlayerGotBall()
	{
		ballShadow.startingYOffset = -0.15f;
		timeoutButton.SetHasBall(playerController.hasBall);
		if (playerController.sprintBar.isTired && enemyPlayerController.sprintBar.isTired && gameRoster.IsLowOnHydration())
		{
			voiceOvers.PlayBothTeamsTired();
		}
	}

	public virtual void UserSwipedUp()
	{
		ShowHint(false);
		hintMessage.DontSwipe();
		/*FlurryAnalytics.Instance().LogEvent("user_swiped_up", new string[4]
		{
			"num_wins:" + Stats.GetNumWins() + string.Empty,
			"num_losses:" + Stats.GetNumLosses() + string.Empty,
			"current_tour:" + Tournaments.GetCurrentTournamentNum() + string.Empty,
			"sessions:" + Stats.GetNumSessions() + string.Empty
		}, false);*/
	}

	public virtual void ShotBall(Ball ball, bool p1ShotBall, bool is3PT, bool isLayup, bool dunkTriggered)
	{
		if (this.ball != null)
		{
			UnityEngine.Object.Destroy(this.ball.gameObject);
		}
		this.ball = ball;
		this.ball.gameSounds = gameSounds;
		this.p1ShotBall = p1ShotBall;
		shooting3PT = is3PT;
		shootingLayup = isLayup;
		this.dunkTriggered = dunkTriggered;
		ballShadow.startingYOffset = 0f;
		ikBall.FixToTarget(this.ball.gameObject);
		((BallVisual)ikBall.GetComponent(typeof(BallVisual))).SetSortingLayerName("Player");
		timeoutButton.SetHasBall(false);
		if (p1ShotBall && numConsecutiveEarlyReleases >= 8)
		{
			ShowHint(false);
			hintMessage.HoldDownShootLonger();
		}
		if (!p1ShotBall)
		{
			return;
		}
		if (recorder != null)
		{
			if (dunkTriggered)
			{
				recorder.framesSinceGoodLookingEvent = 1;
			}
			else if (isLayup)
			{
				recorder.framesSinceGoodLookingEvent = 0;
			}
			else
			{
				recorder.framesSinceGoodLookingEvent = -1;
			}
		}
		Stats.numShots++;
		if (is3PT)
		{
			Stats.num3PtShots++;
		}
		int num = ((!is3PT) ? 2 : 3);
		if (playerController.sprintBar.isTired && !gameRoster.IsLowOnHydration() && !showedEnduranceHint && numTutorialBalls + num < 7 && !isShowingHint)
		{
			ShowHint(false);
			hintMessage.EnergyRecovery();
			PlayerPrefsHelper.SetInt("SHOWED_ENDURANCE", 1);
			showedEnduranceHint = true;
		}
	}

	public virtual IEnumerator EnterHalfTime()
	{
		Time.timeScale = 1f;
		/*FlurryAnalytics.Instance().LogEvent("enter_halftime", new string[3]
		{
			"numShots:" + Stats.numShots + string.Empty,
			"seconds:" + DoubleTapUtils.GetRoundedSeconds((int)totalSeconds) + string.Empty,
			"fps: " + Fps.GetFpsAverageForAnalytics()
		}, false);*/
		if (hintMessage.gameObject.activeInHierarchy)
		{
			hintMessage.gameObject.SetActive(false);
		}
		gameNoise.PauseBgSqueaks();
		showingMsg = true;
		timeouts = 3;
		score1Timeouts.SetNumTimeouts(timeouts);
		halfTime.SetActive(true);
		gameControls.gameObject.SetActive(false);
		yield return new WaitForSeconds(1.25f);
		voiceOvers.PlayEndOfHalf();
	}

	public virtual void TakeTimeout()
	{
		gameNoise.PauseBgSqueaks();
		gameSounds.Play_light_click();
		timeoutButton.SetHasBall(false);
		if (playerController.hasBall)
		{
			if (timeouts > 0)
			{
				gameSounds.Play_whistle_02();
				timeouts--;
				showingMsg = true;
				StartCoroutine(ShowTimeoutScreen());
			}
			else
			{
				ShowHint(true);
				hintMessageTopOfScreen.NoTimeoutsLeft();
			}
		}
		else
		{
			ShowHint(true);
			hintMessageTopOfScreen.NeedBallForTimeout();
		}
		AdMediation.HideTopBanner();
	}

	private IEnumerator ShowTimeoutScreen()
	{
		score1Timeouts.SetNumTimeouts(timeouts);
		pauseDialog.SetActive(false);
		gameControls.gameObject.SetActive(false);
		playerController.Reset();
		timeoutScreen.SetActive(true);
		if (Time.timeScale == 0f)
		{
			Time.timeScale = 1f;
		}
		yield return new WaitForSeconds(0.05f);
		Time.timeScale = 0f;
	}

	public virtual void ExitTimeout()
	{
		gameSounds.Play_select();
		HideHintArrows();
		showingMsg = false;
		Time.timeScale = gamePlayTimeScale;
		timeoutScreen.SetActive(false);
		gameControls.gameObject.SetActive(true);
		StartCoroutine(NewPlay(true));
	}

	public virtual void ShowHint(bool topOfScreen)
	{
		secShowingHint = 0f;
		if (topOfScreen)
		{
			hintMessageTopOfScreen.gameObject.SetActive(true);
			if (isShowingHint)
			{
			}
		}
		else
		{
			hintMessage.gameObject.SetActive(true);
			if (isShowingHint)
			{
			}
		}
		isShowingHint = true;
		if (!topOfScreen)
		{
		}
	}

	public virtual void CloseHint()
	{
		isShowingHint = false;
		HideHintMessage();
	}

	private void HideHintMessage()
	{
		hintMessageTopOfScreen.gameObject.SetActive(false);
		hintMessage.gameObject.SetActive(false);
	}

	public virtual bool ComputerCloseToWinning()
	{
		return score.player2Score + 4 >= playToScore || tournament.type == tournamentTypeEnum.LiveEvent;
	}

	public virtual int GetCurrentHalf()
	{
		return currentHalf;
	}

	public virtual int GetTimeouts()
	{
		return timeouts;
	}

	public virtual int GetPlayToScore()
	{
		return playToScore;
	}

	public virtual bool IsShooting3PT()
	{
		return shooting3PT;
	}

	public virtual bool IsDunkTriggered()
	{
		return dunkTriggered;
	}

	private void SetPlayerStats()
	{
		if (!twoPlayerMode)
		{
			if (inTutorial || inScrimmage)
			{
				gameRoster.SetStarterPlayerNum(3);
				gameRoster.SetPlayerInGame(0);
				SetPlayerSprites();
			}
			int num = tournament.num;
			int currentRound = tournament.currentRound;
			float statByNum = GameComputer.GetStatByNum(Players.JUMP, false, num, currentRound);
			float statByNum2 = GameComputer.GetStatByNum(Players.DEFENSE, false, num, currentRound);
			float currentPlayerSize = gameRoster.GetCurrentPlayerSize();
			float currentPlayerJumpForce = gameRoster.GetCurrentPlayerJumpForce();
			float energyRegenerationSpeed = ((!inTutorial) ? gameRoster.GetEnergyRegenerationSpeed() : 2.5f);
			float currentShootingArch = gameRoster.GetCurrentShootingArch(statByNum);
			float currentDefendedMultiplier = gameRoster.GetCurrentDefendedMultiplier(statByNum2);
			playerController.SetSize(currentPlayerSize);
			playerController.SetJumpForce(currentPlayerJumpForce);
			playerController.SetEnergyRegenerationSpeed(energyRegenerationSpeed);
			playerController.shooter.SetPlayerShootingArch(currentShootingArch);
			playerController.SetDefendedMultiplier(currentDefendedMultiplier);
			if (enemyPlayerController.enabled)
			{
				SetEnemyControllerStats();
			}
		}
	}

	private void SetEnemyControllerStats()
	{
		int num = currentHalf;
		if (inScrimmage)
		{
			num = 1;
		}
		float currentPlayerSize = gameComputer.GetCurrentPlayerSize(tournament.type, tournament.num, tournament.currentRound, num);
		float currentPlayerJumpForce = gameComputer.GetCurrentPlayerJumpForce();
		float energyRegenerationSpeed = gameComputer.GetEnergyRegenerationSpeed();
		float currentShootingArch = gameComputer.GetCurrentShootingArch(gameRoster.GetStat(Players.JUMP), num);
		float currentDefendedMultiplier = gameComputer.GetCurrentDefendedMultiplier(gameRoster.GetStat(Players.DEFENSE), num);
		enemyPlayerController.SetSize(currentPlayerSize);
		enemyPlayerController.SetJumpForce(currentPlayerJumpForce);
		enemyPlayerController.SetEnergyRegenerationSpeed(energyRegenerationSpeed);
		enemyPlayerController.shooter.SetPlayerShootingArch(currentShootingArch);
		enemyPlayerController.SetDefendedMultiplier(currentDefendedMultiplier);
	}

	private void SetPlayerSprites()
	{
		int computerNum = gameComputer.GetComputerNum();
		int num = 0;
		int num2 = 0;
		List<CustomItem> items = customItems.GetItems(CustomItems.JERSEY);
		int num3 = ((computerNum % 2 != 0) ? (computerNum + 2) : (computerNum + 23));
		int index = ((!inScrimmage) ? (num3 % items.Count) : 5);
		CustomItem customItem = items[index];
		List<CustomItem> items2 = customItems.GetItems(CustomItems.SHOES);
		int index2 = ((!inScrimmage) ? (computerNum % items2.Count) : 0);
		CustomItem customItem2 = items2[index2];
		if (computerNum % 3 == 0)
		{
			num2 = 4;
			num = customItem.color;
		}
		else if (computerNum % 2 == 0)
		{
			num2 = customItem.overlayColor;
			num = 4;
		}
		else
		{
			num2 = customItem.color;
			num = customItem.overlayColor;
		}
		if (twoPlayerMode)
		{
			Player player = new Player();
			player.num = 14;
			player.isP2 = true;
			playerVisual.SetVisual(player, characterSprites, customItems, arenaNum);
			playerVisual.SetJerseyGraphicsSprite(characterSprites.jerseyGraphics[0]);
			playerVisual.SetJerseyGraphicsColor(characterSprites.clothingColors[11]);
			playerVisual.SetJerseyColor(characterSprites.clothingColors[0]);
			playerVisual.SetArmBandColor(characterSprites.clothingColors[0]);
			playerVisual.SetPantsColor(characterSprites.clothingColors[1]);
			Player player2 = new Player();
			player2.num = 2;
			player2.isP2 = true;
			enemyPlayerVisual.SetVisual(player2, characterSprites, customItems, arenaNum);
			enemyPlayerVisual.SetJerseyGraphicsSprite(characterSprites.jerseyGraphics[0]);
			enemyPlayerVisual.SetJerseyGraphicsColor(characterSprites.clothingColors[11]);
			enemyPlayerVisual.SetJerseyColor(characterSprites.clothingColors[12]);
			enemyPlayerVisual.SetArmBandColor(characterSprites.clothingColors[12]);
			enemyPlayerVisual.SetPantsColor(characterSprites.clothingColors[1]);
			return;
		}
		playerVisual.SetVisual(gameRoster.GetPlayerObjectInGame(), characterSprites, customItems, arenaNum);
		Player player3 = new Player();
		player3.isP2 = true;
		int num4 = 2;
		num4 = ((!inScrimmage) ? (num4 + (computerNum + 36)) : 14);
		int num5 = characterSprites.p2Head.Length;
		if (tournament.isFemale)
		{
			num5 = characterSprites.p2FemaleHead.Length;
		}
		if (num4 > num5 - 1)
		{
			num4 %= num5;
		}
		int num6 = num4;
		if (currentHalf == 2 && !inScrimmage)
		{
			num6 += 3;
			if (num6 > num5 - 1)
			{
				num6 %= num5;
			}
		}
		player3.num = num6;
		Debug.Log("========= looksNumP2: " + num6 + " )))))))))))");
		if (tournament.isFemale)
		{
			player3.isFemale = true;
		}
		else
		{
			player3.isFemale = false;
		}
		enemyPlayerVisual.SetVisual(player3, characterSprites, customItems, arenaNum);
		if (tournament.isFemale)
		{
			enemyPlayerVisual.SetJerseySprite(characterSprites.jerseyFemale);
		}
		else
		{
			enemyPlayerVisual.SetJerseySprite(characterSprites.jersey);
		}
		enemyPlayerVisual.SetJerseyGraphicsSprite(characterSprites.jerseyGraphics[customItem.overlaySprite]);
		enemyPlayerVisual.SetJerseyGraphicsColor(characterSprites.clothingColors[customItem.overlayColor]);
		enemyPlayerVisual.SetJerseyColor(characterSprites.clothingColors[customItem.color]);
		enemyPlayerVisual.SetArmBandColor(characterSprites.clothingColors[num]);
		enemyPlayerVisual.SetPantsColor(characterSprites.clothingColors[num2]);
		enemyPlayerVisual.SetShoesColor(characterSprites.clothingColors[customItem2.overlayColor]);
		enemyPlayerVisual.SetShoesFillColor(characterSprites.clothingColors[customItem2.color]);
	}

	public virtual IEnumerator ShowReleaseMsg(bool isP2, bool lateRelease)
	{
		yield return new WaitForSeconds(0.05f);
		GameObject o = null;
		float xMove = 0f;
		float yMove = 0f;
		if (twoPlayerMode)
		{
			if (isP2)
			{
				o = ((!lateRelease) ? earlyRP2 : lateRP2);
				xMove = -0.5f;
			}
			else
			{
				o = ((!lateRelease) ? earlyRP1 : lateRP1);
				xMove = 0.5f;
			}
		}
		else
		{
			o = ((!lateRelease) ? earlyR : lateR);
			yMove = 0.35f;
		}
		o.SetActive(true);
		Vector3 origPos = o.transform.position;
		LeanTween.move(o, new Vector2(o.transform.position.x + xMove, o.transform.position.y + yMove), 0.35f).setEase(LeanTweenType.easeOutSine);
		yield return new WaitForSeconds(2f);
		o.transform.position = origPos;
		o.SetActive(false);
	}

	public virtual void ShowHintArrow(int num)
	{
		HideHintArrows();
		hintArrows[num].SetActive(true);
	}

	private void HideHintArrows()
	{
		GameObject[] array = hintArrows;
		foreach (GameObject gameObject in array)
		{
			gameObject.SetActive(false);
		}
	}

	public virtual float GetGamePlayTimeScale()
	{
		return gamePlayTimeScale;
	}

	public virtual void DebugButtonPressed()
	{
		debugPresses++;
		if (debugPresses >= 10)
		{
			debugText.gameObject.SetActive(true);
		}
	}

	private void SetDebugText()
	{
		string empty = string.Empty;
		int currentTournamentNum = Tournaments.GetCurrentTournamentNum();
		int currentRound = Tournaments.GetCurrentRound(currentTournamentNum);
		empty = empty + "\ntNum: " + currentTournamentNum + " rNum: " + currentRound;
		empty += "\nCOMPUTER STATS:";
		empty = empty + "Offense: " + gameComputer.GetOffenseLevel();
		empty = empty + "\nD: " + gameComputer.GetDefenseLevel();
		empty = empty + "\nCan: " + gameComputer.CanWin();
		empty = empty + "\nShould: " + gameComputer.ShouldWin();
		empty = empty + "\nShooting: " + GameComputer.GetStatByNum(Players.SHOOTING, false, currentTournamentNum, currentRound);
		float statByNum = GameComputer.GetStatByNum(Players.JUMP, false, currentTournamentNum, currentRound);
		float statByNum2 = GameComputer.GetStatByNum(Players.DEFENSE, false, currentTournamentNum, currentRound);
		float currentPlayerSize = gameRoster.GetCurrentPlayerSize();
		float currentPlayerJumpForce = gameRoster.GetCurrentPlayerJumpForce();
		float energyRegenerationSpeed = gameRoster.GetEnergyRegenerationSpeed();
		float currentShootingArch = gameRoster.GetCurrentShootingArch(statByNum);
		float currentDefendedMultiplier = gameRoster.GetCurrentDefendedMultiplier(statByNum2);
		empty += "\nUSER STATS:";
		empty = empty + "\nsize: " + currentPlayerSize;
		empty = empty + "\njump: " + currentPlayerJumpForce;
		empty = empty + "\nenergy: " + energyRegenerationSpeed;
		empty = empty + "\nshooting: " + currentShootingArch;
		empty = empty + "\ndef: " + currentDefendedMultiplier;
		debugText.text = empty;
	}

	public virtual void OnModifyTouramentNameLocalization()
	{
		if (!string.IsNullOrEmpty(Localize.MainTranslation))
		{
			string text = tournament.name + " ";
			Localize.MainTranslation = text + Localize.MainTranslation;
		}
	}

	public void AddGameSeconds(float seconds)
	{
		gameSeconds += seconds;
	}
}
