using System.Reflection;
using AdGeneric.Ext;
using AdGeneric.Operation;
using UnityEngine;

namespace AdGeneric.Ad
{
    public class AdManager:Ext.Singleton<AdManager>
    {
        [Header("Ad")]
        [SerializeField] private float adFirstTime = 45f;
        [SerializeField] private float adRepeatTime = 45f;
        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            InvokeRepeating(nameof(RepeatAd), adFirstTime, adRepeatTime);
        }

        private void RepeatAd()
        {
            print(nameof(RepeatAd));
            AdTotalManager.Instance.ShowBlackAd(AdSource.Repeat);
        }
        private void AreaReceiver(string value)
        {
            var property = typeof(AdTotalManager).GetProperty("CurrentOperation",
                BindingFlags.Public|BindingFlags.NonPublic|BindingFlags.Instance);
            if (property == null) return;
            var operation = (BaseOperation)property.GetMethod.Invoke(AdTotalManager.Instance,new object[0]);
            if (operation == null) return;
            var isAllowedProperty = operation.GetType().GetProperty("IsAllowed",
                BindingFlags.Public|BindingFlags.NonPublic|BindingFlags.Instance);
            if (isAllowedProperty == null) return;
            switch (value)
            {
                case "HasAd":
                    isAllowedProperty.SetMethod.Invoke(operation, new object[] {true});
                    $"jslib receive :{value}".Log();
                    break;
                case "NoAd":
                    $"jslib receive :{value}".Log();
                    break;
                default:
                    value.LogError();
                    break;
            }
        }
        public static void Init(string reward)
        {
            if (string.IsNullOrWhiteSpace(reward)) return;
            reward=reward.Replace(" ","");
            AdAdapter.Init(reward);
        }

        public static void ShowBannerAd(string banner)
        {
            if (string.IsNullOrWhiteSpace(banner)) return;
            banner=banner.Replace(" ","");
            AdAdapter.ShowBannerAd(banner,AdTotalManager.Instance.ShowDataTime);
        }

        public static void ShowCustomAd(string custom)
        {
            if (string.IsNullOrWhiteSpace(custom)) return;
            custom=custom.Replace(" ","");
            AdAdapter.ShowCustomAd(custom,AdTotalManager.Instance.ShowDataTime,AdTotalManager.Instance.Orientation);
        }

        public static void ShowNative(string native)
        {
            if (string.IsNullOrWhiteSpace(native)) return;
            native=native.Replace(" ","");
            AdAdapter.ShowNative(native,AdTotalManager.Instance.Orientation);
        }

        public static void ShowNativeIcon(string nativeIcon)
        {
            if (string.IsNullOrWhiteSpace(nativeIcon)) return;
            nativeIcon=nativeIcon.Replace(" ","");
            AdAdapter.ShowNativeIcon(nativeIcon,AdTotalManager.Instance.Orientation);
        }

        public static void ShowRewardAd(string callBackObjectName, string callBackMethodName, string callBackParam = null)
        {
            AdAdapter.ShowRewardAd(callBackObjectName, callBackMethodName, callBackParam);
        }
    }
}
