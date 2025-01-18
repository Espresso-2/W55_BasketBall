//------------------------------------------------------------
// Copyright © 2020-2021 Lefend. All rights reserved.
//------------------------------------------------------------

using UnityEngine;

namespace PublicComponentCenter
{
    public class Demo_Ad : MonoBehaviour
    {
        void Start()
        {
            //这里只是为了Demo正常运行而设定，调用的时候只需  GameEntry.Ad.XXX即可
            DontDestroyOnLoad(gameObject);
        }


        void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                Debug.Log("支付");
                //华为支付
                GameEntry.Ad.Pay(10, 6001);
            }

            //插屏
            if (Input.GetKeyDown(KeyCode.I))
            {
                //无奖励
                GameEntry.Ad.ShowInterstitialAd();
                //带奖励
                GameEntry.Ad.ShowInterstitialAd(6002);
            }

            //Banner
            if (Input.GetKeyDown(KeyCode.B))
            {
                //无奖励
                GameEntry.Ad.ShowBannerAd();
                //带奖励
                GameEntry.Ad.ShowBannerAd(6003);
            }

            //游戏结束的全屏
            if (Input.GetKeyDown(KeyCode.O))
            {
                //如果是白包,弹插屏+Banner
                if (GameEntry.Ad.IsFreeApp)
                {
                    GameEntry.Ad.ShowInterstitialAd();
                    GameEntry.Ad.ShowBannerAd();
                }
                else //黑包,弹全屏
                {
                    //无奖励
                    GameEntry.Ad.ShowFullScreenAd();
                    //带奖励
                    GameEntry.Ad.ShowFullScreenAd(6004);
                }
            }

            //激励视频
            if (Input.GetKeyDown(KeyCode.R))
            {
                Debug.Log("播放解锁第20关视频");
                //无奖励
                GameEntry.Ad.ShowRewardedVideoAd();
                //带奖励
                GameEntry.Ad.ShowRewardedVideoAd(20);
            }
            
            //激励视频
            if (Input.GetKeyDown(KeyCode.G))
            {
                Debug.Log("插屏视频广告");
                GameEntry.Ad.ShowInterstitialVideoAd();
            }

            //补单
            if (Input.GetKeyDown(KeyCode.U))
            {
                GameEntry.Ad.Replenishment();
            }

            //更多精彩
            if (Input.GetKeyDown(KeyCode.M))
            {
                GameEntry.Ad.MoreWonderful();
            }

            //退出游戏
            if (Input.GetKeyDown(KeyCode.Q))
            {
                GameEntry.Ad.Quit();
            }
        }
    }
}