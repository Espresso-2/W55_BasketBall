using System;
using UnityEngine;

namespace W_Log.w_Script
{
    public class W_Singleton<T> : MonoBehaviour where T : W_Singleton<T>
    {
        private static T instance;
        public static T Instance
        {
            get {
                if (instance == null)
                {
                    instance = FindObjectOfType<T>();
                    if (instance == null)
                    {
                        instance = new GameObject(typeof(T).Name).AddComponent<T>();
                    }
                }
                return instance;
            }
        }

        protected void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }
            instance = this as T;
            DontDestroyOnLoad(gameObject);
        }

        protected virtual void Init()
        {
            
        }
    }
}