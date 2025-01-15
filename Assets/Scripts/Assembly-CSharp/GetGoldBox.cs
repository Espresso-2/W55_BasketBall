using System;
using I2.Loc;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class GetGoldBox : MonoBehaviour
{
	public MessageBox messageBox;

	public Localize messageBoxMessage;

	public Text amount;

	public GameObject cashAmountHolder;

	public Text cashAmount;

	private GameSounds gameSounds;

	public TopNavBar topNavBar;

	public virtual void Awake()
	{
		gameSounds = GameSounds.GetInstance();
	}

	public virtual void OnEnable()
	{
		gameSounds.Play_dunk();
		CheckForEarnedCurrency();
	}

	public virtual void Start()
	{
		InvokeRepeating(nameof(CheckForEarnedCurrency), 0f, 1.5f);
	}

	public virtual void AddedGoldPackage(int pkgNum)
	{
		topNavBar.UpdateCurrencyDisplay();
		int goldAmountForPackage = PurchaseGiver.GetGoldAmountForPackage(pkgNum);
		messageBox.gameObject.SetActive(true);
		messageBox.SetToPurchasedCurrency(goldAmountForPackage, 0);
	}

	public virtual void AddedGoldAndCashPackage(int pkgNum)
	{
		topNavBar.UpdateCurrencyDisplay();
		int goldAmountForPackage = PurchaseGiver.GetGoldAmountForPackage(pkgNum);
		int cash = PurchaseGiver.cashPackAmounts[pkgNum];
		messageBox.gameObject.SetActive(true);
		messageBox.SetToPurchasedCurrency(goldAmountForPackage, cash);
	}

	public virtual void AdComplete(int amount)
	{
		RewardGold(amount, amount.ToString("n0"), "rewVid");
	}

	public virtual void CheckForEarnedCurrency()
	{
		//AdMediation.CheckForEarnedCurrency();
		int[] intArray = PlayerPrefsX.GetIntArray(AdMediation.GOLD_OFFER_PREF);
		int num = 0;
		string text = string.Empty;
		for (int i = 0; i < intArray.Length; i++)
		{
			int num2 = intArray[i];
			if (num2 > 0)
			{
				num += num2;
				if (i > 0)
				{
					text += "+";
				}
				text += num2.ToString("n0");
			}
		}
		if (num > 0)
		{
			RewardGold(num, text, "offerwall");
			PlayerPrefsX.SetIntArray(AdMediation.GOLD_OFFER_PREF, new int[0]);
		}
	}

	private void RewardGold(int amount, string amountTxt, string method)
	{
		Currency.AddGold(amount, method);
		topNavBar.UpdateCurrencyDisplay();
		if (messageBox.gameObject.activeInHierarchy)
		{
			this.amount.text = this.amount.text + "+" + amountTxt;
		}
		else
		{
			this.amount.text = amountTxt;
			messageBox.gameObject.SetActive(true);
		}
		messageBoxMessage.SetTerm("REWARD RECEIVED", null);
	}

	public virtual void Update()
	{
	}
}
