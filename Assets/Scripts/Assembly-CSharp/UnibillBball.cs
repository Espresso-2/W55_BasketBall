using System;
using System.Collections;
using MiniJSON;
using TapjoyUnity;
using Unibill;
using UnityEngine;

[AddComponentMenu("Unibill/UnibillBball")]
public class UnibillBball : MonoBehaviour
{
    private void Start()
    {
        if (Resources.Load("unibillInventory.json") == null)
        {
            Debug.LogError("You must define your purchasable inventory within the inventory editor!");
            base.gameObject.SetActive(false);
            return;
        }
        Unibiller.onBillerReady += onBillerReady;
        Unibiller.onTransactionsRestored += onTransactionsRestored;
        Unibiller.onPurchaseFailed += onFailed;
        //Unibiller.onPurchaseCompleteEvent += onPurchased;
        Unibiller.onPurchaseDeferred += onDeferred;
        Unibiller.Initialise();
    }

    private void onBillerReady(UnibillState state)
    {
        Debug.Log("onBillerReady:" + state);
    }

    private void onTransactionsRestored(bool success)
    {
        Debug.Log("Transactions restored.");
        if (GameObject.Find("RestoreButton") != null)
        {
            GameObject.Find("RestoreButton").SendMessage("Restored");
        }
    }

    private void onPurchased(PurchaseEvent e)
    {
        string id = e.PurchasedItem.Id;
        Debug.Log("e.PurchasedItem.Id: " + id);
        int num = -1;
        int num2 = -1;
        int num3 = -1;
        int num4 = -1;
        int num5 = -1;
        switch (id)
        {
            case "com.doubletapsoftware.basketballbattle.goldpack1":
            case "com.doubletapsoftware.basketballbattle.goldspecial1":
                num = 0;
                break;
            case "com.doubletapsoftware.basketballbattle.goldpack2":
            case "com.doubletapsoftware.basketballbattle.goldspecial2":
                num = 1;
                break;
            case "com.doubletapsoftware.basketballbattle.goldpack3":
                num = 2;
                break;
            case "com.doubletapsoftware.basketballbattle.goldpack4":
                num = 3;
                break;
            case "com.doubletapsoftware.basketballbattle.goldpack5":
                num = 4;
                break;
            case "com.doubletapsoftware.basketballbattle.goldpack6":
                num = 5;
                break;
            case "com.doubletapsoftware.basketballbattle.goldpack4xbonusoffer":
                num = 6;
                break;
            case "com.doubletapsoftware.basketballbattle.goldpack5xbonusoffer":
                num = 7;
                break;
            case "com.doubletapsoftware.basketballbattle.deal1":
                num2 = 0;
                break;
            case "com.doubletapsoftware.basketballbattle.deal2":
                num2 = 1;
                break;
            case "com.doubletapsoftware.basketballbattle.bigwallet":
                num3 = 0;
                break;
            case "com.doubletapsoftware.basketballbattle.removeads":
                num4 = 0;
                break;
            case "com.doubletapsoftware.basketballbattle.15prizeballs":
                num5 = 0;
                break;
            case "com.doubletapsoftware.basketballbattle.5prizeballs":
                num5 = 1;
                break;
        }
        if (num >= 0)
        {
            giveGoldPackage(num, false);
        }
        else if (num3 >= 0)
        {
            giveGoldPackage(num3, true);
        }
        else if (num2 >= 0)
        {
            giveDeal(num2);
        }
        else if (num4 >= 0)
        {
            removeAds();
        }
        else if (num5 >= 0)
        {
            givePrizeBallsPackage(num5);
        }
        destroyLoadingBox();
        logPurchaseInTapjoy(e);
        if (PlayerPrefs.GetInt("IS_TAPJOY_PURCHASE_REQUEST") == 1)
        {
            AdMediation.MarkLastTjPurchaseRequestAsCompleted();
        }
        AdMediation.ShowTjpIapCompleted();
    }

    private void giveGoldPackage(int num, bool includeCash)
    {
        if (includeCash)
        {
            GameObject.Find("PurchaseGiver").SendMessage("AddGoldAndCashPackage", num);
        }
        else
        {
            GameObject.Find("PurchaseGiver").SendMessage("AddGoldPackage", num);
        }
        GameObject gameObject = GameObject.Find("GetGoldBox");
        if (gameObject == null)
        {
            GameObject gameObject2 = GameObject.Find("PlusButton");
            if (gameObject2 != null)
            {
                gameObject2.SendMessage("ShowGetGoldBox");
                gameObject = GameObject.Find("GetGoldBox");
            }
        }
        if (gameObject != null)
        {
            if (includeCash)
            {
                gameObject.SendMessage("AddedGoldAndCashPackage", num);
            }
            else
            {
                gameObject.SendMessage("AddedGoldPackage", num);
            }
            GameObject gameObject3 = GameObject.Find("BonusOfferMsgBox");
            if (gameObject3 != null)
            {
                gameObject3.SendMessage("CompletedPurchase");
            }
        }
        else
        {
            GameObject gameObject4 = GameObject.Find("HotPurchaseBox");
            if (gameObject4 != null && gameObject4.activeInHierarchy)
            {
                gameObject4.SendMessage("AddedGoldPackage", num);
            }
        }
    }

    private void giveDeal(int num)
    {
        GameObject.Find("PurchaseGiver").SendMessage("RecieveDeal", num);
        GameObject gameObject = GameObject.Find("DealPack");
        if (gameObject != null)
        {
            gameObject.SendMessage("RecievedDeal", num);
        }
    }

