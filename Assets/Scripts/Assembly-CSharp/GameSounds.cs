using System;
using UnityEngine;

[Serializable]
public class GameSounds : MonoBehaviour
{
	private static GameSounds instance;

	public AudioClip light_click;

	public AudioClip light_click_2;

	public AudioClip coin_glow;

	public AudioClip coin_glow_2;

	public AudioClip ended_chime;

	public AudioClip applause;

	public AudioClip pop;

	public AudioClip swoosh;

	public AudioClip tap;

	public AudioClip break_tackle;

	public AudioClip select;

	public AudioClip unselect;

	public AudioClip ball_dribble;

	public AudioClip bball_buzzer;

	public AudioClip rattling_hinge;

	public AudioClip pinball_beep;

	public AudioClip chime_shimmer;

	public AudioClip whistle_02;

	public AudioClip crowd_boo_01;

	public AudioClip crowd_long_cheer_01;

	public AudioClip quick_applause;

	public AudioClip air_pump;

	public AudioClip dunk;

	public AudioClip ascend_chime_low;

	public AudioClip ascend_chime_bright;

	public AudioClip ascend_chime_bright_2;

	public AudioClip one_dribble;

	public AudioClip three_dribbles;

	public AudioClip catch_ball;

	public AudioClip sniff_02;

	public AudioClip gulp;

	public AudioClip thump_on_floor;

	public AudioClip thump_on_floor_2;

	public AudioClip whoose_tennis_hit;

	public AudioClip whoose_tennis_hit_2;

	public AudioClip whoose_tennis_racket;

	public AudioClip slap_deeper;

	public AudioClip whoose_low;

	public AudioClip whoose_low_2;

	public AudioClip whoose_sharp_whip;

	public AudioClip chime_2_beeps;

	public AudioClip chime_2_beeps_2;

	public AudioClip trumpet_chime_2;

	public AudioClip trumpet_chime_3;

	public AudioClip tap_2;

	private AudioSource audioSource;

	private AudioClip lastClipPlayed;

	private float timeSinceLastPlay;

	public virtual void Awake()
	{
		if (GameObject.FindGameObjectsWithTag("GameSounds").Length > 1)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		UnityEngine.Object.DontDestroyOnLoad(base.transform.gameObject);
		instance = this;
	}

	public static GameSounds GetInstance()
	{
		return instance;
	}

	public virtual void Update()
	{
		timeSinceLastPlay += Time.deltaTime;
	}

	private void PlayAudioClip(AudioClip audioClip)
	{
		PlayAudioClip(audioClip, 1f);
	}

	private void PlayAudioClip(AudioClip audioClip, float volume)
	{
		if (audioSource == null)
		{
			audioSource = GetComponent<AudioSource>();
		}
		if (timeSinceLastPlay >= 0.15f || lastClipPlayed != audioClip || Time.timeScale == 0f)
		{
			audioSource.PlayOneShot(audioClip, volume);
			lastClipPlayed = audioClip;
			timeSinceLastPlay = 0f;
		}
	}

	public virtual void Play_light_click()
	{
		PlayAudioClip(light_click);
	}

	public virtual void Play_light_click_2()
	{
		PlayAudioClip(light_click_2, 1.95f);
	}

	public virtual void Play_coin_glow()
	{
		PlayAudioClip(coin_glow, 0.45f);
	}

	public virtual void Play_coin_glow_2()
	{
		PlayAudioClip(coin_glow_2, 0.45f);
	}

	public virtual void Play_ended_chime()
	{
		PlayAudioClip(ended_chime);
	}

	public virtual void Play_applause()
	{
		PlayAudioClip(applause, 0.65f);
	}

	public virtual void Play_pop()
	{
		PlayAudioClip(pop);
	}

	public virtual void Play_swoosh()
	{
		PlayAudioClip(swoosh);
	}

	public virtual void Play_tap()
	{
		PlayAudioClip(tap);
	}

	public virtual void Play_break_tackle()
	{
		PlayAudioClip(break_tackle);
	}

	public virtual void Play_select()
	{
		PlayAudioClip(select, 0.25f);
	}

	public virtual void Play_unselect()
	{
		PlayAudioClip(unselect, 0.23f);
	}

	public virtual void Play_ball_dribble()
	{
		PlayAudioClip(ball_dribble);
	}

	public virtual void Play_ball_dribble(float volume)
	{
		PlayAudioClip(ball_dribble, volume);
	}

