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
		if (theObject.gameObject.tag == "Ball")
		{
			Ball ball = (Ball)theObject.gameObject.GetComponent(typeof(Ball));
			if (!ball.wasBlocked && !ball.didScore && ball.numRimHits == 0)
			{
				voiceOvers.PlayNotEvenClose();
			}
		}
	}
}
