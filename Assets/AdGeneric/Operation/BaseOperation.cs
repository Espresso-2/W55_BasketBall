using System;
using AdGeneric.Ext;
using UnityEngine;

namespace AdGeneric.Operation
{
    public abstract class BaseOperation :MonoBehaviour
    {
        protected abstract void Start();
        public abstract void Init();
        public abstract void ShowBlackAd(AdSource source=AdSource.Generic);
        public abstract void ShowWhiteAd(AdSource source=AdSource.Generic);
        public abstract void Show(Addition addition);
        public abstract void ShowRewardAd(string callBackObjectName, string callBackMethodName, string callBackParam=null,AdSource source=AdSource.Generic);
        public abstract void CreateShortcutBlack();
        public abstract void SimpleShortCurBlack();

    }
}