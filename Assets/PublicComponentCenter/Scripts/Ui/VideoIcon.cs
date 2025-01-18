using UnityEngine;

namespace PublicComponentCenter
{
    /// <summary>
    /// 激励视频角标
    /// </summary>
    public class VideoIcon : MonoBehaviour
    {
        void Awake()
        {
            gameObject.SetActive(GameEntry.Ad.RewardIconEnable);
        }
    }
}