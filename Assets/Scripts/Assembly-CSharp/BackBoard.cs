using System;
using UnityEngine;

[Serializable]
public class BackBoard : MonoBehaviour
{
	private GameSounds gameSounds;

	public ShakeEffect hoopVisual;

	public virtual void Start()
	{
		gameSounds = GameSounds.GetInstance();
	}

	public virtual void Update()
	{
	}

	public virtual void OnCollisionEnter2D(Collision2D coll)
	{
		gameSounds.Play_unselect();
		GameVibrations.Instance().PlayHitBackBoard();
		hoopVisual.Shake(1f, 0.045f);
	}
}
