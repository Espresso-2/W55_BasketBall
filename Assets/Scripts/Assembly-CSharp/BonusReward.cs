using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class BonusReward : MonoBehaviour
{
	public GameObject bag;

	public GameObject cash;

	public Text cashAmount;

	public GameObject starParticles;

	public NativeAdHolder nativeAdHolder;

	public GameObject closeAdButton;

	private bool rewardIsBag;

	public void Start()
	{
		starParticles.SetActive(false);
	}

	public virtual void GiveBagReward()
	{
		DealBag.AddStandardBag("bonusReward");
		rewardIsBag = true;
		ShowAd();
	}

	public virtual void GiveCashReward()
	{
		int amount = GetCashAmount();
		Currency.AddCash(amount, "bonusReward");
		cashAmount.text = amount.ToString("n0");
		rewardIsBag = false;
		ShowAd();
	}

	private void ShowAd()
	{
		bool flag = false;
		closeAdButton.SetActive(false);
		if (AdMediation.nativeAdSmallIsLoaded || flag)
		{
			Debug.Log("BonusReward.ShowAd(): Call NativeAd");
			StartCoroutine(ShowContinueButton());
			nativeAdHolder.ShowAd();
		}
		else
		{
			Debug.Log("BonusReward.ShowAd(): AdMediation.nativeAdSmallIsLoaded == false");
			UnityEngine.Object.Destroy(nativeAdHolder.gameObject);
			ShowReward();
		}
	}

	public void ShowReward()
	{
		bag.SetActive(rewardIsBag);
		cash.SetActive(!rewardIsBag);
		starParticles.SetActive(true);
		GameSounds.GetInstance().Play_coin_glow_2();
		/*if (PlayerPrefs.GetInt("NUM_PURCHASES") == 0 || PlayerPrefs.GetInt("IS_FRAUDULENT_USER") == 1)
		{
			AdMediation.RequestNativeAdSmall();
		}*/
	}

	private IEnumerator ShowContinueButton()
	{
		if (Time.timeScale <= 0f)
		{
			Time.timeScale = 1f;
		}
		yield return new WaitForSeconds(4f);
		closeAdButton.SetActive(true);
	}

	private int GetCashAmount()
	{
		int num = 0;
		int num2 = UnityEngine.Random.Range(0, 100);
		if (SessionVars.GetInstance().goToTutorial)
		{
			return 2000;
		}
		if (num2 >= 90)
		{
			return 200;
		}
		if (num2 >= 80)
		{
			return 125;
		}
		if (num2 >= 70)
		{
			return 125;
		}
		if (num2 >= 60)
		{
			return 115;
		}
		if (num2 >= 50)
		{
			return 100;
		}
		if (num2 >= 40)
		{
			return 75;
		}
		if (num2 >= 30)
		{
			return 50;
		}
		return 25;
	}
}
