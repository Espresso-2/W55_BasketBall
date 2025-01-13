using System;
using System.Collections;
using GoogleMobileAds.Api;
using TapjoyUnity;
using UnityEngine;

public class AdMediation : MonoBehaviour
{
	public enum AdNetworks
	{
		TAPJOY = 0,
		HEYZAP = 1
	}

	public static AdMediation instance;

	public GameObject gameSounds;

	public GameObject collectingVidRewButtonPrefab;

	private CollectingVidRewButton collectingVidRewButton;

	private float secondsSinceIntAd = 999f;

	public static string GOLD_OFFER_PREF = "GOLD_OFFERWALL_REWARDS";

	private float _currentAudioVolume;

	private static UnifiedNativeAd nativeAd;

	public static bool nativeAdIsLoaded;

	private static UnifiedNativeAd nativeAdSmall;

	public static bool nativeAdSmallIsLoaded;

	private static InterstitialAd adMobInt;

	private static bool vidRewardEarned;

	private static BannerView adMobTopBannerView;

	public static bool adMobTopBannerIsLoaded;

	private static BannerView adMobCenterBannerView;

	public static bool adMobCenterBannerIsLoaded;

	private static TJPlacement _tjpOfferWall;

	private static TJPlacement _tjpRewVid;

	private static TJPlacement _tjpAppLaunch;

	private static TJPlacement _tjpAppResume;

	private static TJPlacement _tjpInsufficientCurrency;

	private static TJPlacement _tjpHomeScreen;

	private static TJPlacement _tjpPlayersScreen;

	private static TJPlacement _tjpStartersScreen;

	private static TJPlacement _tjpBackupsScreen;

	private static TJPlacement _tjpUpgradeScreen;

	private static TJPlacement _tjpStoreScreen;

	private static TJPlacement _tjpDealsScreen;

	private static TJPlacement _tjpTwoPlayerScreen;

	private static TJPlacement _tjpTourScreen;

	private static TJPlacement _tjpTourScreenAfterFirstWin;

	private static TJPlacement _tjpTourScreenAfterSecondWin;

	private static TJPlacement _tjpTourScreenAfterFirstLoss;

	private static TJPlacement _tjpTourScreenAfterWin;

	private static TJPlacement _tjpTourScreenAfterLoss;

	private static TJPlacement _tjpMatchupScreen;

	private static TJPlacement _tjpGamePlayPause;

	private static TJPlacement _tjpOutOfStoreItem;

	private static TJPlacement _tjpPurchasedStoreItem;

	private static TJPlacement _tjpIapCompleted;

	private static TJPlacement _tjpIapCancelled;

