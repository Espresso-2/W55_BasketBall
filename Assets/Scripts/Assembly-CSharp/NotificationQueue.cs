using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class NotificationQueue : MonoBehaviour
{
    public int currentTabNum;

    private static string PREF_TIME;

    private static string PREF_TYPE;

    private static string PREF_NUM;

    private List<Notification> notifications;

    private SessionVars sessionVars;

    public NotificationQueue()
    {
        currentTabNum = -1;
    }

    static NotificationQueue()
    {
        PREF_TIME = "NOTIFY_QUEUE_TIME";
        PREF_TYPE = "NOTIFY_QUEUE_TYPE";
        PREF_NUM = "NOTIFY_QUEUE_NUM";
    }

    public virtual void Awake()
    {
        UnityEngine.Object.DontDestroyOnLoad(this);
    }

    public virtual IEnumerator Start()
    {
        if (GameObject.FindGameObjectsWithTag("NotificationQueue").Length > 1)
        {
            UnityEngine.Object.Destroy(base.gameObject);
        }
        sessionVars = SessionVars.GetInstance();
        notifications = GetPersistedNotifications();
        //每5秒通知一下队列
        InvokeRepeating(nameof(CheckForNotifications), 0f, 5f);
        yield return new WaitForSeconds(2f);
    }
    /// <summary>
    /// 场景加载过后检查通知队列
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    public virtual IEnumerator OnLevelWasLoaded(int level)
    {
        Debug.Log("LEVEL WAS LOADED!");
        yield return new WaitForSeconds(0.01f);
        CheckForNotifications();
    }
    /// <summary>
    /// 检查通知队列
    /// </summary>
    public virtual void CheckForNotifications()
    {
        if (Application.loadedLevelName != "MainUI")
        {
            return;
        }
        if (notifications == null)
        {
            notifications = GetPersistedNotifications();
        }
        GameObject gameObject = null;
        if (sessionVars == null)
        {
            return;
        }
        int currentTimestamp = sessionVars.currentTimestamp;
        for (int i = 0; i < notifications.Count; i++)
        {
            Notification notification = notifications[i];
            if (currentTimestamp < notification.time)
            {
                continue;
            }
            if (gameObject == null)
            {
                gameObject = GameObject.Find("NotificationsPanel");
            }
            if (!(gameObject != null))
            {
                continue;
            }
            NotificationsPanel notificationsPanel = (NotificationsPanel)gameObject.GetComponent(typeof(NotificationsPanel));
            if (notification.type == (float)Notification.UPGRADE_FINISHED)
            {
                notificationsPanel.ShowBoxNotification("PlayerUpgrade", notification.num);
                notifications.RemoveAt(i);
            }
            else if (notification.type == (float)Notification.TAB_HOME)
            {
                if (currentTabNum == 0)
                {
                    notifications.RemoveAt(i);
                    notificationsPanel.ShowTabNotification((int)notification.type, false);
                }
                else
                {
                    notificationsPanel.ShowTabNotification((int)notification.type, true);
                }
            }
            else if (notification.type == (float)Notification.TAB_PLAYERS)
            {
                if (currentTabNum == 1)
                {
                    notifications.RemoveAt(i);
                    notificationsPanel.ShowTabNotification((int)notification.type, false);
                }
                else
                {
                    notificationsPanel.ShowTabNotification((int)notification.type, true);
                }
            }
            else if (notification.type == (float)Notification.TAB_STORE)
            {
                if (currentTabNum == 2 && PlayerPrefs.GetInt(Supplies.COLLECTED_FREE_KEY + Supplies.DRINK) != 0)
                {
                    notifications.RemoveAt(i);
                    notificationsPanel.ShowTabNotification((int)notification.type, false);
                }
                else
                {
                    notificationsPanel.ShowTabNotification((int)notification.type, true);
                }
            }
            else if (notification.type == (float)Notification.TAB_2_PLAYER)
            {
                if (currentTabNum == 4)
                {
                    notifications.RemoveAt(i);
                    notificationsPanel.ShowTabNotification((int)notification.type, false);
                }
                else
                {
                    notificationsPanel.ShowTabNotification((int)notification.type, true);
                }
            }
            else if (notification.type == (float)Notification.TAB_TOUR)
            {
                if (currentTabNum == 5)
                {
                    notifications.RemoveAt(i);
                    notificationsPanel.ShowTabNotification((int)notification.type, false);
                }
                else
                {
                    notificationsPanel.ShowTabNotification((int)notification.type, true);
                }
            }
        }
        PersistNotifications();
    }

    /// <summary>
    /// 移除通知
    /// </summary>
    /// <param name="type"></param>
    /// <param name="num"></param>
    public virtual void RemoveNotification(int type, int num)
    {
        for (int i = 0; i < notifications.Count; i++)
        {
            Notification notification = notifications[i];
            if (notification.type == (float)type && notification.num == num)
            {
                notifications.RemoveAt(i);
            }
        }
        PersistNotifications();
    }

    /// <summary>
    /// 添加新的通知
    /// </summary>
    /// <param name="completionTimeStamp"></param>
    /// <param name="type"></param>
    /// <param name="num"></param>
    public virtual void Add(int completionTimeStamp, int type, int num)
    {
        Debug.Log("通知列表.Add(" + completionTimeStamp + ", " + type + ", " + num + ")");
        Notification notification = new Notification();
        notification.time = completionTimeStamp;
        notification.type = type;
        notification.num = num;
        if (notifications == null)
        {
            notifications = GetPersistedNotifications();
        }
        notifications.Add(notification);
    }

    /// <summary>
    /// 持久化通知
    /// </summary>
    private void PersistNotifications()
    {
        RemovePersistedNotifications();
        for (int i = 0; i < notifications.Count; i++)
        {
            Notification notification = notifications[i];
            StoreTimeStamp(notification.time);
            StoreType((int)notification.type);
            StoreNum(notification.num);
        }
    }

    /// <summary>
    /// 移除存档通知
    /// </summary>
    public virtual void RemovePersistedNotifications()
    {
        PlayerPrefsX.SetIntArray(PREF_TIME, new int[1]);
        PlayerPrefsX.SetIntArray(PREF_TYPE, new int[1]);
        PlayerPrefsX.SetIntArray(PREF_NUM, new int[1]);
    }

    /// <summary>
    /// 得到存档通知
    /// </summary>
    /// <returns></returns>
    private List<Notification> GetPersistedNotifications()
    {
        List<Notification> list = new List<Notification>();
        int[] intArray = PlayerPrefsX.GetIntArray(PREF_TIME);
        int[] intArray2 = PlayerPrefsX.GetIntArray(PREF_TYPE);
        int[] intArray3 = PlayerPrefsX.GetIntArray(PREF_NUM);
        Debug.Log("获得通知列表 (长度: " + intArray.Length + " )");
        for (int i = 0; i < intArray.Length; i++)
        {
            if (intArray[i] != 0)
            {
                try
                {
                    Notification notification = new Notification();
                    notification.time = intArray[i];
                    notification.type = intArray2[i];
                    notification.num = intArray3[i];
                    list.Add(notification);
                    Debug.Log(string.Concat("名字: ", notification, " 类型: ", notification.type, " 数量: ", notification.num, " 时间: ", notification.time));
                }
                catch (Exception ex)
                {
                    Debug.Log("错误的加载通知: " + ex.ToString());
                }
            }
        }
        return list;
    }

    /// <summary>
    /// 商店时间戳
    /// </summary>
    /// <param name="ts"></param>
    private void StoreTimeStamp(int ts)
    {
        int[] intArray = PlayerPrefsX.GetIntArray(PREF_TIME);
        int[] array = new int[intArray.Length + 1];
        for (int i = 0; i < intArray.Length; i++)
        {
            array[i] = intArray[i];
        }
        array[^1] = ts;
        PlayerPrefsX.SetIntArray(PREF_TIME, array);
    }

    /// <summary>
    /// 商店类型
    /// </summary>
    /// <param name="type"></param>
    private void StoreType(int type)
    {
        int[] intArray = PlayerPrefsX.GetIntArray(PREF_TYPE);
        int[] array = new int[intArray.Length + 1];
        for (int i = 0; i < intArray.Length; i++)
        {
            array[i] = intArray[i];
        }
        array[array.Length - 1] = type;
        PlayerPrefsX.SetIntArray(PREF_TYPE, array);
    }

    /// <summary>
    /// 商店数量
    /// </summary>
    /// <param name="num"></param>
    private void StoreNum(int num)
    {
        int[] intArray = PlayerPrefsX.GetIntArray(PREF_NUM);
        int[] array = new int[intArray.Length + 1];
        for (int i = 0; i < intArray.Length; i++)
        {
            array[i] = intArray[i];
        }
        array[array.Length - 1] = num;
        PlayerPrefsX.SetIntArray(PREF_NUM, array);
    }
}