using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// TODO:游戏开始=============================
/// </summary>
[Serializable]
public class LoadApp : MonoBehaviour
{
    public GameObject hintMsg;

    private SessionVars sessionVars;

    private bool socialPlatformEnabled;

    public static int BUILD_NUMBER = 2011201;

    public virtual void Awake()
    {
        Application.targetFrameRate = 60;
    }

    public virtual IEnumerator Start()
    {
        sessionVars = SessionVars.GetInstance();
        int numSessions = Stats.GetNumSessions() + 1;
        Stats.SetNumSessions(numSessions);
        if (PlayerPrefs.GetInt("LEAGUE_NUM") == 0)
        {
            PlayerPrefsHelper.SetInt("LEAGUE_NUM", UnityEngine.Random.Range(1, 6));
            PlayerPrefsHelper.SetInt("LB_VERSION", 0);
        }
        if (numSessions == 1)
        {
            hintMsg.gameObject.SetActive(false);
            sessionVars.goToTutorial = true;
            Players.SetActiveStarterNum(false, -1);
            Currency.SetStartingAmounts();
            SetupSplitTests();
            //FlurryAnalytics.Instance().LogEvent("FIRST_APP_LAUNCH");
            AdMediation.TrackEventInTj("FIRST_APP_LAUNCH", 0L);
        }
        else
        {
            socialPlatformEnabled = PlayerPrefs.GetInt("SOCIAL_PLATFORM_ENABLED") == 1;
            string text = string.Empty;
            switch (numSessions)
            {
                case 5:
                    text = "session_05";
                    break;
                case 10:
                    text = "session_10";
                    break;
                case 20:
                    text = "session_20";
                    break;
                case 50:
                    text = "session_50";
                    break;
                case 100:
                    text = "session_100";
                    break;
            }
            if (text != string.Empty)
            {
                /*FlurryAnalytics.Instance().LogEvent(text, new string[5]
                {
                    "wins:" + Stats.GetNumWins() + string.Empty,
                    "losses:" + Stats.GetNumLosses() + string.Empty,
                    "currentTournament:" + Tournaments.GetCurrentTournamentNum() + string.Empty,
                    "video_ads:" + PlayerPrefs.GetInt("NUM_VIDEO_ADS") + string.Empty,
                    "iap_gold_packs:" + PlayerPrefs.GetInt("NUM_PURCHASES") + string.Empty
                }, false);*/
                AdMediation.TrackEventInTj(text, Stats.GetNumWins());
            }
        }
        if (PlayerPrefs.GetInt("FEMALES_AND_CUSTOMIZATIONS_ARE_SETUP") == 0)
        {
            Players.SetActiveStarterNum(true, -1);
            PlayerPrefsHelper.SetInt("FEMALES_AND_CUSTOMIZATIONS_ARE_SETUP", 1);
        }
        if (PlayerPrefs.GetInt(MuteButton.SOUND_OFF_PREF_KEY) == 1)
        {
            AudioListener.volume = 0f;
        }
        else
        {
            AudioListener.volume = MuteButton.AUDIO_VOLUME;
        }
        LogIntoSocialPlatform();
        PlayFabManager.Instance().LoginAsGuest(true);
        yield return new WaitForSeconds(1f);
        if (Stats.GetNumSessions() <= 3)
        {
            if (PlayerPrefs.GetInt("STARTED_TUTORIAL") == 0)
            {
                sessionVars.goToTutorial = true;
            }
            else if (PlayerPrefs.GetInt("COMPLETED_SCRIM") == 0)
            {
                sessionVars.goToTutorial = true;
                sessionVars.goToScrimmage = true;
            }
        }
        if (sessionVars.goToTutorial)
        {
            Application.LoadLevel("FirstLaunchIntro");
        }
        else if (PlayerPrefs.GetString("TEAM_NAME") == string.Empty)
        {
            Application.LoadLevel("NameTeam");
        }
        else if (Stats.GetNumWins() < 2)
        {
            TabChanger.currentTabNum = tabEnum.Tour;
            Application.LoadLevel("MainUI");
        }
        else
        {
            Application.LoadLevel("MainUI");
        }
    }