	public virtual void Play_bball_buzzer()
	{
		PlayAudioClip(bball_buzzer, 0.35f);
	}

	public virtual void Play_rattling_hinge()
	{
		PlayAudioClip(rattling_hinge);
	}

	public virtual void Play_pinball_beep()
	{
		PlayAudioClip(pinball_beep);
	}

	public virtual void Play_chime_shimmer()
	{
		PlayAudioClip(chime_shimmer, 0.55f);
	}

	public virtual void Play_whistle_02()
	{
		PlayAudioClip(whistle_02);
	}

	public virtual void Play_crowd_boo_01()
	{
		PlayAudioClip(crowd_boo_01);
	}

	public virtual void Play_crowd_long_cheer_01()
	{
		PlayAudioClip(crowd_long_cheer_01, 0.75f);
	}

	public virtual void Play_quick_applause()
	{
		PlayAudioClip(quick_applause);
	}

	public virtual void Play_air_pump()
	{
		PlayAudioClip(air_pump);
	}

	public virtual void Play_dunk()
	{
		PlayAudioClip(dunk);
	}

	public virtual void Play_ascend_chime_low()
	{
		PlayAudioClip(ascend_chime_low, 0.5f);
	}

	public virtual void Play_ascend_chime_bright()
	{
		PlayAudioClip(ascend_chime_bright);
	}

	public virtual void Play_ascend_chime_bright_2()
	{
		PlayAudioClip(ascend_chime_bright_2, 0.25f);
	}

	public virtual void Play_ascend_chime_bright_2_loud()
	{
		PlayAudioClip(ascend_chime_bright_2, 0.75f);
	}

	public virtual void Play_one_dribble()
	{
		PlayAudioClip(one_dribble);
	}

	public virtual void Play_three_dribbles()
	{
		PlayAudioClip(three_dribbles);
	}

	public virtual void Play_catch_ball()
	{
		PlayAudioClip(catch_ball);
	}

	public virtual void Play_sniff_02()
	{
		PlayAudioClip(sniff_02);
	}

	public virtual void Play_gulp()
	{
		PlayAudioClip(gulp);
	}

	public virtual void Play_thump_on_floor()
	{
		PlayAudioClip(thump_on_floor);
	}

	public virtual void Play_thump_on_floor_2()
	{
		PlayAudioClip(thump_on_floor_2);
	}

	public virtual void Play_whoose_tennis_hit()
	{
		PlayAudioClip(whoose_tennis_hit, 0.55f);
	}

	public virtual void Play_whoose_tennis_hit_loud()
	{
		PlayAudioClip(whoose_tennis_hit, 1.55f);
	}

	public virtual void Play_whoose_tennis_hit_2()
	{
		PlayAudioClip(whoose_tennis_hit_2, 0.65f);
	}

	public virtual void Play_whoose_tennis_racket()
	{
		PlayAudioClip(whoose_tennis_racket);
	}

	public virtual void Play_slap_deeper()
	{
		PlayAudioClip(slap_deeper);
	}

	public virtual void Play_whoose_low()
	{
		PlayAudioClip(whoose_low, 0.5f);
	}

	public virtual void Play_whoose_low_2()
	{
		float volume = 1.5f;
		if (Application.isEditor)
		{
			volume = 0f;
		}
		PlayAudioClip(whoose_low_2, volume);
	}

	public virtual void Play_whoose_sharp_whip()
	{
		PlayAudioClip(whoose_sharp_whip, 0.75f);
	}

	public virtual void Play_whoose_sharp_whip_quiet()
	{
		PlayAudioClip(whoose_sharp_whip, 0.25f);
	}

	public virtual void Play_chime_2_beeps()
	{
		PlayAudioClip(chime_2_beeps, 0.75f);
	}

	public virtual void Play_chime_2_beeps_2()
	{
		PlayAudioClip(chime_2_beeps_2);
	}

	public virtual void Play_trumpet_chime_2()
	{
		PlayAudioClip(trumpet_chime_2, 0.65f);
	}

	public virtual void Play_trumpet_chime_3()
	{
		PlayAudioClip(trumpet_chime_3, 0.55f);
	}

	public virtual void Play_tap_2()
	{
		PlayAudioClip(tap_2, 0.75f);
	}

	public virtual void Play_BlockSound()
	{
		PlayAudioClip(catch_ball, 0.55f);
		PlayAudioClip(whoose_sharp_whip, 0.25f);
	}
}
