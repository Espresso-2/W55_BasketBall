using System;
using System.Collections;
using I2.Loc;
using UnityEngine;
using UnityEngine.UI;
using W_Log;

[Serializable]
public class TopNavBar : MonoBehaviour
{
    public Text teamName;

    public Text xpLevelNum;

    public RectTransform xpLevelBarBg;

    public GameObject xpLevelBarFill;

    public Image xpLevelBarFillImage;

    public Image cashIcon;

    public Text cashNum;

    public Image goldIcon;

    public Text goldNum;

    public Localize title;

    private static float lastShownXpProgress;

    private static int lastShownCashNum;

    private static int lastShownGoldNum;

    private RectTransform xpRectTransform;

    private float animXpFreq = 0.1f;

    private float animXpTimer;

    private Color origXpColor;

    private Color animXpColor;

    private bool animXpColorFlag;

    private float animXpPctStart = 0.5f;

    private float animXpPctFinish;

    private float animXpPct;

    private float animXpNewProgress;

    private bool animXpToNewLevel;

    private bool animXpCompleted;

    private float animTotTime;

    private static GameSounds gameSounds;

    public GameObject seasonCompletePanel;

    private bool showedSeasonCompletePanel;

    private float refreshTimer;

    private float refreshTime = 1.5f;

    static TopNavBar()
    {
        lastShownXpProgress = -1f;
        lastShownCashNum = -1;
        lastShownGoldNum = -1;
    }

    public virtual void OnEnable()
    {
        if (gameSounds == null)
        {
            gameSounds = GameSounds.GetInstance();
        }
        if (lastShownXpProgress == -1f)
        {
            lastShownXpProgress = Currency.GetCurrentXpLevelProgress();
        }
        if (lastShownCashNum == -1)
        {
            lastShownCashNum = Currency.GetCurrentCash();
        }
        if (lastShownGoldNum == -1)
        {
            lastShownGoldNum = Currency.GetCurrentGold();
        }
    }

    public virtual void Start()
    {
        if (teamName != null)
        {
            teamName.text = TeamDetails.GetTeamName();
        }
        UpdateCurrencyDisplay();
        /*AdMediation.HideTopBanner();*/
    }

    /// <summary>
    /// 更新当前展示
    /// </summary>
    public virtual void UpdateCurrencyDisplay()
    {
        Debug.Log("------------------>更新视图".FH3_Red());
        UpdateXpDisplay();
        UpdateGoldDisplay();
        UpdateCashDisplay();
    }

    private void UpdateXpDisplay()
    {
        int currentXpLevel = Currency.GetCurrentXpLevel();
        if (xpLevelBarFill != null)
        {
            animXpNewProgress = Currency.GetCurrentXpLevelProgress();
            animXpPct = lastShownXpProgress;
            xpRectTransform = (RectTransform)xpLevelBarFill.GetComponent(typeof(RectTransform));
            Vector2 sizeDelta = new Vector2(xpLevelBarBg.rect.width * (0f - (1f - animXpPct)), xpRectTransform.sizeDelta.y);
            xpRectTransform.sizeDelta = sizeDelta;
            if (lastShownXpProgress > animXpNewProgress)
            {
                animXpToNewLevel = true;
            }
            xpLevelBarFillImage = (Image)xpLevelBarFill.GetComponent<Image>();
            origXpColor = xpLevelBarFillImage.color;
            animXpColor = new Color(origXpColor.r, origXpColor.g, origXpColor.b, 0.75f);
            lastShownXpProgress = animXpNewProgress;
        }
        if (xpLevelNum != null)
        {
            if (animXpToNewLevel)
            {
                xpLevelNum.text = string.Empty + (currentXpLevel - 1);
            }
            else
            {
                xpLevelNum.text = string.Empty + currentXpLevel;
            }
        }
    }

    public virtual void UpdateGoldDisplay()
    {
        int currentGoldNum = Currency.GetCurrentGold();
        StartCoroutine(AnimateTextNum(goldNum, lastShownGoldNum, currentGoldNum));
        lastShownGoldNum = Currency.GetCurrentGold();
    }

    public virtual void UpdateCashDisplay()
    {
        int currentCash = Currency.GetCurrentCash();
        StartCoroutine(AnimateTextNum(cashNum, lastShownCashNum, currentCash));
        lastShownCashNum = Currency.GetCurrentCash();
    }

    public virtual void SetTitleTerm(string term)
    {
        title.gameObject.SetActive(true);
        title.SetTerm(term, null);
    }

    public virtual void HideTitle()
    {
        title.gameObject.SetActive(false);
    }

