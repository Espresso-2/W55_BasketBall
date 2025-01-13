using System;
using UnityEngine;

[Serializable]
public class Music : MonoBehaviour
{
	public AudioClip soundMusic1;

	private bool musicIsPlaying;

	private AudioSource audioS;

	private float originalVolume;

	private bool isFadingOut;

	public virtual void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.transform.gameObject);
		audioS = GetComponent<AudioSource>();
		originalVolume = audioS.volume;
		audioS.clip = soundMusic1;
		musicIsPlaying = false;
	}

	public virtual void Start()
	{
		StartMusic();
	}

	public virtual void StartMusic()
	{
		if (!musicIsPlaying && PlayerPrefs.GetInt(MuteButton.MUSIC_OFF_PREF_KEY) != 1)
		{
			audioS.volume = originalVolume;
			audioS.Play();
			musicIsPlaying = true;
		}
	}

	public virtual void StopMusic()
	{
		audioS.Stop();
		musicIsPlaying = false;
		isFadingOut = false;
	}

	public virtual void FadeOutMusic()
	{
		isFadingOut = true;
	}

	public virtual bool IsMusicPlaying()
	{
		return musicIsPlaying;
	}

	public virtual void Update()
	{
		if (isFadingOut)
		{
			float num = audioS.volume - 0.0025f * Time.deltaTime;
			if (num > 0f)
			{
				audioS.volume = num;
			}
			else
			{
				StopMusic();
			}
		}
	}
}
