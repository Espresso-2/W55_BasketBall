using System;
using PublicComponentCenter;
using UnityEngine;
using UnityEngine.UI;
using W_Log.w_Script;

[Serializable]
public class BuyGoldButton : MonoBehaviour
{
    public Text amountText;

    public Text priceText;

    public GameObject featuredPackToHide;

    private LoadingBox loadingBox;

    private GameSounds gameSounds;

    public int num;

    private string[] iapIds;

    private PurchasableItem iapItem;

    public BuyGoldButton()
    {
        iapIds = new string[8]
        {
            "com.doubletapsoftware.basketballbattle.goldpack1", "com.doubletapsoftware.basketballbattle.goldpack2",
            "com.doubletapsoftware.basketballbattle.goldpack3", "com.doubletapsoftware.basketballbattle.goldpack4",
            "com.doubletapsoftware.basketballbattle.goldpack5", "com.doubletapsoftware.basketballbattle.goldpack6",
            "com.doubletapsoftware.basketballbattle.goldpack4xbonusoffer", "com.doubletapsoftware.basketballbattle.goldpack5xbonusoffer"
        };
    }

    public virtual void OnEnable()
    {
        if (iapItem == null)
        {
            return;
        }
        int num = PurchaseGiver.goldPackAmounts[this.num];
        int @int = PlayerPrefs.GetInt("NUM_PURCHASES");
        int num2 = 1;
        if (iapIds[this.num] == "com.doubletapsoftware.basketballbattle.goldpack4xbonusoffer")
        {
            num2 = 4;
        }
        else if (iapIds[this.num] == "com.doubletapsoftware.basketballbattle.goldpack5xbonusoffer")
        {
            num2 = 5;
        }
        if (num2 > 1 && @int >= 1)
        {
            if (featuredPackToHide != null)
            {
                featuredPackToHide.SetActive(true);
            }
            base.gameObject.SetActive(false);
            return;
        }
        num /= num2;
        amountText.text = num.ToString("n0");
        priceText.text = iapItem.localizedPriceString;
        if (featuredPackToHide != null)
        {
            featuredPackToHide.SetActive(false);
        }
    }

    public virtual void Start()
    {
        gameSounds = GameSounds.GetInstance();
    }

    public virtual void Buy()
    {
        /*AdTotalManager.Instance.ShowRewardAd(gameObject.name, nameof(RewardBug));*/
        GameEntry.Ad.NativeMsgReceiver.AddEvent("BugGold", (s) => RewardBug());
        GameEntry.Ad.ShowRewardedVideoAd("BugGold");
    }

    private void RewardBug()
    {
        gameSounds.Play_ball_dribble();
        PlayerPrefsHelper.SetInt("TAPJOY_BONUS", 0);
        PlayerPrefsHelper.SetInt("IS_TAPJOY_PURCHASE_REQUEST", 0);
        w_RewardAdapter.Instance.ADDGold(200);
        GameEntry.Ad.NativeMsgReceiver.RemoveEvent("BugGold");
    }

    // public virtual void Update()
    // {
    // }
    //
    // private void ShowLoadingBox()
    // {
    // 	if (loadingBoxParent == null)
    // 	{
    // 		GameObject gameObject = GameObject.Find("Canvas");
    // 		if (gameObject != null)
    // 		{
    // 			loadingBoxParent = gameObject.transform;
    // 		}
    // 	}
    // 	if (loadingBoxParent != null)
    // 	{
    // 		loadingBox = UnityEngine.Object.Instantiate(loadingBoxPrefab, loadingBoxParent).GetComponent<LoadingBox>();
    // 	}
    // }
}