using System;
using I2.Loc;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class MessageBox : MonoBehaviour
{
	public Localize bodyText;

	public GameObject picture;

	public Text amount;

	public GameObject cashAmountHolder;

	public Text cashAmount;

	public Localize continueButtonText;

	public TabChanger tabChanger;

	private int goToTab;

	private GameSounds gameSounds;

	public virtual void Awake()
	{
		gameSounds = GameSounds.GetInstance();
	}

	public virtual void OnEnable()
	{
		goToTab = -1;
		picture.SetActive(true);
		amount.gameObject.SetActive(true);
		cashAmountHolder.SetActive(false);
		gameSounds.Play_coin_glow();
	}

	public virtual void SetToPurchasedDeal(string title)
	{
		bodyText.SetTerm("SUCCESSFULLY PURCHASED " + title, null);
		continueButtonText.SetTerm("PLAYERS", null);
		picture.SetActive(false);
		amount.gameObject.SetActive(false);
		goToTab = 1;
	}

	public virtual void SetToPurchasedCurrency(int gold, int cash)
	{
		bodyText.SetTerm("YOU HAVE PURCHASED", null);
		continueButtonText.SetTerm("OK", null);
		amount.text = gold.ToString("n0");
		if (cash > 0)
		{
			cashAmountHolder.SetActive(true);
			cashAmount.text = cash.ToString("n0");
		}
	}

	public virtual void ContinueButton()
	{
		gameSounds.Play_select();
		if (goToTab >= 0)
		{
			tabChanger.SetToTab((tabEnum)goToTab);
		}
		base.gameObject.SetActive(false);
	}
}