    private void LogIntoSocialPlatform()
    {
        if (socialPlatformEnabled)
        {
            SocialPlatform.Instance.Activate();
            if (PlayerPrefs.GetInt("LAST_SOCIALPLATFORM_LOGIN_FAILED") == 1)
            {
                Debug.Log(
                    "LoadApp: The last time we tried to login it failed, so don't keep trying to log them in each startup because the popup may get annoying.");
                return;
            }
            if (PlayerPrefs.GetInt("USER_LOGGED_OUT_OF_SOCIALPLATFORM") == 1)
            {
                Debug.Log("LoadApp: User had purposefully logged out, so don't try and log them back in.");
                return;
            }
            Debug.Log("LoadApp: Try to log user into SocialPlatform");
            SocialPlatform.Instance.LogIn();
        }
    }

    private void SetupSplitTests()
    {
        PlayerPrefsHelper.SetInt("BUILD_NUMBER_AT_FIRST_LAUNCH", BUILD_NUMBER);
        if (UnityEngine.Random.Range(0, 100) >= 1)
        {
            /*FlurryAnalytics.Instance().LogEvent("ADS_ON");
            FlurryAnalytics.Instance().LogEvent("SPLIT_TEST_ADS_OFF", new string[1] { "test:ADS_ON" }, false);*/
            PlayerPrefsHelper.SetInt("ADS_OFF", 0);
        }
        else
        {
            /*FlurryAnalytics.Instance().LogEvent("ADS_OFF");
            FlurryAnalytics.Instance().LogEvent("SPLIT_TEST_ADS_OFF", new string[1] { "test:ADS_OFF" }, false);*/
            PlayerPrefsHelper.SetInt("ADS_OFF", 1);
        }
        int num = 2;
        int num2 = UnityEngine.Random.Range(0, 100);
        if (num2 >= 99)
        {
            num = 0;
        }
        PlayerPrefsHelper.SetInt("BANNER_AD_SETTING", num);
        //FlurryAnalytics.Instance().LogEvent("BANNER_AD_SETTING_" + num);
        int val = 1;
        PlayerPrefsHelper.SetInt("NATIVE_HALFTIME_ADS_ENABLED", val);
        int num3 = 1;
        socialPlatformEnabled = true;
        PlayerPrefsHelper.SetInt("SOCIAL_PLATFORM_ENABLED", num3);
        //FlurryAnalytics.Instance().LogEvent("SOCIAL_PLATFORM_ENABLED_" + num3);
        int val2 = 3;
        PlayerPrefsHelper.SetInt("UPGRADE_PRICE_MULTIPLIER", val2);
        int val3 = 5;
        PlayerPrefsHelper.SetInt("INCREASE_UPGRADE_TIME", val3);
        int val4 = 3;
        PlayerPrefsHelper.SetInt("DOUBLEREWARD_BUBBLE", val4);
        int val5 = 1;
        PlayerPrefsHelper.SetInt("COACH_REWARD_TIMER", val5);
        int val6 = 0;
        PlayerPrefsHelper.SetInt("GOLD_VID", val6);
        int val7 = 2;
        PlayerPrefsHelper.SetInt("PLAYER_PRICING", val7);
        PlayFabManager.Instance().SetUserDataForKey("PLAYER_PRICING", val7);
        int num4 = 5;
        if (UnityEngine.Random.Range(0, 100) >= 50)
        {
            num4 = 15;
        }
        PlayerPrefsHelper.SetInt("PRIZE_BALL_AMOUNT", num4);
        //FlurryAnalytics.Instance().LogEvent("PRIZE_BALL_AMOUNT_" + num4);
        int num5 = UnityEngine.Random.Range(0, 100);
        string text = "GROUP_A";
        if (num5 >= 95)
        {
            text = "GROUP_B";
        }
        else if (num5 >= 90)
        {
            text = "GROUP_C";
        }
        else if (num5 >= 85)
        {
            text = "GROUP_D";
        }
        AdMediation.AddTjTag(text);
        AdMediation.SetTjCohort(1, text);
        PlayerPrefs.SetString("TAPJOY_GROUP", text);
    }
}