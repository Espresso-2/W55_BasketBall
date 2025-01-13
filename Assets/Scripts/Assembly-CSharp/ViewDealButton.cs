using System;
using I2.Loc;
using UnityEngine;

[Serializable]
public class ViewDealButton : MonoBehaviour
{
	public TabChanger tabChanger;

	public Localize dealName;

	private int dealNum;

	private GameSounds gameSounds;

	public virtual void Start()
	{
		gameSounds = GameSounds.GetInstance();
	}

	public virtual void SetDeal(int num)
	{
		dealNum = num;
		dealName.SetTerm(DealPack.titles[num], null);
	}

	public virtual void OnClick()
	{
		gameSounds.Play_one_dribble();
		TabChanger.currentBackAction = backAction.StartingPlayers;
		TabChanger.subSection = dealNum + 3;
		tabChanger.SetToTab(tabEnum.Deals);
	}
}
