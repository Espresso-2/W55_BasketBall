using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class TabChanger : MonoBehaviour
{
	public GameObject[] tabs;

	public GameObject[] screens;

	public Image[] tabButtonImages;

	public Color tabButtonImageSelectedColor;

	public Color tabButtonImageUnselectedColor;

	public TopNavBar topNavBar;

	public GameObject playButton;

	public GameObject[] backgrounds;

	public GameObject topNavSettingsButton;

	public GameObject topNavBackButton;

	public GameObject bottomNavBarContent;

	public GameObject[] hintArrows;

	public static tabEnum currentTabNum;

	public static screenEnum currentScreenNum;

	public static backAction currentBackAction;

	public static int subSection;

	public static float bottomNavSlideInDelay;

	public GameObject[] objectsToHideWhenNoNav;

	private SessionVars sessionVars;

	private NotificationQueue notificationQueue;

	static TabChanger()
	{
		currentTabNum = tabEnum.Home;
		currentScreenNum = screenEnum.NotSet;
		currentBackAction = backAction.None;
	}

	public virtual void Start()
	{
		sessionVars = SessionVars.GetInstance();
		notificationQueue = (NotificationQueue)GameObject.Find("NotificationQueue").GetComponent(typeof(NotificationQueue));
		SetToTab(currentTabNum);
	}

	public virtual void FixedUpdate()
	{
		if (Input.GetKeyDown(KeyCode.Escape) && currentScreenNum == screenEnum.NotSet && currentTabNum != tabEnum.Tour)
		{
			SetToTab(tabEnum.Tour);
		}
	}

	public virtual void SetToTab(tabEnum num)
	{
		StartCoroutine(SetToTab(num, false));
	}

	public virtual IEnumerator SetToTab(tabEnum num, bool slideInBottomNav)
	{
		currentTabNum = num;
		currentScreenNum = screenEnum.NotSet;
		Time.timeScale = 1f;
		for (int s = 0; s < screens.Length; s++)
		{
			if (screens[s] != null)
			{
				screens[s].SetActive(false);
			}
		}
		topNavSettingsButton.SetActive(true);
		topNavBackButton.SetActive(currentBackAction != backAction.None);
		topNavBar.HideTitle();
		HideHintArrows();
		notificationQueue.currentTabNum = (int)num;
		notificationQueue.CheckForNotifications();
		for (int i = 0; i < tabs.Length; i++)
		{
			if (i == (int)num)
			{
				tabs[i].SetActive(true);
				tabButtonImages[i].color = tabButtonImageSelectedColor;
			}
			else
			{
				tabs[i].SetActive(false);
				tabButtonImages[i].color = tabButtonImageUnselectedColor;
			}
		}
		playButton.SetActive(num == tabEnum.Tour);
		backgrounds[0].SetActive(num == tabEnum.Home || num == tabEnum.Players || num == tabEnum.Supplies);
		backgrounds[1].SetActive(num == tabEnum.Deals || num == tabEnum.TwoPlayer);
		/*switch (num)
		{
		case tabEnum.Home:
			AdMediation.ShowTjpHomeScreen();
			break;
		case tabEnum.Players:
			AdMediation.ShowTjpPlayersScreen();
			break;
		case tabEnum.Supplies:
			AdMediation.ShowTjpStoreScreen();
			break;
		case tabEnum.Deals:
			AdMediation.ShowTjpDealsScreen();
			break;
		case tabEnum.TwoPlayer:
			AdMediation.ShowTjpTwoPlayerScreen();
			break;
		}*/
		if (bottomNavSlideInDelay > 0f)
		{
			bottomNavBarContent.SetActive(false);
			GameObject[] array = objectsToHideWhenNoNav;
			foreach (GameObject gameObject in array)
			{
				gameObject.SetActive(false);
			}
			yield return new WaitForSeconds(bottomNavSlideInDelay);
			bottomNavBarContent.SetActive(true);
			yield return new WaitForSeconds(0.5f);
			GameObject[] array2 = objectsToHideWhenNoNav;
			foreach (GameObject gameObject2 in array2)
			{
				gameObject2.SetActive(true);
			}
			bottomNavSlideInDelay = 0f;
		}
		else if (!bottomNavBarContent.activeInHierarchy)
		{
			bottomNavBarContent.SetActive(true);
		}
	}

	public virtual void SetToScreen(screenEnum num)
	{
		//7
		currentScreenNum = num;
		Time.timeScale = 1f;
		for (int i = 0; i < tabs.Length; i++)
		{
			tabs[i].SetActive(false);
		}
		bottomNavSlideInDelay = 0f;
		bottomNavBarContent.SetActive(false);
		topNavSettingsButton.SetActive(false);
		topNavBackButton.SetActive(currentBackAction != backAction.None);
		HideHintArrows();
		notificationQueue.currentTabNum = -1;
		notificationQueue.CheckForNotifications();
		for (int j = 0; j < screens.Length; j++)
		{
			if (screens[j] != null)
			{
				if (j == (int)num)
				{
					screens[j].SetActive(true);
				}
				else
				{
					screens[j].SetActive(false);
				}
			}
		}
		if (num == screenEnum.OpenBag)
		{
			backgrounds[0].SetActive(false);
			backgrounds[1].SetActive(true);
		}
		else
		{
			backgrounds[0].SetActive(true);
			backgrounds[1].SetActive(false);
		}
	}

	private void HideHintArrows()
	{
		for (int i = 0; i < hintArrows.Length; i++)
		{
			hintArrows[i].SetActive(false);
		}
	}
}
