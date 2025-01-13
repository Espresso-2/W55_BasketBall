using System;
using UnityEngine;

[Serializable]
public class Hoop : MonoBehaviour
{
	private GameSounds gameSounds;

	public ShakeEffect hoopVisual;

	private float minTimeBetweenHits;

	private float timer;

	public Hoop()
	{
		minTimeBetweenHits = 0.1f;
		timer = 1f;
	}

	public virtual void Start()
	{
		gameSounds = GameSounds.GetInstance();
	}

	public virtual void Update()
	{
		timer += Time.deltaTime;
	}

	public virtual void OnCollisionEnter2D(Collision2D coll)
	{
		if (timer > minTimeBetweenHits)
		{
			timer = 0f;
			int num = UnityEngine.Random.Range(0, 100);
			if (num >= 0)
			{
				gameSounds.Play_rattling_hinge();
			}
			else
			{
				gameSounds.Play_break_tackle();
			}
			hoopVisual.Shake(2f, 0.065f);
			GameVibrations.Instance().PlayHitHoop();
		}
	}
}
