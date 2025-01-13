using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Timer : MonoBehaviour
{
	public GameObject notifyObject;

	public Text timeText;

	private GameSounds gameSounds;

	private int waitSeconds;

	private int intValPassed;

	private int secondsWaited;

	public virtual void Start()
	{
		if (timeText == null)
		{
			timeText = (Text)base.gameObject.GetComponent(typeof(Text));
		}
		gameSounds = GameSounds.GetInstance();
	}

	public virtual void SetSecondsToWait(int seconds, int intVal)
	{
		waitSeconds = seconds;
		intValPassed = intVal;
		secondsWaited = 0;
		if (timeText == null)
		{
			timeText = (Text)base.gameObject.GetComponent(typeof(Text));
		}
		timeText.text = DoubleTapUtils.GetTimeFromSeconds(waitSeconds);
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
		if (waitSeconds == 0)
		{
			return;
		}
		secondsWaited++;
		int num = waitSeconds - secondsWaited;
		if (num <= 0)
		{
			if (notifyObject != null)
			{
				notifyObject.SendMessage("TimerComplete", intValPassed);
			}
		}
		else
		{
			timeText.text = DoubleTapUtils.GetTimeFromSeconds(num);
		}
	}
}
