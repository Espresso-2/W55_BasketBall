using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class WatchVideoButton : MonoBehaviour
{
	public GetGoldBox getGoldBox;

	public Button button;

	public bool allowInteractingIfNoAd;

	public NumGoldVidsIcon numGoldVidsIcon;

	private static int numGoldVids;

	public Timer timer;

	private int amount;

	private int hours;

	private GameSounds gameSounds;

	private float checkForVidTimer;

	public WatchVideoButton()
	{
		amount = 3;
		hours = 1;
	}

	public virtual void Start()
	{
		gameSounds = GameSounds.GetInstance();
		SetupSplitTest();
	}

	public virtual void OnEnable()
	{
		numGoldVids = GetNumGoldVids();
		Setup();
	}

	private void SetupSplitTest()
	{
		switch (PlayerPrefs.GetInt("GOLD_VID"))
		{
		case 0:
			hours = 1;
			amount = 3;
			break;
		case 1:
			hours = 1;
			amount = 5;
			break;
		case 2:
			hours = 1;
			amount = 10;
			break;
		case 3:
			hours = 4;
			amount = 3;
			break;
		case 4:
			hours = 4;
			amount = 5;
			break;
		case 5:
			hours = 4;
			amount = 10;
			break;
		}
	}

	private void Setup()
	{
		if (numGoldVids >= 1)
		{
			timer.gameObject.SetActive(false);
			button.gameObject.SetActive(AdMediation.IsVidAvail());
			button.interactable = true;
		}
		else
		{
			timer.gameObject.SetActive(true);
			timer.SetSecondsToWait(SecondsUntilGoldVidAvailable(), 2);
			button.interactable = allowInteractingIfNoAd;
		}
	}

	public virtual void WatchAd()
	{
		if (!getGoldBox.gameObject.activeInHierarchy)
		{
			getGoldBox.gameObject.SetActive(true);
		}
		else if (numGoldVids >= 1)
		{
			gameSounds.Play_select();
			/*FlurryAnalytics.Instance().LogEvent("PLAY_VIDEO_AD", new string[2]
			{
				"type:FREEGOLD",
				"num_wins_milestone:" + Stats.GetNumWinsMilestone() + string.Empty
			}, false);*/
			AdMediation.PlayVid(AdMediation.AdNetworks.HEYZAP);
		}
	}

	public virtual void FixedUpdate()
	{
		checkForVidTimer += Time.deltaTime;
		if (!(checkForVidTimer >= 2f))
		{
			return;
		}
		if (AdMediation.IsVidAvail() || numGoldVids == 0)
		{
			button.gameObject.SetActive(true);
			if (numGoldVids == 0 && !timer.gameObject.activeInHierarchy)
			{
				Setup();
			}
		}
		checkForVidTimer = 0f;
	}

	public virtual void AdComplete()
	{
		UseGoldVid();
		getGoldBox.AdComplete(amount);
		/*FlurryAnalytics.Instance().LogEvent("CLAIMED_GOLD_VID", new string[2]
		{
			"type:FREEGOLD",
			"num_wins_milestone:" + Stats.GetNumWinsMilestone() + string.Empty
		}, false);*/
		LogVideoView("FREE_GOLD");
	}

	public static void LogVideoView(string reward)
	{
		int @int = PlayerPrefs.GetInt("NUM_VIDEO_ADS");
		@int++;
		int numVidMilestone = GetNumVidMilestone();
		PlayerPrefsHelper.SetInt("NUM_VIDEO_ADS", @int, true);
		if (@int == 1)
		{
			/*FlurryAnalytics.Instance().LogEvent("FIRST_VIDEO_AD", new string[6]
			{
				"num_wins:" + Stats.GetNumWins() + string.Empty,
				"num_losses:" + Stats.GetNumLosses() + string.Empty,
				"current_tour:" + Tournaments.GetCurrentTournamentNum() + string.Empty,
				"sessions:" + Stats.GetNumSessions() + string.Empty,
				"reward:" + reward,
				"num_wins_milestone:" + Stats.GetNumWinsMilestone() + string.Empty
			}, false);*/
			/*AdMediation.TrackEventInTj("FIRST_VIDEO_AD", Stats.GetNumSessions());*/
		}
		/*FlurryAnalytics.Instance().LogEvent("VIDEO_AD", new string[6]
		{
			"num_video_ads:" + @int + string.Empty,
			"sessions:" + Stats.GetNumSessions() + string.Empty,
			"reward:" + reward,
			"num_iap:" + PlayerPrefs.GetInt("NUM_PURCHASES") + string.Empty,
			"num_wins_milestone:" + Stats.GetNumWinsMilestone() + string.Empty,
			"num_vid_milestone:" + numVidMilestone + string.Empty
		}, false);*/
		/*AdMediation.TrackEventInTj("VIDEO_AD", Stats.GetNumSessions());
		AdMediation.TrackEventInTj("VIDEO_AD_COMPLETED", Stats.GetNumSessions());*/
	}

	public static int GetNumGoldVids()
	{
		int result = 0;
		if (SecondsUntilGoldVidAvailable() <= 2)
		{
			result = 1;
		}
		return result;
	}

	public static int SecondsUntilGoldVidAvailable()
	{
		int @int = PlayerPrefs.GetInt("NEXT_GOLD_VID_TS");
		SessionVars instance = SessionVars.GetInstance();
		int currentTimestamp = instance.currentTimestamp;
		return @int - currentTimestamp;
	}

	private void UseGoldVid()
	{
		numGoldVids = 0;
		SessionVars instance = SessionVars.GetInstance();
		int num = 45;
		int val = instance.currentTimestamp + hours * 60 * 60 - num;
		PlayerPrefsHelper.SetInt("NEXT_GOLD_VID_TS", val, true);
		Setup();
		if (numGoldVidsIcon != null)
		{
			numGoldVidsIcon.UpdateNum();
		}
	}

	public virtual void TimerComplete(int num)
	{
		if (numGoldVidsIcon != null)
		{
			numGoldVidsIcon.UpdateNum();
		}
		numGoldVids = GetNumGoldVids();
		Setup();
		gameSounds.Play_light_click_2();
	}

	public static int GetNumVidMilestone()
	{
		int @int = PlayerPrefs.GetInt("NUM_VIDEO_ADS");
		int result = 1;
		if (@int >= 200)
		{
			result = 200;
		}
		else if (@int >= 100)
		{
			result = 100;
		}
		else if (@int >= 50)
		{
			result = 50;
		}
		else if (@int >= 25)
		{
			result = 25;
		}
		else if (@int >= 10)
		{
			result = 10;
		}
		else if (@int >= 5)
		{
			result = 5;
		}
		else if (@int >= 2)
		{
			result = 2;
		}
		return result;
	}
}
