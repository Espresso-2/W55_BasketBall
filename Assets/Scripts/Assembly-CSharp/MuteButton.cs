using System;
using UnityEngine;

[Serializable]
public class MuteButton : MonoBehaviour
{
	public bool isMusic;

	public bool isCommentator;

	/*public bool isVibrate;*/

	public GameObject muteIcon;

	public GameObject muteOffIcon;

	public bool isSettingsScreen;

	public GameObject musicButton;

	public GameObject commentatorButton;

	public VoiceOvers voiceOvers;

	/*public GameVibrations gameVibrations;
	*/

	public static string SOUND_OFF_PREF_KEY;

	public static string MUSIC_OFF_PREF_KEY;

	public static string COMMENTATOR_OFF_PREF_KEY;

	public static string VIBRATE_OFF_PREF_KEY;

	public static float AUDIO_VOLUME;

	private Music music;

	static MuteButton()
	{
		SOUND_OFF_PREF_KEY = "SOUND_OFF";
		MUSIC_OFF_PREF_KEY = "MUSIC_OFF";
		COMMENTATOR_OFF_PREF_KEY = "COMMENTATOR_OFF";
		VIBRATE_OFF_PREF_KEY = "VIBRATE_OFF";
		AUDIO_VOLUME = 10f;
	}

	public virtual void Start()
	{
		if (isMusic)
		{
			GameObject gameObject = GameObject.Find("Music");
			if (gameObject != null)
			{
				music = (Music)gameObject.GetComponent(typeof(Music));
			}
		}
		if (CheckIfMuted())
		{
			muteIcon.SetActive(true);
			muteOffIcon.SetActive(false);
			if (!isMusic && !isCommentator )
			{
				commentatorButton.SetActive(false);
				if (isSettingsScreen)
				{
					musicButton.SetActive(false);
				}
			}
		}
		else
		{
			muteIcon.SetActive(false);
			muteOffIcon.SetActive(true);
		}
	}

	private bool CheckIfMuted()
	{
		bool result = false;
		if (isMusic)
		{
			if (PlayerPrefs.GetInt(MUSIC_OFF_PREF_KEY) == 1)
			{
				result = true;
			}
		}
		else if (isCommentator)
		{
			if (PlayerPrefs.GetInt(COMMENTATOR_OFF_PREF_KEY) == 1)
			{
				result = true;
			}
		}
		/*else if (isVibrate)
		{
			if (PlayerPrefs.GetInt(VIBRATE_OFF_PREF_KEY) == 1)
			{
				result = true;
			}
		}*/
		else if (AudioListener.volume == 0f)
		{
			result = true;
		}
		return result;
	}

	public virtual void OnClick()
	{
		if (CheckIfMuted())
		{
			muteIcon.SetActive(false);
			muteOffIcon.SetActive(true);
			Debug.Log("TURN BACK ON");
			if (isMusic)
			{
				if (music != null)
				{
					PlayerPrefsHelper.SetInt(MUSIC_OFF_PREF_KEY, 0);
					music.StartMusic();
				}
				return;
			}
			if (isCommentator)
			{
				PlayerPrefsHelper.SetInt(COMMENTATOR_OFF_PREF_KEY, 0);
				if (voiceOvers != null)
				{
					voiceOvers.UnMute();
				}
				return;
			}
			/*if (isVibrate)
			{
				PlayerPrefsHelper.SetInt(VIBRATE_OFF_PREF_KEY, 0);
				/*if (gameVibrations != null)
				{
					gameVibrations.UnMute();
				}#1#
				return;
			}*/
			AudioListener.volume = AUDIO_VOLUME;
			AudioListener.pause = false;
			PlayerPrefsHelper.SetInt(SOUND_OFF_PREF_KEY, 0);
			if (!isMusic && !isCommentator)
			{
				if (commentatorButton != null)
				{
					commentatorButton.SetActive(true);
				}
				if (isSettingsScreen)
				{
					musicButton.SetActive(true);
				}
			}
			return;
		}
		muteIcon.SetActive(true);
		muteOffIcon.SetActive(false);
		Debug.Log("TURN OFF");
		if (isMusic)
		{
			if (music != null)
			{
				PlayerPrefsHelper.SetInt(MUSIC_OFF_PREF_KEY, 1);
				music.StopMusic();
			}
			return;
		}
		if (isCommentator)
		{
			PlayerPrefsHelper.SetInt(COMMENTATOR_OFF_PREF_KEY, 1);
			if (voiceOvers != null)
			{
				voiceOvers.Mute();
			}
			return;
		}
		/*if (isVibrate)
		{
			PlayerPrefsHelper.SetInt(VIBRATE_OFF_PREF_KEY, 1);
			/*if (gameVibrations != null)
			{
				gameVibrations.Mute();
			}#1#
			return;
		}*/
		AudioListener.volume = 0f;
		PlayerPrefsHelper.SetInt(SOUND_OFF_PREF_KEY, 1);
		if (!isMusic && !isCommentator)
		{
			if (commentatorButton != null)
			{
				commentatorButton.SetActive(false);
			}
			if (isSettingsScreen)
			{
				musicButton.SetActive(false);
			}
		}
	}
}
