using System;
using UnityEngine;

[Serializable]
public class ShowDialogButton : MonoBehaviour
{
	public GameObject objectToShow;

	private GameSounds gameSounds;

	public virtual void Start()
	{
		gameSounds = GameSounds.GetInstance();
	}

	public virtual void OnClick()
	{
		gameSounds.Play_select();
		objectToShow.SetActive(true);
	}
}
