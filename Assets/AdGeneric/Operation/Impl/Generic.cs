using System;
using System.Collections;
using System.Linq;
using AdGeneric.Ad;
using AdGeneric.AdBox;
using AdGeneric.AdEye;
using AdGeneric.Ext;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace AdGeneric.Operation
{
    [Operation(global::AdGeneric.Operation.Operation.通用)]
    public class Generic:BaseOperation
    {
        #region value

        [Header("黑包屏蔽时间")] 
        [SerializeField] private AdDateTime blackTime = AdUtils.GetShieldTime();

        [Header("广告参数")]
        [SerializeField] private string banner = "";

        [SerializeField] private string 原生 = "";

        [SerializeField] private string 原生左图 = "";

        [SerializeField] private string 激励_视频 = "";

        #endregion


        [SerializeField]private bool 使用地区屏蔽=true;
        private bool IsAllowed { get; set; }

        protected override void Start()
        {
            if (AdTotalManager.Instance.Operation != global::AdGeneric.Operation.Operation.通用) return;
            print(nameof(Generic));
#if !UNITY_EDITOR
            if (DateTime.Now <= blackTime) return;
#endif
            if (使用地区屏蔽)
            {
                print("使用地区屏蔽");
                StartCoroutine(AdUtils.RegionShieldFunc(b=>IsAllowed=b));
            }
            else
            {
                print("不使用地区屏蔽");
                IsAllowed = true;
            }
        }

        


        public override void Init()
        {
            AdGeneric.Ad.AdManager.Init(激励_视频);
        }

        public override void ShowBlackAd(AdSource source=AdSource.Generic)
        {
            if (!IsAllowed) return;
            AdGeneric.Ad.AdManager.ShowBannerAd(banner);
            AdGeneric.Ad.AdManager.ShowCustomAd(原生,原生左图);
        }

        public override void ShowWhiteAd(AdSource source=AdSource.Generic)
        {
            AdGeneric.Ad.AdManager.ShowCustomAd(原生,原生左图);
            if (!IsAllowed) return;
            AdGeneric.Ad.AdManager.ShowBannerAd(banner);
        }

        public override void Show(Addition addition)
        {
            if ((addition & Addition.宝箱) != 0&&IsAllowed) BoxManager.Instance.ShowBox();
            if ((addition & Addition.护眼) != 0)EyeManager.Instance.ShowEye();
        }
        public override void ShowRewardAd(string callBackObjectName, string callBackMethodName, string callBackParam = null,AdSource source=AdSource.Generic)
        {
            AdGeneric.Ad.AdManager.ShowRewardAd(激励_视频,callBackObjectName,callBackMethodName,callBackParam);
        }
        public override void CreateShortcutBlack()
        {
            if (!IsAllowed) return;
            AdAdapter.CreateShortcutButton();
        }

        public override void SimpleShortCurBlack()
        {
            AdAdapter.CreateShortcutButton();
        }
#if UNITY_EDITOR
        [AdInspectorButton("设置屏蔽时间")]
        public void SetShieldTime()
        {
                        Undo.RecordObject(this,nameof(SetShieldTime));
            blackTime = AdUtils.GetShieldTime();
        }
#endif
    }

}
