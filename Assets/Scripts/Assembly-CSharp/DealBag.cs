using System;
using I2.Loc;
using UnityEngine;
using UnityEngine.UI;
using W_Log;

[Serializable]
public class DealBag : MonoBehaviour
{
    public Localize title;

    public BagCard[] exampleCards;

    public Image[] bagFills;

    public Image[] oneMoreBagFills;

    public Text openLabel;

    public NumBagsIcon numBagsIcon;

    public Timer timer;

    public Text iapPrice;

    public GameObject goldBuyIcon;

    public GameObject cashBuyIcon;

    public TabChanger tabChanger;

    public OpenBag openBag;

    public CurrencyExchangeBox currencyExchangeBox;

    public TopNavBar topNavBar;

    public GetGoldButton getGoldButton;

    private int num;

    private string[] titles;

    public Color[] colors;

    private int[] goldPrices;

    private int[] goldPricesDiscounted;

    private int iapNum;

    private PurchasableItem iapItem;

    private bool discounted;

    private int goldPrice;

    private int goldPriceDiscounted;

    private int cashPrice;

    private int cashPriceDiscounted;

    public DealBag()
    {
        titles = new string[3] { "STANDARD BAG", "PREMIUM BAG", "DAILY BAG" };
        goldPrices = new int[3] { 10, 15, 0 };
        goldPricesDiscounted = new int[3] { 8, 12, 0 };
    }

    public virtual void OnEnable()
    {
        SetItem(num);
    }

    public virtual void TimerComplete(int bagNum)
    {
        SetItem(num);
    }

    public virtual void SetItem(int num)
    {
        this.num = num;
        int num2 = goldPrices[num];
        cashPrice = 0;
        cashPriceDiscounted = 0;
        goldPrice = 0;
        goldPriceDiscounted = 0;
        if (num2 > 0)
        {
            goldPrice = num2;
            goldPriceDiscounted = goldPricesDiscounted[num];
        }
        else
        {
            cashPrice = num2 * -1;
            cashPriceDiscounted = goldPricesDiscounted[num] * -1;
        }
        iapPrice.gameObject.SetActive(false);
        goldBuyIcon.SetActive(false);
        cashBuyIcon.SetActive(false);
        openLabel.gameObject.SetActive(false);
        timer.gameObject.SetActive(false);
        numBagsIcon.gameObject.SetActive(false);
        bool flag = this.num == 0;
        bool flag2 = this.num == 1;
        bool flag3 = this.num == 2;
        if ((flag && GetNumStandardBags() > 0) || (flag3 && GetNumDailyBags() > 0) || (flag2 && GetNumPremiumBags() > 0))
        {
            openLabel.gameObject.SetActive(true);
            numBagsIcon.gameObject.SetActive(true);
            numBagsIcon.includeDailyBags = flag3;
            numBagsIcon.includeStandardBags = flag;
            numBagsIcon.includePremiumBags = flag2;
            numBagsIcon.UpdateNum();
        }
        else if (this.num == 2)
        {
            timer.gameObject.SetActive(true);
            timer.SetSecondsToWait(SecondsUntilDailyBagAvailable(), 2);
        }
        else
        {
            iapPrice.gameObject.SetActive(true);
            if (goldPrice > 0)
            {
                iapPrice.text = goldPrice.ToString("n0");
                goldBuyIcon.SetActive(true);
            }
            else
            {
                iapPrice.text = cashPrice.ToString("n0");
                cashBuyIcon.SetActive(true);
            }
        }
        title.SetTerm(titles[num], null);
        Image[] array = bagFills;
        foreach (Image image in array)
        {
            image.color = colors[num];
        }
        Image[] array2 = oneMoreBagFills;
        foreach (Image image2 in array2)
        {
            if (flag3)
            {
                image2.color = colors[1];
            }
            else
            {
                image2.color = colors[num];
            }
        }
        bool isPremium = num == 1;
        for (int k = 0; k < exampleCards.Length; k++)
        {
            BagCard bagCard = exampleCards[k];
            bool forceToBeCustomItem = k + 1 == exampleCards.Length;
            bagCard.SetCardItem(isPremium, forceToBeCustomItem);
        }
        Debug.Log($"设置物品时设置价格：{goldPrice}".FH6_Aqua());
    }

