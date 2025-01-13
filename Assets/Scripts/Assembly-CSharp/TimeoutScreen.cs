using System;
using I2.Loc;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class TimeoutScreen : MonoBehaviour
{
	public Image[] typeButtonImages;

	public Sprite unselectedTypeGraphic;

	public GameObject[] useDrinkButtons;

	public Text[] drinkAmounts;

	public Localize leftHeading;

	public Localize rightHeading;

	public Text rightHeadingText;

	public GameController gameController;

	public SprintBar sprintBar;

	public SprintBar sprintBarEnemy;

	public GameRoster gameRoster;

	public HotPurchaseBox hotPurchaseBox;

	public GameObject[] hydrationBarFill;

	public GameObject[] putInGameButtons;

	public SpriteRenderer[] ballOutlines;

	public SpriteRenderer[] ballFills;

	public Color hydrationFullColor;

	public Color hydrationLowColor;

	private int currentHalf;

	public Localize resumeButtonText;

	private GameSounds gameSounds;

	public bool isHalfTime;

	private bool didPutInPlayer;

	public bool DidPutInPlayer
	{
		get
		{
			return didPutInPlayer;
		}
	}

	public TimeoutScreen()
	{
		currentHalf = 1;
	}

	public virtual void OnEnable()
	{
		Debug.Log("currentHalf: " + currentHalf);
		Image[] array = typeButtonImages;
		foreach (Image image in array)
		{
			image.sprite = unselectedTypeGraphic;
		}
		isHalfTime = currentHalf != gameController.GetCurrentHalf();
		currentHalf = gameController.GetCurrentHalf();
		if (isHalfTime)
		{
			leftHeading.SetTerm("HALFTIME", null);
			rightHeadingText.text = gameController.score.player1Score + " - " + gameController.score.player2Score + " : FIRST TO " + gameController.GetPlayToScore();
			resumeButtonText.SetTerm("PLAY 2ND HALF", null);
			UpdateBars(0f);
		}
		else
		{
			leftHeading.SetTerm("TIMEOUT TAKEN", null);
			rightHeading.SetTerm("TIMEOUTS REMAINING", null);
			resumeButtonText.SetTerm("RESUME GAME", null);
		}
		sprintBar.Reset();
		sprintBarEnemy.Reset();
		if (!isHalfTime)
		{
			UpdateBars(0f);
		}
		UpdateSupplyAmounts();
		LeanTween.pauseAll();
		if (PlayerPrefs.GetInt("USED_DRINK") == 0)
		{
			if (!gameRoster.PlayerHydrationIsFull(0))
			{
				LeanTween.scale(useDrinkButtons[0], new Vector3(1.1f, 1.1f, 1f), 0.35f).setEase(LeanTweenType.easeInOutSine).setLoopPingPong()
					.setIgnoreTimeScale(true);
			}
			if (!gameRoster.PlayerHydrationIsFull(1))
			{
				LeanTween.scale(useDrinkButtons[1], new Vector3(1.1f, 1.1f, 1f), 0.35f).setEase(LeanTweenType.easeInOutSine).setLoopPingPong()
					.setIgnoreTimeScale(true);
			}
		}
	}

	public virtual void UpdateBars(float animationLength)
	{
		for (int i = 0; i < 2; i++)
		{
			float playerHydration = gameRoster.GetPlayerHydration(i);
			if (playerHydration <= 0.05f)
			{
				((Image)hydrationBarFill[i].GetComponent(typeof(Image))).color = hydrationLowColor;
			}
			else
			{
				((Image)hydrationBarFill[i].GetComponent(typeof(Image))).color = hydrationFullColor;
			}
			float x = hydrationBarFill[i].transform.localScale.x;
			if (playerHydration <= x || animationLength == 0f)
			{
				float x2 = playerHydration;
				Vector3 localScale = hydrationBarFill[i].transform.localScale;
				localScale.x = x2;
				hydrationBarFill[i].transform.localScale = localScale;
			}
			else
			{
				LeanTween.scale(hydrationBarFill[i], new Vector3(playerHydration, 1f, 1f), animationLength).setEase(LeanTweenType.easeOutQuad).setUseEstimatedTime(true);
			}
		}
	}

	public virtual void UpdateSupplyAmounts()
	{
		int itemAmount = Supplies.GetItemAmount(Supplies.DRINK);
		Text[] array = drinkAmounts;
		foreach (Text text in array)
		{
			text.text = itemAmount.ToString();
		}
	}

	public virtual void Start()
	{
		gameSounds = GameSounds.GetInstance();
		if (Supplies.GetItemAmount(Supplies.DRINK) == 0 && PlayerPrefs.GetInt(Supplies.COLLECTED_FREE_KEY + Supplies.DRINK) == 0)
		{
			useDrinkButtons[0].SetActive(false);
			useDrinkButtons[1].SetActive(false);
		}
	}

	public virtual void UseDrink(int player)
	{
		if (!gameRoster.PlayerHydrationIsFull(player))
		{
			if (Supplies.UseItem(Supplies.DRINK, 1))
			{
				gameRoster.AddPlayerHydration(player, 0.5f);
				gameSounds.Play_gulp();
				gameSounds.Play_gulp();
				gameSounds.Play_gulp();
				UpdateBars(0.45f);
				PlayerPrefsHelper.SetInt("USED_DRINK", 1);
			}
			else
			{
				ShowHotPurchase(Supplies.DRINK);
			}
			UpdateSupplyAmounts();
		}
	}

	public virtual void PutInPlayer(int player)
	{
		gameSounds.Play_select();
		GameVibrations.Instance().PlayPutInPlayer();
		gameRoster.SetPlayerInGame(player);
		for (int i = 0; i < 2; i++)
		{
			putInGameButtons[i].SetActive(i != gameRoster.GetPlayerInGame());
			ballOutlines[i].enabled = i == gameRoster.GetPlayerInGame();
			ballFills[i].enabled = i == gameRoster.GetPlayerInGame();
		}
		if (Stats.GetNumWins() + 1 <= 3 && !didPutInPlayer)
		{
			gameController.ShowHintArrow(1);
		}
		didPutInPlayer = true;
		/*FlurryAnalytics.Instance().LogEvent("PUT_IN_PLAYER", new string[6]
		{
			"player:" + player + string.Empty,
			"isHalfTime:" + isHalfTime + string.Empty,
			"num_wins:" + Stats.GetNumWins() + string.Empty,
			"num_losses:" + Stats.GetNumLosses() + string.Empty,
			"current_tour:" + Tournaments.GetCurrentTournamentNum() + string.Empty,
			"sessions:" + Stats.GetNumSessions() + string.Empty
		}, false);*/
	}

	public virtual void Update()
	{
	}

	public virtual void ShowHotPurchase(int type)
	{
		hotPurchaseBox.gameObject.SetActive(true);
		hotPurchaseBox.SelectItemType(type);
	}

	public virtual void OnModifyRightHeadingLocalization()
	{
		if (!string.IsNullOrEmpty(Localize.MainTranslation))
		{
			string newValue = gameController.GetTimeouts().ToString();
			Localize.MainTranslation = Localize.MainTranslation.Replace("{num}", newValue);
		}
	}
}
