/*using UnityEngine;

public class ConversionTracker : MonoBehaviour
{
    private static int iapGoldAmount;

    private static int offerwallGoldAmount;

    private static int rewVidGoldAmount;

    public static void AddedGold(int amount, string method)
    {
        switch (method)
        {
            case "iap":
                iapGoldAmount = amount;
                break;
            case "offerwall":
                offerwallGoldAmount = amount;
                break;
            case "rewVid":
                rewVidGoldAmount = amount;
                break;
        }
    }

    public static void SpentGold(int spentAmount, string itemType)
    {
        int num = 0;
        string empty = string.Empty;
        if (iapGoldAmount > 0)
        {
            empty = ((iapGoldAmount >= 3000)
                ? "SPENT_LARGEST_IAP_GOLD"
                : ((PlayerPrefs.GetInt("NUM_PURCHASES") != 1) ? "SPENT_IAP_GOLD" : "SPENT_FIRST_IAP_GOLD"));
            num = iapGoldAmount;
        }
        else if (offerwallGoldAmount > 0)
        {
            empty = ((offerwallGoldAmount < 100) ? "SPENT_OFFERWALL_GOLD" : "SPENT_LARGE_OFFERWALL_GOLD");
            num = offerwallGoldAmount;
        }
        else
        {
            empty = "SPENT_REWVID_GOLD";
            num = rewVidGoldAmount;
        }
        /*if (num > 0)
        {
            FlurryAnalytics.Instance().LogEvent(empty, new string[3]
            {
                "itemType:" + itemType,
                "spentAmount:" + spentAmount,
                "goldReceived:" + num
            }, false);
        }#1#
        iapGoldAmount = 0;
        offerwallGoldAmount = 0;
        rewVidGoldAmount = 0;
    }
}*/