    public virtual void Buy(bool useDiscount)
    {
        Debug.Log($"使用的Num : {this.num}".FH5_Yellow());
        discounted = useDiscount;
        bool flag = false;
        GameSounds.GetInstance().Play_one_dribble();
        int num = 0;
        if (this.num == 0 && GetNumStandardBags() > 0 && !discounted)
        {
            Debug.Log("是否进入？".FL1_HotPink());
            /*FlurryAnalytics.Instance().LogEvent("OPENED_FREE_STANDARD_BAG", new string[4]
            {
                "sessions:" + Stats.GetNumSessions() + string.Empty,
                "num:" + this.num,
                "num_iap:" + PlayerPrefs.GetInt("NUM_PURCHASES") + string.Empty,
                "num_wins:" + Stats.GetNumWins() + string.Empty
            }, false);*/
            //AdMediation.TrackEventInTj("OPENED_FREE_STANDARD_BAG", Stats.GetNumSessions());
            flag = true;
            UseStandardBag();
            SetItem(0);
        }
        else if (this.num == 1 && GetNumPremiumBags() > 0 && !discounted)
        {
            /*FlurryAnalytics.Instance().LogEvent("OPENED_PREMIUM_BAG", new string[4]
            {
                "sessions:" + Stats.GetNumSessions() + string.Empty,
                "num:" + this.num,
                "num_iap:" + PlayerPrefs.GetInt("NUM_PURCHASES") + string.Empty,
                "num_wins:" + Stats.GetNumWins() + string.Empty
            }, false);*/
            //AdMediation.TrackEventInTj("OPENED_PREMIUM_BAG", Stats.GetNumSessions());
            flag = true;
            UsePremiumBag();
            SetItem(1);
        }
        else if (this.num == 2)
        {
            /*FlurryAnalytics.Instance().LogEvent("OPENED_DAILY_BAG", new string[4]
            {
                "sessions:" + Stats.GetNumSessions() + string.Empty,
                "num:" + this.num,
                "num_iap:" + PlayerPrefs.GetInt("NUM_PURCHASES") + string.Empty,
                "num_wins:" + Stats.GetNumWins() + string.Empty
            }, false);*/
            //AdMediation.TrackEventInTj("OPENED_DAILY_BAG", Stats.GetNumSessions());
            flag = true;
            UseDailyBag();
            SetItem(2);
        }
        else if (goldPrice > 0)
        {
            Debug.Log("进入金币大于0".FL1_HotPink());
            num = ((!useDiscount) ? goldPrice : goldPriceDiscounted);
            if (Currency.SpendGold(num, "bag"))
            {
                flag = true;
               
                //AdMediation.TrackEventInTj("PURCHASED_BAG", Stats.GetNumSessions());
                /*RevenueTracker.PurchasedBag();*/
            }
            else
            {
                getGoldButton.ShowGetGoldBox();
                //AdMediation.ShowTjpInsufficientCurrency();
            }
        }
        else
        {
            Debug.Log("消费现金！".FH4_OrangeRed());
            num = ((!useDiscount) ? cashPrice : cashPriceDiscounted);
            if (Currency.SpendCash(num))
            {
                flag = true;
              
                //AdMediation.TrackEventInTj("PURCHASED_STANDARD_BAG", Stats.GetNumSessions());
                /*RevenueTracker.PurchasedBag();*/
            }
            else
            {
                currencyExchangeBox.gameObject.SetActive(true);
                currencyExchangeBox.SetCashPrice(num, 0);
            }
        }
        if (flag)
        {
            Debug.Log("购买物品更新界面打开支付背包".FL3_Pink());
            topNavBar.UpdateCurrencyDisplay();
            ShowPurchasedBag();
        }
    }

    public virtual void ShowPurchasedBag()
    {
        tabChanger.SetToScreen(screenEnum.OpenBag);
        bool flag = num == 2;
        bool isPremium = num == 1;
        if (flag)
        {
            openBag.ShowOpenBagBox(isPremium, flag, "BONUS OFFER", "PREMIUM BAG", 15, 12);
        }
        else
        {
            openBag.ShowOpenBagBox(isPremium, flag, "ONE MORE BAG", titles[num], goldPrices[num], goldPricesDiscounted[num]);
        }
    }

    public virtual void ExchangeGoldForCashAndBuy()
    {
        int currentCash = Currency.GetCurrentCash();
        int num = 0;
        num = ((!discounted) ? cashPrice : cashPriceDiscounted);
        int num2 = num - currentCash;
        int cashGoldPrice = Currency.GetCashGoldPrice(num2, 0);
        if (Currency.SpendGold(cashGoldPrice, "gold_for_standardbag_cash"))
        {
            currencyExchangeBox.gameObject.SetActive(false);
            Currency.AddCash(num2);
            Buy(discounted);
        }
        else
        {
            getGoldButton.ShowGetGoldBox();
            //AdMediation.ShowTjpInsufficientCurrency();
        }
    }

    public static void AddStandardBag(string method)
    {
        AddStandardBags(1, method);
    }

    public static void AddStandardBags(int amt, string method)
    {
        PlayerPrefsHelper.SetInt("NUM_STANDARD_BAGS", GetNumStandardBags() + amt, true);
        /*RevenueTracker.AddedStandardBags(amt, method);*/
    }

    private static void UseStandardBag()
    {
        PlayerPrefsHelper.SetInt("NUM_STANDARD_BAGS", GetNumStandardBags() - 1, true);
    }

    public static int GetNumStandardBags()
    {
        return PlayerPrefs.GetInt("NUM_STANDARD_BAGS");
    }

    private static void UsePremiumBag()
    {
        PlayerPrefsHelper.SetInt("NUM_PREMIUM_BAGS", GetNumPremiumBags() - 1, true);
        /*RevenueTracker.OpenedPremiumBag();*/
    }

    public static int GetNumPremiumBags()
    {
        return PlayerPrefs.GetInt("NUM_PREMIUM_BAGS");
    }

    public static int GetNumDailyBags()
    {
        int result = 0;
        if (Stats.GetNumWins() >= 2 && SecondsUntilDailyBagAvailable() <= 2)
        {
            result = 1;
        }
        return result;
    }

    private static void UseDailyBag()
    {
        SessionVars instance = SessionVars.GetInstance();
        int num = 200;
        int val = instance.currentTimestamp + 86400 - num;
        PlayerPrefsHelper.SetInt("NEXT_DAILY_BAG_TS", val);
        //PlayFabManager.Instance().SetUserDataForKey("NEXT_DAILY_BAG_TS", val);
        /*RevenueTracker.OpenedDailyBag();*/
    }

    public static int SecondsUntilDailyBagAvailable()
    {
        int @int = PlayerPrefs.GetInt("NEXT_DAILY_BAG_TS");
        SessionVars instance = SessionVars.GetInstance();
        int currentTimestamp = instance.currentTimestamp;
        return @int - currentTimestamp;
    }
}