    private void givePrizeBallsPackage(int num)
    {
        GameObject.Find("PurchaseGiver").SendMessage("AddPrizeBallsPackage", num);
        GameObject gameObject = GameObject.Find("PrizeBallGame");
        if (gameObject != null)
        {
            PrizeBallGameController component = gameObject.GetComponent<PrizeBallGameController>();
            component.PurchasedPrizeBalls();
        }
    }

    private void removeAds()
    {
        GameObject.Find("PurchaseGiver").SendMessage("RemoveAds");
        GameObject gameObject = GameObject.Find("RemoveAdsMsgBox");
        if (gameObject != null)
        {
            gameObject.SendMessage("RemovedAds");
        }
    }

    private string GetIapSignature(string receipt)
    {
        IDictionary dictionary = (IDictionary)Json.Deserialize(receipt);
        string text = string.Empty;
        try
        {
            if (dictionary != null && dictionary["signature"] != null)
            {
                text = dictionary["signature"].ToString();
            }
        }
        catch
        {
        }
        Debug.Log("signature: " + text);
        return text;
    }

    private string GetIapData(string receipt)
    {
        IDictionary dictionary = (IDictionary)Json.Deserialize(receipt);
        string text = string.Empty;
        try
        {
            if (dictionary != null && dictionary["json"] != null)
            {
                text += dictionary["json"].ToString();
            }
        }
        catch
        {
        }
        Debug.Log("receiptForTracking: " + text);
        return text;
    }

    private void logPurchaseInTapjoy(PurchaseEvent e)
    {
        string receipt = e.Receipt;
        IDictionary dictionary = (IDictionary)Json.Deserialize(receipt);
        Debug.Log("receipt: " + receipt);
        Debug.Log("dict: " + dictionary);
        string text = string.Empty;
        string text2 = string.Empty;
        try
        {
            if (dictionary != null && dictionary["json"] != null)
            {
                text += dictionary["json"].ToString();
            }
            if (dictionary != null && dictionary["signature"] != null)
            {
                text2 = dictionary["signature"].ToString();
            }
        }
        catch
        {
        }
        Debug.Log("receiptForTracking: " + text);
        Debug.Log("signature: " + text2);
        string text3 = e.PurchasedItem.name;
        string localizedPriceString = e.PurchasedItem.localizedPriceString;
        string text4 = "inapp";
        string description = e.PurchasedItem.description;
        int num = Convert.ToInt32(e.PurchasedItem.priceInLocalCurrency * 1000000m);
        string isoCurrencySymbol = e.PurchasedItem.isoCurrencySymbol;
        string id = e.PurchasedItem.Id;
        string text5 = "{\"title\":\"" + text3 + "\",\"price\":\"" + localizedPriceString + "\",\"type\":\"" + text4 + "\",\"description\":\"" +
                       description + "\",\"price_amount_micros\":" + num + ",\"price_currency_code\":\"" + isoCurrencySymbol + "\",\"productId\":\"" +
                       id + "\"}";
        string text6 = text;
        string dataSignature = text2;
        string campaignId = null;
        Debug.Log("skuDetails: " + text5);
        Debug.Log("purchaseData: " + text6);
        Debug.Log("dataSignature: " + text2);
        Tapjoy.TrackPurchaseInGooglePlayStore(text5, text6, dataSignature, campaignId);
    }

    private void onCancelled(PurchasableItem item)
    {
        Debug.Log("Purchase cancelled: " + item.Id);
        updateLoadingBox("PURCHASE CANCELLED", "Contact SUPPORT@DOUBLETAPSOFTWARE.COM for help.");
        /*FlurryAnalytics.Instance().LogEvent("IAP_CANCELLED", new string[1] { "item.Id:" + item.Id }, false);*/
        if (PlayerPrefs.GetInt("IS_TAPJOY_PURCHASE_REQUEST") == 1)
        {
            AdMediation.MarkLastTjPurchaseRequestAsCancelled();
        }
        AdMediation.ShowTjpIapCancelled();
    }

    private void onDeferred(PurchasableItem item)
    {
        Debug.Log("Purchase deferred blud: " + item.Id);
        updateLoadingBox("PURCHASE DEFERRED", string.Empty);
    }

    private void onFailed(PurchaseFailedEvent e)
    {
        Debug.Log("onFailed e.Message: " + e.Message);
        Debug.Log("onFailed e.Reason: " + e.Reason);
        Debug.Log("onFailed e: " + e);
        updateLoadingBox("PURCHASE FAILED", "CONTACT SUPPORT@DOUBLETAPSOFTWARE.COM ERROR: " + e.Message + " " + e.Reason);
        /*FlurryAnalytics.Instance().LogEvent("IAP_FAILED", new string[3]
        {
            "item.Id:" + e.PurchasedItem.Id,
            "e.Message:" + e.Message,
            "e.Reason:" + e.Reason
        }, false);*/
    }

    private void destroyLoadingBox()
    {
        GameObject[] array = GameObject.FindGameObjectsWithTag("LoadingBox");
        GameObject[] array2 = array;
        foreach (GameObject obj in array2)
        {
            UnityEngine.Object.Destroy(obj);
        }
    }

    private void updateLoadingBox(string msg, string debug)
    {
        GameObject[] array = GameObject.FindGameObjectsWithTag("LoadingBox");
        GameObject[] array2 = array;
        foreach (GameObject gameObject in array2)
        {
            LoadingBox component = gameObject.GetComponent<LoadingBox>();
            if (component != null)
            {
                component.loadingText.text = msg;
                component.debugText.text = debug;
                component.userCanClose = true;
            }
        }
    }
}