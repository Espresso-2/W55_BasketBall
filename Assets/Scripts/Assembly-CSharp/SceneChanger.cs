using System;
using UnityEngine;

[Serializable]
public class SceneChanger : MonoBehaviour
{
	public string scene;

	public int tabNum;

	public bool callWhenEscapeIsPressed;

	private GameSounds gameSounds;

	private SessionVars sessionVars;

	public SceneChanger()
	{
		tabNum = -1;
	}

	public virtual void Start()
	{
		gameSounds = GameSounds.GetInstance();
		sessionVars = SessionVars.GetInstance();
	}

	public virtual void FixedUpdate()
	{
		if (callWhenEscapeIsPressed && Input.GetKeyUp(KeyCode.Escape))
		{
			OnClick();
		}
	}

	public virtual void OnClick()
	{
		GoToScene(scene);
	}

	public virtual void GoToScene(string scene)
	{
		Time.timeScale = 1f;
		gameSounds.Play_one_dribble();
		if (scene == "MainUI" && PlayerPrefs.GetString("TEAM_NAME") == string.Empty)
		{
			Application.LoadLevel("NameTeam");
			return;
		}
		if (tabNum >= 0)
		{
			TabChanger.currentTabNum = (tabEnum)tabNum;
		}
		Application.LoadLevel(scene);
	}
}
