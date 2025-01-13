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
		InvokeRepeating("CheckForNotifications", 0f, 5f);
		yield return new WaitForSeconds(2f);
	}

	public virtual IEnumerator OnLevelWasLoaded(int level)
	{
		Debug.Log("LEVEL WAS LOADED!");
		yield return new WaitForSeconds(0.01f);
		CheckForNotifications();
	}

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

	public virtual void FixedUpdate()
	{
	}

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

	public virtual void Add(int completionTimeStamp, int type, int num)
	{
		Debug.Log("NotificationQueue.Add(" + completionTimeStamp + ", " + type + ", " + num + ")");
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

	public virtual void RemovePersistedNotifications()
	{
		PlayerPrefsX.SetIntArray(PREF_TIME, new int[1]);
		PlayerPrefsX.SetIntArray(PREF_TYPE, new int[1]);
		PlayerPrefsX.SetIntArray(PREF_NUM, new int[1]);
	}

	private List<Notification> GetPersistedNotifications()
	{
		List<Notification> list = new List<Notification>();
		int[] intArray = PlayerPrefsX.GetIntArray(PREF_TIME);
		int[] intArray2 = PlayerPrefsX.GetIntArray(PREF_TYPE);
		int[] intArray3 = PlayerPrefsX.GetIntArray(PREF_NUM);
		Debug.Log("GET NOTIFICATION QUEUE (length: " + intArray.Length + " )");
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
					Debug.Log(string.Concat("n: ", notification, " type: ", notification.type, " num: ", notification.num, " time: ", notification.time));
				}
				catch (Exception ex)
				{
					Debug.Log("Error loading notification: " + ex.ToString());
				}
			}
		}
		return list;
	}

	private void StoreTimeStamp(int ts)
	{
		int[] intArray = PlayerPrefsX.GetIntArray(PREF_TIME);
		int[] array = new int[intArray.Length + 1];
		for (int i = 0; i < intArray.Length; i++)
		{
			array[i] = intArray[i];
		}
		array[array.Length - 1] = ts;
		PlayerPrefsX.SetIntArray(PREF_TIME, array);
	}

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
