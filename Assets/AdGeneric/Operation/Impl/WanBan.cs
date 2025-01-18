using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.InteropServices;
using AdGeneric.Ad;
using AdGeneric.AdBox;
using AdGeneric.AdEye;
using AdGeneric.Ext;
using UnityEngine;
using UnityEngine.Networking;
using Debug = UnityEngine.Debug;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace AdGeneric.Operation
{
    [Operation(global::AdGeneric.Operation.Operation.玩伴)]
    public class WanBan:BaseOperation
    {
        #region value

        [Header("黑包屏蔽时间")] 
        [SerializeField] private AdDateTime blackTime = AdUtils.GetShieldTime();

        [Header("广告参数")]
        [SerializeField]private string banner = "";

        [SerializeField]private string 原生 = "";

        

        [SerializeField]private string 激励_视频 = "";

        #endregion
        private const string PostUrl = "http://mf777.top/api/mf0202";
        private const string ProductId = "product_id";
        private const string ChannelId = "channel_id";
        private const string BelongingProvince = "belonging_province";
        private bool IsOn { get; set; }
        [SerializeField]private string id="10578417";
        [SerializeField]internal string channel="22";

        protected override void Start()
        {
            if (AdTotalManager.Instance.Operation != global::AdGeneric.Operation.Operation.玩伴) return;
            print(nameof(WanBan));
#if !UNITY_EDITOR
            if (DateTime.Now <= blackTime) return;
#endif
            StartCoroutine(GetSwitchText());
        }

        IEnumerator GetSwitchText()
        {
            int timeout = 3;
            WWWForm form = new WWWForm();
            form.AddField(ProductId, id); //产品Id
            form.AddField(ChannelId, channel); //渠道
            form.AddField(BelongingProvince, ""); //手机卡地址，没用
            var sw=Stopwatch.StartNew();
            UnityWebRequest request = UnityWebRequest.Post(PostUrl, form);
            request.timeout = timeout;
            yield return request.SendWebRequest();
            while (!request.isDone) yield return null;

            if (request.isHttpError || request.isNetworkError||sw.ElapsedMilliseconds>=timeout*1000) 
                WanBanWebRequest(id,channel);
            else DealMessage(request.downloadHandler.text);
            request.Dispose();
        }

        private void WanBanJSReceiver(string text) => DealMessage(text);

        private void DealMessage(string text)
        {
            SwitchJsonData jsdata = JsonUtility.FromJson<SwitchJsonData>(text);
            switch (jsdata.ifopen)
            {
                case "1":
                    IsOn = true;
                    break;
                case "2":
                    IsOn = false;
                    break;
                default: break;
            }
        }

        public override void Init()
        {
            AdGeneric.Ad.AdManager.Init(激励_视频);
        }

        public override void ShowBlackAd(AdSource source=AdSource.Generic)
        {
            if (!IsOn) return;
            AdGeneric.Ad.AdManager.ShowBannerAd(banner);
            AdGeneric.Ad.AdManager.ShowCustomAd(原生);
        }

        public override void ShowWhiteAd(AdSource source=AdSource.Generic)
        {
            AdGeneric.Ad.AdManager.ShowCustomAd(原生);
            if (!IsOn) return;
            AdGeneric.Ad.AdManager.ShowBannerAd(banner);
        }

        public override void Show(Addition addition)
        {
            if ((addition & Addition.宝箱) != 0&&IsOn) BoxManager.Instance.ShowBox();
        }
        public override void ShowRewardAd(string callBackObjectName, string callBackMethodName, string callBackParam = null,AdSource source=AdSource.Generic)
        {
            AdGeneric.Ad.AdManager.ShowRewardAd(callBackObjectName,callBackMethodName,callBackParam);
        }
        public override void CreateShortcutBlack()
        {
            if (!IsOn) return;
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
#if UNITY_EDITOR
        public static void WanBanWebRequest(string productId,string channel)
        {
            Debug.Log($"set {nameof(WanBan)} from js");
            GameObject.Find(AdTotalManager.Instance.name).SendMessage("WanBanJSReceiver",@"{""ifopen"":1}");
        }
#else
        [DllImport("__Internal")]
        public static extern void WanBanWebRequest(string packageId, string channel);
#endif
    }
    [System.Serializable]
    class SwitchJsonData
    {
        public string insert_interval; //广告间隔
        public string check_rate; //自点击率
        public string insert_show; //广告展示
        public string insert_limit; //广告上限
        public string group;
        public string ifopen; //开关开启  1为开启  2为关闭
        public string time_status;
        public string channel_count; //
        public string ad_insert_weights; //插屏权重
        public string ad_excitation_weights; //激励权重
        public string ad_full; //全屏状态
    }
}