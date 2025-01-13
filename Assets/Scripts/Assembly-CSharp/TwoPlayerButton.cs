using System;
using UnityEngine;

[Serializable]
public class TwoPlayerButton : MonoBehaviour
{
	private SessionVars sessionVars;

	private GameSounds gameSounds;

	private Music music;

	public virtual void Start()
	{
		sessionVars = SessionVars.GetInstance();
		gameSounds = GameSounds.GetInstance();
		GameObject gameObject = GameObject.Find("Music");
		if (gameObject != null)
		{
			music = (Music)gameObject.GetComponent(typeof(Music));
		}
		if (music != null && !music.IsMusicPlaying())
		{
			music.StartMusic();
		}
	}

	public virtual void OnClick()
	{
		gameSounds.Play_one_dribble();
		sessionVars.twoPlayerMode = true;
		sessionVars.goToPractice = false;
		if (music != null)
		{
			music.FadeOutMusic();
		}
		GoToScene("GamePlay");
	}

	private void GoToScene(string scene)
	{
		Time.timeScale = 1f;
		Application.LoadLevel(scene);
	}
}
