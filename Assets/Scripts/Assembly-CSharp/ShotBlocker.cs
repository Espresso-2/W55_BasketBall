using System;
using UnityEngine;

[Serializable]
public class ShotBlocker : MonoBehaviour
{
	public PlayerController pc;

	private GameSounds gameSounds;

	public VoiceOvers voiceOvers;

	public Transform opponentHoop;

	public BoxCollider2D bc2D;

	private bool wideShotBlocker;

	public virtual void Start()
	{
		gameSounds = GameSounds.GetInstance();
	}

	public virtual void FixedUpdate()
	{
		SetSize();
	}

	private void SetSize()
	{
		float distanceToHoop = GetDistanceToHoop(opponentHoop.position.x);
		if (!wideShotBlocker && distanceToHoop < 1f)
		{
			if (pc.isComputer)
			{
				bc2D.size = new Vector2(0.2f, 0.95f);
			}
			else
			{
				bc2D.size = new Vector2(0.55f, 1.05f);
			}
			wideShotBlocker = true;
		}
		else if (wideShotBlocker && distanceToHoop >= 1f)
		{
			bc2D.size = new Vector2(0.12f, 1f);
			wideShotBlocker = false;
		}
	}

	private float GetDistanceToHoop(float hoopPosX)
	{
		Vector2 vector = new Vector2(hoopPosX, base.transform.position.y);
		return Vector3.Distance(vector, base.transform.position);
	}

	public virtual void OnTriggerEnter2D(Collider2D theObject)
	{
		if (theObject.gameObject.tag == "Ball")
		{
			Ball ball = (Ball)theObject.GetComponent(typeof(Ball));
			if (!ball.didScore && !pc.grounded && !ball.wasBlocked && ball.CanBeBlocked() && !ball.isTipoff && pc.timeWithoutBall > 2f)
			{
				Block(ball);
			}
		}
	}

	private void Block(Ball ball)
	{
		Vector2 velocity = ball.gameObject.GetComponent<Rigidbody2D>().velocity;
		Vector2 velocity2 = pc.gameObject.GetComponent<Rigidbody2D>().velocity;
		bool flag = pc.IsFacingRight();
		bool flag2 = false;
		if (!pc.isPlayer2 && !flag)
		{
			flag2 = true;
		}
		bool flag3 = false;
		if (pc.isPlayer2 && flag)
		{
			flag3 = true;
		}
		float num = 0f;
		float num2 = 0f;
		int num3 = UnityEngine.Random.Range(0, 100);
		bool flag4 = false;
		if (num3 > 75)
		{
			num = -0.65f;
			num2 = 1.35f;
			flag4 = true;
		}
		else if (num3 > 60)
		{
			num = -0.3f;
			num2 = 1.25f;
			flag4 = true;
		}
		else if (num3 > 45)
		{
			num = -0.35f;
			num2 = 1.25f;
			flag4 = true;
		}
		else if (num3 > 30)
		{
			num = -0.15f;
			num2 = 1.25f;
			flag4 = true;
		}
		else if (num3 > 15)
		{
			num = 0.15f;
			num2 = 1.4f;
		}
		else if (pc.opponent.isComputer)
		{
			num = 0.35f;
			num2 = 1.35f;
		}
		else
		{
			num = 0.75f;
			num2 = 1.35f;
		}
		float num4 = velocity.x * num;
		float num5 = velocity.y * num2;
		if (num4 > 0f && flag2)
		{
			num4 *= -1f;
			flag4 = false;
		}
		else if (num4 < 0f && flag3)
		{
			num4 *= -1f;
			flag4 = false;
		}
		num4 += velocity2.x * 0.75f;
		num5 += velocity2.y * 0.25f;
		float num6 = 1.75f;
		if (pc.IsFacingRight() && num4 < num6)
		{
			num4 = num6;
		}
		else if (!pc.IsFacingRight() && num4 > 0f - num6)
		{
			num4 = 0f - num6;
		}
		if (ball.wasDunked)
		{
			num5 = 15f;
			gameSounds.Play_dunk();
		}
		Vector2 velocity3 = new Vector2(num4, num5);
		ball.gameObject.GetComponent<Rigidbody2D>().velocity = velocity3;
		ball.wasBlocked = true;
		gameSounds.Play_BlockSound();
		if (!pc.isComputer)
		{
			Stats.numBlocks++;
		}
		if (flag4)
		{
			voiceOvers.PlayBlocked(!pc.isComputer);
			//GameVibrations.Instance().PlayBigBlock();
		}
		else
		{
			voiceOvers.PlayTipped(!pc.isComputer);
			//GameVibrations.Instance().PlayTipped();
		}
		if (pc.gameController.recorder != null)
		{
			pc.gameController.recorder.framesSinceGoodLookingEvent = 0;
		}
		((BallVisual)pc.gameController.ikBall.GetComponent(typeof(BallVisual))).Flicker();
	}
}
