//------------------------------------------------------------
// Copyright © 2020-2021 Lefend. All rights reserved.
//------------------------------------------------------------

using UnityEngine;

namespace PublicComponentCenter
{
    /// <summary>
    /// 事件模块测试
    /// </summary>
    public class EventTestMain : MonoBehaviour
    {
        private void Start()
        {
            //订阅事件

            GameEntry.Event.Subscribe(EventArgsTest.EventId, EventTestMethod);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                EventArgsTest e = ReferencePool.Acquire<EventArgsTest>();
                e.Fill("EventTest");
                //派发事件
                GameEntry.Event.Fire(this, e);
            }
        }

        /// <summary>
        /// 事件处理方法
        /// </summary>
        private void EventTestMethod(object sender, GlobalEventArgs e)
        {
            EventArgsTest args = e as EventArgsTest;
            if (args != null)
            {
                Debug.Log(args.m_Name);
            }
            else
            {
                Debug.Log("Args is null");
            }
        }
    }
}