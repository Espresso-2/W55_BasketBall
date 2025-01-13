using System;
using UnityEngine;

[Serializable]
public class BasketTrigger : MonoBehaviour
{
	public GameController gameController;

	public bool isEnemyBasket;

	public ShakeEffect hoopVisual;

	public Animator starParticles;

	public virtual void Start()
	{
	}

	public virtual void Update()
	{
	}

	public virtual void OnTriggerEnter2D(Collider2D theObject)
	{
		if (!(theObject.tag == "Ball"))
		{
			return;
		}
		Ball ball = (Ball)theObject.GetComponent(typeof(Ball));
		if (!ball.canScore || ball.didScore)
		{
			return;
		}
		ball.gameObject.layer = LayerMask.NameToLayer("ScoredBall");
		ball.didScore = true;
		ball.beingLayedUp = false;
		ball.gameObject.GetComponent<Rigidbody2D>().velocity = ball.gameObject.GetComponent<Rigidbody2D>().velocity / 5f;
		int num = gameController.BallInBasket(isEnemyBasket, ball.numRimHits, ball.wasBlocked);
		if (num == 3)
		{
			if (!isEnemyBasket || gameController.twoPlayerMode)
			{
				starParticles.SetTrigger("Score3");
			}
			else
			{
				starParticles.SetTrigger("Score3Enemy");
			}
		}
		else if (gameController.IsDunkTriggered())
		{
			if (!isEnemyBasket || gameController.twoPlayerMode)
			{
				starParticles.SetTrigger("ScoreDunk");
			}
			else
			{
				starParticles.SetTrigger("Score2Enemy");
			}
		}
		else if (!isEnemyBasket || gameController.twoPlayerMode)
		{
			starParticles.SetTrigger("Score2");
		}
		else
		{
			starParticles.SetTrigger("Score2Enemy");
		}
		hoopVisual.Shake(0.85f, 0.035f);
	}
}
