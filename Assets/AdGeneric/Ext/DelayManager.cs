using System;
using System.Collections;
using UnityEngine;

namespace AdGeneric.Ext
{
    public class DelayManager:MonoBehaviour
    {
        private static readonly object SyncRoot = new object();
        private static DelayManager instance;

        private static DelayManager Instance {
            get
            {
                if (instance != null) return instance;
                lock (SyncRoot)
                {
                    var o = new GameObject(nameof(DelayManager));
                    DontDestroyOnLoad(o);
                    instance = o.AddComponent<DelayManager>();
                }

                return instance;
            }
        }

        public static void Add(Action action,float seconds)
        {
            Instance.StartCoroutine(DelayFunc(action, seconds));
        }

        private static IEnumerator DelayFunc(Action action, float seconds)
        {
            yield return new WaitForSeconds(seconds);
            action?.Invoke();
        }
    }
}