	private static TJActionRequest _lastPurchaseRequest;

	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.transform.gameObject);
		instance = this;
	}

	private void Start()
	{
		_currentAudioVolume = AudioListener.volume;
		SetupAdMob();
		ReqVid();
		if (PlayerPrefs.GetInt("NUM_PURCHASES") == 0 || PlayerPrefs.GetInt("IS_FRAUDULENT_USER") == 1)
		{
			RequestNativeAdSmall();
		}
		SetupTapjoy();
	}

	private void FixedUpdate()
	{
		secondsSinceIntAd += Time.deltaTime;
	}

	private void SetupAdMob()
	{
		string appId = "ca-app-pub-6792208077970765~5963944035";
		MobileAds.Initialize(appId);
		SetAdMobRewListener();
	}

	public static void ShowMediationTestSuite()
	{
		string text = "ca-app-pub-6792208077970765~5963944035";
		Debug.Log("AdMediation.ShowMediationTestSuite(): MediationTestSuite.Show(" + text + ")");
	}

	private void SetupTapjoy()
	{
		bool flag = false;
		string text = SystemInfo.deviceModel.ToLower();
		if (text.Contains("kindle") || text.Contains("amazon"))
		{
			flag = true;
		}
		if (!flag)
		{
			Tapjoy.OnConnectSuccess += HandleTjConnectSuccess;
			Tapjoy.OnConnectFailure += HandleTjConnectFailure;
			Tapjoy.OnEarnedCurrency += HandleTjEarnedCurrency;
			TJPlacement.OnContentReady += HandTjContentReady;
			TJPlacement.OnContentShow += HandleTjContentShow;
			TJPlacement.OnContentDismiss += HandleTjContentDismiss;
			TJPlacement.OnRewardRequest += HandleTjRewardRequest;
			TJPlacement.OnPurchaseRequest += HandleTjPurchaseRequest;
		}
	}

	public static void RequestNativeAd()
	{
		MonoBehaviour.print("AdMediation.RequestNativeAd()");
		string text = "ca-app-pub-6792208077970765/2580760333";
		if (nativeAd != null)
		{
			nativeAd.Destroy();
		}
		nativeAdIsLoaded = false;
		AdLoader adLoader = new AdLoader.Builder(text).ForUnifiedNativeAd().Build();
		adLoader.OnUnifiedNativeAdLoaded += instance.HandleUnifiedNativeAdLoaded;
		adLoader.OnAdFailedToLoad += instance.HandleNativeAdFailedToLoad;
		adLoader.LoadAd(new AdRequest.Builder().Build());
	}

	private void HandleUnifiedNativeAdLoaded(object sender, UnifiedNativeAdEventArgs args)
	{
		MonoBehaviour.print("AdMediation.HandleUnifiedNativeAdLoaded() Unified Native ad loaded.");
		nativeAd = args.nativeAd;
		nativeAdIsLoaded = true;
	}

	private void HandleNativeAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
	{
		MonoBehaviour.print("AdMediation.HandleNativeAdFailedToLoad() Native ad failed to load: " + args.Message);
	}

	public static UnifiedNativeAd GetNativeAd()
	{
		if (nativeAd != null && nativeAdIsLoaded)
		{
			return nativeAd;
		}
		return null;
	}

	public static void RequestNativeAdSmall()
	{
		MonoBehaviour.print("AdMediation.RequestNativeAdSmall()");
		string text = "ca-app-pub-6792208077970765/9567306639";
		if (nativeAdSmall != null)
		{
			nativeAdSmall.Destroy();
		}
		nativeAdSmallIsLoaded = false;
		AdLoader adLoader = new AdLoader.Builder(text).ForUnifiedNativeAd().Build();
		adLoader.OnUnifiedNativeAdLoaded += instance.HandleUnifiedNativeAdSmallLoaded;
		adLoader.OnAdFailedToLoad += instance.HandleNativeAdSmallFailedToLoad;
		adLoader.LoadAd(new AdRequest.Builder().Build());
	}

	private void HandleUnifiedNativeAdSmallLoaded(object sender, UnifiedNativeAdEventArgs args)
	{
		MonoBehaviour.print("AdMediation.HandleUnifiedNativeAdSmallLoaded() Unified Native ad loaded.");
		nativeAdSmall = args.nativeAd;
		nativeAdSmallIsLoaded = true;
	}

	private void HandleNativeAdSmallFailedToLoad(object sender, AdFailedToLoadEventArgs args)
	{
		MonoBehaviour.print("AdMediation.HandleNativeAdSmallFailedToLoad() Native ad failed to load: " + args.Message);
	}

	public static UnifiedNativeAd GetNativeAdSmall()
	{
		if (nativeAdSmall != null && nativeAdSmallIsLoaded)
		{
			return nativeAdSmall;
		}
		return null;
	}

	public static void RequestCenterBanner()
	{
		string text = ((PlayerPrefs.GetInt("NATIVE_HALFTIME_ADS_ENABLED") != 1) ? "ca-app-pub-6792208077970765/6872612143" : "ca-app-pub-6792208077970765/4054877113");
		string text2 = "ca-app-pub-6792208077970765/1746236607";
		if (PlayerPrefs.GetInt("BUILD_NUMBER_AT_FIRST_LAUNCH") < 2010801)
		{
			text = text2;
		}
		if (adMobCenterBannerView != null)
		{
			adMobCenterBannerView.Destroy();
		}
		adMobCenterBannerIsLoaded = false;
		AdSize adSize = AdSize.MediumRectangle;
		if (Screen.height < 720)
		{
			adSize = AdSize.Banner;
		}
		adMobCenterBannerView = new BannerView(text, adSize, AdPosition.Center);
		instance.SetAdMobBannerAdListeners(adMobCenterBannerView);
		adMobCenterBannerView.OnAdLoaded += instance.HandleOnCenterBannerAdLoaded;
		AdRequest request = new AdRequest.Builder().Build();
		adMobCenterBannerView.LoadAd(request);
	}

	public static void ShowTopBanner()
	{
		if (adMobTopBannerView != null)
		{
			adMobTopBannerView.Show();
		}
	}

	public static void ShowCenterBanner()
	{
		if (adMobCenterBannerView != null)
		{
			adMobCenterBannerView.Show();
		}
	}

	public static void HideTopBanner()
	{
		if (adMobTopBannerView != null)
		{
			adMobTopBannerView.Hide();
		}
	}

	public static void HideCenterBanner()
	{
		if (adMobCenterBannerView != null)
		{
			adMobCenterBannerView.Hide();
		}
	}

	public static void DestroyCenterBanner()
	{
		if (adMobCenterBannerView != null)
		{
			adMobCenterBannerView.Destroy();
		}
	}

	private void SetAdMobBannerAdListeners(BannerView bannerView)
	{
		bannerView.OnAdFailedToLoad += HandleOnBannerAdFailedToLoad;
		bannerView.OnAdOpening += HandleOnBannerAdOpened;
		bannerView.OnAdClosed += HandleOnBannerAdClosed;
		bannerView.OnAdLeavingApplication += HandleOnBannerAdLeavingApplication;
	}

	public void HandleOnTopBannerAdLoaded(object sender, EventArgs args)
	{
		MonoBehaviour.print("HandleAdLoaded event received");
		adMobTopBannerIsLoaded = true;
		adMobTopBannerView.Hide();
	}

	public void HandleOnCenterBannerAdLoaded(object sender, EventArgs args)
	{
		MonoBehaviour.print("HandleAdLoaded event received");
		adMobCenterBannerIsLoaded = true;
		adMobCenterBannerView.Hide();
	}

	public void HandleOnBannerAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
	{
		MonoBehaviour.print("HandleFailedToReceiveAd event received with message: " + args.Message);
	}

	public void HandleOnBannerAdOpened(object sender, EventArgs args)
	{
		MonoBehaviour.print("HandleAdOpened event received");
	}

	public void HandleOnBannerAdClosed(object sender, EventArgs args)
	{
		MonoBehaviour.print("HandleAdClosed event received");
	}

	public void HandleOnBannerAdLeavingApplication(object sender, EventArgs args)
	{
		MonoBehaviour.print("HandleAdLeavingApplication event received");
	}

	public static void ReqVid()
	{
		string adUnitId = "ca-app-pub-6792208077970765/4987832835";
		AdRequest request = new AdRequest.Builder().Build();
		RewardBasedVideoAd.Instance.LoadAd(request, adUnitId);
	}

	public static void PlayVid()
	{
		PlayVid(AdNetworks.HEYZAP);
	}

	public static void PlayVid(AdNetworks priority)
	{
		Transform parent = null;
		GameObject gameObject = GameObject.Find("Canvas");
		if (gameObject != null)
		{
			parent = gameObject.transform;
		}
		instance.collectingVidRewButton = UnityEngine.Object.Instantiate(instance.collectingVidRewButtonPrefab, parent).GetComponent<CollectingVidRewButton>();
		bool flag = false;
		vidRewardEarned = false;
		if (flag)
		{
			TestCompletedVid();
		}
		else if (priority == AdNetworks.TAPJOY && IsTjpRewVidAvail())
		{
			ShowTjpRewVid();
		}
		else if (RewardBasedVideoAd.Instance.IsLoaded())
		{
			RewardBasedVideoAd.Instance.Show();
		}
		else if (IsTjpRewVidAvail())
		{
			ShowTjpRewVid();
		}
		PlayerPrefsHelper.SetInt("GAMES_SINCE_WATCHING_AD", 0, true);
		ReqVid();
	}

	private static IEnumerator TestCompletedVid()
	{
		yield return new WaitForSeconds(3.5f);
		CompletedVid();
	}

	public static bool IsVidAvail()
	{
		if (false)
		{
			return true;
		}
		bool result = false;
		if (RewardBasedVideoAd.Instance.IsLoaded())
		{
			result = true;
		}
		else
		{
			ReqVid();
		}
		return result;
	}

	private void SetAdMobRewListener()
	{
		RewardBasedVideoAd rewardBasedVideoAd = RewardBasedVideoAd.Instance;
		rewardBasedVideoAd.OnAdLoaded += HandleRewardBasedVideoLoaded;
		rewardBasedVideoAd.OnAdFailedToLoad += HandleRewardBasedVideoFailedToLoad;
		rewardBasedVideoAd.OnAdOpening += HandleRewardBasedVideoOpened;
		rewardBasedVideoAd.OnAdStarted += HandleRewardBasedVideoStarted;
		rewardBasedVideoAd.OnAdRewarded += HandleRewardBasedVideoRewarded;
		rewardBasedVideoAd.OnAdClosed += HandleRewardBasedVideoClosed;
		rewardBasedVideoAd.OnAdLeavingApplication += HandleRewardBasedVideoLeftApplication;
	}

	public void HandleRewardBasedVideoLoaded(object sender, EventArgs args)
	{
		MonoBehaviour.print("HandleRewardBasedVideoLoaded event received");
	}

	public void HandleRewardBasedVideoFailedToLoad(object sender, AdFailedToLoadEventArgs args)
	{
		MonoBehaviour.print("HandleRewardBasedVideoFailedToLoad event received");
		if (args != null)
		{
			MonoBehaviour.print("with message: " + args.Message);
		}
	}

	public void HandleRewardBasedVideoOpened(object sender, EventArgs args)
	{
		MonoBehaviour.print("HandleRewardBasedVideoOpened event received");
	}

	public void HandleRewardBasedVideoStarted(object sender, EventArgs args)
	{
		MonoBehaviour.print("HandleRewardBasedVideoStarted event received");
		if (PlayerPrefs.GetInt("SOUND_OFF") == 0)
		{
			AudioListener.pause = true;
		}
	}

	public void HandleRewardBasedVideoClosed(object sender, EventArgs args)
	{
		MonoBehaviour.print("HandleRewardBasedVideoClosed event received");
		SetCollectingVidDebugText("AD HIDDEN");
		if (vidRewardEarned)
		{
			MonoBehaviour.print("HandleRewardBasedVideoClosed AdMediation.vidRewardEarned == true");
			CompletedVid();
			MakeAudioOn();
		}
		else
		{
			MonoBehaviour.print("HandleRewardBasedVideoClosed AdMediation.vidRewardEarned == false");
		}
	}

	public void HandleRewardBasedVideoRewarded(object sender, Reward args)
	{
		vidRewardEarned = true;
		if (sender != null && args != null)
		{
			string type = args.Type;
			MonoBehaviour.print("HandleRewardBasedVideoRewarded event received for " + args.Amount + " " + type);
		}
	}

	public void HandleRewardBasedVideoLeftApplication(object sender, EventArgs args)
	{
		MonoBehaviour.print("HandleRewardBasedVideoLeftApplication event received");
		SetCollectingVidDebugText("AD CLICKED");
	}

	private void SetCollectingVidDebugText(string text)
	{
		if (!(instance.collectingVidRewButton == null))
		{
			instance.collectingVidRewButton.debugText.text = text;
		}
	}

	public static void CompletedVid()
	{
		MonoBehaviour.print("AdMediation.CompletedVid()");
		if (instance.collectingVidRewButton == null)
		{
			MonoBehaviour.print("already collected reward");
			return;
		}
		UnityEngine.Object.Destroy(instance.collectingVidRewButton.gameObject);
		GameObject gameObject = GameObject.Find("CoachRewardBox");
		if (gameObject != null)
		{
			gameObject.GetComponent<CoachRewardBox>().AdComplete();
		}
		else
		{
			GameObject gameObject2 = GameObject.Find("WatchVideoButton");
			if (gameObject2 != null)
			{
				gameObject2.SendMessage("AdComplete");
			}
			else
			{
				GameObject gameObject3 = GameObject.Find("GameResults");
				if (gameObject3 != null)
				{
					gameObject3.SendMessage("AdComplete");
				}
			}
		}
		//FlurryAnalytics.Instance().LogEvent("ADMEDIATION_CompletedVid");
	}

	public void InCompleteVid()
	{
		//FlurryAnalytics.Instance().LogEvent("REWVID_INCENTIVIZED_RESULT_INCOMPLETE");
	}

	public static void CheckForEarnedCurrency()
	{
		Tapjoy.GetCurrencyBalance();
	}

	public static void ReqInt()
	{
		string text = "ca-app-pub-6792208077970765/7781517131";
		if (adMobInt != null)
		{
			adMobInt.Destroy();
		}
		adMobInt = new InterstitialAd(text);
		instance.SetAdMobIntListener();
		AdRequest request = new AdRequest.Builder().Build();
		adMobInt.LoadAd(request);
	}

	public static void ShowInt()
	{
		if (adMobInt != null && adMobInt.IsLoaded())
		{
			adMobInt.Show();
			LogInt();
		}
	}

	private static void LogInt()
	{
		PlayerPrefsHelper.SetInt("NUM_INT_ADS", PlayerPrefs.GetInt("NUM_INT_ADS") + 1, true);
		instance.secondsSinceIntAd = 0f;
		/*FlurryAnalytics.Instance().LogEvent("SHOWED_INT_AD", new string[2]
		{
			"type:int",
			"num_wins_milestone:" + Stats.GetNumWinsMilestone() + string.Empty
		}, false);*/
	}

	public static bool IsIntAvail()
	{
		return adMobInt != null && adMobInt.IsLoaded();
	}

	public float GetSecondsSinceIntAd()
	{
		return secondsSinceIntAd;
	}

	public void MakeAudioOn()
	{
		try
		{
			if (PlayerPrefs.GetInt("SOUND_OFF") == 0)
			{
				if (AudioListener.pause)
				{
					AudioListener.pause = false;
				}
				if (AudioListener.volume <= 0f)
				{
					AudioListener.volume = _currentAudioVolume;
				}
			}
		}
		catch (Exception ex)
		{
			Debug.Log("ERROR Happened when we called MakeAudioOn(): " + ex);
		}
	}

	private void SetAdMobIntListener()
	{
		adMobInt.OnAdLoaded += HandleOnAdLoaded;
		adMobInt.OnAdFailedToLoad += HandleOnAdFailedToLoad;
		adMobInt.OnAdOpening += HandleOnAdOpened;
		adMobInt.OnAdClosed += HandleOnAdClosed;
		adMobInt.OnAdLeavingApplication += HandleOnAdLeavingApplication;
	}

	public void HandleOnAdLoaded(object sender, EventArgs args)
	{
		MonoBehaviour.print("HandleAdLoaded event received");
	}

	public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
	{
		MonoBehaviour.print("HandleFailedToReceiveAd event received");
		if (args != null)
		{
			MonoBehaviour.print("with message: " + args.Message);
		}
	}

	public void HandleOnAdOpened(object sender, EventArgs args)
	{
		MonoBehaviour.print("HandleAdOpened event received");
		if (PlayerPrefs.GetInt("SOUND_OFF") == 0)
		{
			AudioListener.volume = _currentAudioVolume * 0.5f;
		}
		//FlurryAnalytics.Instance().LogEvent("SHOW_MEDIATED_INT_SUCCESS");
	}

	public void HandleOnAdClosed(object sender, EventArgs args)
	{
		MonoBehaviour.print("HandleAdClosed event received");
		GameObject gameObject = GameObject.Find("GameResults");
		if (gameObject != null)
		{
			gameObject.SendMessage("ContinueButton");
		}
		else
		{
			Time.timeScale = 1f;
		}
		MakeAudioOn();
	}

	public void HandleOnAdLeavingApplication(object sender, EventArgs args)
	{
		MonoBehaviour.print("HandleAdLeavingApplication event received");
	}

	public void HandleTjConnectSuccess()
	{
		Tapjoy.GetCurrencyBalance();
		if (PlayerPrefs.GetInt("HZ_OFFERWALL_ON") == 0)
		{
			_tjpOfferWall = TJPlacement.CreatePlacement("GoldOfferWall");
			_tjpOfferWall.RequestContent();
		}
		_tjpRewVid = TJPlacement.CreatePlacement("RewVid");
		_tjpRewVid.RequestContent();
		_tjpAppLaunch = TJPlacement.CreatePlacement("AppLaunch");
		_tjpAppLaunch.RequestContent();
		_tjpAppResume = TJPlacement.CreatePlacement("AppResume");
		_tjpAppResume.RequestContent();
		_tjpInsufficientCurrency = TJPlacement.CreatePlacement("InsufficientCurrency");
		_tjpInsufficientCurrency.RequestContent();
		_tjpHomeScreen = TJPlacement.CreatePlacement("HomeScreen");
		_tjpHomeScreen.RequestContent();
		_tjpPlayersScreen = TJPlacement.CreatePlacement("PlayersScreen");
		_tjpPlayersScreen.RequestContent();
		_tjpStartersScreen = TJPlacement.CreatePlacement("StartersScreen");
		_tjpStartersScreen.RequestContent();
		_tjpBackupsScreen = TJPlacement.CreatePlacement("BackupsScreen");
		_tjpBackupsScreen.RequestContent();
		_tjpUpgradeScreen = TJPlacement.CreatePlacement("UpgradeScreen");
		_tjpUpgradeScreen.RequestContent();
		_tjpStoreScreen = TJPlacement.CreatePlacement("StoreScreen");
		_tjpStoreScreen.RequestContent();
		_tjpDealsScreen = TJPlacement.CreatePlacement("DealsScreen");
		_tjpDealsScreen.RequestContent();
		_tjpTwoPlayerScreen = TJPlacement.CreatePlacement("TwoPlayerScreen");
		_tjpTwoPlayerScreen.RequestContent();
		_tjpTourScreen = TJPlacement.CreatePlacement("TourScreen");
		_tjpTourScreen.RequestContent();
		_tjpTourScreenAfterFirstWin = TJPlacement.CreatePlacement("TourScreenAfterFirstWin");
		_tjpTourScreenAfterFirstWin.RequestContent();
		_tjpTourScreenAfterSecondWin = TJPlacement.CreatePlacement("TourScreenAfterSecondWin");
		_tjpTourScreenAfterSecondWin.RequestContent();
		_tjpTourScreenAfterFirstLoss = TJPlacement.CreatePlacement("TourScreenAfterFirstLoss");
		_tjpTourScreenAfterFirstLoss.RequestContent();
		_tjpTourScreenAfterWin = TJPlacement.CreatePlacement("TourScreenAfterWin");
		_tjpTourScreenAfterWin.RequestContent();
		_tjpTourScreenAfterLoss = TJPlacement.CreatePlacement("TourScreenAfterLoss");
		_tjpTourScreenAfterLoss.RequestContent();
		_tjpMatchupScreen = TJPlacement.CreatePlacement("MatchupScreen");
		_tjpMatchupScreen.RequestContent();
		_tjpGamePlayPause = TJPlacement.CreatePlacement("GamePlayPause");
		_tjpGamePlayPause.RequestContent();
		_tjpOutOfStoreItem = TJPlacement.CreatePlacement("OutOfStoreItem");
		_tjpOutOfStoreItem.RequestContent();
		_tjpPurchasedStoreItem = TJPlacement.CreatePlacement("PurchasedStoreItem");
		_tjpPurchasedStoreItem.RequestContent();
		_tjpIapCompleted = TJPlacement.CreatePlacement("IapCompleted");
		_tjpIapCompleted.RequestContent();
		_tjpIapCancelled = TJPlacement.CreatePlacement("IapCancelled");
		_tjpIapCancelled.RequestContent();
	}

	public static void SetTjLevel(int lvl)
	{
		Tapjoy.SetUserLevel(lvl);
	}

	public static void AddTjTag(string tag)
	{
		Tapjoy.AddUserTag(tag);
	}

	public static void SetTjCohort(int variable, string val)
	{
		Tapjoy.SetUserCohortVariable(variable, val);
	}

	public static void TrackEventInTj(string category, string name, string param1, string param2, string val1Name, long val1, string val2Name, long val2, string val3Name, long val3)
	{
		Tapjoy.TrackEvent(category, name, param1, param2, val1Name, val1, val2Name, val2, val3Name, val3);
	}

	public static void TrackEventInTj(string name, long val)
	{
		Tapjoy.TrackEvent(name, val);
	}

	public static void ActionCompleteInTj(string engagementID)
	{
		Tapjoy.ActionComplete(engagementID);
	}

	public void HandleTjConnectFailure()
	{
	}

	public void HandTjContentReady(TJPlacement placement)
	{
		if (placement.GetName() == "AppLaunch")
		{
			placement.ShowContent();
		}
	}

	public void HandleTjContentShow(TJPlacement placement)
	{
		switch (placement.GetName())
		{
		case "GoldOfferWall":
			_tjpOfferWall.RequestContent();
			//FlurryAnalytics.Instance().LogEvent("VIEWED_OFFERS");
			break;
		case "RewVid":
			_tjpRewVid.RequestContent();
			//FlurryAnalytics.Instance().LogEvent("TAPJOY_REW_VID_CLICKED");
			break;
		case "TourScreen":
			_tjpTourScreen.RequestContent();
			break;
		case "MatchupScreen":
			_tjpMatchupScreen.RequestContent();
			break;
		}
	}

	public void HandleTjPurchaseRequest(TJPlacement placement, TJActionRequest request, string productId)
	{
		_lastPurchaseRequest = request;
		gameSounds.SendMessage("Play_select");
		Debug.Log("HandleTjPurchaseRequest: productId: " + productId);
		int num = productId.IndexOf("_");
		int result = 0;
		if (num > 0)
		{
			string s = productId.Substring(num + 1, 2);
			int.TryParse(s, out result);
			productId = productId.Substring(0, num);
		}
		Debug.Log("PARSED productId: " + productId + " BONUS: " + result);
		PurchasableItem purchasableItem = null;
		purchasableItem = Unibiller.GetPurchasableItemById(productId);
		if (purchasableItem != null)
		{
			PlayerPrefsHelper.SetInt("TAPJOY_BONUS", result);
			PlayerPrefsHelper.SetInt("IS_TAPJOY_PURCHASE_REQUEST", 1);
			Unibiller.initiatePurchase(purchasableItem, string.Empty);
		}
	}

	public static void MarkLastTjPurchaseRequestAsCompleted()
	{
		if (_lastPurchaseRequest != null)
		{
			_lastPurchaseRequest.Completed();
		}
	}

	public static void MarkLastTjPurchaseRequestAsCancelled()
	{
		if (_lastPurchaseRequest != null)
		{
			_lastPurchaseRequest.Cancelled();
		}
	}

	public void HandleTjContentDismiss(TJPlacement placement)
	{
		Debug.Log("placement.GetName (): " + placement.GetName());
		if (placement.GetName() == "RewVid")
		{
			CompletedVid();
			return;
		}
		gameSounds.SendMessage("Play_select");
		Tapjoy.GetCurrencyBalance();
	}

	public void HandleTjRewardRequest(TJPlacement placement, TJActionRequest request, string itemId, int amount)
	{
		Debug.Log("HandleTjRewardRequest: itemId: " + itemId + ", amount: " + amount);
		if (itemId == "GOLD")
		{
			RecordEarnedAmount(itemId, amount);
		}
		if (placement.GetName().ToLower().Contains("vid"))
		{
			TrackEventInTj("VIDEO_AD_TJ", PlayerPrefs.GetInt("NUM_SESSIONS"));
			TrackEventInTj("VIDEO_AD_COMPLETED", PlayerPrefs.GetInt("NUM_SESSIONS"));
		}
	}

	public void HandleTjEarnedCurrency(string currencyName, int amount)
	{
		RecordEarnedAmount(currencyName, amount);
	}

	private static void RecordEarnedAmount(string currencyName, int amount)
	{
		if (amount > 0)
		{
			//FlurryAnalytics.Instance().LogEvent("EARNED_OFFERWALL_GOLD");
			if (amount >= 50)
			{
			//	FlurryAnalytics.Instance().LogEvent("EARNED_OFFERWALL_GOLD_OVER_50");
			}
			else
			{
			//	FlurryAnalytics.Instance().LogEvent("EARNED_OFFERWALL_GOLD_UNDER_50");
			}
			Debug.Log("HandleEarnedCurrency: currencyName: " + currencyName + ", amount: " + amount);
			int[] intArray = PlayerPrefsX.GetIntArray(GOLD_OFFER_PREF);
			int[] array = new int[intArray.Length + 1];
			for (int i = 0; i < intArray.Length; i++)
			{
				array[i] = intArray[i];
			}
			array[array.Length - 1] = amount;
			PlayerPrefsX.SetIntArray(GOLD_OFFER_PREF, array);
			GameObject gameObject = GameObject.Find("PlusButton");
			if (gameObject != null)
			{
				gameObject.SendMessage("CheckForEarnedCurrency");
			}
		}
	}

	public static bool IsTjpOfferWallAvail()
	{
		if (_tjpOfferWall == null)
		{
			return false;
		}
		return _tjpOfferWall.IsContentReady();
	}

	public static void ShowTjpOfferWall()
	{
		if (_tjpOfferWall != null)
		{
			_tjpOfferWall.ShowContent();
		}
	}

	public static bool IsTjpRewVidAvail()
	{
		if (_tjpRewVid == null)
		{
			return false;
		}
		return _tjpRewVid.IsContentReady();
	}

	public static void ShowTjpRewVid()
	{
		if (_tjpRewVid != null)
		{
			_tjpRewVid.ShowContent();
		}
	}

	public static void ShowTjpAppResume()
	{
		Debug.Log("ShowTjpAppResume()");
		if (_tjpAppResume != null)
		{
			_tjpAppResume.ShowContent();
		}
	}

	public static void ShowTjpInsufficientCurrency()
	{
		Debug.Log("ShowTjpInsufficientCurrency()");
		if (_tjpInsufficientCurrency != null)
		{
			_tjpInsufficientCurrency.ShowContent();
		}
	}

	public static void ShowTjpHomeScreen()
	{
		Debug.Log("ShowTjpHomeScreen()");
		if (_tjpHomeScreen != null)
		{
			_tjpHomeScreen.ShowContent();
		}
	}

	public static void ShowTjpPlayersScreen()
	{
		Debug.Log("ShowTjpPlayersScreen()");
		if (_tjpPlayersScreen != null)
		{
			_tjpPlayersScreen.ShowContent();
		}
	}

	public static void ShowTjpStartersScreen()
	{
		Debug.Log("ShowTjpStartersScreen()");
		if (_tjpStartersScreen != null)
		{
			_tjpStartersScreen.ShowContent();
		}
	}

	public static void ShowTjpBackupsScreen()
	{
		Debug.Log("ShowTjpBackupsScreen()");
		if (_tjpBackupsScreen != null)
		{
			_tjpBackupsScreen.ShowContent();
		}
	}

	public static void ShowTjpUpgradeScreen()
	{
		Debug.Log("ShowTjpUpgradeScreen()");
		if (_tjpUpgradeScreen != null)
		{
			_tjpUpgradeScreen.ShowContent();
		}
	}

	public static void ShowTjpStoreScreen()
	{
		Debug.Log("ShowTjpStoreScreen()");
		if (_tjpStoreScreen != null)
		{
			_tjpStoreScreen.ShowContent();
		}
	}

	public static void ShowTjpDealsScreen()
	{
		Debug.Log("ShowTjpDealsScreen()");
		if (_tjpDealsScreen != null)
		{
			_tjpDealsScreen.ShowContent();
		}
	}

	public static void ShowTjpTwoPlayerScreen()
	{
		Debug.Log("ShowTjpTwoPlayerScreen()");
		if (_tjpTwoPlayerScreen != null)
		{
			_tjpTwoPlayerScreen.ShowContent();
		}
	}

	public static void ShowTjpTourScreen()
	{
		Debug.Log("ShowTjpTourScreen()");
		if (_tjpTourScreen != null)
		{
			_tjpTourScreen.ShowContent();
		}
	}

	public static void ShowTjpTourScreenAfterFirstWin()
	{
		Debug.Log("ShowTjpTourScreenAfterFirstWin()");
		if (_tjpTourScreenAfterFirstWin != null)
		{
			_tjpTourScreenAfterFirstWin.ShowContent();
		}
	}

	public static void ShowTjpTourScreenAfterSecondWin()
	{
		Debug.Log("ShowTjpTourScreenAfterSecondWin()");
		if (_tjpTourScreenAfterSecondWin != null)
		{
			_tjpTourScreenAfterSecondWin.ShowContent();
		}
	}

	public static void ShowTjpTourScreenAfterFirstLoss()
	{
		Debug.Log("ShowTjpTourScreenAfterFirstLoss()");
		if (_tjpTourScreenAfterFirstLoss != null)
		{
			_tjpTourScreenAfterFirstLoss.ShowContent();
		}
	}

	public static void ShowTjpTourScreenAfterWin()
	{
		Debug.Log("ShowTjpTourScreenAfterWin()");
		if (_tjpTourScreenAfterWin != null)
		{
			_tjpTourScreenAfterWin.ShowContent();
		}
	}

	public static void ShowTjpTourScreenAfterLoss()
	{
		Debug.Log("ShowTjpTourScreenAfterLoss()");
		if (_tjpTourScreenAfterLoss != null)
		{
			_tjpTourScreenAfterLoss.ShowContent();
		}
	}

	public static void ShowTjpMatchupScreen()
	{
		Debug.Log("ShowTjpMatchupScreen()");
		if (_tjpMatchupScreen != null)
		{
			_tjpMatchupScreen.ShowContent();
		}
	}

	public static void ShowTjpGamePlayPause()
	{
		Debug.Log("ShowTjpGamePlayPause()");
		if (_tjpGamePlayPause != null)
		{
			_tjpGamePlayPause.ShowContent();
		}
	}

	public static void ShowTjpOutOfStoreItem()
	{
		Debug.Log("ShowTjpOutOfStoreItem()");
		if (_tjpOutOfStoreItem != null)
		{
			_tjpOutOfStoreItem.ShowContent();
		}
	}

	public static void ShowTjpPurchasedStoreItem()
	{
		Debug.Log("ShowTjpPurchasedStoreItem()");
		if (_tjpPurchasedStoreItem != null)
		{
			_tjpPurchasedStoreItem.ShowContent();
		}
	}

	public static void ShowTjpIapCompleted()
	{
		Debug.Log("ShowTjpIapCompleted()");
		if (_tjpIapCompleted != null)
		{
			_tjpIapCompleted.ShowContent();
		}
	}

	public static void ShowTjpIapCancelled()
	{
		Debug.Log("ShowTjpIapCancelled()");
		if (_tjpIapCancelled != null)
		{
			_tjpIapCancelled.ShowContent();
		}
	}
}
