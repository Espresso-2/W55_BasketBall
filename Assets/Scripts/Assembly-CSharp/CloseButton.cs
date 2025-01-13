using System;
using UnityEngine;

[Serializable]
public class CloseButton : MonoBehaviour
{
	public GameObject objectToClose;

	private GameSounds gameSounds;

	public virtual void Start()
	{
		gameSounds = GameSounds.GetInstance();
	}

	public virtual void Close()
	{
		gameSounds.Play_select();
		objectToClose.SetActive(false);
	}

	public virtual void Update()
	{
	}
}
