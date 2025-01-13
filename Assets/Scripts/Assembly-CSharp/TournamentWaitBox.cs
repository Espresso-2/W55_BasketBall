using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class TournamentWaitBox : MonoBehaviour
{
	public PlayButton playButton;

	public Text timeText;

	private int waitSeconds;

	private int secondsWaited;

	private GameSounds gameSounds;

	public bool isDevTest;

	public virtual void Start()
	{
		gameSounds = GameSounds.GetInstance();
		if (isDevTest)
		{
			SessionVars instance = SessionVars.GetInstance();
			int num = instance.currentTimestamp - PlayerPrefs.GetInt("TEST_WAIT_TIMESTAMP");
			SetSecondsToWait(90 - num);
		}
	}

	public virtual void ResetDevTestTimestamp()
	{
		SessionVars instance = SessionVars.GetInstance();
		PlayerPrefsHelper.SetInt("TEST_WAIT_TIMESTAMP", instance.currentTimestamp);
		SetSecondsToWait(90);
	}

	public virtual void SetSecondsToWait(int seconds)
	{
		waitSeconds = seconds;
		secondsWaited = 0;
		timeText.text = DoubleTapUtils.GetTimeFromSeconds(waitSeconds);
	}

	public virtual void Update()
	{
	}

	public virtual void OnEnable()
	{
		InvokeRepeating("CountSeconds", 1f, 1f);
	}

	public virtual void OnDisable()
	{
		CancelInvoke("CountSeconds");
	}

	public virtual void CountSeconds()
	{
		secondsWaited++;
		int num = waitSeconds - secondsWaited;
		if (num <= 0)
		{
			gameSounds.Play_chime_shimmer();
			base.gameObject.SetActive(false);
			if (playButton != null)
			{
				playButton.SetToPlay();
				playButton.EnableButton();
			}
		}
		else
		{
			timeText.text = DoubleTapUtils.GetTimeFromSeconds(num);
		}
	}
}
