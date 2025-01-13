using System;
using UnityEngine;

[Serializable]
public class BackButton : MonoBehaviour
{
	public TabChanger tabChanger;

	private SessionVars sessionVars;

	private GameSounds gameSounds;

	public virtual void Start()
	{
		sessionVars = SessionVars.GetInstance();
		gameSounds = GameSounds.GetInstance();
	}

	public virtual void OnClick()
	{
		gameSounds.Play_one_dribble();
		if (TabChanger.currentBackAction == backAction.Settings)
		{
			TabChanger.currentBackAction = backAction.CurrentTab;
			tabChanger.SetToScreen(screenEnum.Settings);
		}
		else if (TabChanger.currentBackAction == backAction.StartingPlayers)
		{
			TabChanger.currentBackAction = backAction.None;
			tabChanger.SetToTab(tabEnum.Players);
			GameObject gameObject = GameObject.Find("PlayerCategories");
			if (gameObject != null)
			{
				((PlayersCategories)gameObject.GetComponent(typeof(PlayersCategories))).ShowStarters(false);
			}
		}
		else if (TabChanger.currentBackAction == backAction.Matchup)
		{
			TabChanger.currentBackAction = backAction.None;
			Application.LoadLevel("Matchup");
		}
		else
		{
			TabChanger.currentBackAction = backAction.None;
			tabChanger.SetToTab(TabChanger.currentTabNum);
		}
	}

	public virtual void FixedUpdate()
	{
		if (Input.GetKeyUp(KeyCode.Escape))
		{
			OnClick();
		}
	}
}
