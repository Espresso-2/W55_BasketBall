using System;
using System.Runtime.InteropServices;
using AdGeneric.Ad;
using AdGeneric.AdBox;
using AdGeneric.Ext;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace AdGeneric.Operation
{
    [Operation(global::AdGeneric.Operation.Operation.深圳路总)]
    public class LuZong : BaseOperation
    {
        #region value

        [Header("广告参数")] [SerializeField] private string banner = "";
        // [SerializeField] private string 原生Icon = "";
        [SerializeField] private string 其他模板 = "";
        [SerializeField] private string 结算模板 = "";
        [SerializeField] private string 设置签到模板 = "";
        [SerializeField] private string 激励_视频 = "";

        // [SerializeField] private string 宝箱 = "";

        // [SerializeField] private float iconFirstTime=30;
        // [SerializeField] private float iconRepeatTime=30;

        #endregion

        private bool IsBoxAllowed { get; set; }

        public void LuZongBoxSwitchReceiver(string value)
        {
            switch (value)
            {
                case "On":
                    IsBoxAllowed = true;
                    break;
                default:
                    value.Log();
                    break;
            }
        }

        protected override void Start()
        {
            if (AdTotalManager.Instance.Operation != global::AdGeneric.Operation.Operation.深圳路总) return;
            print(nameof(LuZong));
            // InvokeRepeating(nameof(RepeatIcon),iconFirstTime,iconRepeatTime);
        }

        public override void Init()
        {
            AdGeneric.Ad.AdManager.Init(激励_视频);
            // LuZongInit(宝箱);
        }

        public override void ShowBlackAd(AdSource source = AdSource.Generic) => ShowWhiteAd(source);

        public override void ShowWhiteAd(AdSource source = AdSource.Generic)
        {
            switch (source)
            {
                case AdSource.GameWin:
                case AdSource.GameLose:
                case AdSource.Pause:
                    DelayManager.Add(() =>
                    {
                        AdGeneric.Ad.AdManager.ShowCustomAd(结算模板);
                        AdGeneric.Ad.AdManager.ShowBannerAd(banner);
                    }, 1);
                    break;
                case AdSource.SignUp:
                case AdSource.Setting:
                    AdGeneric.Ad.AdManager.ShowCustomAd(设置签到模板);
                    AdGeneric.Ad.AdManager.ShowBannerAd(banner);
                    break;
                default:
                    AdGeneric.Ad.AdManager.ShowCustomAd(其他模板);
                    AdGeneric.Ad.AdManager.ShowBannerAd(banner);
                    break;
            }
        }

        public override void Show(Addition addition)
        {
            if ((addition & Addition.宝箱) != 0 && IsBoxAllowed) BoxManager.Instance.ShowBox();
        }

        public override void ShowRewardAd(string callBackObjectName, string callBackMethodName, string callBackParam = null,
            AdSource source = AdSource.Generic)
        {
            AdGeneric.Ad.AdManager.ShowRewardAd(callBackObjectName, callBackMethodName, callBackParam);
        }

        public override void CreateShortcutBlack()
        {
            AdAdapter.CreateShortcutButton();
        }

        public override void SimpleShortcutBlack()
        {
            AdAdapter.CreateShortcutButton();
        }

        private void RepeatIcon()
        {
            // AdGeneric.Ad.AdManager.ShowNativeIcon(原生Icon);
        }
#if UNITY_EDITOR
        public static void LuZongInit(string baoXiangPosIdStr)
        {
            Debug.Log("路总初始化");
        }

#else
        [DllImport("__Internal")]
        public static extern void LuZongInit(string baoXiangPosIdStr);
#endif
    }
}