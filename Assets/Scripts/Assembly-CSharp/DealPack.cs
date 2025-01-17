using System;
using I2.Loc;
using UnityEngine;
using UnityEngine.UI;
using W_Log.w_Script;

[Serializable]
public class DealPack : MonoBehaviour
{
	public Localize title;

	public Text goldPrice;

	public Text iapPrice;

	public Localize iapPriceLocalize;

	public GameObject[] supplies;

	public Text[] supplyAmounts;

	public PlayerDetails player;

	public MessageBox messageBox;

	public GameObject loadingBoxPrefab;

	public Transform loadingBoxParent;

	private LoadingBox loadingBox;

	private int num;

	public static string[] titles;

	private int[] goldPrices;

	private GameSounds gameSounds;

	private SessionVars sessionVars;

	private int iapNum;

	private PurchasableItem iapItem;

	private bool dealExpired;

	public DealPack()
	{
		goldPrices = new int[2] { 133, 370 };
	}

	static DealPack()
	{
		titles = new string[2] { "ALL STAR PACKAGE", "SUPER STAR PACKAGE" };
	}

	public virtual void Start()
	{
		gameSounds = GameSounds.GetInstance();
		sessionVars = SessionVars.GetInstance();
		player.gameObject.SetActive(false);
	}

	public virtual void SetItem(int num)
	{
		this.num = num;
		dealExpired = Players.IsStarterPurchased(false, PurchaseGiver.dealPlayers[num]);
		if (dealExpired)
		{
			iapPriceLocalize.SetTerm("DEAL CLOSED", null);
		}
		else
		{
			iapNum = 6 + this.num;
			 /*iapItem = Unibiller.AllPurchasableItems[iapNum];*/
			iapPrice.text = "获取" /*iapItem.localizedPriceString*/;
		}
		title.SetTerm(titles[num], null);
		goldPrice.text = goldPrices[num].ToString("n0");
		player.gameObject.SetActive(true);
		player.SetPlayerAndGender(PurchaseGiver.dealPlayers[num], false);
		for (int i = 0; i < supplies.Length; i++)
		{
			GameObject gameObject = supplies[i];
			int num2 = PurchaseGiver.dealAmounts[num][i];
			supplyAmounts[i].text = "X " + num2;
			gameObject.SetActive(num2 > 0);
		}
	}

	public virtual void Buy()
	{
		//TODO:激励点
		if (!dealExpired)
		{
			gameSounds.Play_catch_ball();
			gameSounds.Play_ascend_chime_bright();
		
			PlayerPrefsHelper.SetInt("TAPJOY_BONUS", 0);
			PlayerPrefsHelper.SetInt("IS_TAPJOY_PURCHASE_REQUEST", 0);
			w_RewardAdapter.Instance.onPurchased(13);
			
		}
	}

	public virtual void RecievedDeal(int dealNum)
	{
		num = dealNum;
		int selectedStarterNum = PurchaseGiver.dealPlayers[num];
		sessionVars.selectedStarterNum = selectedStarterNum;
		messageBox.gameObject.SetActive(true);
		messageBox.SetToPurchasedDeal(titles[num]);
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
