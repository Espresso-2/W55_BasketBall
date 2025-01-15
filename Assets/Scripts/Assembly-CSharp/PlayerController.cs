using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class PlayerController : MonoBehaviour
{
	public bool isComputer;

	public ComputerAI computerAI;

	public PlayerController opponent;

	private float maxSpeed = 6f;

	private float maxDefendedAcceleration = 4f;

	private float defendedMultiplier = 0.01f;

	private float accelerateRate = 0.2f;

	private float currentAcceleration = 0.1f;

	public SprintBar sprintBar;

	public SprintBar opponentSprintBar;

	private bool sneakyFastEnergyWhenOpponentTired;

	private bool facingRight = true;

	public Animator anim;

	public GameObject global_CTRL;

	public bool grounded;

	public Transform groundCheck;

	private float groundRadius = 0.2f;

	private float timeNotGrounded;

	public LayerMask whatIsGround;

	public Shooter shooter;

	public Dunker dunker;

	public HoopVisualAnchor hoopVisualAnchor;

	public GameObject animatedBall;

	public Transform animatedHead;

	private float jumpForce = 50f;

	private float jumpShotForce = 586f;

	private float energyRegenerationSpeed = 0.1f;

	private float size = 1.2f;

	public float jumpShotWindupTimer;

	private float jumpShotWindupTimeLength = 0.4f;

	private bool startingLayup;

	public bool jumpShotWindingup;

	public bool isJumpShooting;

	public bool dunkWindingup;

	private float dunkStartDistance;

	private bool moveToHangOnRim;

	private float springTimer;

	private float springTimeLength = 0.14f;

	public bool springing;

	private bool celebrationSpring;

	private float timeSinceLastSpring;

	private float timeSinceStartOfPlay;

	private float timeSinceLastShot;

	public bool hasBall;

	public bool hasDribble;

	public bool isDribbling;

	public bool hasTripleThreat;

	public float timeWithoutDribble;

	public float timeWithoutBall = 10f;

	private float bufferAfterShootingBall = 0.25f;

	private bool ballJustStolen;

	private GameSounds gameSounds;

	public GameController gameController;

	public float timeWithBall;

	private float timeToGatherBall = 0.2f;

	public float timeDribbling;

	public float timeOnGround;

	private float timeSinceLastLastFlip = 10f;

	public bool jumpButtonDown;

	private bool jumpButtonUp;

	public bool lastAirWasFromShot;

	public float startPosX;

	private float startPosY = -1.77f;

	public float move;

	private float moveLast;

	private bool isPeformingStepBack;

	private float stepBackTimer;

	private float stepBackLength = 0.45f;

	private float minTimeBetweenStepBacks = 0.45f;

	private float firstDribbleWindupTimer;

	private float firstDribbleWindupTimerLength = 0.35f;

	private bool isWindingUpFirstDribble;

	private int numFirstDribbleWindupsThisPossesion;

	private bool isPerformingFirstDribble;

	private float firstDribbleTimer;

	private float firstDribbleLength = 0.3f;

	private float firstDribbleSpeed = 5f;

	private bool shootingLayup;

	private bool hasBeetDefender;

	public float secondsStuckDribblingIntoDefender;

	public bool isUnderHoop;

	public bool isPlayer2;

	public float maxX;

	public float minX;

	public Tutorial tutorial;

	private bool hasBeenTired;

	public Rigidbody2D rig2D;

	private bool springWhenPlayerLands;

	public virtual void Start()
	{
		gameSounds = GameSounds.GetInstance();
		rig2D = GetComponent<Rigidbody2D>();
		hasBeenTired = PlayerPrefs.GetInt("HAS_BEEN_TIRED") == 1;
		if (!isComputer && !gameController.twoPlayerMode && Stats.GetNumWins() < 3)
		{
			sneakyFastEnergyWhenOpponentTired = true;
		}
		dunker.gameObject.SetActive(false);
		hasDribble = true;
		float distanceToHoop = shooter.GetDistanceToHoop(shooter.gameObject.transform.position);
		anim.SetFloat("Distance", distanceToHoop);
		if (isComputer)
		{
			timeToGatherBall = 0.2f;
			int numWins = Stats.GetNumWins();
			if (numWins <= 3)
			{
				jumpShotForce = 500f;
			}
			else if (numWins <= 9)
			{
				jumpShotForce = 520f;
			}
			else if (numWins <= 50)
			{
				jumpShotForce = 586f;
			}
			else if (numWins <= 200)
			{
				jumpShotForce = 595f;
			}
			else
			{
				jumpShotForce = 605f;
			}
		}
	}

	public virtual void Update()
	{
		if (moveToHangOnRim)
		{
			Vector3 position = base.gameObject.transform.position;
			Vector3 position2 = dunker.gameObject.transform.position;
			float y = shooter.hoop.gameObject.transform.position.y;
			if (position2.y > y + 0.4f)
			{
				float num = ((!isPlayer2) ? 2.25f : (-2.25f));
				base.gameObject.transform.position = new Vector3(position.x + num * Time.deltaTime, position.y - 5.5f * Time.deltaTime, position.z);
			}
			else
			{
				moveToHangOnRim = false;
				hoopVisualAnchor.PullDownHoop();
				LeanTween.moveY(base.gameObject, position.y - 0.25f, 0.35f).setEase(LeanTweenType.easeOutQuad).setUseEstimatedTime(true);
			}
		}
		if (!gameController.playingGame && !gameController.gameIsOver)
		{
			if (springTimer > springTimeLength)
			{
				if (grounded)
				{
					StartCoroutine(JumpBlock());
				}
			}
			else if (springing)
			{
				springTimer += Time.deltaTime;
			}
			if (grounded)
			{
				sprintBar.generate = energyRegenerationSpeed;
			}
			return;
		}
		if (!jumpShotWindingup)
		{
			jumpShotWindupTimer = 0f;
			startingLayup = false;
		}
		if ((jumpShotWindupTimer > jumpShotWindupTimeLength || startingLayup) && !isJumpShooting)
		{
			if (grounded)
			{
				Debug.Log("DO THE JUMP (this.jumpShotWindupTimer: " + jumpShotWindupTimer + ")");
				JumpShot();
			}
			else
			{
				Debug.Log("CANT JUMP, NOT GROUNDED");
			}
		}
		if (jumpShotWindingup && (grounded || isJumpShooting))
		{
			if (timeWithBall >= 0.25f && !isPeformingStepBack && !isPerformingFirstDribble)
			{
				jumpShotWindupTimer += Time.deltaTime;
			}
		}
		else if (jumpButtonDown && grounded && hasBall && !jumpShotWindingup)
		{
			Debug.Log("Start Jumpshot windup: " + Time.time);
			ResetAnimTriggers();
			jumpShotWindingup = true;
			isJumpShooting = false;
			jumpButtonUp = false;
			jumpShotWindupTimer = 0f;
			startingLayup = false;
			isDribbling = false;
			float distanceToHoop = shooter.GetDistanceToHoop(shooter.gameObject.transform.position);
			bool flag = distanceToHoop < GetDistanceForLayup();
			if (flag)
			{
				Debug.Log("DO LAYUPPP");
				dunker.gameObject.SetActive(true);
				dunkWindingup = IsDunk(distanceToHoop);
				dunkStartDistance = distanceToHoop;
				startingLayup = true;
				grounded = false;
				anim.SetBool("Ground", grounded);
				if (opponent.computerAI != null)
				{
					opponent.computerAI.OpponentStartedLayup();
				}
				gameController.ikBall.FixToTarget(animatedBall);
				if (dunkWindingup)
				{
					anim.SetTrigger("StartDunk");
					anim.SetBool("LastShotWasLayup", true);
				}
				else
				{
					anim.SetTrigger("StartLayup");
					anim.SetBool("LastShotWasLayup", true);
				}
				gameSounds.Play_whoose_low_2();
			}
			else
			{
				anim.SetTrigger("StartJumpShotWindup");
				anim.SetBool("LastShotWasLayup", false);
				gameSounds.Play_whoose_sharp_whip_quiet();
			}
			if (!hasTripleThreat)
			{
				hasDribble = false;
			}
			if (!isComputer)
			{
				gameSounds.Play_select();
			}
			shooter.StartedShotWindup(flag);
			FaceCorrectDirection(false);
			CancelWindupFirstDribble();
		}
		if (springTimer > springTimeLength)
		{
			if (grounded)
			{
				StartCoroutine(JumpBlock());
			}
		}
		else if (springing)
		{
			springTimer += Time.deltaTime;
		}
		else if ((jumpButtonDown || springWhenPlayerLands) && !hasBall && !springing)
		{
			if (grounded && (timeWithoutBall >= 0.75f || ballJustStolen) && timeSinceLastSpring >= 0.75f)
			{
				Debug.Log("Start jump block windup: " + Time.time);
				springing = true;
				springWhenPlayerLands = false;
				timeSinceLastSpring = 0f;
				anim.SetTrigger("StartSpringing");
				anim.SetBool("Springing", true);
				if (!isComputer)
				{
					gameSounds.Play_select();
				}
			}
			else if (timeSinceLastSpring > 0.5f)
			{
				springWhenPlayerLands = true;
			}
		}
		if (isWindingUpFirstDribble)
		{
			firstDribbleWindupTimer += Time.deltaTime;
			float num2 = firstDribbleWindupTimerLength;
			if (timeWithBall < firstDribbleWindupTimerLength + 0.3f)
			{
				num2 = firstDribbleWindupTimerLength + 0.3f;
			}
			if (firstDribbleWindupTimer > num2)
			{
				if (CheckForPerformFirstDribble())
				{
					firstDribbleWindupTimer = 0f;
					isWindingUpFirstDribble = false;
				}
				else if (firstDribbleWindupTimer > 1f && !isComputer && !gameController.hintMessage.isActiveAndEnabled)
				{
					gameController.ShowHint(false);
					gameController.hintMessage.DoubleDribble();
				}
			}
		}
		firstDribbleTimer += Time.deltaTime;
		if (isPerformingFirstDribble && firstDribbleTimer > firstDribbleLength)
		{
			firstDribbleTimer = 0f;
			isPerformingFirstDribble = false;
		}
		if (hasDribble)
		{
			timeWithoutDribble = 0f;
		}
		else
		{
			timeWithoutDribble += Time.deltaTime;
		}
		if (!hasBall)
		{
			timeWithoutBall += Time.deltaTime;
			timeWithBall = 0f;
		}
		else if (grounded)
		{
			timeWithBall += Time.deltaTime;
		}
		if (grounded)
		{
			timeOnGround += Time.deltaTime;
		}
		else
		{
			timeOnGround = 0f;
		}
		timeSinceStartOfPlay += Time.deltaTime;
		timeSinceLastLastFlip += Time.deltaTime;
		timeSinceLastSpring += Time.deltaTime;
		timeSinceLastShot += Time.deltaTime;
	}

	public virtual void FixedUpdate()
	{
		grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
		if (Math.Abs(moveLast - move) > 0f)
		{
			currentAcceleration = 0.1f;
		}
		if (jumpShotWindingup)
		{
			springing = false;
			springTimer = 0f;
		}
		else if (IsMovingAwayFromHoop(move))
		{
			CheckForPerformStepBack();
		}
		else if (IsMovingTowardHoop(move))
		{
			CheckForWindUpFirstDribble();
		}
		if (grounded)
		{
			if (timeNotGrounded >= 0.25f)
			{
				currentAcceleration = 0f;
			}
			timeNotGrounded = 0f;
		}
		else
		{
			timeNotGrounded += Time.deltaTime;
		}
		stepBackTimer += Time.deltaTime;
		if (isPeformingStepBack && stepBackTimer > stepBackLength)
		{
			stepBackTimer = 0f;
			isPeformingStepBack = false;
			anim.SetBool("StepBack", false);
		}
		bool flag = IsBeet();
		anim.SetBool("Ground", grounded);
		anim.SetBool("HasBall", hasBall);
		anim.SetBool("WindingUp", jumpShotWindingup);
		anim.SetBool("WindingUpFirstDribble", isWindingUpFirstDribble);
		anim.SetBool("IsDefending", opponent.hasBall && !flag);
		anim.SetFloat("vSpeed", rig2D.velocity.y);
		Vector3 position = shooter.gameObject.transform.position;
		float distanceToHoop = shooter.GetDistanceToHoop(position);
		anim.SetFloat("Distance", distanceToHoop);
		float num = 0f;
		if (celebrationSpring)
		{
			if (position.x > 4f)
			{
				move = -1f;
			}
			else if (position.x < -4f)
			{
				move = 1f;
			}
			else
			{
				move = 0f;
			}
		}
		else if (!gameController.playingGame)
		{
			anim.SetFloat("Speed", 0f);
			return;
		}
		float num2 = maxSpeed;
		if (!hasBall && (!opponent.hasBall || flag))
		{
			num2 += 2f;
		}
		if (Math.Abs(move) > 0f && !jumpShotWindingup)
		{
			if (grounded)
			{
				currentAcceleration += accelerateRate;
			}
			else
			{
				currentAcceleration += 0.15f;
			}
		}
		if (currentAcceleration > num2)
		{
			currentAcceleration = num2;
		}
		num = currentAcceleration;
		if (move < 0f)
		{
			num *= -1f;
		}
		if (sprintBar.isTired)
		{
			num *= 0.85f;
		}
		if (isWindingUpFirstDribble && (!IsMovingTowardHoop(move) || !hasBall))
		{
			CancelWindupFirstDribble();
			num = 0f;
		}
		if (IsDribbling())
		{
			isDribbling = true;
		}
		float num3 = 0f;
		if (isPeformingStepBack)
		{
			num = ((!isPlayer2) ? (-4f) : 4f);
			rig2D.velocity = new Vector2(num, rig2D.velocity.y);
		}
		else if ((!hasBall || hasDribble) && !jumpShotWindingup && (grounded || !lastAirWasFromShot))
		{
			if (isWindingUpFirstDribble)
			{
				num = 0f;
			}
			else if (hasBall && grounded && IsCloselyDefended() && ((isPlayer2 && !facingRight) || (!isPlayer2 && facingRight)) && IsMovingTowardHoop(num))
			{
				if (currentAcceleration > maxDefendedAcceleration)
				{
					currentAcceleration = maxDefendedAcceleration;
				}
				num *= defendedMultiplier;
				if (sprintBar.isTired)
				{
					num *= 0.5f;
				}
				if (opponentSprintBar.isTired)
				{
					num *= 1.25f;
				}
				if (Mathf.Abs(num) < 0.85f)
				{
					num3 = secondsStuckDribblingIntoDefender + Time.deltaTime;
				}
			}
			else if (hasBall && grounded && IsMovingAwayFromHoop(move))
			{
				num *= 0.55f;
			}
			else if (hasBall)
			{
				num *= 0.9f;
			}
			float num4 = num;
			if (!hasBall && (!opponent.hasBall || flag))
			{
				num4 = Mathf.Abs(num4);
			}
			else if (isPlayer2)
			{
				num4 *= -1f;
			}
			anim.SetFloat("Speed", num4);
			if (!grounded && Mathf.Abs(rig2D.velocity.x) * 0.75f > Mathf.Abs(num))
			{
				num = rig2D.velocity.x * 0.75f;
			}
			rig2D.velocity = new Vector2(num, rig2D.velocity.y);
			bool flag2 = stepBackTimer < 2f;
			if (((hasBall && Mathf.Abs(num) > 0.1f && timeWithBall > timeToGatherBall && hasTripleThreat) || flag2) && hasTripleThreat)
			{
				shooter.ResetNumPumpFakes();
				Debug.Log("Set this.hasTripleThreat = false; this.timeWithBall=" + timeWithBall + " justDidStepback=" + flag2 + " moveSpeed=" + num);
				anim.SetBool("HasTripleThreat", false);
				hasTripleThreat = false;
			}
		}
		if (!isComputer || (isComputer && hasBall && num < 0f))
		{
			float num5 = ((!hasBall) ? 9.5f : 7.75f);
			if ((position.x > num5 && num > 0f) || (position.x < num5 * -1f && num < 0f))
			{
				rig2D.velocity = new Vector2(0f, rig2D.velocity.y);
			}
			else
			{
				float num6 = 8f;
				if (dunkWindingup)
				{
					if (position.y >= 1.75f)
					{
						num6 = 5.35f;
					}
				}
				else if (hasBall)
				{
					num6 = 5.75f;
				}
				if ((position.x > num6 && num > 0f && !isPlayer2) || (position.x < num6 * -1f && num < 0f && isPlayer2))
				{
					if (rig2D.velocity.x > 1.25f && !isPlayer2)
					{
						rig2D.velocity = new Vector2(1.25f, rig2D.velocity.y);
					}
					else if (rig2D.velocity.x < -1.25f && isPlayer2)
					{
						rig2D.velocity = new Vector2(-1.25f, rig2D.velocity.y);
					}
				}
			}
		}
		secondsStuckDribblingIntoDefender = num3;
		if (!hasDribble && hasBall)
		{
			anim.SetFloat("Speed", 0f);
		}
		if (hasBall && timeWithBall <= timeToGatherBall && stepBackTimer >= 2f)
		{
			hasTripleThreat = true;
			anim.SetBool("HasTripleThreat", true);
		}
		FaceCorrectDirection(flag);
		if (hasBall && Math.Abs(move) > 0f && hasDribble)
		{
			timeDribbling += Time.deltaTime;
		}
		else
		{
			timeDribbling = 0f;
		}
		SetSprintBarGeneration();
		if (!hasBeenTired && sprintBar.isTired)
		{
			hasBeenTired = true;
			PlayerPrefsHelper.SetInt("HAS_BEEN_TIRED", 1);
			if (!isPlayer2 && grounded)
			{
				gameController.ShowHint(false);
				gameController.hintMessage.Fatigued();
			}
		}
		if (BallShouldFlicker())
		{
			((BallVisual)gameController.ikBall.GetComponent(typeof(BallVisual))).FlickerRed();
		}
		else if (timeWithBall > 1f)
		{
			((BallVisual)gameController.ikBall.GetComponent(typeof(BallVisual))).StopFlicker();
		}
		moveLast = move;
	}

	public virtual void NewGame()
	{
		sprintBar.Reset();
		hasBeenTired = false;
	}

	public virtual void Reset()
	{
		hasBall = false;
		currentAcceleration = 0.1f;
		springing = false;
		springTimer = 0f;
		springWhenPlayerLands = false;
		jumpShotWindingup = false;
		jumpShotWindupTimer = 0f;
		startingLayup = false;
		moveToHangOnRim = false;
		timeWithoutBall = 10f;
		timeSinceStartOfPlay = 0f;
		timeSinceLastLastFlip = 10f;
		timeSinceLastSpring = 10f;
		timeSinceLastShot = 10f;
		stepBackTimer = 10f;
		firstDribbleWindupTimer = 0f;
		firstDribbleTimer = 10f;
		jumpButtonDown = false;
		jumpButtonUp = false;
		anim.SetBool("HasBall", hasBall);
		anim.SetBool("Springing", false);
		dunker.gameObject.SetActive(false);
		float x = startPosX;
		Vector3 position = base.gameObject.transform.position;
		position.x = x;
		base.gameObject.transform.position = position;
		float y = startPosY;
		Vector3 position2 = base.gameObject.transform.position;
		position2.y = y;
		base.gameObject.transform.position = position2;
		if (!facingRight && !isPlayer2)
		{
			Flip(true);
		}
		else if (facingRight && isPlayer2)
		{
			Flip(true);
		}
		SetScale();
	}

	private void SetScale()
	{
		float num = size;
		float x = num;
		Vector3 localScale = base.gameObject.transform.localScale;
		localScale.x = x;
		base.gameObject.transform.localScale = localScale;
		float y = num;
		Vector3 localScale2 = base.gameObject.transform.localScale;
		localScale2.y = y;
		base.gameObject.transform.localScale = localScale2;
		float num2 = 0.33f;
		float num3 = num2 * num;
		float num4 = 1.09f;
		float num5 = num4 + 1f - num3 / num2;
		animatedHead.localScale = new Vector3(num5, num5, num5);
	}

	public virtual void JumpShot()
	{
		Debug.Log("JumpShot() dunk: " + dunkWindingup);
		gameController.ikBall.FixToTarget(animatedBall);
		isJumpShooting = true;
		hasDribble = false;
		anim.SetBool("Ground", false);
		grounded = false;
		lastAirWasFromShot = true;
		float num = jumpShotForce;
		if (startingLayup)
		{
			num = 520f;
		}
		if (dunkWindingup)
		{
			if (sprintBar.isFull && !isComputer && dunkStartDistance <= 4.15f && dunkStartDistance >= 3.7f)
			{
				num = 777f;
				gameSounds.Play_quick_applause();
				gameSounds.Play_ascend_chime_bright_2_loud();
			}
			else
			{
				num = 676f;
			}
		}
		else if (sprintBar.isTired && IsMovingFastTowardHoop())
		{
			num *= 0.95f;
		}
		else if (sprintBar.isTired)
		{
			num *= 0.87f;
		}
		rig2D.AddForce(new Vector2(0f, num));
		if (!isComputer)
		{
			gameSounds.Play_whoose_tennis_hit();
		}
		else
		{
			gameSounds.Play_whoose_tennis_hit();
		}
		timeSinceLastShot = 0f;
	}

	public virtual IEnumerator JumpBlock()
	{
		Debug.Log("JumpBlock()");
		jumpButtonDown = false;
		springing = false;
		springTimer = 0f;
		if (!hasBall)
		{
			lastAirWasFromShot = false;
			grounded = false;
			anim.SetBool("Ground", grounded);
			float force = jumpForce * 1.2f;
			if (sprintBar.isTired)
			{
				force *= 0.9f;
			}
			if (celebrationSpring)
			{
				force = jumpForce * 1.35f;
				celebrationSpring = false;
			}
			rig2D.AddForce(new Vector2(0f, force));
			if (!isComputer)
			{
				gameSounds.Play_whoose_tennis_hit();
			}
			else
			{
				gameSounds.Play_whoose_tennis_hit();
			}
			if (!isPlayer2 && gameController.InTutorial)
			{
				StartCoroutine(tutorial.JumpBlocked());
			}
			yield return new WaitForSeconds(0.1f);
			anim.SetBool("Springing", false);
			yield return new WaitForSeconds(0.65f);
			gameSounds.Play_thump_on_floor();
		}
	}

	public virtual void Flip(bool newPlay)
	{
		if ((newPlay || ((!hasBall || !(timeWithBall < 0.3f) || jumpShotWindingup || isWindingUpFirstDribble) && grounded && !(timeOnGround < 0.2f))) && (!(timeSinceLastLastFlip < 0.5f) || jumpShotWindingup))
		{
			timeSinceLastLastFlip = 0f;
			facingRight = !facingRight;
			if (global_CTRL != null)
			{
				global_CTRL.SendMessage("SetFlip", !facingRight);
			}
		}
	}

	public virtual IEnumerator ShotBall(bool isLayup, bool dunkTriggered)
	{
		Debug.Log("ShotBall()");
		anim.SetBool("HasBall", false);
		hasBall = false;
		timeWithBall = 0f;
		dunker.gameObject.SetActive(false);
		isJumpShooting = false;
		((BallVisual)gameController.ikBall.GetComponent(typeof(BallVisual))).StopFlicker();
		if (dunkTriggered)
		{
			rig2D.bodyType = RigidbodyType2D.Static;
			moveToHangOnRim = true;
			anim.SetTrigger("CompleteDunk");
		}
		else
		{
			anim.SetBool("ShootingBall", true);
			yield return new WaitForSeconds(0.25f);
			anim.SetBool("ShootingBall", false);
		}
		if (!isPlayer2 && gameController.InTutorial)
		{
			StartCoroutine(tutorial.ShotBall());
		}
		if (dunkTriggered)
		{
			yield return new WaitForSeconds(0.75f);
			rig2D.bodyType = RigidbodyType2D.Dynamic;
			yield return new WaitForSeconds(0.35f);
			gameSounds.Play_thump_on_floor();
		}
		else
		{
			yield return new WaitForSeconds(0.05f);
			gameSounds.Play_thump_on_floor();
		}
	}

	public virtual void BallStolen()
	{
		gameSounds.Play_air_pump();
		anim.SetBool("HasBall", false);
		anim.SetTrigger("BallStolen");
		hasBall = false;
		timeWithBall = 0f;
		ballJustStolen = true;
		if (jumpShotWindingup)
		{
			springing = true;
			jumpShotWindingup = false;
		}
		isJumpShooting = false;
	}

	public virtual bool IsGrounded()
	{
		return grounded;
	}

	public virtual bool IsCloselyDefended()
	{
		if (stepBackTimer < 0.55f)
		{
			return false;
		}
		if (isPerformingFirstDribble)
		{
			return false;
		}
		float x = opponent.gameObject.transform.position.x;
		float x2 = base.gameObject.transform.position.x;
		if (!opponent.grounded || opponent.springing)
		{
			return false;
		}
		if (isPlayer2)
		{
			if (x < x2 - 0.15f && x > x2 - 2.25f)
			{
				return true;
			}
			return false;
		}
		if (x > x2 + 0.15f && x < x2 + 2.25f)
		{
			return true;
		}
		return false;
	}

	public virtual void GetBall()
	{
		anim.SetBool("ShootingBall", false);
		anim.SetBool("HasBall", true);
		anim.SetBool("HasTripleThreat", true);
		hasBall = true;
		hasDribble = true;
		isDribbling = false;
		hasTripleThreat = true;
		dunker.gameObject.SetActive(false);
		dunkWindingup = false;
		jumpButtonDown = false;
		jumpButtonUp = false;
		if (grounded)
		{
			rig2D.velocity = new Vector2(0f, rig2D.velocity.y);
		}
		timeWithoutBall = 0f;
		ballJustStolen = false;
		currentAcceleration = 0f;
		numFirstDribbleWindupsThisPossesion = 0;
		gameController.PlayerGotBall();
		gameController.ikBall.FollowTarget(animatedBall);
		string sortingLayerName = ((!isPlayer2) ? "Player" : "Player2");
		gameController.ballTail.trailRenderer.sortingLayerName = sortingLayerName;
		((BallVisual)gameController.ikBall.GetComponent(typeof(BallVisual))).SetSortingLayerName(sortingLayerName);
		gameSounds.Play_slap_deeper();
		FaceCorrectDirection(false);
		if (computerAI != null)
		{
			computerAI.GotBall();
		}
		if (!isComputer)
		{
			/*if (!gameController.twoPlayerMode)
			{
				GameVibrations.Instance().PlayGotBall();
			}*/
			if (gameController.InTutorial || gameController.InScrimmage)
			{
				//FlurryAnalytics.Instance().LogEvent("GOTBALL_PLAYER");
			}
		}
		if (!isPlayer2 && gameController.InTutorial)
		{
			StartCoroutine(tutorial.GotBall());
		}
	}

	public virtual bool IsBeet()
	{
		float x = opponent.gameObject.transform.position.x;
		float x2 = base.gameObject.transform.position.x;
		bool flag = false;
		if (!isPlayer2)
		{
			return x2 >= x;
		}
		return x2 <= x;
	}

	public virtual void SetMove(float m)
	{
		if (jumpShotWindupTimer >= 0.2f)
		{
			moveLast = 0f;
		}
		else
		{
			move = m;
		}
	}

	private void CheckForPerformStepBack()
	{
		if (hasBall && grounded && stepBackTimer >= minTimeBetweenStepBacks && !jumpShotWindingup && !IsMovingAwayFromHoop(moveLast) && hasDribble)
		{
			float distanceToHoop = shooter.GetDistanceToHoop(shooter.gameObject.transform.position);
			if (!(distanceToHoop >= 12.5f))
			{
				isPeformingStepBack = true;
				anim.SetBool("StepBack", true);
				anim.SetTrigger("StepBackTrigger");
				stepBackTimer = 0f;
				anim.SetBool("HasTripleThreat", false);
				hasTripleThreat = false;
				gameSounds.Play_whoose_low_2();
			}
		}
	}

	private void CheckForWindUpFirstDribble()
	{
		if (hasBall && grounded && !isPerformingFirstDribble && !isWindingUpFirstDribble && (!IsMovingTowardHoop(moveLast) || (Math.Abs(rig2D.velocity.x) <= 0f && timeWithBall >= 0.15f)) && (isComputer || timeWithBall > 0.18f) && !isDribbling)
		{
			Debug.Log("Start the WindupUpFirstDribble! + this.timeWithBall=" + timeWithBall + " this.isDribbling=" + isDribbling + " this.rig2D.velocity.x=" + rig2D.velocity.x + " this.moveLast=" + moveLast);
			isWindingUpFirstDribble = true;
			firstDribbleWindupTimer = 0f;
			anim.SetBool("WindingUpFirstDribble", true);
			FaceCorrectDirection(false);
			numFirstDribbleWindupsThisPossesion++;
			gameSounds.Play_whoose_tennis_hit_2();
			if (opponent.computerAI != null)
			{
				opponent.computerAI.OpponentStartedFirstDribbleWindup(numFirstDribbleWindupsThisPossesion);
			}
		}
	}

	private void CancelWindupFirstDribble()
	{
		if (opponent.computerAI != null && firstDribbleWindupTimer >= firstDribbleWindupTimerLength / 2f)
		{
			opponent.computerAI.OpponentCancelledFirstDribbleWindup(numFirstDribbleWindupsThisPossesion);
		}
		isWindingUpFirstDribble = false;
		firstDribbleWindupTimer = 0f;
		currentAcceleration = 0f;
	}

	private bool CheckForPerformFirstDribble()
	{
		bool result = false;
		if (!isPerformingFirstDribble && hasBall && grounded && hasDribble)
		{
			Debug.Log("Start to PerformFirstDribble!");
			result = true;
			isPerformingFirstDribble = true;
			firstDribbleTimer = 0f;
			anim.SetFloat("Speed", firstDribbleSpeed);
			currentAcceleration = 10f;
			gameSounds.Play_whoose_tennis_racket();
		}
		return result;
	}

	private void ResetAnimTriggers()
	{
		anim.ResetTrigger("PickupBall");
		anim.ResetTrigger("PickupBallMiddle");
		anim.ResetTrigger("CatchOnGround");
		anim.ResetTrigger("StartSpringing");
	}

	private bool BallShouldFlicker()
	{
		float num = 1.75f;
		if (isComputer || gameController.twoPlayerMode)
		{
			num = 2.25f;
		}
		if (opponent.sprintBar.isFull && sprintBar.isTired && !jumpShotWindingup && grounded && timeWithBall > num && (!gameController.InScrimmage || isComputer))
		{
			return true;
		}
		return false;
	}

	public virtual void OnTipOff(bool ballToP1, bool ballToP2, bool extraHighBall, bool hardForUserToGet)
	{
		currentAcceleration = 0f;
		if (computerAI != null && computerAI.isActiveAndEnabled)
		{
			StartCoroutine(computerAI.OnTipOff(ballToP1, ballToP2, extraHighBall, hardForUserToGet));
		}
	}

	public virtual float GetDistanceForLayup()
	{
		float num = ((!isPlayer2) ? rig2D.velocity.x : (rig2D.velocity.x * -1f));
		float result = Shooter.LAYUP_DISTANCE;
		if (num > 4f)
		{
			result = Shooter.LAYUP_DISTANCE * 2.5f;
		}
		else if (num < -2f)
		{
			result = 0f;
		}
		return result;
	}

	public virtual bool IsDunk(float distance)
	{
		float num = ((!isPlayer2) ? rig2D.velocity.x : (rig2D.velocity.x * -1f));
		return num > 3.8f && distance > 2.25f && !sprintBar.isTired;
	}

	private bool IsMovingFastTowardHoop()
	{
		float num = ((!isPlayer2) ? rig2D.velocity.x : (rig2D.velocity.x * -1f));
		return num > 3.5f;
	}

	private bool IsMovingTowardHoop(float m)
	{
		return (!isPlayer2 && m > 0f) || (isPlayer2 && m < 0f);
	}

	private bool IsMovingAwayFromHoop(float m)
	{
		return (!isPlayer2 && m < 0f) || (isPlayer2 && m > 0f);
	}

	private void FaceCorrectDirection(bool isBeet)
	{
		if ((hasBall && shooter.GetDistanceToHoop(shooter.gameObject.transform.position) < GetDistanceForLayup() && !isWindingUpFirstDribble) || !grounded)
		{
			return;
		}
		if ((opponent.hasBall && !isBeet) || hasBall)
		{
			if (!isPlayer2 && !facingRight)
			{
				Flip(false);
			}
			if (isPlayer2 && facingRight)
			{
				Flip(false);
			}
		}
		else if (Mathf.Abs(rig2D.velocity.x) < 0.2f)
		{
			if (gameController.ball != null && gameController.ball.rb != null)
			{
				if (gameController.ball.rb.position.x + 2f < rig2D.position.x && facingRight)
				{
					Flip(false);
				}
				else if (gameController.ball.rb.position.x - 2f > rig2D.position.x && !facingRight)
				{
					Flip(false);
				}
			}
		}
		else if (!isPlayer2)
		{
			if (move > 0f && !facingRight)
			{
				Flip(false);
			}
			else if (move < 0f && facingRight)
			{
				Flip(false);
			}
		}
		else if (isPlayer2)
		{
			if (move < 0f && facingRight)
			{
				Flip(false);
			}
			else if (move > 0f && !facingRight)
			{
				Flip(false);
			}
		}
	}

	private bool IsDribbling()
	{
		bool result = false;
		if (isPerformingFirstDribble || isPeformingStepBack)
		{
			result = true;
		}
		else if (!isWindingUpFirstDribble && !dunkWindingup && !jumpShotWindingup && Math.Abs(move) > 0f && hasDribble && timeWithBall > timeToGatherBall)
		{
			result = true;
		}
		return result;
	}

	private void SetSprintBarGeneration()
	{
		if (hasBall && Math.Abs(move) > 0f && !jumpShotWindingup)
		{
			if (hasDribble)
			{
				if (opponent.IsBeet())
				{
					if (hasBeetDefender)
					{
						return;
					}
					hasBeetDefender = true;
					anim.SetTrigger("BeetDefender");
					if (timeWithBall > 1f && IsMovingTowardHoop(move))
					{
						gameSounds.Play_whoose_low();
						if (!isPerformingFirstDribble)
						{
							gameController.voiceOvers.PlayCrossesOver();
						}
					}
				}
				else
				{
					if (isWindingUpFirstDribble || isPerformingFirstDribble)
					{
						return;
					}
					sprintBar.generate = -1f;
					if (hasBeetDefender)
					{
						hasBeetDefender = false;
						anim.SetTrigger("DefenderBackInFront");
						if (timeWithBall > 1f)
						{
							gameSounds.Play_whoose_low();
						}
					}
				}
			}
			else if (isWindingUpFirstDribble && firstDribbleWindupTimer < firstDribbleWindupTimerLength / 2f)
			{
				sprintBar.generate = -1.15f;
			}
			else
			{
				sprintBar.generate = 0f;
			}
		}
		else if (sneakyFastEnergyWhenOpponentTired && opponentSprintBar.isTired && grounded)
		{
			sprintBar.generate = energyRegenerationSpeed * 2.25f;
		}
		else if (jumpShotWindingup)
		{
			sprintBar.generate = -1f;
		}
		else if (springing)
		{
			sprintBar.generate = -1.5f;
		}
		else if (!lastAirWasFromShot && !grounded)
		{
			sprintBar.generate = -1f;
		}
		else if (move != 0f)
		{
			if (opponentSprintBar.isTired)
			{
				sprintBar.generate = 0f;
			}
			else
			{
				sprintBar.generate = 0f;
			}
		}
		else if (!hasBall)
		{
			if (grounded && timeOnGround > 0.25f)
			{
				sprintBar.generate = energyRegenerationSpeed;
			}
		}
		else if (isComputer)
		{
			float generate = -0.4f;
			sprintBar.generate = generate;
		}
		else
		{
			sprintBar.generate = -0.2f;
		}
	}

	public virtual float GetTimeSinceStartOfPlay()
	{
		return timeSinceStartOfPlay;
	}

	public virtual float GetTimeSinceLastShot()
	{
		return timeSinceLastShot;
	}

	public virtual IEnumerator CelebrateWin()
	{
		sprintBar.gameObject.SetActive(false);
		gameController.gameRoster.hydrationWarning.gameObject.SetActive(false);
		celebrationSpring = true;
		timeSinceLastSpring = 0f;
		anim.SetBool("GameOver", true);
		yield return new WaitForSeconds(0.5f);
		springing = true;
		anim.SetTrigger("StartSpringing");
		anim.SetBool("Springing", true);
	}

	public bool IsPerformingStepBack()
	{
		return isPeformingStepBack;
	}

	public virtual bool IsFacingRight()
	{
		return facingRight;
	}

	public virtual void SetJumpButtonDown(bool j)
	{
		jumpButtonDown = j;
	}

	public virtual bool GetJumpButtonDown()
	{
		return jumpButtonDown;
	}

	public virtual void SetJumpButtonUp(bool j)
	{
		jumpButtonUp = j;
	}

	public virtual bool GetJumpButtonUp()
	{
		return jumpButtonUp;
	}

	public virtual void SetJumpForce(float f)
	{
		jumpForce = f;
	}

	public virtual void SetEnergyRegenerationSpeed(float s)
	{
		energyRegenerationSpeed = s;
	}

	public virtual void SetDefendedMultiplier(float d)
	{
		defendedMultiplier = d;
	}

	public float GetDefendedMultiplier()
	{
		return defendedMultiplier;
	}

	public virtual void SetSize(float s)
	{
		size = s;
	}

	public float GetTimeNotGrounded()
	{
		return timeNotGrounded;
	}

	public float GetJumpShotWindupTimeLength()
	{
		return jumpShotWindupTimeLength;
	}

	public float GetFirstDribbleWindupTimerLength()
	{
		return firstDribbleWindupTimerLength;
	}

	public bool GetIsPerformingFirstDribble()
	{
		return isPerformingFirstDribble;
	}

	public float GetBufferAfterShootingBall()
	{
		return bufferAfterShootingBall;
	}
}
