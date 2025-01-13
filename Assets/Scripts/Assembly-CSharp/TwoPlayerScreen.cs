using System;
using UnityEngine;

[Serializable]
public class TwoPlayerScreen : MonoBehaviour
{
	private GameSounds gameSounds;

	public virtual void Start()
	{
		Time.timeScale = 1f;
		gameSounds = GameSounds.GetInstance();
	}

	public virtual void Update()
	{
	}

	public virtual void JoinGameOnlick()
	{
		gameSounds.Play_one_dribble();
	}

	public virtual void BackButtonOnlick()
	{
		gameSounds.Play_one_dribble();
	}
}
