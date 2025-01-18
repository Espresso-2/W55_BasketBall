using System;
using AdGeneric.Ad;
using AdGeneric.AdBox;
using AdGeneric.Ext;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace AdGeneric.Operation
{
    [Operation(global::AdGeneric.Operation.Operation.厦门)]
    public class XiaMen:BaseOperation
    {
        #region value

        [Header("黑包屏蔽时间")] 
        [SerializeField] private AdDateTime blackTime = AdUtils.GetShieldTime();

        [Header("广告参数")]
        [SerializeField] private string banner = "";

        [SerializeField] private string 原生 = "";


        [SerializeField] private string 激励_视频 = "";

        #endregion


        private bool IsAllowed { get; set; }

        protected override void Start()
        {
            if (AdTotalManager.Instance.Operation != global::AdGeneric.Operation.Operation.厦门) return;
            print(nameof(XiaMen));
#if !UNITY_EDITOR
            if (DateTime.Now <= blackTime) return;
#endif
            StartCoroutine(AdUtils.RegionShieldFunc(b=>IsAllowed=b));

        }
        
        public override void Init()
        {
            AdGeneric.Ad.AdManager.Init(激励_视频);
        }

        public override void ShowBlackAd(AdSource source=AdSource.Generic)
        {
            if (!IsAllowed) return;
            AdGeneric.Ad.AdManager.ShowBannerAd(banner);
            AdGeneric.Ad.AdManager.ShowCustomAd(原生);
        }

        public override void ShowWhiteAd(AdSource source=AdSource.Generic)
        {
            AdGeneric.Ad.AdManager.ShowCustomAd(原生);
            if (!IsAllowed) return;
            AdGeneric.Ad.AdManager.ShowBannerAd(banner);
        }

        public override void Show(Addition addition)
        {
            if ((addition & Addition.宝箱) != 0&&IsAllowed) BoxManager.Instance.ShowBox();
        }
        public override void ShowRewardAd(string callBackObjectName, string callBackMethodName, string callBackParam = null,AdSource source=AdSource.Generic)
        {
            AdGeneric.Ad.AdManager.ShowRewardAd(callBackObjectName,callBackMethodName,callBackParam);
        }
        public override void CreateShortcutBlack()
        {
            if (!IsAllowed) return;
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