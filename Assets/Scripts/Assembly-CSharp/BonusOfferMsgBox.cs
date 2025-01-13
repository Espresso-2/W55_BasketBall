using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class BonusOfferMsgBox : MonoBehaviour
{
	public Text oldPriceText;

	public Text priceText;

	public Text cashAmtText;

	public Text goldAmtText;

	public GameObject loadingBoxPrefab;

	public Transform loadingBoxParent;

	private LoadingBox loadingBox;

	private GameSounds gameSounds;

	private PurchasableItem iapItemForOldPrice;

	private PurchasableItem iapItem;

	public virtual void Awake()
	{
		gameSounds = GameSounds.GetInstance();
	}

	public virtual void OnEnable()
	{
		try
		{
			iapItemForOldPrice = Unibiller.GetPurchasableItemById("com.doubletapsoftware.basketballbattle.goldpack1");
			iapItem = Unibiller.GetPurchasableItemById("com.doubletapsoftware.basketballbattle.bigwallet");
		}
		catch (Exception ex)
		{
			Debug.Log("ERROR GETTING IAP: " + ex.ToString());
		}
		if (iapItem != null && iapItemForOldPrice != null)
		{
			oldPriceText.text = iapItemForOldPrice.localizedPriceString;
			priceText.text = iapItem.localizedPriceString;
			int num = PurchaseGiver.cashPackAmounts[0];
			int num2 = PurchaseGiver.goldPackAmounts[0];
			cashAmtText.text = "X " + num.ToString("n0");
			goldAmtText.text = "X " + num2.ToString("n0");
		}
		gameSounds.Play_coin_glow();
		//FlurryAnalytics.Instance().LogEvent("SHOW_BIG_WALLET_OFFER", new string[1] { "num_wins_milestone:" + Stats.GetNumWinsMilestone() + string.Empty }, false);
	}

	public virtual void Start()
	{
	}

	public virtual void BuyButtonOnClick()
	{
		gameSounds.Play_select();
		ShowLoadingBox();
		Unibiller.initiatePurchase(iapItem, string.Empty);
	}

	public virtual void CompletedPurchase()
	{
		base.gameObject.SetActive(false);
	}

	public virtual void CloseButtonOnClick()
	{
		gameSounds.Play_select();
		base.gameObject.SetActive(false);
	}

	private void ShowLoadingBox()
	{
		if (loadingBoxParent == null)
		{
			GameObject gameObject = GameObject.Find("Canvas");
			if (gameObject != null)
			{
				loadingBoxParent = gameObject.transform;
			}
		}
		if (loadingBoxParent != null)
		{
			loadingBox = UnityEngine.Object.Instantiate(loadingBoxPrefab, loadingBoxParent).GetComponent<LoadingBox>();
		}
	}
}
