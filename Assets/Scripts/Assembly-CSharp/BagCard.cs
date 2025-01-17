using System;
using System.Collections;
using System.Collections.Generic;
using I2.Loc;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class BagCard : MonoBehaviour
{
	public PlayerDetails playerDetails;

	public CustomItemDisplay customItemDisplay;

	public bool isExampleCard;

	public float secShowingExampleCard;

	private bool isPremiumBag;

	public OpenBag openBag;

	public GameObject front;

	public bool flipped;

	public GameObject rareIcon;

	public GameObject premiumIcon;

	public GameObject customItemReward;

	public CustomItem customItem;

	public CustomItemVisual customItemVisual;

	public GameObject currencyReward;

	public GameObject[] goldPhotos;

	public GameObject goldAmountIcon;

	public Text goldAmountText;

	public GameObject[] cashPhotos;

	public GameObject cashAmountIcon;

	public Text cashAmountText;

	public GameObject supplyReward;

	public Image supplyPhoto;

	public Text supplyAmountText;

	public Sprite[] supplySprites;

	public GameObject activateButton;

	public Localize activateButtonText;

	public GameObject activeLabel;

	private CustomItems customItems;

	public TopNavBar topNavBar;

	public CurrencyCollectionAnim cashAnim;

	public CurrencyCollectionAnim goldAnim;

	public GameObject collectCashAnimPrefab;

	public GameObject collectGoldAnimPrefab;

	private GameSounds gameSounds;

	private int goldAmount;

	private int cashAmount;

	private int supplyAmount;

	private bool playChime;

	public virtual void Start()
	{
		gameSounds = GameSounds.GetInstance();
	}

	public virtual void BackOnClick()
	{
		if (!isExampleCard)
		{
			gameSounds.Play_tap();
			StartCoroutine(openBag.CardFlipped());
			if (goldAmount > 0)
			{
				Currency.AddGold(goldAmount);
			}
			else if (cashAmount > 0)
			{
				Currency.AddCash(cashAmount);
			}
			StartCoroutine(PlayAwardEffects());
		}
		LeanTween.scale(base.gameObject, new Vector3(0.05f, 1f, 1f), 0.15f).setEase(LeanTweenType.easeInOutQuad);
		flipped = true;
		StartCoroutine(ShowFrontVisual());
	}

	public virtual void FixedUpdate()
	{
		if (isExampleCard)
		{
			secShowingExampleCard += Time.deltaTime;
			if (secShowingExampleCard > 0.8f && !flipped)
			{
				BackOnClick();
			}
			else if (secShowingExampleCard > 5f)
			{
				StartCoroutine(ShowNewExample());
			}
		}
	}

	public virtual IEnumerator ShowNewExample()
	{
		secShowingExampleCard = 0f;
		LeanTween.scale(base.gameObject, new Vector3(0.05f, 1f, 1f), 0.15f).setEase(LeanTweenType.easeInOutQuad);
		yield return new WaitForSeconds(0.15f);
		SetCardItem(forceToBeCustomItem: UnityEngine.Random.Range(0, 100) >= 25, isPremium: isPremiumBag);
		LeanTween.scale(base.gameObject, new Vector3(1f, 1f, 1f), 0.15f).setEase(LeanTweenType.easeInOutQuad);
	}

	private IEnumerator PlayAwardEffects()
	{
		yield return new WaitForSeconds(0.1f);
		if (playChime)
		{
			gameSounds.Play_coin_glow_2();
		}
		yield return new WaitForSeconds(0.1f);
		if (goldAmount > 0)
		{
			ShowCurrencyAnimPrefab(collectGoldAnimPrefab, topNavBar.goldIcon.transform);
			topNavBar.UpdateGoldDisplay();
		}
		else if (cashAmount > 0)
		{
			ShowCurrencyAnimPrefab(collectCashAnimPrefab, topNavBar.cashIcon.transform);
			topNavBar.UpdateCashDisplay();
		}
		else if (supplyAmount > 0)
		{
			gameSounds.Play_thump_on_floor_2();
		}
	}

	private void ShowCurrencyAnimPrefab(GameObject prefab, Transform dest)
	{
		CurrencyCollectionAnim currencyCollectionAnim = (CurrencyCollectionAnim)UnityEngine.Object.Instantiate(prefab, prefab.transform.position, prefab.transform.rotation).GetComponent(typeof(CurrencyCollectionAnim));
		currencyCollectionAnim.transform.position = base.transform.position + new Vector3(1.5f, 1.5f, 0f);
		currencyCollectionAnim.destination = dest;
		currencyCollectionAnim.transform.parent = topNavBar.transform;
	}

	private IEnumerator ShowFrontVisual()
	{
		yield return new WaitForSeconds(0.15f);
		LeanTween.scale(base.gameObject, new Vector3(1f, 1f, 1f), 0.15f).setEase(LeanTweenType.easeInOutQuad);
		front.SetActive(true);
	}

	public virtual bool SetCardItem(bool isPremium, bool forceToBeCustomItem)
	{
		isPremiumBag = isPremium;
		bool result = false;
		int num = UnityEngine.Random.Range(0, 100);
		if (forceToBeCustomItem)
		{
			num = 0;
		}
		if (num >= 50)
		{
			bool isCash = UnityEngine.Random.Range(0, 100) >= 50 && (!isExampleCard || UnityEngine.Random.Range(0, 100) >= 90);
			SetToCurrency(GenerateAmount(isPremium, isCash, false), isPremium, isCash);
		}
		else if (num >= 25)
		{
			int supplyType = UnityEngine.Random.Range(1, 5);
			SetToSupply(supplyType, GenerateAmount(isPremium, false, true), isPremium);
		}
		else
		{
			result = true;
			int num2 = UnityEngine.Random.Range(0, 5);
			if (customItems == null)
			{
				customItems = (CustomItems)GameObject.Find("CustomItems").GetComponent(typeof(CustomItems));
			}
			List<CustomItem> items = customItems.GetItems(num2);
			int index = UnityEngine.Random.Range(0, items.Count);
			CustomItem customItem = items[index];
			int num3 = (int)Mathf.Round(items.Count / 5);
			int num4 = 0;
			int num5 = 0;
			int num6 = 999;
			if (isPremium || UnityEngine.Random.Range(0, 100) >= 50)
			{
				num5 = 10;
			}
			else
			{
				num6 = 5;
			}
			bool flag = false;
			while (customItem.goldValue > num6 || customItem.goldValue < num5 || customItem.owned)
			{
				index = UnityEngine.Random.Range(0, items.Count);
				customItem = items[index];
				num4++;
				if (num4 > num3)
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				bool isCash2 = UnityEngine.Random.Range(0, 100) >= 50 && (!isExampleCard || UnityEngine.Random.Range(0, 100) >= 90);
				SetToCurrency(GenerateAmount(isPremium, isCash2, false), isPremium, isCash2);
			}
			else
			{
				if (!isExampleCard)
				{
					customItem.Claim();
				}
				SetToItem(customItem, isPremium);
			}
			CustomItemDisplay.currentItemType = num2;
		}
		return result;
	}

	private int GenerateAmount(bool isPremium, bool isCash, bool isSupply)
	{
		int num = 0;
		int num2 = UnityEngine.Random.Range(0, 100);
		if (isExampleCard)
		{
			num2 = ((!isPremium) ? UnityEngine.Random.Range(65, 100) : UnityEngine.Random.Range(80, 100));
		}
		num = ((num2 >= 90) ? 8 : ((num2 >= 80) ? 6 : ((num2 >= 70) ? 4 : ((num2 >= 60) ? 2 : ((num2 >= 50) ? 2 : ((num2 >= 40) ? 2 : ((num2 >= 30) ? 2 : ((num2 >= 20) ? 2 : ((num2 < 10) ? 1 : 1)))))))));
		if (isPremium)
		{
			num = (int)((float)num * 1.5f);
			if (num == 1)
			{
				num = 2;
			}
		}
		if (isCash)
		{
			num *= 750;
		}
		else if (isSupply)
		{
			num *= 2;
		}
		return num;
	}

	public virtual void SetToCurrency(int amount, bool isPremiumBag, bool isCash)
	{
		Reset();
		currencyReward.SetActive(true);
		if (isCash)
		{
			cashAmount = amount;
			cashAmountIcon.SetActive(true);
			cashAmountText.text = "X " + amount.ToString("n0");
		}
		else
		{
			goldAmount = amount;
			goldAmountIcon.SetActive(true);
			goldAmountText.text = "X " + amount.ToString("n0");
		}
		bool flag = amount >= 4;
		GameObject[] array = goldPhotos;
		GameObject[] array2 = cashPhotos;
		if (isCash)
		{
			flag = amount >= 4000;
			array = cashPhotos;
			array2 = goldPhotos;
		}
		for (int i = 0; i < array.Length; i++)
		{
			GameObject gameObject = array[i];
			if (i == 0)
			{
				gameObject.SetActive(!flag);
			}
			else
			{
				gameObject.SetActive(flag);
			}
		}
		for (int j = 0; j < array2.Length; j++)
		{
			array2[j].SetActive(false);
		}
		premiumIcon.SetActive(flag || isPremiumBag);
		if (flag)
		{
			playChime = true;
		}
	}

	public virtual void SetToSupply(int supplyType, int amount, bool isPremiumBag)
	{
		Reset();
		supplyReward.SetActive(true);
		supplyAmount = amount;
		supplyPhoto.sprite = supplySprites[supplyType];
		bool flag = amount >= 8;
		supplyAmountText.text = "X " + amount.ToString("n0");
		premiumIcon.SetActive(flag || isPremiumBag);
		if (flag)
		{
			playChime = true;
		}
		if (!isExampleCard)
		{
			GiveSupply(supplyType, amount);
		}
	}

	public virtual void SetToItem(CustomItem customItem, bool isPremiumBag)
	{
		Reset();
		this.customItem = customItem;
		customItemReward.SetActive(true);
		customItemVisual.gameObject.SetActive(true);
		activateButton.SetActive(true);
		if (activateButtonText != null)
		{
			if (customItem.type == CustomItems.BALL)
			{
				activateButtonText.SetTerm("USE", null);
			}
			else if (customItem.type == CustomItems.ARM_BAND)
			{
				activateButtonText.SetTerm("WEAR ARMBANDS", null);
			}
			else
			{
				activateButtonText.SetTerm("WEAR", null);
			}
		}
		if (customItemVisual != null)
		{
			customItemVisual.SetVisual(customItem);
		}
		rareIcon.SetActive(customItem.goldValue >= 15);
		premiumIcon.SetActive(customItem.goldValue >= 10 || isPremiumBag);
		if (customItem.goldValue >= 15)
		{
			playChime = true;
		}
	}

	private void Reset()
	{
		if (currencyReward != null)
		{
			currencyReward.SetActive(false);
		}
		if (supplyReward != null)
		{
			supplyReward.SetActive(false);
		}
		customItemReward.SetActive(false);
		goldAmount = 0;
		cashAmount = 0;
		supplyAmount = 0;
		flipped = false;
		front.SetActive(false);
		playChime = false;
		activateButton.SetActive(false);
		activeLabel.SetActive(false);
		rareIcon.SetActive(false);
		premiumIcon.SetActive(false);
		if (cashAmountIcon != null)
		{
			cashAmountIcon.SetActive(false);
		}
		if (goldAmountIcon != null)
		{
			goldAmountIcon.SetActive(false);
			goldAmountText.text = string.Empty;
			cashAmountText.text = string.Empty;
		}
	}

	public virtual void ActivateOnClick()
	{
		bool toActive = true;
		customItem.SetToActive(toActive);
		gameSounds.Play_select();
		activateButton.SetActive(false);
		activeLabel.SetActive(true);
		if (playerDetails != null)
		{
			playerDetails.ShowCorrectPlayer();
		}
		if (customItemDisplay != null)
		{
			customItemDisplay.Refresh(customItem.num);
		}
	}

	private void GiveSupply(int supplyType, int amount)
	{
		if (supplyType == Supplies.OXYGEN)
		{
			Supplies.AddItem(Supplies.OXYGEN, amount);
		}
		if (supplyType == Supplies.GRIP)
		{
			Supplies.AddItem(Supplies.GRIP, amount);
		}
		if (supplyType == Supplies.CHALK)
		{
			Supplies.AddItem(Supplies.CHALK, amount);
		}
		if (supplyType == Supplies.PROTEIN)
		{
			Supplies.AddItem(Supplies.PROTEIN, amount);
		}
		if (supplyType == Supplies.DRINK)
		{
			Supplies.AddItem(Supplies.DRINK, amount);
		}
	}
}
