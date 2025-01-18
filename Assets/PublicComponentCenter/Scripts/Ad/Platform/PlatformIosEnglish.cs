﻿using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace PublicComponentCenter
{
#if UNITY_IOS
    /// <summary>
    /// IOS英文广告策略(GoogleAdMob)
    /// </summary>
    public class PlatformIosEnglish : AppPlatformBase
    {
        public override bool IsFreeApp
        {
            get { return true; }
        }

        public override bool AllowUnityQuit
        {
            get { return false; }
        }

        #region Oc Method

        /// <summary>
        /// IOS层显示激励视频方法
        /// </summary>
        /// <param name="args"> 参数 </param>
        [DllImport("__Internal")]
        private static extern void ShowRewardVideoAdGam(string args);

        /// <summary>
        /// IOS层显示Banner方法
        /// </summary>
        [DllImport("__Internal")]
        private static extern void ShowBannerAdGam();

        /// <summary>
        /// IOS层显示插屏方法
        /// </summary>
        [DllImport("__Internal")]
        private static extern void ShowInterstitialAdGam();

        /// <summary>
        /// IOS层显示全屏视频方法
        /// </summary>
        [DllImport("__Internal")]
        private static extern void ShowFullScreenVideoAdGam();

        #endregion

        public override bool ShowRewardedVideoAd(params object[] args)
        {
            if (args.Length > 0)
            {
                Debug.LogFormat("Dxy Unity call ShowRewardVideoAd method,args is :{0} ", args[0]);
                try
                {
                    ShowRewardVideoAdGam(args[0].ToString());
                }
                catch (Exception e)
                {
                    Debug.LogWarningFormat("Dxy {1} ShowRewardedVideoAd , Error:{0}",
                        e.Message, GetType().Name);
                }
            }

            return true;
        }


        public override bool ShowFullScreenAd(params object[] args)
        {
            try
            {
                ShowFullScreenVideoAdGam();
                ;
            }
            catch (Exception e)
            {
                Debug.LogWarningFormat("Dxy {1} ShowFullScreenAd , Error:{0}",
                    e.Message, GetType().Name);
            }

            return true;
        }

        public override bool ShowInterstitialAd(params object[] args)
        {
            try
            {
                ShowInterstitialAdGam();
            }
            catch (Exception e)
            {
                Debug.LogWarningFormat("Dxy {1} ShowInterstitialAd , Error:{0}",
                    e.Message, GetType().Name);
            }

            return true;
        }

        public override bool ShowInterstitialVideoAd(params object[] args)
        {
            return false;
        }

        public override bool ShowBannerAd(params object[] args)
        {
            try
            {
                ShowBannerAdGam();
            }
            catch (Exception e)
            {
                Debug.LogWarningFormat("{1} ShowBannerAd , Error:{0}",
                    e.Message, GetType().Name);
            }

            return true;
        }


        public override bool MoreWonderful(params object[] args)
        {
            return false;
        }

        public override bool ContactCustomerService(params object[] args)
        {
            return false;
        }

        public override bool PrivacyPolicy(params object[] args)
        {
            return false;
        }

        public override Language GetAppLanguage(params object[] args)
        {
            return GameEntry.Localization.Language;
        }

        public override bool Quit(params object[] args)
        {
            return false;
        }

        public override bool Pay(int money, params object[] args)
        {
            return false;
        }

        public override bool Replenishment(params object[] args)
        {
            return false;
        }

        public override int GetAppType(params object[] args)
        {
            return 0;
        }
    }

#endif
}