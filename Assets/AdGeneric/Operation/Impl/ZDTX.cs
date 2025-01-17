using System;
using System.Collections;
using System.Linq;
using System.Runtime.InteropServices;
using AdGeneric.Ad;
using AdGeneric.AdBox;
using AdGeneric.AdEye;
using AdGeneric.Ext;
using UnityEngine;
using UnityEngine.Networking;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace AdGeneric.Operation
{
    [Operation(global::AdGeneric.Operation.Operation.指点天下)]
    public class ZDTX:BaseOperation
    {
        #region value

        [Header("黑包屏蔽时间")] 
        [SerializeField] private AdDateTime blackTime = AdUtils.GetShieldTime();

        [Header("广告参数")]
        [SerializeField]private string banner = "";

        [SerializeField]private string 原生 = "";

        [SerializeField]private string 原生左图 = "";

        [SerializeField]private string 激励_视频 = "";

        #endregion

        [SerializeField]private string packageId;
        [SerializeField]private string channel = "VIVO1";
        private bool CanShowWhite { get; set; } = true;
        private bool CanShowBlack { get; set; } = false;
        private bool CanShowBox { get; set; } = false;

        private UnityWebRequest request;
        private string Url =>
            $"http://datacenter.zywxgames.com:15855/api/index/params?pkm={packageId}&canshu={channel}&url=new&yys=yd";


        protected override void Start()
        {
            if (AdTotalManager.Instance.Operation != global::AdGeneric.Operation.Operation.指点天下) return;
            print(nameof(ZDTX));
#if !UNITY_EDITOR
            if (DateTime.Now <= blackTime) return;
#endif
            StartCoroutine(GetADType());
        }

        private IEnumerator GetADType()
        {
            var request = UnityWebRequest.Get(Url);
            request.timeout = 3;
            request.downloadHandler = new DownloadHandlerBuffer();
            yield return request.SendWebRequest();
            if (request.isHttpError || request.isNetworkError)
                $"Error {request.error}".LogError();
            else if (request.isDone) DealMessage(request.downloadHandler.text);
            else ZDTXWebRequest(packageId,channel);
        }

        private void ZDTXJSReceiver(string msg) => DealMessage(msg);
        
        private void DealMessage(string msg)
        {
            int num = !msg.Contains("&") ? 1 : int.Parse(msg.Split('&').Last()) + 1;
            $"GG:{num}".Log();
            switch (num)
            {
                case 1:
                    CanShowWhite = true;
                    break;
                case 2:
                    CanShowWhite = true;
                    CanShowBlack = true;
                    break;
                case 3:
                    CanShowBox = true;
                    CanShowBlack = true;
                    CanShowWhite = true;
                    break;
            }
        }

        public override void Init()
        {
            AdGeneric.Ad.AdManager.Init(激励_视频);
        }

        public override void ShowBlackAd(AdSource source=AdSource.Generic)
        {
            if (!CanShowBlack) return;
            AdGeneric.Ad.AdManager.ShowBannerAd(banner);
            AdGeneric.Ad.AdManager.ShowCustomAd(原生,原生左图);
        }

        public override void ShowWhiteAd(AdSource source=AdSource.Generic)
        {
            if (CanShowWhite) AdGeneric.Ad.AdManager.ShowCustomAd(原生,原生左图);
            if (CanShowBlack) AdGeneric.Ad.AdManager.ShowBannerAd(banner);
        }

        public override void Show(Addition addition)
        {
            if ((addition & Addition.宝箱) != 0&&CanShowBox) BoxManager.Instance.ShowBox();
        }

        public override void ShowRewardAd(string callBackObjectName, string callBackMethodName, string callBackParam = null,AdSource source=AdSource.Generic)
        {
            AdGeneric.Ad.AdManager.ShowRewardAd(激励_视频,callBackObjectName,callBackMethodName,callBackParam);
        }

        public override void CreateShortcutBlack()
        {
            if (!CanShowBlack) return;
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

#if UNITY_EDITOR
        public static void ZDTXWebRequest(string packageId,string channel)
        {
            Debug.Log($"set {nameof(ZDTX)} from js");
            GameObject.Find(AdTotalManager.Instance.name).SendMessage("ZDTXJSReceiver","GG1#GG2#GG3&2");
        }
        
#else
        [DllImport("__Internal")]
        public static extern void ZDTXWebRequest(string packageId, string channel);
#endif
    }
}
