using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class GameNoise : MonoBehaviour
{
	public AudioSource backgroundSqueaks;

	public AudioSource backgroundCrowd;

	private float originalCrowdVolume;

	private bool isFadingCrowdIn;

	public virtual void Start()
	{
		backgroundSqueaks.loop = true;
		backgroundCrowd.loop = true;
		originalCrowdVolume = backgroundCrowd.volume;
	}

	public virtual IEnumerator PlayBackgroundCrowd()
	{
		yield return new WaitForSeconds(0.5f);
		backgroundCrowd.volume = 0f;
		isFadingCrowdIn = true;
		backgroundCrowd.Play();
	}

	public virtual void PauseBackgroundCrowd()
	{
		backgroundCrowd.Pause();
	}

	public virtual void ResumeBackgroundCrowd()
	{
		backgroundCrowd.Play();
	}

	public virtual IEnumerator PlayBgSqueaks()
	{
		yield return new WaitForSeconds(3f);
		backgroundSqueaks.Play();
	}

	public virtual void PauseBgSqueaks()
	{
		backgroundSqueaks.Pause();
	}

	public virtual void Update()
	{
		if (isFadingCrowdIn)
		{
			float num = backgroundCrowd.volume + 0.0075f * Time.deltaTime;
			if (num < originalCrowdVolume)
			{
				backgroundCrowd.volume = num;
				return;
			}
			backgroundCrowd.volume = originalCrowdVolume;
			isFadingCrowdIn = false;
		}
	}
}
