using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Fps : MonoBehaviour
{
	public Text text;

	public float updateInterval;

	private float accum;

	private int frames;

	private float timeleft;

	private int totIntervals;

	private float totCumFPSs;

	private static float lastFpsTotAverage;

	public Fps()
	{
		updateInterval = 1f;
	}

	public virtual void Start()
	{
		timeleft = updateInterval;
	}

	public virtual float GetFpsTotAverage()
	{
		return lastFpsTotAverage = Mathf.Round(totCumFPSs / (float)totIntervals);
	}

	public virtual void Update()
	{
		timeleft -= Time.deltaTime;
		accum += Mathf.Round(Time.timeScale / Time.deltaTime);
		frames++;
		if (timeleft <= 0f)
		{
			if (!float.IsNaN(accum) && accum > 1f && frames > 1)
			{
				int num = (int)Mathf.Round(accum / (float)frames);
				totIntervals++;
				totCumFPSs += num;
				text.text = string.Empty + num + " " + GetFpsTotAverage();
			}
			timeleft = updateInterval;
			accum = 0f;
			frames = 0;
		}
	}

	public static float GetFpsAverageForAnalytics()
	{
		float num = lastFpsTotAverage;
		if (num >= 60f)
		{
			num = 60f;
		}
		else if (num >= 55f)
		{
			num = 55f;
		}
		else if (num >= 50f)
		{
			num = 50f;
		}
		else if (num >= 40f)
		{
			num = 40f;
		}
		else if (num >= 30f)
		{
			num = 30f;
		}
		else if (num >= 20f)
		{
			num = 20f;
		}
		else if (num >= 10f)
		{
			num = 10f;
		}
		else if (num >= 5f)
		{
			num = 5f;
		}
		return num;
	}
}
