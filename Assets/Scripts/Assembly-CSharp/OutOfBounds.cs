using System;
using UnityEngine;

[Serializable]
public class OutOfBounds : MonoBehaviour
{
	public bool isEnemy;

	public VoiceOvers voiceOvers;

	public virtual void Start()
	{
	}

	public virtual void Update()
	{
	}

	public virtual void OnTriggerEnter2D(Collider2D theObject)
	{
		if (theObject.gameObject.CompareTag("Ball"))
		{
			theObject.gameObject.TryGetComponent(out Ball ball);
			if (!ball.wasBlocked && !ball.didScore && ball.numRimHits == 0)
			{
				voiceOvers?.PlayNotEvenClose();
			}
		}
	}
}
