using System;
using UnityEngine;

[Serializable]
public class PracticePlayButton : MonoBehaviour
{
	public bool female;

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
	}

	public virtual void OnClick()
	{
		gameSounds.Play_one_dribble();
		sessionVars.twoPlayerMode = false;
		sessionVars.goToPractice = true;
		sessionVars.showingFemales = female;
		if (music != null)
		{
			music.FadeOutMusic();
		}
		GoToScene("GamePlay");
	}

	public virtual void Update()
	{
	}

	private void GoToScene(string scene)
	{
		Time.timeScale = 1f;
		Application.LoadLevel(scene);
	}
}
