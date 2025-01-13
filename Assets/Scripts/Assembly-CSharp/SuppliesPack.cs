using System;
using System.Collections;
using I2.Loc;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class SuppliesPack : MonoBehaviour
{
	public Sprite[] pictures;

	public string[] titles;

	public string[] descsPrefix;

	public string[] descs;

	public int[] prices;

	public GameObject priceIcon;

	public int[] amounts;

	public Text[] buttonAmounts;

	public TopNavBar topNavBar;

	public GetGoldButton getGoldButton;

	public Text notEnoughGold;

	public int packNum;

	public Image picture;

	public Localize title;

	public Localize desc;

	public Text priceText;

	public Localize priceTextlocalize;

	public TimeoutScreen timeoutScreen;

	public GameObject hintArrow;

	private int currentType;

	private bool isFree;

	private bool claimedFree;

	private int priceMultiplier;

	private GameSounds gameSounds;

	public virtual void Start()
	{
		gameSounds = GameSounds.GetInstance();
	}

	public virtual IEnumerator SetType(int type, bool isFree)
	{
		priceMultiplier = 3;
		currentType = type;
		this.isFree = isFree;
		claimedFree = false;
		picture.sprite = pictures[currentType];
		title.SetTerm(titles[currentType], null);
		desc.SetTerm(descs[currentType], null);
		if (this.isFree)
		{
			priceTextlocalize.SetTerm("ONE FREE", null);
			priceIcon.SetActive(false);
			yield return new WaitForSeconds(0.65f);
			hintArrow.SetActive(true);
			yield break;
		}
		priceText.text = (prices[currentType] * priceMultiplier).ToString("n0");
		priceIcon.SetActive(true);
		if (hintArrow != null)
		{
			hintArrow.SetActive(false);
		}
	}

	public virtual void BuyPack()
	{
		if (claimedFree)
		{
			return;
		}
		if (notEnoughGold != null)
		{
			notEnoughGold.gameObject.SetActive(false);
		}
		int amount = prices[currentType] * priceMultiplier;
		int num = amounts[currentType];
		if (isFree || Currency.SpendGold(amount, Supplies.NAMES_FOR_LOGGING[currentType]))
		{
			gameSounds.Play_catch_ball();
			gameSounds.Play_ascend_chime_bright();
			gameSounds.Play_thump_on_floor_2();
			StartCoroutine(UpdateButtonSupplyAmount(currentType, num, Supplies.AddItem(currentType, num)));
			if (timeoutScreen != null)
			{
				timeoutScreen.UpdateSupplyAmounts();
			}
			topNavBar.UpdateCurrencyDisplay();
			if (isFree)
			{
				PlayerPrefsHelper.SetInt(Supplies.COLLECTED_FREE_KEY + currentType, 1);
				priceTextlocalize.SetTerm("RECEIVED", null);
				claimedFree = true;
				if (hintArrow != null)
				{
					hintArrow.SetActive(false);
				}
			}
			else
			{
				/*FlurryAnalytics.Instance().LogEvent("PURCHASE_STOREITEM", new string[2]
				{
					"currentType:" + currentType,
					"num_wins_milestone:" + Stats.GetNumWinsMilestone() + string.Empty
				}, false);*/
				AdMediation.ShowTjpPurchasedStoreItem();
			}
		}
		else
		{
			if (getGoldButton != null)
			{
				getGoldButton.ShowGetGoldBox();
				AdMediation.ShowTjpInsufficientCurrency();
			}
			if (notEnoughGold != null)
			{
				gameSounds.Play_pinball_beep();
				notEnoughGold.gameObject.SetActive(true);
			}
		}
		if (isFree)
		{
			((NotificationQueue)GameObject.Find("NotificationQueue").GetComponent(typeof(NotificationQueue))).CheckForNotifications();
		}
	}

	private IEnumerator UpdateButtonSupplyAmount(int type, int purchasedAmount, int newTotalAmount)
	{
		int previousNumCredits = newTotalAmount - purchasedAmount;
		float countTimeTotal = 25f;
		int prevAmountDisplay = 0;
		buttonAmounts[type].fontStyle = FontStyle.Bold;
		for (int i = 1; (float)i <= countTimeTotal; i++)
		{
			yield return new WaitForSeconds(0.02f);
			float percentComplete = (float)i / countTimeTotal;
			int newAmountDisplay = (int)Mathf.Round((float)previousNumCredits + (float)purchasedAmount * percentComplete);
			if (newAmountDisplay != prevAmountDisplay)
			{
				prevAmountDisplay = newAmountDisplay;
				buttonAmounts[type].text = newAmountDisplay.ToString();
			}
		}
		buttonAmounts[type].fontStyle = FontStyle.Normal;
	}

	public virtual void OnModifyDescLocalization()
	{
		if (!string.IsNullOrEmpty(Localize.MainTranslation))
		{
			string text = descsPrefix[currentType];
			Localize.MainTranslation = text + Localize.MainTranslation;
		}
	}

	public virtual void Update()
	{
	}
}
