using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class RemoveAdsMsgBox : MonoBehaviour
{
	public Text oldPriceText;

	public Text priceText;

	public GameObject box;

	public GameObject purchaseCompleteBox;

	public ShowRemoveAdsButton showRemoveAdsButton;

	public TopNavBar topNavBar;

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
		/*try
		{
			*//*iapItemForOldPrice = Unibiller.GetPurchasableItemById("com.doubletapsoftware.basketballbattle.goldpack1");
			iapItem = Unibiller.GetPurchasableItemById("com.doubletapsoftware.basketballbattle.removeads")*//*;
		}
		catch (Exception ex)
		{
			Debug.Log("Error loading remove ads IAP: " + ex.ToString());
		}*/
		if (iapItem != null && iapItemForOldPrice != null)
		{
			oldPriceText.text = iapItemForOldPrice.localizedPriceString;
			priceText.text = iapItem.localizedPriceString;
		}
		gameSounds.Play_coin_glow();
		//FlurryAnalytics.Instance().LogEvent("SHOW_REMOVE_ADS_OFFER_" + Application.loadedLevelName);
	}

	public virtual void Start()
	{
	}

	public virtual void BuyButtonOnClick()
	{
		gameSounds.Play_ball_dribble();
		ShowLoadingBox();
		/*Unibiller.initiatePurchase(iapItem, string.Empty);*/
	}

	public virtual void RemovedAds()
	{
		purchaseCompleteBox.SetActive(true);
		gameSounds.Play_coin_glow();
		if (showRemoveAdsButton != null)
		{
			showRemoveAdsButton.gameObject.SetActive(false);
		}
		if (topNavBar != null)
		{
			topNavBar.UpdateCurrencyDisplay();
		}
	}

	public virtual void CloseButtonOnClick()
	{
		gameSounds.Play_select();
		purchaseCompleteBox.SetActive(false);
		if (Application.loadedLevelName != "MainUI")
		{
			box.SetActive(false);
			Application.LoadLevel("MainUI");
		}
		else
		{
			base.gameObject.SetActive(false);
		}
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
