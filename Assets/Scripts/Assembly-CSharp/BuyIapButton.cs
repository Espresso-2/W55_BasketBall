using System;
using UnityEngine;
using UnityEngine.UI;

public class BuyIapButton : MonoBehaviour
{
	public string iapId;

	public int pkgNum;

	public Text amountText;

	public Text priceText;

	public GameObject loadingBoxPrefab;

	public Transform loadingBoxParent;

	private LoadingBox loadingBox;

	private GameSounds gameSounds;

	private PurchasableItem iapItem;

	public virtual void Awake()
	{
		gameSounds = GameSounds.GetInstance();
	}

	public virtual void OnEnable()
	{
		try
		{
			iapItem = Unibiller.GetPurchasableItemById(iapId);
		}
		catch (Exception ex)
		{
			Debug.Log("ERROR GETTING IAP: " + ex.ToString());
		}
		if (iapItem != null)
		{
			int num = PurchaseGiver.prizeBallsPackAmounts[pkgNum];
			if (amountText != null)
			{
				amountText.text = num.ToString("n0");
			}
			if (priceText != null)
			{
				priceText.text = iapItem.localizedPriceString;
			}
		}
	}

	public virtual void OnClick()
	{
		gameSounds.Play_select();
		ShowLoadingBox();
		Unibiller.initiatePurchase(iapItem, string.Empty);
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