    public virtual void FixedUpdate()
    {
        if (refreshTimer > refreshTime)
        {
            StartCoroutine(CheckLeaderboardReset());
            refreshTimer = 0f;
        }
        else
        {
            refreshTimer += Time.deltaTime;
        }
        if (animXpCompleted)
        {
            return;
        }
        if (animXpPct < animXpNewProgress || animXpToNewLevel)
        {
            animTotTime += Time.deltaTime;
            float num = 0.005f;
            if (animTotTime >= 5f)
            {
                num = 0.07f;
            }
            else if (animTotTime >= 4f)
            {
                num = 0.06f;
            }
            else if (animTotTime >= 3f)
            {
                num = 0.04f;
            }
            animXpPct += Time.deltaTime * num;
            if (animXpPct > 1f && animXpToNewLevel)
            {
                animXpPct = 0.05f;
                animXpToNewLevel = false;
                xpLevelNum.text = string.Empty + Currency.GetCurrentXpLevel();
            }
            Vector2 sizeDelta = new Vector2(xpLevelBarBg.rect.width * (0f - (1f - animXpPct)), xpRectTransform.sizeDelta.y);
            xpRectTransform.sizeDelta = sizeDelta;
            if (animXpTimer > animXpFreq)
            {
                if (animXpColorFlag)
                {
                    xpLevelBarFillImage.color = animXpColor;
                }
                else
                {
                    xpLevelBarFillImage.color = origXpColor;
                }
                animXpColorFlag = !animXpColorFlag;
                animXpTimer = 0f;
            }
            else
            {
                animXpTimer += Time.deltaTime;
            }
        }
        else
        {
            xpLevelBarFillImage.color = origXpColor;
            animXpCompleted = true;
        }
    }

    private IEnumerator AnimateTextNum(Text t, int oldNum, int newNum)
    {
        t.text = oldNum.ToString("n0");
        if (newNum <= oldNum)
        {
            t.text = newNum.ToString("n0");
            yield break;
        }
        // yield return new WaitForSeconds(2.5f);
        // gameSounds.Play_light_click_2();
        // LeanTween.scale(t.gameObject, new Vector3(1.15f, 1.15f, 1f), 0.05f).setEase(LeanTweenType.easeOutExpo);
        // yield return new WaitForSeconds(0.25f);
        // LeanTween.scale(t.gameObject, new Vector3(1f, 1f, 1f), 0.05f).setEase(LeanTweenType.easeOutExpo);
        // yield return new WaitForSeconds(0.25f);
        t.text = newNum.ToString("n0");
        // LeanTween.scale(t.gameObject, new Vector3(1.15f, 1.15f, 1f), 0.05f).setEase(LeanTweenType.easeOutExpo);
        // yield return new WaitForSeconds(0.25f);
        // LeanTween.scale(t.gameObject, new Vector3(1f, 1f, 1f), 0.05f).setEase(LeanTweenType.easeOutExpo);
    }

    public virtual void UpdateGoldWithNoAnimationOrAnyDelay()
    {
        lastShownGoldNum = Currency.GetCurrentGold();
        goldNum.text = lastShownGoldNum.ToString("n0");
    }

    private IEnumerator CheckLeaderboardReset()
    {
        string stat = "EVENT_SCORE_" + PlayerPrefs.GetInt("LEAGUE_NUM");
        if (PlayerPrefs.GetInt("SHOW_USER_SEASON_RESULTS") == 1)
        {
            // Debug.Log("CheckLeaderboardReset(): SHOW THE USER THEIR SEASON RESULTS");
            if (!seasonCompletePanel.activeInHierarchy)
            {
                seasonCompletePanel.SetActive(true);
                showedSeasonCompletePanel = true;
            }
        }
        yield return null;
        // else if (PlayFabLeaderboard.DidWeReachNextResetTimeSinceLogin())
        // {
        //     //Debug.Log("CheckLeaderboardReset(): DidWeReachNextResetTimeSinceLogin = true!!!!!!");
        //     PlayFabLeaderboard.GetCurrentUserLeaderBoardRank(stat, false, null);
        //     yield return new WaitForSeconds(10f);
        //     if (!seasonCompletePanel.activeInHierarchy && !showedSeasonCompletePanel)
        //     {
        //         // Debug.Log("CheckLeaderboardReset(): Try again to get new leaderboard (1)");
        //         PlayFabLeaderboard.GetCurrentUserLeaderBoardRank(stat, false, null);
        //     }
        //     yield return new WaitForSeconds(10f);
        //     if (!seasonCompletePanel.activeInHierarchy && !showedSeasonCompletePanel)
        //     {
        //         // Debug.Log("CheckLeaderboardReset(): Try again to get new leaderboard (2)");
        //         PlayFabLeaderboard.GetCurrentUserLeaderBoardRank(stat, false, null);
        //     }
        //     yield return new WaitForSeconds(10f);
        //     if (!seasonCompletePanel.activeInHierarchy && !showedSeasonCompletePanel)
        //     {
        //         // Debug.Log("CheckLeaderboardReset(): Try again to get new leaderboard (3)");
        //         PlayFabLeaderboard.GetCurrentUserLeaderBoardRank(stat, false, null);
        //     }
        // }
    }
}