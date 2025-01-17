using System;
using UnityEngine;

namespace AdGeneric.Ext
{
    public class Singleton<T>:MonoBehaviour
        where T:Singleton<T>
    {
        public static T Instance { get; private set; }

        protected virtual void Awake() => Instance = (T)this;
    }
}