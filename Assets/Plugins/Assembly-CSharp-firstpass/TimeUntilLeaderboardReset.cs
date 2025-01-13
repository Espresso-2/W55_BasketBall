using System;
using UnityEngine;
using UnityEngine.UI;

public class TimeUntilLeaderboardReset : MonoBehaviour
{
	public GameObject objectToShowWhenLeaderboardIsOver;

	public GameObject playButton;

	public Text label;

	public string labelText;

	public string labelForZeroDaysText;

	public bool hideDaysIfZero;

	public Text days;

	public Text hours;

	public Text minutes;

	public Text hrMinSec;

	private float updateTime = 1f;

	private float updateTimer;

	private void OnEnable()
	{
		UpdateDisplay();
	}

	private void FixedUpdate()
	{
		updateTimer += Time.deltaTime;
		if (updateTimer >= updateTime)
		{
			updateTimer = 0f;
			UpdateDisplay();
		}
	}

	private void UpdateDisplay()
	{
		if (GetNextReset() != default(DateTime))
		{
			float num = -300f;
			TimeSpan timeSpan = GetNextReset().AddSeconds(num) - GetCurrentTime();
			if (timeSpan.TotalSeconds < 0.0)
			{
				if (objectToShowWhenLeaderboardIsOver != null)
				{
					objectToShowWhenLeaderboardIsOver.SetActive(true);
				}
				if (playButton != null && playButton.activeInHierarchy)
				{
					playButton.SendMessage("DisableButton");
				}
			}
			else if (objectToShowWhenLeaderboardIsOver != null)
			{
				objectToShowWhenLeaderboardIsOver.SetActive(false);
			}
			int num2 = timeSpan.Days;
			int h = timeSpan.Hours;
			int m = timeSpan.Minutes;
			int seconds = timeSpan.Seconds;
			SetText(days, (!hideDaysIfZero || num2 != 0) ? num2.ToString() : string.Empty);
			SetText(hours, h.ToString());
			SetText(minutes, m.ToString());
			SetText(hrMinSec, CreateHrMinSecText(num2, h, m, seconds));
			if (num2 > 0)
			{
				SetText(label, labelText);
			}
			else
			{
				SetText(label, labelForZeroDaysText);
			}
		}
		else
		{
			SetText(days, string.Empty);
			SetText(hours, string.Empty);
			SetText(minutes, string.Empty);
			SetText(hrMinSec, string.Empty);
			SetText(label, string.Empty);
		}
	}

	public void OnApplicationPause(bool pause)
	{
		if (!pause)
		{
			SetText(days, string.Empty);
			SetText(hours, string.Empty);
			SetText(minutes, string.Empty);
			SetText(hrMinSec, string.Empty);
		}
	}

	private void SetText(Text t, string s)
	{
		if (t != null)
		{
			t.text = s;
		}
	}

	private string CreateHrMinSecText(int d, int h, int m, int s)
	{
		string empty = string.Empty;
		empty += ((h < 10) ? ("0" + h) : h.ToString());
		empty += ":";
		empty += ((m < 10) ? ("0" + m) : m.ToString());
		empty += ":";
		return empty + ((s < 10) ? ("0" + s) : s.ToString());
	}

	private DateTime GetNextReset()
	{
		return PlayFabLeaderboard.leaderboardNextReset;
	}

	private DateTime GetCurrentTime()
	{
		return PlayFabManager.Instance().GetCurrentTime();
	}
}
