using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class ComputerAI : MonoBehaviour
{
	public PlayerController pc;

	public Text inputDebug;

	public GameComputer gameComputer;

	public PlayerState opponentState;

	private int xMovement;

	private float xMovementChangeUpdateFreq = 0.15f;

	private float xMovementChangeUpdateTimer;

	private float xMovementChangeUpdateTimerResetValue;

	private bool jumpButton;

	private bool jumpButtonLast;

	private float secondsToHoldJumpButton;

	private bool holdJumpButtonUntilPeak;

	private float timeToWaitBeforeCanMove = 0.55f;

	private float shootCurrentDestination;

	private float secondsTravelingToCurrentShootDest;

	private float maxSecondsTravelingToShootDest = 20f;

	private float maxSecondsStuckDribblingIntoDefender = 3f;

	private bool pumpFaking;

	private int numPumpFakes;

	private float secondsSincePumpFaking;

	private float delayAfterPumpFaking;

	private bool aboutToShoot;

	private bool aboutToSpring;

	private float maxTimeWithBall = 20f;

	private bool shootingEmergencyShot;

	private bool shotEmergencyShot;

	private int numStepBacks;

	private int numStepBackReqBeforeShooting = 10;

	private bool lastMoveWasStepback;

	private bool isJabStepping;

	private float secondsJabStepping;

	private int defense;

	private float defenseTimer;

	private float defenseLength = 3f;

	private int offenseLevel;

	private int defenseLevel;

	private float anticipateShotTime;

	private bool canWin;

	private bool computerNoWayPhysicallyPossibleToWin;

	private bool shouldWin;

	private bool defeatJoeOffense;

	private bool defeatJoeDefense;

	private bool defendingLayup;

	private Ball ball;

	private static int DEFENSE_STANDARD;

	private static int DEFENSE_NO_JUMPSHOT = 1;

	private static int DEFENSE_NO_DRIVE = 2;

	private static int DEFENSE_STEEL = 3;

	public virtual IEnumerator Start()
	{
		yield return new WaitForSeconds(0.75f);
		SetStats();
	}

	public virtual void FixedUpdate()
	{
		secondsSincePumpFaking += Time.deltaTime;
		if (pc.hasBall)
		{
			secondsTravelingToCurrentShootDest += Time.deltaTime;
		}
		else
		{
			secondsTravelingToCurrentShootDest = 0f;
			aboutToShoot = false;
		}
		if (pc.secondsStuckDribblingIntoDefender > maxSecondsStuckDribblingIntoDefender)
		{
			Debug.Log("Computer stuck dribbling into defender for " + pc.secondsStuckDribblingIntoDefender + " seconds");
			Debug.Log("Which is more than max: " + maxSecondsStuckDribblingIntoDefender);
			if (pc.gameController.GetCurrentHalf() == 1)
			{
				Debug.Log("DO STEPBACK JUMP SHOT");
				shootCurrentDestination = GetDestForStepBack();
			}
			else
			{
				Debug.Log("DO STEPBACK AND BACK A FEW MORE STEPS BEFORE SHOT");
				shootCurrentDestination = GetDestForStepBack() + 2f;
			}
		}
		else if (secondsTravelingToCurrentShootDest >= maxSecondsTravelingToShootDest)
		{
			Debug.Log("COMPUTER HAS TAKEN TOO LONG, TRY TO SHOOT THE SHOT HERE: ");
			Debug.Log("(this.secondsTravelingToCurrentShootDest >= this.maxSecondsTravelingToShootDest:" + secondsTravelingToCurrentShootDest);
			shootCurrentDestination = pc.shooter.GetDistanceToHoop(pc.shooter.gameObject.transform.position);
			Debug.Log("SHOOT IT HERE: this.shootCurrentDestination = " + shootCurrentDestination);
			secondsTravelingToCurrentShootDest = 0f;
		}
		SetDefense();
		SetMovement();
		if (pc.timeWithBall > maxTimeWithBall)
		{
			maxTimeWithBall -= 5f;
			if (maxTimeWithBall < 5f)
			{
				maxTimeWithBall = 5f;
			}
			StartCoroutine(ShootEmergencyShot());
		}
		if (holdJumpButtonUntilPeak)
		{
			if (pc.rig2D.velocity.y <= -0.1f && pc.GetTimeNotGrounded() > 0.15f)
			{
				jumpButton = false;
				holdJumpButtonUntilPeak = false;
			}
			else
			{
				jumpButton = true;
			}
		}
		else if (secondsToHoldJumpButton > 0f)
		{
			if (!pc.IsPerformingStepBack())
			{
				secondsToHoldJumpButton -= Time.deltaTime;
			}
			jumpButton = true;
		}
		else
		{
			jumpButton = false;
			if (pumpFaking)
			{
				CompletedPumpFake();
			}
		}
	}

	public virtual void Update()
	{
		if (!(Time.timeScale <= 0f))
		{
			InputMovesIntoController();
		}
	}

	private void InputMovesIntoController()
	{
		pc.SetMove(xMovement);
		pc.SetJumpButtonDown(false);
		pc.SetJumpButtonUp(false);
		if (jumpButton != jumpButtonLast)
		{
			if (jumpButton)
			{
				pc.SetJumpButtonDown(true);
			}
			else
			{
				pc.SetJumpButtonUp(true);
			}
			jumpButtonLast = jumpButton;
		}
		DisplayInputDebug();
	}

	public virtual IEnumerator OnTipOff(bool ballToP1, bool ballToP2, bool extraHighBall, bool hardForUserToGet)
	{
		if (ballToP1)
		{
			timeToWaitBeforeCanMove = 1f;
		}
		else
		{
			timeToWaitBeforeCanMove = 0.55f;
		}
		shootingEmergencyShot = false;
		shotEmergencyShot = false;
		SetStats();
		float springDelay2 = 0f;
		if (!ballToP2)
		{
			springDelay2 = ((!hardForUserToGet) ? 0.75f : ((UnityEngine.Random.Range(0, 100) < 50) ? 0.55f : 0.42f));
		}
		else
		{
			springDelay2 = (extraHighBall ? ((!hardForUserToGet) ? ((UnityEngine.Random.Range(0, 100) < 50) ? 0.7f : 0.6f) : ((UnityEngine.Random.Range(0, 100) < 50) ? 0.27f : 0.14f)) : ((!hardForUserToGet) ? 0.3f : ((UnityEngine.Random.Range(0, 100) < 50) ? 0.12f : 0.08f)));
			timeToWaitBeforeCanMove = springDelay2 + 0.25f;
		}
		Debug.Log("======= ComputerAI.OnTipOff() springDelay: " + springDelay2 + " (IS extraHighBall = " + extraHighBall + ")");
		if (!ballToP1)
		{
			if (springDelay2 > 0f)
			{
				yield return new WaitForSeconds(springDelay2);
			}
			StartCoroutine(Spring(true));
		}
	}

	private void SetStats()
	{
		if (!(gameComputer == null))
		{
			offenseLevel = gameComputer.GetOffenseLevel();
			defenseLevel = gameComputer.GetDefenseLevel();
			canWin = gameComputer.CanWin();
			computerNoWayPhysicallyPossibleToWin = gameComputer.NoWayPhysicallyPossibleToWin();
			shouldWin = gameComputer.ShouldWin();
			defeatJoeOffense = gameComputer.IsDefeatJoeOffense();
			defeatJoeDefense = gameComputer.IsDefeatJoeDefense();
			xMovementChangeUpdateTimerResetValue = gameComputer.XMovementChangeUpdateTimerResetValue();
			maxSecondsStuckDribblingIntoDefender = gameComputer.MaxSecondsStuckDribblingIntoDefender();
		}
	}

	private void SetMovement()
	{
		if (pc.GetTimeSinceStartOfPlay() < timeToWaitBeforeCanMove)
		{
			xMovement = 0;
			return;
		}
		if (isJabStepping)
		{
			if (!pc.hasBall)
			{
				isJabStepping = false;
				secondsJabStepping = 0f;
			}
			if ((double)pc.timeWithBall > 0.25 && pc.grounded)
			{
				secondsJabStepping += Time.deltaTime;
				xMovement = -1;
			}
			else
			{
				xMovement = 0;
			}
			if (secondsJabStepping >= pc.GetFirstDribbleWindupTimerLength() + 0.1f)
			{
				isJabStepping = false;
				secondsJabStepping = 0f;
			}
			else if (secondsJabStepping >= pc.GetFirstDribbleWindupTimerLength() - 0.1f)
			{
				if (DrivingLanePossiblyOpen() && pc.hasDribble)
				{
					xMovement = -1;
					Debug.Log("SetShootDest() [SetMovement() -> DrivingLanePossiblyOpen() == true]");
					SetShootDest();
				}
				else
				{
					xMovement = 0;
				}
			}
			return;
		}
		int m = xMovement;
		float x = pc.gameObject.transform.position.x;
		m = ((!pc.hasBall) ? MoveWithoutBall(m, x) : MoveWithBall(m, x));
		bool flag = false;
		if (defeatJoeOffense && (pc.hasBall || !pc.opponent.hasBall))
		{
			flag = true;
		}
		else if (defeatJoeDefense && pc.opponent.hasBall)
		{
			flag = true;
		}
		if (xMovementChangeUpdateTimer > xMovementChangeUpdateFreq || flag)
		{
			if (pc.timeOnGround >= 0.05f || !pc.opponent.hasBall)
			{
				xMovement = m;
			}
			xMovementChangeUpdateTimer = xMovementChangeUpdateTimerResetValue;
		}
		xMovementChangeUpdateTimer += Time.deltaTime;
	}

	private int MoveWithBall(int m, float playerX)
	{
		if (secondsSincePumpFaking < delayAfterPumpFaking)
		{
			return 0;
		}
		if (aboutToShoot)
		{
			return m;
		}
		float distanceToHoop = pc.shooter.GetDistanceToHoop(pc.shooter.gameObject.transform.position);
		float num = 0.5f;
		if (defeatJoeOffense && pc.hasDribble && (shootCurrentDestination > 4f || shootCurrentDestination <= 0f) && DriveToTheRack())
		{
			Debug.Log("SetShootDest() [MoveWithBall() -> DriveToTheRack() == true]");
			SetShootDest();
		}
		else
		{
			bool flag = false;
			if (defeatJoeOffense && (IsOpenForStartingShotWindup() || pc.hasTripleThreat) && IsReasonableShotDistance(distanceToHoop) && !IsDrivingLaneOpen() && (numStepBacks >= numStepBackReqBeforeShooting || !pc.sprintBar.isFull || !pc.hasDribble))
			{
				flag = true;
			}
			else if (shootCurrentDestination - num < distanceToHoop && shootCurrentDestination + num > distanceToHoop)
			{
				if (numStepBacks >= numStepBackReqBeforeShooting || !pc.sprintBar.isFull || pc.hasTripleThreat || !pc.hasDribble || distanceToHoop <= pc.GetDistanceForLayup())
				{
					flag = true;
				}
				else
				{
					Debug.Log("this.numStepBacks=" + numStepBacks + " this.numStepBackWhenGotBall=" + numStepBackReqBeforeShooting);
				}
			}
			if (flag && pc.IsGrounded())
			{
				Debug.Log("IN POSITION TO SHOOT (distance: " + distanceToHoop + " this.shootCurrentDestination: " + shootCurrentDestination);
				if (ShouldJabStep())
				{
					JabStep();
				}
				else
				{
					Shoot();
				}
			}
		}
		if (distanceToHoop - num > shootCurrentDestination && playerX > pc.shooter.shootTarget.position.x)
		{
			m = -1;
		}
		else if (distanceToHoop + num < shootCurrentDestination || playerX < pc.shooter.shootTarget.position.x)
		{
			m = 1;
		}
		else
		{
			if (!pc.IsGrounded() && defeatJoeOffense)
			{
				m = 0;
			}
			if (!aboutToShoot && pc.grounded)
			{
				Debug.Log("SetShootDest() [MoveWithBall()]");
				SetShootDest();
			}
		}
		return m;
	}

	private int MoveWithoutBall(int m, float playerX)
	{
		float num = 0f;
		if (pc.opponent.hasBall)
		{
			float x = pc.opponent.gameObject.transform.position.x;
			num = (pc.opponent.isUnderHoop ? (x + 0.9f) : ((defense == DEFENSE_NO_JUMPSHOT) ? (x + 0.5f) : ((defense == DEFENSE_STEEL) ? (x - 2.5f) : ((defense != DEFENSE_NO_DRIVE) ? (x + 1.1f) : (x + 2.5f)))));
			bool flag = playerX > pc.maxX && x + 1.1f > playerX;
			bool flag2 = false;
			if (pc.opponent.jumpShotWindingup && !pc.springing && !aboutToSpring)
			{
				flag2 = true;
			}
			if (defeatJoeDefense && pc.opponent.jumpShotWindupTimer > 0.05f)
			{
				defense = DEFENSE_NO_JUMPSHOT;
			}
			if (flag2 && pc.opponent.jumpShotWindupTimer > anticipateShotTime)
			{
				Debug.Log("ComputerAI anticipate the shot (anticipateShotTime: " + anticipateShotTime + ")");
				StartCoroutine(Spring(false));
			}
			else if (flag)
			{
				m = 0;
			}
			else if (num + 0.1f < playerX)
			{
				m = -1;
			}
			else if (num - 0.1f > playerX)
			{
				m = 1;
			}
			else
			{
				if (pc.grounded)
				{
					m = 0;
				}
				if (defenseTimer >= 0.5f)
				{
					defense = DEFENSE_STANDARD;
				}
			}
		}
		else
		{
			if (ball == null)
			{
				ball = pc.gameController.ball;
			}
			if (ball != null && pc.timeOnGround >= GetFetchBallDelay())
			{
				float x2 = ball.gameObject.transform.position.x;
				float num2 = 0.1f;
				if (ball.gameObject.transform.position.y > -1f)
				{
					num2 = 0.5f;
				}
				m = ((playerX < x2 - num2) ? 1 : ((playerX > x2 + num2) ? (-1) : 0));
				if (!(playerX < x2 - num2 * 10f) && !(playerX > x2 + num2 * 10f) && !(x2 >= 5.75f) && ball.gameObject.transform.position.y >= -0.95f && defenseLevel >= 10 && pc.GetTimeSinceStartOfPlay() > 3f)
				{
					StartCoroutine(Spring(true));
				}
			}
			else if (ball != null && (ball.secondsAlive > 0.75f || pc.GetTimeSinceLastShot() <= 0.75f))
			{
				m = 0;
			}
		}
		return m;
	}

	private void SetDefense()
	{
		defenseTimer += Time.deltaTime;
		if (!(defenseTimer > defenseLength))
		{
			return;
		}
		defenseTimer = 0f;
		if (xMovement != 0)
		{
			SetNewDefenseLocation();
			SetAnticipateShotTime();
		}
		else if (pc.opponent.sprintBar.isTired && pc.opponent.timeWithBall > 2.25f && !pc.sprintBar.isTired && (UnityEngine.Random.Range(0, 100) >= 50 || defeatJoeDefense))
		{
			defense = DEFENSE_STEEL;
		}
		else if (UnityEngine.Random.Range(0, 100) >= 25)
		{
			SetNewDefenseLocation();
			SetAnticipateShotTime();
		}
		PlayerState.Location currentLocation = opponentState.currentLocation;
		if (!pc.opponent.isUnderHoop || (defeatJoeDefense && UnityEngine.Random.Range(0, 100) < 50))
		{
			return;
		}
		switch (currentLocation)
		{
		case PlayerState.Location.DirectlyOnTopOfBall:
			if (UnityEngine.Random.Range(0, 100) < 90)
			{
				break;
			}
			goto case PlayerState.Location.DirectlyBehindBall;
		case PlayerState.Location.DirectlyBehindBall:
		case PlayerState.Location.FarBehindBall:
			StartCoroutine(Spring(true));
			break;
		}
	}

	private void SetNewDefenseLocation()
	{
		int num = UnityEngine.Random.Range(0, 3);
		if (num == 0 || defeatJoeDefense)
		{
			defense = DEFENSE_STANDARD;
			return;
		}
		switch (num)
		{
		case 1:
			defense = DEFENSE_NO_JUMPSHOT;
			break;
		case 2:
			defense = DEFENSE_NO_DRIVE;
			break;
		}
	}

	private void SetAnticipateShotTime()
	{
		int num = UnityEngine.Random.Range(0, 100);
		if (num >= 66 || (defeatJoeDefense && num >= 5) || pc.opponent.shooter.GetNumPumpFakes() >= 4)
		{
			anticipateShotTime = 0.38f;
		}
		else if (num >= 33)
		{
			anticipateShotTime = 0.25f;
		}
		else
		{
			anticipateShotTime = 0.15f;
		}
	}

	private bool ShouldJabStep()
	{
		bool result = false;
		if (pc.isDribbling)
		{
			return false;
		}
		if (defeatJoeOffense && opponentState.currentLocation == PlayerState.Location.DirectlyInFrontOfBall && pc.sprintBar.isFull && pc.hasDribble && UnityEngine.Random.Range(0, 100) >= 50)
		{
			result = true;
		}
		if (UnityEngine.Random.Range(0, 100) >= 80 && !IsOpenForStartingShotWindup() && (pc.sprintBar.isFull || pc.hasDribble))
		{
			result = true;
		}
		return result;
	}

	private void JabStep()
	{
		Debug.Log("JabStep()");
		secondsJabStepping = 0f;
		isJabStepping = true;
	}

	private void Shoot()
	{
		Debug.Log("ComputerAI.Shoot()");
		if (pc.jumpShotWindingup || pumpFaking || aboutToShoot || jumpButton || shootingEmergencyShot || gameComputer.NeverShoot())
		{
			Debug.Log("Shoot() -> CANNOT SHOOT:" + pc.jumpShotWindingup + pumpFaking + aboutToShoot + jumpButton + shootingEmergencyShot + gameComputer.NeverShoot());
		}
		else
		{
			float distanceToHoop = pc.shooter.GetDistanceToHoop(pc.shooter.gameObject.transform.position);
			if (distanceToHoop < pc.GetDistanceForLayup())
			{
				StartCoroutine(ShootLayup(distanceToHoop));
			}
			else
			{
				StartCoroutine(ShootJumpShot());
			}
		}
	}

	private IEnumerator ShootLayup(float distance)
	{
		aboutToShoot = true;
		float delay = 0f;
		float layupLength = ((UnityEngine.Random.Range(0, 100) < 50) ? 0.32f : 0.25f);
		float minTimeWithBallForLayup = GetMinTimeWithBallForLayup();
		if (pc.timeWithBall < minTimeWithBallForLayup)
		{
			delay = minTimeWithBallForLayup - pc.timeWithBall;
		}
		if (distance >= 2f)
		{
			delay = 0f;
			layupLength = ((UnityEngine.Random.Range(0, 100) <= 50) ? 0.25f : 0.4f);
		}
		else
		{
			xMovement = 0;
		}
		Debug.Log("ShootLayup() -> layupLength=" + layupLength + " delay=" + delay + " distance=" + distance);
		yield return new WaitForSeconds(delay);
		distance = pc.shooter.GetDistanceToHoop(pc.shooter.gameObject.transform.position);
		if (pc.IsDunk(distance))
		{
			layupLength = 0.65f;
		}
		if (distance < pc.GetDistanceForLayup())
		{
			if ((defeatJoeOffense && pc.sprintBar.isTired) || (pc.IsPerformingStepBack() && canWin))
			{
				holdJumpButtonUntilPeak = true;
			}
			else
			{
				secondsToHoldJumpButton = layupLength;
			}
		}
		else
		{
			Debug.Log("Computer moved out of layup range. Shoot jumpshot instead");
			if (defeatJoeOffense)
			{
				secondsToHoldJumpButton = GetSecondsToHoldPumpfake();
			}
			else
			{
				secondsToHoldJumpButton = GetShotLength();
			}
		}
		aboutToShoot = false;
	}

	private IEnumerator ShootJumpShot()
	{
		Debug.Log("COMPUTER SHOOT JUMPSHOT");
		aboutToShoot = true;
		yield return new WaitForSeconds(GetShootDelay());
		int ran = UnityEngine.Random.Range(0, 100);
		bool pumpFake = (ran >= 50 || pc.hasDribble) && pc.IsCloselyDefended() && !pc.gameController.InScrimmage;
		if (defeatJoeOffense)
		{
			pumpFake = true;
		}
		if (pumpFake)
		{
			pumpFaking = true;
			secondsToHoldJumpButton = GetSecondsToHoldPumpfake();
		}
		else
		{
			secondsToHoldJumpButton = GetShotLength();
			xMovement = 0;
		}
		aboutToShoot = false;
	}

	private float GetSecondsToHoldPumpfake()
	{
		float jumpShotWindupTimeLength = pc.GetJumpShotWindupTimeLength();
		int num = UnityEngine.Random.Range(0, 100);
		if (num >= 66)
		{
			return jumpShotWindupTimeLength - 0.15f;
		}
		if (num >= 33)
		{
			return jumpShotWindupTimeLength - 0.2f;
		}
		return jumpShotWindupTimeLength - 0.35f;
	}

	private void CompletedPumpFake()
	{
		pumpFaking = false;
		numPumpFakes++;
		if (ShouldConvertPumpFakeToShot())
		{
			holdJumpButtonUntilPeak = true;
			jumpButton = true;
			return;
		}
		secondsSincePumpFaking = 0f;
		if (defeatJoeOffense && UnityEngine.Random.Range(0, 100) >= 25)
		{
			delayAfterPumpFaking = ((UnityEngine.Random.Range(0, 100) < 50) ? 0.35f : 0.15f);
		}
		else
		{
			delayAfterPumpFaking = 0.5f;
		}
		Debug.Log("SetShootDest() [CompletedPumpFake() -> cancelled the pumpfake]");
		SetShootDest();
	}

	private IEnumerator Spring(bool noDelay)
	{
		if (!noDelay && (pc.springing || pc.gameController.InScrimmage || pc.timeWithoutBall < 0.75f || shootingEmergencyShot))
		{
			Debug.Log("DONT SPRING: " + pc.springing + " " + pc.gameController.InScrimmage + " " + (pc.timeWithoutBall < 0.75f) + " " + (pc.timeOnGround < 0.15f) + " " + shootingEmergencyShot);
			yield break;
		}
		xMovementChangeUpdateTimer = xMovementChangeUpdateTimerResetValue;
		aboutToSpring = true;
		float delay = 0f;
		if (!noDelay)
		{
			delay = GetSpringDelay();
		}
		if (delay > 0f)
		{
			yield return new WaitForSeconds(delay);
		}
		if (noDelay || UnityEngine.Random.Range(0, 100) < 90 || (defeatJoeDefense && UnityEngine.Random.Range(0, 100) < 75))
		{
			secondsToHoldJumpButton = 0.65f;
		}
		aboutToSpring = false;
	}

	private float GetShotLength()
	{
		bool flag = false;
		bool flag2 = false;
		if (UnityEngine.Random.Range(0, 100) < offenseLevel)
		{
			flag = true;
		}
		if (!canWin && pc.gameController.ComputerCloseToWinning())
		{
			flag2 = true;
		}
		float num = 0f;
		if (flag2)
		{
			num = ((UnityEngine.Random.Range(0, 100) < 50) ? 1f : 0.6f);
		}
		else if (!flag)
		{
			num = ((UnityEngine.Random.Range(0, 100) < 50) ? UnityEngine.Random.Range(0.96f, 1f) : UnityEngine.Random.Range(0.6f, 0.84f));
		}
		else
		{
			num = UnityEngine.Random.Range(0.84f, 0.98f);
			if (pc.sprintBar.isTired)
			{
				num -= 0.15f;
			}
		}
		Debug.Log("shootBadShot: " + flag2);
		Debug.Log("===========NEW SHOT LENGTH: " + num + " VELOCITY.Y: " + pc.rig2D.velocity.y);
		return num;
	}

	private float GetSpringDelay()
	{
		float num = 0f;
		if (defendingLayup)
		{
			num = ((defeatJoeDefense && UnityEngine.Random.Range(0, 100) >= 2) ? 0.01f : ((!pc.gameController.ComputerCloseToWinning()) ? ((UnityEngine.Random.Range(0, 100) >= 50) ? 0.08f : ((UnityEngine.Random.Range(0, 100) < 50) ? 0.3f : 0.01f)) : 0.4f));
			defendingLayup = false;
		}
		else if (UnityEngine.Random.Range(1, 100) < defenseLevel || defeatJoeDefense)
		{
			num = ((UnityEngine.Random.Range(0, 100) < 50) ? 0.1f : 0.15f);
			Debug.Log("ComputerAI: GREAT BLOCK ATTEMPT delay=" + num);
		}
		else
		{
			num = ((UnityEngine.Random.Range(0, 100) < 50) ? 0.55f : 0.35f);
			Debug.Log("ComputerAI: POOR BLOCK ATTEMPT delay=" + num);
		}
		if (UnityEngine.Random.Range(0, 100) >= 90 && (!defeatJoeDefense || UnityEngine.Random.Range(0, 100) >= 75))
		{
			num = ((UnityEngine.Random.Range(0, 100) < 50) ? 0.05f : 0.01f);
		}
		Debug.Log("ComputerAI GetSpringDelay(): delay=" + num);
		return num;
	}

	private float GetDestForStepBack()
	{
		float distanceToHoop = pc.shooter.GetDistanceToHoop(pc.shooter.gameObject.transform.position);
		if (distanceToHoop < 11f)
		{
			return distanceToHoop + 1f;
		}
		return distanceToHoop + 0.05f;
	}

	private float GetDestForCrossOver()
	{
		return pc.shooter.GetDistanceToHoop(pc.shooter.gameObject.transform.position) - 1.25f;
	}

	private float GetFetchBallDelay()
	{
		float num = 0f;
		if (pc.GetTimeSinceStartOfPlay() < timeToWaitBeforeCanMove + 0.5f)
		{
			num = 0f;
		}
		else if (!pc.lastAirWasFromShot)
		{
			num = ((offenseLevel < 20) ? 0.5f : 0f);
		}
		else
		{
			num = 0.35f + (float)(100 - offenseLevel) * 0.01f;
			if (defeatJoeOffense)
			{
				num = 0.08f;
			}
			else if (offenseLevel >= 100)
			{
				num = 0.2f;
			}
		}
		if (pc.gameController.InScrimmage)
		{
			num = 3.5f;
		}
		return num;
	}

	private float GetMinTimeWithBallForLayup()
	{
		if (defeatJoeOffense)
		{
			float num = ((UnityEngine.Random.Range(0, 100) <= 50) ? 0.75f : 0.45f);
		}
		else
		{
			float num = ((UnityEngine.Random.Range(0, 100) <= 50) ? 1.15f : 0.65f);
		}
		return 0.45f;
	}

	private float GetShootDelay()
	{
		float num = (float)(100 - offenseLevel) * 0.01f;
		if (UnityEngine.Random.Range(0, 100) >= 80)
		{
			num = 0.01f;
		}
		if (num < 0.5f && pc.timeWithBall < 0.5f)
		{
			num = 0.5f - pc.timeWithBall;
		}
		if (defeatJoeOffense)
		{
			num = ((!(pc.timeOnGround < 0.5f) && !(pc.timeWithBall < 0.5f)) ? 0.01f : 0.35f);
		}
		return num;
	}

	private void SetShootDest()
	{
		float x = pc.opponent.gameObject.transform.position.x;
		float x2 = pc.gameObject.transform.position.x;
		float distanceToHoop = pc.shooter.GetDistanceToHoop(pc.shooter.gameObject.transform.position);
		float num = shootCurrentDestination;
		if (!pc.hasDribble)
		{
			Debug.Log("computer has got stuck in a spot wihout its dribble, make them shoot");
			num = distanceToHoop;
		}
		else if (DriveToTheRack())
		{
			int num2 = UnityEngine.Random.Range(0, 100);
			num = ((num2 > 66) ? 3.35f : ((num2 <= 33 && !defeatJoeOffense) ? 2.15f : 2.75f));
			if (distanceToHoop < num)
			{
				num = distanceToHoop;
			}
		}
		else if (pc.hasTripleThreat && numPumpFakes == 0 && distanceToHoop > Shooter.LAYUP_DISTANCE + 2f && UnityEngine.Random.Range(0, 100) <= offenseLevel)
		{
			num = distanceToHoop;
			if (num > Shooter.THREEPOINT_DISTANCE + 2.1f && UnityEngine.Random.Range(0, 50) <= offenseLevel)
			{
				num = UnityEngine.Random.Range(7f, 10.75f);
			}
		}
		else if (pc.hasDribble && (numStepBacks < numStepBackReqBeforeShooting || !pc.sprintBar.isFull) && (!defeatJoeOffense || !pc.sprintBar.isTired) && (canWin || !pc.gameController.ComputerCloseToWinning()))
		{
			if (!lastMoveWasStepback)
			{
				num = GetDestForStepBack();
				numStepBacks++;
				lastMoveWasStepback = true;
			}
			else
			{
				num = GetDestForCrossOver();
				lastMoveWasStepback = false;
			}
		}
		else if ((UnityEngine.Random.Range(0, 100) >= 50 && !pumpFaking && !IsDrivingLaneOpen()) || shootCurrentDestination <= 0f || (shootCurrentDestination < 8f && !canWin && pc.gameController.ComputerCloseToWinning()))
		{
			if (canWin || !pc.gameController.ComputerCloseToWinning())
			{
				float min = 0.2f;
				if (pc.sprintBar.isTired)
				{
					min = 8f;
				}
				num = UnityEngine.Random.Range(min, 11f);
				if (num > 8.5f && num < Shooter.THREEPOINT_DISTANCE + 0.1f && UnityEngine.Random.Range(0, 100) <= offenseLevel)
				{
					num = Shooter.THREEPOINT_DISTANCE + 0.3f;
				}
			}
			else
			{
				num = UnityEngine.Random.Range(8f, 10.5f);
			}
		}
		if (pc.sprintBar.isTired || UnityEngine.Random.Range(0, 100) >= 75)
		{
			maxSecondsTravelingToShootDest = 1f;
		}
		else if (UnityEngine.Random.Range(0, 100) >= 50)
		{
			maxSecondsTravelingToShootDest = 2.5f;
		}
		else
		{
			maxSecondsTravelingToShootDest = 5.5f;
		}
		Debug.Log("New shoot dest: this.shootCurrentDestination = " + num + " prev = " + shootCurrentDestination + " currentDistance = " + distanceToHoop);
		shootCurrentDestination = num;
		secondsTravelingToCurrentShootDest = 0f;
	}

	private bool DriveToTheRack()
	{
		return (IsDrivingLaneOpen() || (pc.GetIsPerformingFirstDribble() && DrivingLanePossiblyOpen())) && UnityEngine.Random.Range(0, 100) < offenseLevel && (canWin || !pc.gameController.ComputerCloseToWinning()) && shootCurrentDestination >= 4f;
	}

	private IEnumerator ShootEmergencyShot()
	{
		if (!shootingEmergencyShot && !shotEmergencyShot && !gameComputer.NeverShoot())
		{
			Debug.Log("EMERGENCY SHOT!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
			Debug.Log("MAX TIME WITH BALL REACHED: MAX TIME = " + maxTimeWithBall);
			Debug.Log("EMERGENCY SHOT!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
			Debug.Log("EMERGENCY SHOT!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
			Debug.Log("EMERGENCY SHOT!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
			Debug.Log("EMERGENCY SHOT!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
			Debug.Log("EMERGENCY SHOT!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
			Debug.Log("EMERGENCY SHOT!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
			shootingEmergencyShot = true;
			secondsToHoldJumpButton = 2f;
			Debug.Log("FINISHED SHOOTING EMERGENCY SHOT!!!!!!");
			shotEmergencyShot = true;
			shootingEmergencyShot = false;
		}
		yield break;
	}

	public virtual void GotBall()
	{
		numPumpFakes = 0;
		numStepBacks = 0;
		lastMoveWasStepback = UnityEngine.Random.Range(0, 100) >= 50;
		if (pc.sprintBar.isFull && defeatJoeOffense)
		{
			numStepBackReqBeforeShooting = UnityEngine.Random.Range(0, 6);
		}
		else
		{
			numStepBackReqBeforeShooting = UnityEngine.Random.Range(0, 3);
		}
		Debug.Log("SetShootDest() [GotBall()]");
		SetShootDest();
	}

	public virtual void OpponentPumpFaked(int numPumpFakes)
	{
		if (UnityEngine.Random.Range(0, 100) >= gameComputer.GetChanceOfGoingForPumpFake() || (defense == DEFENSE_STANDARD && numPumpFakes > 3))
		{
			Debug.Log("ComputerAI.js: Don't go for pumpfake");
		}
		else if (pc.timeOnGround >= 0.15f || UnityEngine.Random.Range(0, 100) >= 50)
		{
			Debug.Log("ComputerAI.js: GO FOR PUMP FAKE (this.xMovement: " + xMovement + ")");
			StartCoroutine(Spring(false));
			if (xMovement == 1)
			{
				xMovement = 0;
			}
			xMovementChangeUpdateTimer = xMovementChangeUpdateTimerResetValue;
		}
	}

	public void OpponentStartedFirstDribbleWindup(int numWindups)
	{
		Debug.Log("OpponentStartedFirstDribbleWindup(): numWindups=" + numWindups);
		if (pc.grounded && !pc.springing && !pc.opponent.sprintBar.isTired && pc.opponent.hasTripleThreat && (UnityEngine.Random.Range(0, 100) >= 50 || true))
		{
			Debug.Log("Prepare for first dribble Set defense = ComputerAI.DEFENSE_NO_DRIVE");
			defense = DEFENSE_NO_DRIVE;
			defenseTimer = ((UnityEngine.Random.Range(0, 100) < 50) ? 1.5f : 2.5f);
		}
	}

	public virtual void OpponentCancelledFirstDribbleWindup(int numWindups)
	{
		Debug.Log("OpponentCancelledFirstDribbleWindup(): numWindups=" + numWindups);
		if (pc.grounded && !pc.springing && !pc.opponent.sprintBar.isTired && UnityEngine.Random.Range(0, 100) >= numWindups * 10)
		{
			Debug.Log("Fall for jabstep... Set defense = ComputerAI.DEFENSE_NO_DRIVE");
			defense = DEFENSE_NO_DRIVE;
			defenseTimer = ((UnityEngine.Random.Range(0, 100) < 50) ? (-1.5f) : (-0.55f));
			if (defeatJoeDefense)
			{
				defenseTimer = 2.5f;
			}
		}
	}

	public virtual void OpponentStartedLayup()
	{
		defendingLayup = true;
		if (base.isActiveAndEnabled && base.gameObject.activeInHierarchy)
		{
			StartCoroutine(Spring(false));
		}
	}

	private bool IsOpenForStartingShotWindup()
	{
		bool result = false;
		PlayerState.Location currentLocation = opponentState.currentLocation;
		PlayerState.Momemtum currentMomemtum = opponentState.currentMomemtum;
		PlayerState.Action currentAction = opponentState.currentAction;
		if (currentLocation == PlayerState.Location.VeryFarInFrontOfBall)
		{
			result = true;
		}
		else if (currentLocation == PlayerState.Location.FarInFrontOfBall && currentMomemtum == PlayerState.Momemtum.MovingAwayFromBall)
		{
			result = true;
		}
		else if (currentAction == PlayerState.Action.WindingUpJump && currentMomemtum == PlayerState.Momemtum.MovingAwayFromBall)
		{
			result = true;
		}
		return result;
	}

	private bool MightBeOpenForShootingThePumpFake()
	{
		bool result = false;
		PlayerState.Location currentLocation = opponentState.currentLocation;
		PlayerState.Momemtum currentMomemtum = opponentState.currentMomemtum;
		PlayerState.Action currentAction = opponentState.currentAction;
		if (currentLocation != PlayerState.Location.DirectlyInFrontOfBall && currentLocation != PlayerState.Location.DirectlyOnTopOfBall && currentMomemtum != PlayerState.Momemtum.MovingToBall)
		{
			result = true;
		}
		else if (currentAction == PlayerState.Action.JumpingOnTheWayUp)
		{
			result = true;
		}
		else if (currentMomemtum == PlayerState.Momemtum.MovingAwayFromBall)
		{
			result = true;
		}
		else if (IsOpenForShootingThePumpFake())
		{
			result = true;
		}
		return result;
	}

	private bool IsOpenForShootingThePumpFake()
	{
		bool result = false;
		PlayerState.Location currentLocation = opponentState.currentLocation;
		PlayerState.Momemtum currentMomemtum = opponentState.currentMomemtum;
		PlayerState.Action currentAction = opponentState.currentAction;
		if (currentLocation == PlayerState.Location.VeryFarInFrontOfBall || currentLocation == PlayerState.Location.FarBehindBall)
		{
			result = true;
		}
		else if (currentAction == PlayerState.Action.WindingUpJump && (currentMomemtum == PlayerState.Momemtum.MovingAwayFromBall || (currentMomemtum == PlayerState.Momemtum.None && currentLocation == PlayerState.Location.FarInFrontOfBall)))
		{
			result = true;
		}
		else if (currentAction == PlayerState.Action.JumpingOnTheWayUp && currentMomemtum == PlayerState.Momemtum.MovingAwayFromBall)
		{
			result = true;
		}
		return result;
	}

	private bool ShouldConvertPumpFakeToShot()
	{
		if (pc.isJumpShooting)
		{
			return true;
		}
		bool result = false;
		if (defeatJoeOffense)
		{
			bool flag = IsReasonableShotDistance(pc.shooter.GetDistanceToHoop(pc.shooter.gameObject.transform.position));
			if (IsOpenForShootingThePumpFake() && flag)
			{
				result = true;
			}
			else if (numPumpFakes >= UnityEngine.Random.Range(1, 3) && (MightBeOpenForShootingThePumpFake() || !pc.sprintBar.isFull || UnityEngine.Random.Range(0, 100) >= 80))
			{
				result = true;
			}
			else if ((numPumpFakes >= 5 && !pc.hasTripleThreat) || numPumpFakes >= 20)
			{
				result = true;
			}
		}
		return result;
	}

	private bool IsReasonableShotDistance(float distance)
	{
		bool result = false;
		if (distance < Shooter.THREEPOINT_DISTANCE + 0.5f)
		{
			result = true;
		}
		else if (pc.shooter.GetPlayerShootingArch() >= 84f && distance < Shooter.THREEPOINT_DISTANCE + 2f)
		{
			result = true;
		}
		return result;
	}

	private bool IsDrivingLaneOpen()
	{
		bool result = false;
		PlayerState.Location currentLocation = opponentState.currentLocation;
		PlayerState.Momemtum currentMomemtum = opponentState.currentMomemtum;
		PlayerState.Action currentAction = opponentState.currentAction;
		if (currentLocation == PlayerState.Location.DirectlyOnTopOfBall || currentLocation == PlayerState.Location.DirectlyBehindBall || currentLocation == PlayerState.Location.FarBehindBall)
		{
			result = true;
		}
		else if (currentLocation == PlayerState.Location.DirectlyInFrontOfBall && (currentAction == PlayerState.Action.WindingUpJump || currentAction == PlayerState.Action.JumpingOnTheWayUp))
		{
			result = true;
		}
		else if (currentLocation == PlayerState.Location.FarInFrontOfBall && currentAction == PlayerState.Action.WindingUpJump && currentMomemtum == PlayerState.Momemtum.MovingToBall)
		{
			result = true;
		}
		return result;
	}

	private bool DrivingLanePossiblyOpen()
	{
		bool result = false;
		PlayerState.Location currentLocation = opponentState.currentLocation;
		if (currentLocation == PlayerState.Location.DirectlyInFrontOfBall)
		{
			result = true;
		}
		else if (IsDrivingLaneOpen())
		{
			result = true;
		}
		return result;
	}

	private void DisplayInputDebug()
	{
		string text = string.Empty;
		if (xMovement == -1)
		{
			text += "< ";
		}
		else if (xMovement == 0)
		{
			text += "  ";
		}
		else if (xMovement == 1)
		{
			text += "  >";
		}
		text = ((!jumpButton) ? (text + "  ") : (text + " ^"));
		if (canWin)
		{
			text += "CW";
		}
		if (shouldWin)
		{
			text += "SW";
		}
		if (defeatJoeOffense)
		{
			text += " DJO";
		}
		if (defeatJoeDefense)
		{
			text += " DJDef";
		}
		if (computerNoWayPhysicallyPossibleToWin)
		{
			text += "NP";
		}
		text += "\n";
		text = text + "DM:" + pc.GetDefendedMultiplier().ToString("F2");
		text += "\n";
		text = text + "SA:" + pc.shooter.GetPlayerShootingArch().ToString("F0");
		text += "\n";
		text = text + "O:" + gameComputer.GetOffenseLevel();
		text = text + "  D:" + gameComputer.GetDefenseLevel();
		text = text + "\nFreq" + xMovementChangeUpdateTimerResetValue;
		text = text + " Stuck" + maxSecondsStuckDribblingIntoDefender;
		inputDebug.text = text;
	}
}
