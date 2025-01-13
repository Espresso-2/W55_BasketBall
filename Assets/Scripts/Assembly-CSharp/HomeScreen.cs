using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class HomeScreen : MonoBehaviour
{
	public Text xpLevelNum;

	public PlayerGenderTab femaleGenderTab;

	public Timer dailyBagTimer;

	public GameObject dailyBagReadyText;

	private SessionVars sessionVars;

	public virtual void Awake()
	{
		sessionVars = SessionVars.GetInstance();
	}

	public virtual void TimerComplete(int bagNum)
	{
		UpdateDailyBagButton();
	}

	public virtual void OnEnable()
	{
		xpLevelNum.text = string.Empty + Currency.GetCurrentXpLevel();
		if (sessionVars.currentTournament != null && sessionVars.currentTournament.isFemale)
		{
			femaleGenderTab.SetGender();
		}
		UpdateDailyBagButton();
	}

	private void UpdateDailyBagButton()
	{
		int num = DealBag.SecondsUntilDailyBagAvailable();
		if (num <= 2)
		{
			dailyBagReadyText.SetActive(true);
			dailyBagTimer.gameObject.SetActive(false);
		}
		else
		{
			dailyBagReadyText.SetActive(false);
			dailyBagTimer.gameObject.SetActive(true);
			dailyBagTimer.SetSecondsToWait(num, 2);
		}
	}
}
