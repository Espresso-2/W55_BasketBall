using System;
using System.Collections;
using I2.Loc;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class CoachRewardBox : MonoBehaviour
{
    public Localize headerText;

    public Localize bodyText;

    public GameObject closeButton;

    public Timer timer;

    public Image reward1Photo;

    public Image reward2Photo;

    public Text reward1Amount;

    public Text reward2Amount;

    public Sprite[] rewardSprites;

    private int[] rewardAmounts;

    public Image reward2Hider;

    public Animator starParticles;

    public Button watchButton;

    public Button continueButton;

    private int reward1;

    private int reward2;

    private SessionVars sessionVars;

    private GameSounds gameSounds;

    public TopNavBar topNavBar;

    public GameObject rateAppPrompt;

    public CoachRewardBox()
    {
        rewardAmounts = new int[7] { 3, 2, 2, 2, 3, 2, 5 };
    }

    public virtual void Start()
    {
        sessionVars = SessionVars.GetInstance();
        gameSounds = GameSounds.GetInstance();
        int num = 1;
        int @int = PlayerPrefs.GetInt("COACH_REWARD_HIDDEN");
        watchButton.gameObject.SetActive(true);
        continueButton.gameObject.SetActive(false);
        if ((@int >= num || (sessionVars.numMapViewsThisSession == 1 && UnityEngine.Random.Range(0, 100) >= 25)) && !sessionVars.goToPractice &&
            PlayerPrefs.GetInt("IAP_ADS_REMOVED") == 0 && Stats.GetNumWins() >= 1 && !TournamentView.showLeaderboardPanel)
        {
            SetUp();
            return;
        }
        base.gameObject.SetActive(false);
        @int++;
        PlayerPrefsHelper.SetInt("COACH_REWARD_HIDDEN", @int);
        if (sessionVars.wonLastGame)
        {
            ShowRateApp();
        }
    }

    private void SetUp()
    {
        if (sessionVars.numMapViewsThisSession == 1)
        {
            headerText.SetTerm("WELCOME BACK!", null);
        }
        else if (!sessionVars.wonLastGame)
        {
            headerText.SetTerm("TUFF LOSS!", null);
        }
        PlayerPrefsHelper.SetInt("COACH_REWARD_HIDDEN", 0);
        /*FlurryAnalytics.Instance().LogEvent("SHOW_COACH_VID_REW_BOX", new string[2]
        {
            "type:vungle",
            "num_wins_milestone:" + Stats.GetNumWinsMilestone() + string.Empty
        }, false);*/
        int @int = PlayerPrefs.GetInt("TOTAL_COACH_REWARDS_OFFERED");
        @int++;
        PlayerPrefsHelper.SetInt("TOTAL_COACH_REWARDS_OFFERED", @int);
        SetRewardItems();
        gameSounds.Play_coin_glow_2();
        int num = PlayerPrefs.GetInt("COACH_REWARD_TIMER");
        if (PlayerPrefs.GetInt("NUM_PURCHASES") > 0)
        {
            num = 0;
        }
        int num2 = 0;
        if (num == 3 || @int <= 1)
        {
            num2 = 10;
        }
        else
        {
            switch (num)
            {
                case 1:
                    num2 = 5;
                    break;
                case 2:
                    num2 = 7;
                    break;
            }
        }
        if (num2 > 0)
        {
            closeButton.SetActive(false);
            timer.gameObject.SetActive(true);
            timer.SetSecondsToWait(num2, 0);
        }
        else
        {
            closeButton.SetActive(true);
            timer.gameObject.SetActive(false);
        }
    }

    public virtual void TimerComplete()
    {
        CloseBox();
    }

    private void SetRewardItems()
    {
        reward1 = UnityEngine.Random.Range(1, rewardAmounts.Length);
        reward2 = UnityEngine.Random.Range(1, rewardAmounts.Length);
        while (reward1 == reward2)
        {
            reward2 = UnityEngine.Random.Range(1, rewardAmounts.Length);
        }
        reward1Amount.text = GetAmountText(reward1);
        reward2Amount.text = GetAmountText(reward2);
        reward1Photo.sprite = rewardSprites[reward1];
        reward2Photo.sprite = rewardSprites[reward2];
    }

    private string GetAmountText(int rewardNum)
    {
        string text = rewardAmounts[rewardNum].ToString();
        return "X " + text;
    }

    public virtual void CloseBox()
    {
        gameSounds.Play_select();
        base.gameObject.SetActive(false);
        topNavBar.UpdateCurrencyDisplay();
    }

    public virtual void WatchVideoOnClick()
    {
        gameSounds.Play_select();
        watchButton.gameObject.SetActive(false);
        timer.gameObject.SetActive(false);
        /*FlurryAnalytics.Instance().LogEvent("PLAY_VIDEO_AD", new string[2]
        {
            "type:COACHREWARD",
            "num_wins_milestone:" + Stats.GetNumWinsMilestone() + string.Empty
        }, false);*/
        //AdMediation.PlayVid();
    }

    public virtual void AdComplete()
    {
        WatchVideoButton.LogVideoView("COACH_REWARD");
        GiveReward(reward1);
        GiveReward(reward2);
        StartCoroutine(ShowReward());
    }

    private IEnumerator ShowReward()
    {
        bodyText.SetTerm("REWARD RECEIVED", null);
        if (Time.timeScale > 0f)
        {
            yield return new WaitForSeconds(1f);
        }
        reward2Hider.gameObject.SetActive(false);
        starParticles.gameObject.SetActive(true);
        gameSounds.Play_coin_glow_2();
        if (Time.timeScale > 0f)
        {
            yield return new WaitForSeconds(1f);
        }
        continueButton.gameObject.SetActive(true);
    }

    private void GiveReward(int rewardNum)
    {
        if (rewardNum == Supplies.OXYGEN)
        {
            Supplies.AddItem(Supplies.OXYGEN, rewardAmounts[rewardNum]);
        }
        if (rewardNum == Supplies.GRIP)
        {
            Supplies.AddItem(Supplies.GRIP, rewardAmounts[rewardNum]);
        }
        if (rewardNum == Supplies.CHALK)
        {
            Supplies.AddItem(Supplies.CHALK, rewardAmounts[rewardNum]);
        }
        if (rewardNum == Supplies.PROTEIN)
        {
            Supplies.AddItem(Supplies.PROTEIN, rewardAmounts[rewardNum]);
        }
        if (rewardNum == Supplies.DRINK)
        {
            Supplies.AddItem(Supplies.DRINK, rewardAmounts[rewardNum]);
        }
        if (rewardNum == 5 || rewardNum == 6)
        {
            Currency.AddGold(rewardAmounts[rewardNum]);
        }
    }

    private void ShowRateApp()
    {
        int numWins = Stats.GetNumWins();
        if (PlayerPrefs.GetInt("SHOWED_RATE_APP_5") == 0 && numWins >= 10)
        {
            rateAppPrompt.SetActive(true);
            PlayerPrefsHelper.SetInt("SHOWED_RATE_APP_5", 1);
            //PlayFabManager.Instance().SetUserDataForKey("SHOWED_RATE_APP_5", 1);
        }
        else if (PlayerPrefs.GetInt("SHOWED_RATE_APP_6") == 0 && numWins >= 75)
        {
            rateAppPrompt.SetActive(true);
            PlayerPrefsHelper.SetInt("SHOWED_RATE_APP_6", 1);
            //PlayFabManager.Instance().SetUserDataForKey("SHOWED_RATE_APP_6", 1);
        }
        else if (PlayerPrefs.GetInt("SHOWED_RATE_APP_7") == 0 && numWins >= 500)
        {
            rateAppPrompt.SetActive(true);
            PlayerPrefsHelper.SetInt("SHOWED_RATE_APP_7", 1);
            //PlayFabManager.Instance().SetUserDataForKey("SHOWED_RATE_APP_7", 1);
        }
    }
}