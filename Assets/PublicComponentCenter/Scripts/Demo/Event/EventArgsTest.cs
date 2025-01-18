//------------------------------------------------------------
// Copyright © 2020-2021 Lefend. All rights reserved.
//------------------------------------------------------------

namespace PublicComponentCenter
{
    /// <summary>
    /// 测试用事件
    /// </summary>
    public class EventArgsTest : GlobalEventArgs
    {
        /// <summary>
        /// 注意：这个字段是每个事件必有块
        /// </summary>
        public static readonly int EventId = typeof(EventArgsTest).GetHashCode();

        /// <summary>
        /// 自定义字段
        /// </summary>
        public string m_Name;

        public override int Id
        {
            get { return EventId; }
        }

        public override void Clear()
        {
            m_Name = string.Empty;
        }

        /// <summary>
        /// 事件填充
        /// </summary>
        public void Fill(string name)
        {
            m_Name = name;
        }
    }
}