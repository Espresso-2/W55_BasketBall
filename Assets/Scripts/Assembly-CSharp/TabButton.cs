using System;
using UnityEngine;

[Serializable]
public class TabButton : MonoBehaviour
{
	public TabChanger tabChanger;

	public tabEnum tabType;

	public backAction backActionType;

	public int subSection;

	private GameSounds gameSounds;

	public TabButton()
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
		TabChanger.subSection = subSection;
		tabChanger.SetToTab(tabType);
	}
}
