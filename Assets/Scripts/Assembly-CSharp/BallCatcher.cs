using System;
using UnityEngine;

[Serializable]
public class BallCatcher : MonoBehaviour
{
	public bool isLow;

	public bool isMiddle;

	public bool isHigh;

	public PlayerController pc;

	public PlayerController opponent;

	public LayerMask whatIsBall;

	private SessionVars sessionVars;

	private GameSounds gameSounds;

	public VoiceOvers voiceOvers;

	private float checkOverlapTimer;

	private float checkOverlapFreq = 0.2f;

	public virtual void Start()
	{
		sessionVars = SessionVars.GetInstance();
		gameSounds = GameSounds.GetInstance();
	}

	public virtual void FixedUpdate()
	{
		if (!isLow || !pc.IsGrounded() || pc.hasBall)
		{
			return;
		}
		checkOverlapTimer += Time.deltaTime;
		if (checkOverlapTimer >= checkOverlapFreq)
		{
			checkOverlapTimer = 0f;
			Ball ball = pc.gameController.ball;
			if (ball != null && (bool)Physics2D.OverlapCircle(base.gameObject.transform.position, 0.5f, whatIsBall))
			{
				GetTheBall(ball);
			}
		}
	}

	public virtual void OnTriggerEnter2D(Collider2D theObject)
	{
		if (pc.timeWithoutBall >= pc.GetBufferAfterShootingBall() && theObject.gameObject.tag == "Ball")
		{
			Ball ball = (Ball)theObject.GetComponent(typeof(Ball));
			GetTheBall(ball);
		}
		else if (isLow && theObject.gameObject.tag == "UnderHoop")
		{
			pc.isUnderHoop = true;
			if (!pc.isPlayer2 && pc.gameController.InTutorial)
			{
				StartCoroutine(pc.tutorial.EnteredUnderHoop());
			}
		}
		else if (isMiddle && theObject.gameObject.tag == "PlayerMiddle")
		{
			TryToSteal();
		}
	}

	private void GetTheBall(Ball ball)
	{
		if (ball.didScore || ball.aboutToBeDestroyed || (ball.isDirectlyAboveCylinder && !ball.CanBeBlocked()) || (isHigh && ball.secondsAlive < 0.55f && pc.timeWithoutBall > 2f && !ball.isTipoff) || (isLow && !pc.IsGrounded()))
		{
			return;
		}
		bool flag = false;
		if (!ball.isTipoff)
		{
			if (ball.secondsAlive < 0.55f)
			{
				Stats.numBlocks++;
				gameSounds.Play_chime_2_beeps();
				voiceOvers.PlayInterceptedShot(!pc.isComputer, sessionVars.currentTournament.isFemale);
				((BallVisual)pc.gameController.ikBall.GetComponent(typeof(BallVisual))).ShortFlicker();
				//GameVibrations.Instance().PlayBlockCatch();
			}
			else if (!pc.isComputer)
			{
				Stats.numRebounds++;
				flag = true;
			}
		}
		Vector3 position = ball.gameObject.transform.position;
		pc.GetBall();
		if (isLow)
		{
			pc.anim.SetTrigger("PickupBall");
		}
		else if (isMiddle)
		{
			pc.anim.SetTrigger("PickupBallMiddle");
			if (!pc.grounded && flag)
			{
				voiceOvers.PlayGetsRebound();
			}
		}
		else if (pc.grounded)
		{
			pc.anim.SetTrigger("CatchOnGround");
		}
		else if (flag)
		{
			voiceOvers.PlayGetsRebound();
		}
		ball.aboutToBeDestroyed = true;
		UnityEngine.Object.Destroy(ball.gameObject);
	}

	public virtual void OnTriggerExit2D(Collider2D theObject)
	{
		if (isLow && theObject.gameObject.tag == "UnderHoop")
		{
			pc.isUnderHoop = false;
		}
	}

	private void TryToSteal()
	{
		if (CanSteal())
		{
			if (!pc.isComputer)
			{
				Stats.numSteals++;
			}
			opponent.BallStolen();
			((BallVisual)pc.gameController.ikBall.GetComponent(typeof(BallVisual))).ShortFlicker();
			pc.GetBall();
			pc.anim.SetTrigger("PickupBall");
			gameSounds.Play_chime_2_beeps();
			voiceOvers.PlayStolen(!pc.isComputer, sessionVars.currentTournament.isFemale);
			//GameVibrations.Instance().PlayStolen();
		}
	}

	private bool CanSteal()
	{
		bool result = false;
		if (pc.sprintBar.isFull && opponent.sprintBar.isTired && (!opponent.jumpShotWindingup || opponent.isComputer) && opponent.jumpShotWindupTimer < 0.4f && opponent.grounded && opponent.timeWithBall > 2.25f && pc.grounded)
		{
			result = true;
		}
		if (pc.isComputer && pc.gameController.InScrimmage)
		{
			result = false;
		}
		return result;
	}
}
