using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class DoubleRewardsBox : MonoBehaviour
{
	public GameObject bubble;

	public GameObject bubbleWithTimer;

	public GameObject claimButton;

	public GameObject dontDoubleButton;

	public GameObject doubleRewardsButton;

	public Timer timer;

	public virtual void Start()
	{
	}

	public virtual void OnEnable()
	{
		claimButton.SetActive(false);
		int num = PlayerPrefs.GetInt("DOUBLEREWARD_BUBBLE");
		if (PlayerPrefs.GetInt("NUM_PURCHASES") > 0 && PlayerPrefs.GetInt("IS_FRADULENT_USER") != 1)
		{
			num = 0;
		}
		int num2 = 0;
		switch (num)
		{
		case 2:
			num2 = 5;
			break;
		case 3:
			num2 = 7;
			break;
		case 4:
			num2 = 10;
			break;
		}
		if (num2 > 0)
		{
			dontDoubleButton.SetActive(false);
			timer.SetSecondsToWait(num2, 0);
		}
		else
		{
			dontDoubleButton.SetActive(true);
		}
		StartCoroutine(ShowBubbles(num2, num));
	}

	private IEnumerator ShowBubbles(int seconds, int splitTestNum)
	{
		bubble.SetActive(false);
		bubbleWithTimer.SetActive(false);
		yield return new WaitForSeconds(0.15f);
		if (splitTestNum == 1)
		{
			bubble.SetActive(true);
		}
		else if (seconds > 0)
		{
			bubbleWithTimer.SetActive(true);
		}
	}

	public virtual void TimerComplete()
	{
		dontDoubleButton.SetActive(false);
		doubleRewardsButton.SetActive(false);
		bubbleWithTimer.SetActive(false);
		claimButton.SetActive(true);
	}
}
