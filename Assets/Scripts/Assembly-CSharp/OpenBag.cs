using System;
using System.Collections;
using I2.Loc;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class OpenBag : MonoBehaviour
{
	public DealsScreen dealsScreen;

	public GameObject openLabel;

	public GameObject openArrow;

	public GameObject collectLabel;

	public Button closeButton;

	public Button customizeScreenButton;

	public Button buyOneMoreButton;

	public Localize buyOneMoreButtonBagLabel;

	public GameObject buyOneMoreDialog;

	public Localize buyOneMoreDialogHeadingTitle;

	public Localize buyOneMoreDialogTitle;

	public Text buyOneMoreDialogOrigPrice;

	public Text buyOneMoreDialogDiscountPrice;

	public GameObject buyOneMoreDialogGoldIcon;

	public GameObject buyOneMoreDialogCashIcon;

	public Animator anim;

	public BagCard[] cards;

	public GameObject confetti;

	public TabChanger tabChanger;

	private bool openedBag;

	public TopNavBar topNavBar;

	private GameSounds gameSounds;

	private bool isPremium;

	private bool isDaily;

	private string buyOneMoreBagHeadingTerm;

	private string buyOneMoreBagTerm;

	private int buyOneMoreOrigPrice;

	private int buyOneMoreDiscountPrice;

	private int numFlips;

	public virtual void Start()
	{
		gameSounds = GameSounds.GetInstance();
		LeanTween.scale(buyOneMoreButton.gameObject, new Vector3(0.95f, 0.95f, 0.95f), 0.25f).setLoopPingPong();
	}

	public virtual void CloseButtonOnClick()
	{
		gameSounds.Play_select();
		tabChanger.SetToTab(tabEnum.Deals);
	}

	public virtual IEnumerator CardFlipped()
	{
		numFlips++;
		if (numFlips == cards.Length)
		{
			yield return new WaitForSeconds(1f);
			closeButton.gameObject.SetActive(true);
			customizeScreenButton.gameObject.SetActive(true);
			buyOneMoreButton.gameObject.SetActive(true);
			buyOneMoreButtonBagLabel.SetTerm(buyOneMoreBagTerm, null);
		}
	}

	public virtual void ShowOpenBagBox(bool isPremium, bool isDaily, string buyOneMoreBagHeadingTerm, string buyOneMoreBagTerm, int buyOneMoreOrigPrice, int buyOneMoreDiscountPrice)
	{
		anim.SetTrigger("Close");
		buyOneMoreDialog.SetActive(false);
		confetti.SetActive(false);
		this.isPremium = isPremium;
		this.isDaily = isDaily;
		this.buyOneMoreBagHeadingTerm = buyOneMoreBagHeadingTerm;
		this.buyOneMoreBagTerm = buyOneMoreBagTerm;
		this.buyOneMoreOrigPrice = buyOneMoreOrigPrice;
		this.buyOneMoreDiscountPrice = buyOneMoreDiscountPrice;
		openedBag = false;
		numFlips = 0;
		closeButton.gameObject.SetActive(false);
		customizeScreenButton.gameObject.SetActive(false);
		buyOneMoreButton.gameObject.SetActive(false);
		buyOneMoreDialog.SetActive(false);
		openLabel.SetActive(true);
		openArrow.SetActive(true);
		collectLabel.SetActive(false);
	}

	public virtual void BagOnClick()
	{
		if (openedBag)
		{
			return;
		}
		openedBag = true;
		openLabel.SetActive(false);
		openLabel.SetActive(false);
		collectLabel.SetActive(true);
		confetti.SetActive(true);
		bool flag = false;
		for (int i = 0; i < cards.Length; i++)
		{
			BagCard bagCard = cards[i];
			bagCard.gameObject.SetActive(true);
			bagCard.gameObject.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
			LeanTween.scale(bagCard.gameObject, new Vector3(0.95f, 0.95f, 0.95f), 1.5f).setEase(LeanTweenType.easeOutElastic);
			bool forceToBeCustomItem = false;
			if (!flag && i + 1 == cards.Length)
			{
				forceToBeCustomItem = true;
			}
			if (bagCard.SetCardItem(isPremium, forceToBeCustomItem))
			{
				flag = true;
			}
		}
		anim.SetTrigger("OpenBag");
		gameSounds.Play_ascend_chime_bright();
		gameSounds.Play_coin_glow_2();
		AchievementsManager.Instance.OpenedBag();
	}

	public virtual void BuyOneMoreOnClick()
	{
		gameSounds.Play_select();
		buyOneMoreDialog.SetActive(true);
		buyOneMoreDialogHeadingTitle.SetTerm(buyOneMoreBagHeadingTerm, null);
		buyOneMoreDialogTitle.SetTerm(buyOneMoreBagTerm, null);
		buyOneMoreDialogOrigPrice.text = buyOneMoreOrigPrice.ToString("n0");
		buyOneMoreDialogDiscountPrice.text = buyOneMoreDiscountPrice.ToString("n0");
		buyOneMoreDialogGoldIcon.SetActive(true);
		buyOneMoreDialogCashIcon.SetActive(false);
		if (isDaily)
		{
			dealsScreen.SelectBag(1);
		}
	}
}
