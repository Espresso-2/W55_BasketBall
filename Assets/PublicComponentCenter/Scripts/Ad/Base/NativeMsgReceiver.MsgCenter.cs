﻿//------------------------------------------------------------
// Copyright © 2020-2021 Lefend. All rights reserved.
//------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PublicComponentCenter
{
    public partial class NativeMsgReceiver
    {
        /// <summary> Android回调参数和事件的映射 </summary>
        private Dictionary<string, UnityAction<string>> m_MsgDic = new Dictionary<string, UnityAction<string>>();

        /// <summary>
        /// 注册Android回调事件
        /// </summary>
        public bool AddEvent(string args, UnityAction<string> processEvent)
        {
            if (string.IsNullOrEmpty(args)) return false;
            if (!HasEvent(args))
            {
                m_MsgDic.Add(args, processEvent);
                return true;
            }

            return false;
        }


        public bool RemoveEvent(string args)
        {
            if (string.IsNullOrEmpty(args)) return false;
            if (HasEvent(args))
            {
                m_MsgDic.Remove(args);
                return true;
            }

            return false;
        }

        public bool HasEvent(string args)
        {
            return m_MsgDic.ContainsKey(args);
        }


        public bool MsgProcess(string args)
        {
            if (string.IsNullOrEmpty(args))
            {
                Debug.LogWarningFormat("发放奖励【{0}】失败.", "null or empty");
                return false;
            }

            UnityAction<string> msgProcessEvent = null;
            if (m_MsgDic.TryGetValue(args, out msgProcessEvent))
            {
                if (GameEntry.Ad.SimulatedRewards)
                {
#if UNITY_EDITOR||UNITY_EDITOR_64||UNITY_EDITOR_OSX
                    StartCoroutine(IeReward(args));
#else
                    msgProcessEvent(args);
#endif
                }
                else
                {
                    msgProcessEvent(args);
                }

                return true;
            }
            // StartCoroutine(IeReward(args));

            Debug.LogWarningFormat("发放奖励{0}失败.", args);
            return false;
        }

        IEnumerator IeReward(string args)
        {
            Debug.LogWarningFormat("UnityEditor，等待3秒后发模拟送奖励，参数:{0}.", args);
            yield return new WaitForSeconds(3);
            UnityAction<string> msgProcessEvent = null;
            if (m_MsgDic.TryGetValue(args, out msgProcessEvent))
            {
                msgProcessEvent(args);
            }
        }
    }
}