using System;
using System.Collections;
using I2.Loc;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class UpgradeScreen : MonoBehaviour
{
	public PlayerDetails playerDetails;

	public Text bballValue;

	public GameObject[] lockIcons;

	public Text[] statValues;

	public GameObject[] buttons;

	public GameObject[] upgradingButtons;

	public GameObject[] owned;

	public GameObject buyPanel;

	public Text[] buyButtonAmount;

	public GameObject timeredPriceCashIcon;

	public GameObject timeredPriceGoldIcon;

	public Localize upgradeType;

	public Localize upgradeTypeDesc;

	public GameObject upgradeInProgress;

	public GameObject purchasedMessage;

	public GameObject goldButton;

	public GameObject[] hintArrows;

	public GameObject signNewPlayerButton;

	public TopNavBar topNavBar;

	public GetGoldButton getGoldButton;

	public CurrencyExchangeBox currencyExchangeBox;

	private Player player;

	private Players players;

	private SessionVars sessionVars;

	private NotificationQueue notificationQueue;

	private GameSounds gameSounds;

	private int currentType;

	private int currentTypeTimeLeft;

	private int currentLevel;

	private int instantPrice;

	private int timeredCashPrice;

	private int timeredGoldPrice;

	public Color unselectedTypeColor;

	public Color selectedTypeColor;

	private bool animatingToNextUpgrade;

	private int priceMultiplier;

	public UpgradeScreen()
	{
		currentType = 2;
		instantPrice = 2;
		priceMultiplier = 1;
	}

	public virtual void Awake()
	{
		priceMultiplier = PlayerPrefs.GetInt("UPGRADE_PRICE_MULTIPLIER");
		players = (Players)base.gameObject.GetComponent(typeof(Players));
		players.InstantiatePlayers();
		sessionVars = SessionVars.GetInstance();
		gameSounds = GameSounds.GetInstance();
		notificationQueue = (NotificationQueue)GameObject.Find("NotificationQueue").GetComponent(typeof(NotificationQueue));
	}

	public virtual void OnEnable()
	{
		AdMediation.ShowTjpUpgradeScreen();
		if (priceMultiplier < 1)
		{
			priceMultiplier = 1;
		}
		bool showingFemales = sessionVars.showingFemales;
		if (sessionVars.showingBackups)
		{
			if (showingFemales)
			{
				player = players.GetBackup(showingFemales, sessionVars.selectedBackupFemaleNum);
			}
			else
			{
				player = players.GetBackup(showingFemales, sessionVars.selectedBackupNum);
			}
			playerDetails.showingBackups = true;
		}
		else
		{
			if (showingFemales)
			{
				player = players.GetStarter(showingFemales, sessionVars.selectedStarterFemaleNum);
			}
			else
			{
				player = players.GetStarter(showingFemales, sessionVars.selectedStarterNum);
			}
			playerDetails.showingBackups = false;
		}
		playerDetails.SetPlayerAndGender(player.num, showingFemales);
		buyPanel.SetActive(false);
		UpdateUpgradeDisplay(false);
		animatingToNextUpgrade = true;
		StartCoroutine(GoToNextUpgradeOption(0.1f));
		if (PlayerPrefs.GetInt("BUY_UPGRADE") == 0)
		{
			hintArrows[0].SetActive(true);
		}
		/*FlurryAnalytics.Instance().LogEvent("SCREEN_UPGRADE", new string[5]
		{
			"num_wins:" + Stats.GetNumWins() + string.Empty,
			"num_losses:" + Stats.GetNumLosses() + string.Empty,
			"current_tour:" + Tournaments.GetCurrentTournamentNum() + string.Empty,
			"sessions:" + Stats.GetNumSessions() + string.Empty,
			"num_wins_milestone:" + Stats.GetNumWinsMilestone() + string.Empty
		}, false);*/
		AdMediation.TrackEventInTj("SCREEN_UPGRADE", Stats.GetNumWins());
	}

	public virtual void UpdateUpgradeDisplay(bool animate)
	{
		UpdateStatValues();
		bballValue.text = player.GetStatTotal().ToString();
		if (animate)
		{
			bballValue.gameObject.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
			LeanTween.scale(bballValue.gameObject, new Vector3(1f, 1f), 0.25f).setEase(LeanTweenType.easeOutQuad);
		}
		currentTypeTimeLeft = 0;
		int num = 0;
		int num2 = 0;
		int upgradeLevelByNum = player.GetUpgradeLevelByNum(num2);
		int statUpgradeTimeStampByNum = player.GetStatUpgradeTimeStampByNum(num2);
		int lowestUpgrade = GetLowestUpgrade();
		for (int i = 0; i < buttons.Length; i++)
		{
			if (num < upgradeLevelByNum)
			{
				buttons[i].SetActive(false);
				upgradingButtons[i].SetActive(false);
				owned[i].SetActive(true);
			}
			else if (num == upgradeLevelByNum && num <= lowestUpgrade)
			{
				buttons[i].SetActive(true);
				((Button)buttons[i].GetComponent(typeof(Button))).interactable = true;
				int num3 = 0;
				if (statUpgradeTimeStampByNum > 0)
				{
					upgradingButtons[i].SetActive(true);
					Timer timer = (Timer)upgradingButtons[i].transform.Find("Text").GetComponent(typeof(Timer));
					num3 = statUpgradeTimeStampByNum + Players.GetUpgradeLength(upgradeLevelByNum) - sessionVars.currentTimestamp;
					timer.SetSecondsToWait(num3 + 2, num2);
				}
				else
				{
					upgradingButtons[i].SetActive(false);
				}
				owned[i].SetActive(false);
				if (num2 == currentType && !animatingToNextUpgrade)
				{
					((Image)buttons[i].GetComponent(typeof(Image))).color = selectedTypeColor;
					((Image)upgradingButtons[i].GetComponent(typeof(Image))).color = selectedTypeColor;
					currentTypeTimeLeft = num3;
				}
				else
				{
					((Image)buttons[i].GetComponent(typeof(Image))).color = unselectedTypeColor;
					((Image)upgradingButtons[i].GetComponent(typeof(Image))).color = unselectedTypeColor;
				}
			}
			else
			{
				buttons[i].SetActive(true);
				((Button)buttons[i].GetComponent(typeof(Button))).interactable = false;
				((Image)buttons[i].GetComponent(typeof(Image))).color = unselectedTypeColor;
				((Image)upgradingButtons[i].GetComponent(typeof(Image))).color = unselectedTypeColor;
				upgradingButtons[i].SetActive(false);
				owned[i].SetActive(false);
			}
			num++;
			if ((i + 1) % 5 == 0 && i != 0)
			{
				num = 0;
				num2++;
				upgradeLevelByNum = player.GetUpgradeLevelByNum(num2);
				statUpgradeTimeStampByNum = player.GetStatUpgradeTimeStampByNum(num2);
			}
		}
		for (int j = 0; j < lockIcons.Length; j++)
		{
			lockIcons[j].SetActive(j == lowestUpgrade);
		}
	}

	private int GetLowestUpgrade()
	{
		int num = 4;
		for (int i = 0; i < 4; i++)
		{
			int upgradeLevelByNum = player.GetUpgradeLevelByNum(i);
			if (upgradeLevelByNum < num)
			{
				num = upgradeLevelByNum;
			}
		}
		return num;
	}

	public virtual void SelectUpgrade(int num)
	{
		currentType = num;
		UpdateUpgradeDisplay(false);
		UpdateBuyPanel();
		hintArrows[2].SetActive(false);
	}

	public virtual void UpdateStatValues()
	{
		for (int i = 0; i < statValues.Length; i++)
		{
			statValues[i].text = player.GetStatByNum(i + 1).ToString();
		}
	}

	public virtual void UpdateBuyPanel()
	{
		buyPanel.SetActive(true);
		purchasedMessage.SetActive(false);
		if (currentTypeTimeLeft > 0)
		{
			upgradeInProgress.SetActive(true);
			Timer timer = (Timer)upgradeInProgress.transform.Find("Timer").GetComponent(typeof(Timer));
			timer.SetSecondsToWait(currentTypeTimeLeft + 2, currentType);
		}
		else
		{
			upgradeInProgress.SetActive(false);
		}
		currentLevel = GetLowestUpgrade();
		bool flag = false;
		if (currentType == 0)
		{
			upgradeType.SetTerm("SPEED", null);
			upgradeTypeDesc.SetTerm("SPEED UPGRADE DESC", null);
			flag = true;
		}
		else if (currentType == 1)
		{
			upgradeType.SetTerm("CONTESTING", null);
			upgradeTypeDesc.SetTerm("CONTESTING UPGRADE DESC", null);
		}
		else if (currentType == 2)
		{
			upgradeType.SetTerm("SHOOTING", null);
			upgradeTypeDesc.SetTerm("SHOOTING UPGRADE DESC", null);
		}
		else if (currentType == 3)
		{
			upgradeType.SetTerm("DEFENSE", null);
			upgradeTypeDesc.SetTerm("DEFENSE UPGRADE DESC", null);
		}
		int num = currentLevel;
		if (num == 1)
		{
			num = 0;
		}
		instantPrice = (int)Mathf.Round((float)(num + 1) * 1.5f);
		instantPrice *= priceMultiplier;
		if (flag)
		{
			timeredCashPrice = 0;
			timeredGoldPrice = instantPrice - 1;
		}
		else
		{
			timeredGoldPrice = 0;
			timeredCashPrice = (num + 1) * 650;
		}
		timeredCashPrice *= priceMultiplier;
		buyButtonAmount[1].text = (timeredGoldPrice + timeredCashPrice).ToString("n0");
		timeredPriceCashIcon.SetActive(timeredCashPrice > 0);
		timeredPriceGoldIcon.SetActive(timeredGoldPrice > 0);
		buyButtonAmount[0].text = instantPrice.ToString("n0");
	}

	public virtual void BuyTimeredUpgrade()
	{
		BuyUpgrade(false, false);
	}

	public virtual void BuyInstantUpgrade()
	{
		BuyUpgrade(true, false);
	}

	public virtual void BuyInstantComplete()
	{
		BuyUpgrade(false, true);
	}

	private void BuyUpgrade(bool instant, bool instantComplete)
	{
		int num = 0;
		bool flag = false;
		if (!instant && !instantComplete && timeredCashPrice > 0)
		{
			int amount = timeredCashPrice;
			if (Currency.SpendCash(amount))
			{
				flag = true;
			}
			else
			{
				gameSounds.Play_one_dribble();
				currencyExchangeBox.gameObject.SetActive(true);
				currencyExchangeBox.SetCashPrice(timeredCashPrice, instantPrice);
			}
		}
		else
		{
			num = (instantComplete ? 1 : ((!instant) ? timeredGoldPrice : instantPrice));
			if (Currency.SpendGold(num, "upgrade"))
			{
				flag = true;
			}
			else
			{
				getGoldButton.ShowGetGoldBox();
				AdMediation.ShowTjpInsufficientCurrency();
			}
		}
		if (!flag)
		{
			return;
		}
		gameSounds.Play_air_pump();
		int upgradeLength = Players.GetUpgradeLength(currentLevel);
		Debug.Log("upgradeTime: " + upgradeLength);
		bool flag2 = PlayerPrefs.GetInt("USED_INSTANT_COMPLETE") == 1;
		bool flag3 = false;
		if (PlayerPrefs.GetInt("BUY_UPGRADE") == 0)
		{
			hintArrows[0].SetActive(false);
			hintArrows[1].SetActive(true);
			PlayerPrefsHelper.SetInt("BUY_UPGRADE", 1, true);
			/*FlurryAnalytics.Instance().LogEvent("PURCHASED_FIRST_UPGRADE", new string[3]
			{
				"num_wins:" + Stats.GetNumWins() + string.Empty,
				"num_losses:" + Stats.GetNumLosses() + string.Empty,
				"sessions:" + Stats.GetNumSessions() + string.Empty
			}, false);*/
			AdMediation.TrackEventInTj("PURCHASED_FIRST_UPGRADE", Stats.GetNumSessions());
			AdMediation.ActionCompleteInTj("e088412d-c699-4c62-b291-6cc71820a83c");
		}
		player.BuyStatUpgradeByNum(currentType, instant, instantComplete);
		if (instant || instantComplete)
		{
			gameSounds.Play_chime_shimmer();
			purchasedMessage.SetActive(true);
			if (!flag2)
			{
				PlayerPrefs.SetInt("USED_INSTANT_COMPLETE", 1);
				hintArrows[1].SetActive(true);
			}
		}
		else
		{
			upgradeInProgress.SetActive(true);
			int upgradeLength2 = Players.GetUpgradeLength(currentLevel);
			Timer timer = (Timer)upgradeInProgress.transform.Find("Timer").GetComponent(typeof(Timer));
			timer.SetSecondsToWait(upgradeLength2 + 2, currentType);
			notificationQueue.Add(sessionVars.currentTimestamp + upgradeLength + 2, Notification.UPGRADE_FINISHED, player.playerPrefNum);
			if (!flag2 && upgradeLength2 > 10)
			{
				flag3 = true;
			}
		}
		if (instantComplete)
		{
			notificationQueue.RemoveNotification(Notification.UPGRADE_FINISHED, player.playerPrefNum);
		}
		else
		{
			gameSounds.Play_three_dribbles();
		}
		if (flag3)
		{
			hintArrows[0].SetActive(false);
			hintArrows[1].SetActive(false);
			hintArrows[2].SetActive(true);
		}
		else
		{
			hintArrows[2].SetActive(false);
			animatingToNextUpgrade = true;
		}
		UpdateUpgradeDisplay(instant || instantComplete);
		if (!flag3)
		{
			StartCoroutine(GoToNextUpgradeOption(0.55f));
		}
		topNavBar.UpdateCurrencyDisplay();
	}

	public virtual void ExchangeGoldForCashAndBuyUpgrade()
	{
		int num = timeredCashPrice - Currency.GetCurrentCash();
		int cashGoldPrice = Currency.GetCashGoldPrice(num, instantPrice);
		if (Currency.SpendGold(cashGoldPrice, "gold_for_upgrade_cash"))
		{
			currencyExchangeBox.gameObject.SetActive(false);
			Currency.AddCash(num, "gold_for_upgrade");
			BuyTimeredUpgrade();
		}
		else
		{
			getGoldButton.ShowGetGoldBox();
			AdMediation.ShowTjpInsufficientCurrency();
		}
	}

	public virtual IEnumerator GoToNextUpgradeOption(float delay)
	{
		int lowest = GetLowestUpgrade();
		int typeToSelect = -1;
		int[] typesInOrderDisplayed = new int[4] { 2, 1, 0, 3 };
		foreach (int num in typesInOrderDisplayed)
		{
			int upgradeLevelByNum = player.GetUpgradeLevelByNum(num);
			if (upgradeLevelByNum == lowest)
			{
				typeToSelect = num;
				if (!player.IsStatBeingUpgraded(num))
				{
					break;
				}
			}
		}
		if (typeToSelect >= 0)
		{
			yield return new WaitForSeconds(delay);
			animatingToNextUpgrade = false;
			SelectUpgrade(typeToSelect);
			signNewPlayerButton.SetActive(false);
		}
		else
		{
			buyPanel.SetActive(false);
			signNewPlayerButton.SetActive(true);
			AchievementsManager.Instance.FullyUpgradedPlayer();
		}
	}

	public virtual void TimerComplete(int completedUpgradeType)
	{
		UpdateUpgradeDisplay(true);
		UpdateBuyPanel();
		if (completedUpgradeType == currentType)
		{
			upgradeInProgress.SetActive(false);
			purchasedMessage.SetActive(true);
			animatingToNextUpgrade = true;
			UpdateUpgradeDisplay(false);
			StartCoroutine(GoToNextUpgradeOption(0.55f));
		}
		gameSounds.Play_chime_shimmer();
	}

	public virtual void OnModifyUpgradeTypeLocalization()
	{
		if (!string.IsNullOrEmpty(Localize.MainTranslation))
		{
			int num = 1;
			if (currentLevel + 1 == 1)
			{
				num++;
			}
			else if (currentLevel + 1 == 3)
			{
				num++;
			}
			else if (currentLevel + 1 == 4)
			{
				num++;
			}
			else if (currentLevel + 1 == 5)
			{
				num += 2;
			}
			Localize.MainTranslation = "+" + num + " " + Localize.MainTranslation;
		}
	}

	public virtual void PlaySelectSound()
	{
		gameSounds.Play_select();
	}

	public virtual void Update()
	{
	}
}
