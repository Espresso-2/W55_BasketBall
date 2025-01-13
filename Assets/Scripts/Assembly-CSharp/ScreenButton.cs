using System;
using UnityEngine;

[Serializable]
public class ScreenButton : MonoBehaviour
{
	public TabChanger tabChanger;

	public screenEnum screenType;

	public backAction backActionType;

	private GameSounds gameSounds;

	public ScreenButton()
	{
		backActionType = backAction.None;
	}

	public virtual void Start()
	{
		gameSounds = GameSounds.GetInstance();
	}

	public virtual void OnClick()
	{
		gameSounds.Play_one_dribble();
		TabChanger.currentBackAction = backActionType;
		tabChanger.SetToScreen(screenType);
	}
}
