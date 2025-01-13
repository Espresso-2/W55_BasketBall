using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class HalfTime : MonoBehaviour
{
	public GameObject cheerLeader;

	public TimeoutScreen timeoutScreen;

	public GameObject halfTimeBox;

	public GameObject cheerleader;

	public GameObject coach;

	public GameObject coachSpeachArrow;

	public GameObject continueButton;

	public GameController gameController;

	public NativeAdHolder nativeAdHolder;

	private GameSounds gameSounds;

	private float secondsToWait = 5f;

	public virtual void Start()
	{
		gameSounds = GameSounds.GetInstance();
	}

	public void OnEnable()
	{
		int @int = PlayerPrefs.GetInt("BANNER_AD_SETTING");
		switch (@int)
		{
		case 3:
			secondsToWait = 7f;
			break;
		case 4:
			secondsToWait = 9f;
			break;
		}
		bool flag = false;
		if (@int == 0 || (PlayerPrefs.GetInt("NUM_PURCHASES") > 0 && PlayerPrefs.GetInt("IS_FRAUDULENT_USER") != 1) || (!AdMediation.nativeAdIsLoaded && !AdMediation.adMobCenterBannerIsLoaded && !flag))
		{
			Debug.Log("Don't show user ad, go right to halftime players");
			StartCoroutine(ShowPlayers());
		}
		else
		{
			continueButton.SetActive(false);
			StartCoroutine(ShowAd());
			StartCoroutine(ShowContinueButton());
		}
	}

	private IEnumerator ShowAd()
	{
		Debug.Log("HalfTime.ShowAd()");
		if (!DtUtils.IsWideScreenDevice())
		{
			coach.SetActive(false);
			coachSpeachArrow.SetActive(false);
		}
		if (Time.timeScale <= 0f)
		{
			Time.timeScale = 1f;
		}
		if (AdMediation.nativeAdIsLoaded)
		{
			Debug.Log("HalfTime.ShowAd(): Call NativeAd");
			nativeAdHolder.ShowAd();
		}
		else
		{
			UnityEngine.Object.Destroy(nativeAdHolder.gameObject);
			yield return new WaitForSeconds(0.85f);
			AdMediation.ShowCenterBanner();
		}
		LogAd();
	}

	private void LogAd()
	{
		Debug.Log("HalfTime.LogAd()");
		PlayerPrefsHelper.SetInt("NUM_HALFTIME_ADS", PlayerPrefs.GetInt("NUM_HALFTIME_ADS") + 1, true);
		/*FlurryAnalytics.Instance().LogEvent("SHOWED_HALFTIME_AD", new string[2]
		{
			"type:halftime",
			"num_wins_milestone:" + Stats.GetNumWinsMilestone() + string.Empty
		}, false);*/
	}

	private IEnumerator ShowContinueButton()
	{
		if (Time.timeScale <= 0f)
		{
			Time.timeScale = 1f;
		}
		yield return new WaitForSeconds(secondsToWait);
		continueButton.SetActive(true);
	}

	public void ContinueOnClick()
	{
		gameSounds.Play_select();
		StartCoroutine(ShowPlayers());
	}

	private IEnumerator ShowPlayers()
	{
		AdMediation.HideCenterBanner();
		halfTimeBox.SetActive(false);
		cheerleader.SetActive(false);
		coach.SetActive(false);
		yield return new WaitForSeconds(0.5f);
		timeoutScreen.gameObject.SetActive(true);
		if (Stats.GetNumWins() + 1 <= 3 && !timeoutScreen.DidPutInPlayer)
		{
			gameController.ShowHintArrow(0);
			gameController.ShowHint(true);
			gameController.hintMessageTopOfScreen.Dehydrated();
		}
		if (Time.timeScale <= 0f)
		{
			Time.timeScale = 1f;
		}
		yield return new WaitForSeconds(0.05f);
		Time.timeScale = 0f;
		base.gameObject.SetActive(false);
	}
}
