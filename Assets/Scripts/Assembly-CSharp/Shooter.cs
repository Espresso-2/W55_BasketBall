using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class Shooter : MonoBehaviour
{
	public static float LAYUP_DISTANCE = 2.1f;

	public static float THREEPOINT_DISTANCE = 9.9f;

	public static float THREEPOINT_DISTANCE_FOR_TOE = 9.4f;

	public GameObject shooterObject;

	public Transform frontToe;

	public GameObject ballPrefab;

	public PlayerController playerController;

	public PlayerController opponentPlayerController;

	public GameController gameController;

	private float xVelocity;

	private float yVelocity;

	public GameObject hoop;

	private GameSounds gameSounds;

	public Transform shootTarget;

	public bool isEnemy;

	private bool isLayup;

	private bool is3PT;

	private float playerShootingArch = 60f;

	private int numPumpFakes;

	public virtual void Start()
	{
		gameSounds = GameSounds.GetInstance();
	}

	public virtual void Update()
	{
		if (!gameController.playingGame || !playerController.hasBall || ((!Input.GetKeyUp(KeyCode.Space) || playerController.isComputer) && !playerController.GetJumpButtonUp() && (!(playerController.jumpShotWindupTimer >= 0.75f) || !playerController.isJumpShooting || !playerController.IsGrounded())) || !playerController.jumpShotWindingup)
		{
			return;
		}
		if (playerController.jumpShotWindupTimer > playerController.GetJumpShotWindupTimeLength() || isLayup)
		{
			playerController.jumpButtonDown = false;
			playerController.SetJumpButtonUp(false);
			Debug.Log("SHOOT");
			float delay = 0f;
			float num = playerController.GetJumpShotWindupTimeLength() + 0.12f;
			if (playerController.dunkWindingup)
			{
				num = 0.24f;
			}
			else if (isLayup)
			{
				num = 0.17f;
			}
			if (playerController.jumpShotWindupTimer < num)
			{
				Debug.Log("PLAYER RELEASED LAYUP/JUMPSHOT WAYYYYYYY TO EARLY");
				Debug.Log("playerController.jumpShotWindupTimer: " + playerController.jumpShotWindupTimer + " minTime: " + num);
				delay = num - playerController.jumpShotWindupTimer;
			}
			StartCoroutine(Shoot(delay, false));
		}
		else
		{
			Debug.Log("PUMP FAKE HERE === playerController.GetJumpButtonUp(): " + playerController.GetJumpButtonUp() + " playerController.jumpShotWindupTimer: " + playerController.jumpShotWindupTimer);
			numPumpFakes++;
			if (opponentPlayerController.computerAI != null && opponentPlayerController.computerAI.isActiveAndEnabled)
			{
				opponentPlayerController.computerAI.OpponentPumpFaked(numPumpFakes);
			}
			playerController.jumpShotWindupTimer = 0f;
			playerController.jumpShotWindingup = false;
			if (!playerController.IsPerformingStepBack())
			{
				gameController.voiceOvers.PlayPumpFakes();
			}
			playerController.isDribbling = false;
		}
	}

	public virtual void StartedShotWindup(bool isLayup)
	{
		Debug.Log("StartShotWindup isLayup: " + isLayup);
		this.isLayup = isLayup;
	}

	public virtual void DunkTriggered()
	{
		if (playerController.dunkWindingup)
		{
			playerController.jumpButtonDown = false;
			playerController.SetJumpButtonUp(false);
			StartCoroutine(Shoot(0f, true));
		}
	}

	private IEnumerator Shoot(float delay, bool dunkTriggered)
	{
		Debug.Log("delay: " + delay);
		numPumpFakes = 0;
		if (delay > 0f)
		{
			yield return new WaitForSeconds(delay);
		}
		Debug.Log("playerController.jumpShotWindupTimer: " + playerController.jumpShotWindupTimer);
		playerController.jumpShotWindupTimer = 0f;
		playerController.jumpShotWindingup = false;
		StartCoroutine(playerController.ShotBall(isLayup, dunkTriggered));
		if (!isLayup)
		{
			yield return new WaitForSeconds(0.02f);
		}
		float shotDistance = GetDistanceToHoop(shooterObject.transform.position);
		is3PT = GetDistanceToHoop(frontToe.position) >= THREEPOINT_DISTANCE_FOR_TOE;
		Ball ball = (Ball)UnityEngine.Object.Instantiate(position: (Vector2)shooterObject.transform.position, original: ballPrefab, rotation: ballPrefab.transform.rotation).GetComponent(typeof(Ball));
		bool shootPhysicallyImpossibleShot = false;
		if (playerController.isComputer && playerController.computerAI.gameComputer.NoWayPhysicallyPossibleToWin() && gameController.ComputerCloseToWinning() && shotDistance < 9.25f)
		{
			shootPhysicallyImpossibleShot = true;
		}
		if (isLayup)
		{
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			bool flag4 = false;
			bool flag5 = false;
			float num = shooterObject.transform.position.x - hoop.transform.position.x;
			if (isEnemy)
			{
				num = hoop.transform.position.x - shooterObject.transform.position.x;
			}
			Debug.Log("LAYUP distanceX=" + num + " this.playerController.rig2D.velocity.x=" + playerController.rig2D.velocity.x);
			if (num > 0.95f)
			{
				flag = true;
			}
			else if (num > 0.5f)
			{
				flag2 = true;
			}
			else if (num > 0.2f)
			{
				flag3 = true;
			}
			else if (num > -0.1f)
			{
				flag4 = true;
			}
			else if (num < -1.5f && playerController.rig2D.velocity.x < 0.1f)
			{
				flag5 = true;
			}
			bool flag6 = ball.gameObject.transform.position.y > hoop.transform.position.y + 0.4f;
			bool flag7 = ball.gameObject.transform.position.y > hoop.transform.position.y - 0.4f;
			float num2 = 2f;
			float num3 = 12f;
			if (flag4)
			{
				num2 *= 0.25f;
			}
			else if (flag3)
			{
				num2 *= -0.25f;
			}
			else if (flag2)
			{
				num2 *= -0.55f;
			}
			else if (flag)
			{
				num2 *= -0.85f;
			}
			else if (flag5)
			{
				num2 *= 1.25f;
				num3 *= 1.05f;
			}
			else
			{
				num2 = Mathf.Abs(playerController.rig2D.velocity.x) * 0.75f;
				if (num2 < 1.25f)
				{
					num2 = 1.25f;
				}
			}
			if (dunkTriggered)
			{
				num3 = -5f;
				num2 = 3.5f;
				ball.SetToCanScore();
				ball.wasDunked = true;
				gameSounds.Play_dunk();
			}
			else if (flag6)
			{
				ball.SetToCanScore();
				num3 *= 0.7f;
			}
			else if (flag7)
			{
				num3 *= 0.85f;
			}
			if (isEnemy)
			{
				num2 *= -1f;
			}
			if (playerController.isComputer && (gameController.score.player2Score >= 4 || gameController.ComputerCloseToWinning()) && (UnityEngine.Random.Range(0, 2) >= playerController.computerAI.gameComputer.GetOffenseLevel() || gameController.InScrimmage || shootPhysicallyImpossibleShot))
			{
				num2 *= 2.5f;
				if (num2 < -2f && num2 > -5f)
				{
					num2 -= 3f;
				}
				num3 = 15f;
				Debug.Log("SHOOT BAD LAYUP... layupVelX: " + num2 + " layupVelY: " + num3);
			}
			ball.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(num2, num3);
			ball.beingLayedUp = true;
			if (!flag5)
			{
				gameController.voiceOvers.PlayShotLayup(!playerController.isComputer, shotDistance, dunkTriggered);
			}
			/*if (gameController.InTutorial || gameController.InScrimmage)
			{
				FlurryAnalytics.Instance().LogEvent("LAYUP");
			}*/
		}
		else
		{
			float y = playerController.rig2D.velocity.y;
			bool flag8 = y <= -4.5f || playerController.IsGrounded();
			bool flag9 = y >= -0.5f;
			if (!playerController.isComputer)
			{
				flag9 = y >= 2.25f;
			}
			if (!playerController.isComputer)
			{
				if (flag8 || flag9)
				{
					StartCoroutine(gameController.ShowReleaseMsg(playerController.isPlayer2, flag8));
				}
				if (flag9)
				{
					gameController.numConsecutiveEarlyReleases++;
				}
				else
				{
					gameController.numConsecutiveEarlyReleases = 0;
				}
			}
			float num4 = playerShootingArch - shotDistance * 3f;
			Debug.Log("shootAngle: " + num4);
			float num5 = 66f;
			if (num4 > num5)
			{
				num4 = num5;
			}
			float num6 = 4f;
			if (shotDistance <= num6)
			{
				num4 *= 1.1f;
			}
			Vector2 vector = BallisticVel(shooterObject.transform, shootTarget, num4);
			if (shotDistance <= num6)
			{
				vector *= 1.0075f;
				if (shotDistance < LAYUP_DISTANCE)
				{
					vector = new Vector2(vector.x, vector.y + 3f);
					if (shotDistance < 0.75f)
					{
						vector = new Vector2(vector.x, vector.y + 1f);
					}
				}
			}
			float num7 = 1f;
			float num8 = 1f;
			int num9 = UnityEngine.Random.Range(0, 100);
			if (playerController.isComputer)
			{
				if (flag8)
				{
					num7 = 0.89f;
					num8 = 1.05f;
				}
				else if (flag9)
				{
					num7 = 0.965f;
					num8 = 1f;
				}
				else if (num9 > 75)
				{
					num7 = 1.006f;
					num8 = 1.008f;
				}
				else if (num9 > 50)
				{
					num7 = 0.993f;
					num8 = 0.994f;
				}
				else if (num9 > 25)
				{
					num7 = 1.055f;
					num8 = 0.95f;
				}
				else
				{
					num7 = 1.1f;
					num8 = 0.91f;
				}
			}
			else if (flag8)
			{
				if (num9 > 75)
				{
					num7 = 0.85f;
					num8 = 1.14f;
				}
				else if (num9 > 50)
				{
					num7 = 0.9f;
					num8 = 1.14f;
				}
				else if (num9 > 25)
				{
					num7 = 0.99f;
					num8 = 0.99f;
				}
			}
			else if (flag9)
			{
				if (num9 > 75)
				{
					num7 = 0.92f;
					num8 = 0.98f;
				}
				else if (num9 > 50)
				{
					num7 = 0.94f;
					num8 = 1.1f;
				}
				else if (num9 > 25)
				{
					num7 = 0.92f;
					num8 = 0.99f;
				}
			}
			else if (num9 > 75)
			{
				num7 = 1.005f;
				num8 = 1.005f;
			}
			else if (num9 > 50)
			{
				num7 = 0.995f;
				num8 = 0.995f;
			}
			else if (num9 > 25)
			{
				num7 = 1.05f;
				num8 = 0.95f;
			}
			vector.x *= num7;
			vector.y *= num8;
			Debug.Log(string.Concat("velocity: ", vector, " shotDistance: ", shotDistance));
			Debug.Log("velocity.y/velocity.x: " + vector.y / Mathf.Abs(vector.x));
			if (shootPhysicallyImpossibleShot)
			{
				if (UnityEngine.Random.Range(0, 100) >= 50)
				{
					Debug.Log("NWPPTW: Shoot high and short shot");
					vector.x *= 0.45f;
					vector.y *= 1.25f;
				}
				else
				{
					Debug.Log("NWPPTW: Shoot long shot");
					vector.x *= 1.45f;
					vector.y *= 1.45f;
				}
			}
			else if (vector.y / Mathf.Abs(vector.x) < 0.65f)
			{
				if (UnityEngine.Random.Range(0, 100) >= 50)
				{
					Debug.Log("Shoot high and short shot");
					vector.x *= 0.45f;
					vector.y *= 1.75f;
				}
				else
				{
					Debug.Log("Shoot long shot");
					vector.x *= 0.85f;
					vector.y *= 1.35f;
				}
			}
			ball.gameObject.GetComponent<Rigidbody2D>().velocity = vector;
			if (vector.x > 0f)
			{
				ball.gameObject.GetComponent<Rigidbody2D>().angularVelocity = 550f;
			}
			else
			{
				ball.gameObject.GetComponent<Rigidbody2D>().angularVelocity = -550f;
			}
			if (is3PT)
			{
				bool forTheWin = gameController.score.player1Score + 3 >= gameController.GetPlayToScore() && !playerController.isPlayer2;
				StartCoroutine(gameController.voiceOvers.PlayShotThree(!playerController.isComputer, forTheWin));
			}
			else
			{
				bool forTheWin = gameController.score.player1Score + 2 >= gameController.GetPlayToScore() && !playerController.isPlayer2;
				StartCoroutine(gameController.voiceOvers.PlayShotTwo(!playerController.isComputer, shotDistance, forTheWin));
			}
			/*if (gameController.InTutorial || gameController.InScrimmage)
			{
				FlurryAnalytics.Instance().LogEvent("JUMP_SHOT");
			}*/
		}
		gameSounds.Play_tap();
		gameController.ShotBall(ball, !isEnemy, is3PT, isLayup, dunkTriggered);
	}

	public virtual float GetDistanceToHoop(Vector2 pos)
	{
		Vector2 vector = new Vector2(pos.x, hoop.transform.position.y);
		return Vector3.Distance(vector, hoop.transform.position);
	}

	public virtual void SetPlayerShootingArch(float arch)
	{
		playerShootingArch = arch;
	}

	public float GetPlayerShootingArch()
	{
		return playerShootingArch;
	}

	public virtual void ResetNumPumpFakes()
	{
		numPumpFakes = 0;
	}

	public virtual int GetNumPumpFakes()
	{
		return numPumpFakes;
	}

	public virtual Vector2 BallisticVel(Transform start, Transform target, float angle)
	{
		Vector3 vector = target.position - start.position;
		float y = vector.y;
		vector.y = 0f;
		float magnitude = vector.magnitude;
		float num = angle * ((float)Math.PI / 180f);
		vector.y = magnitude * Mathf.Tan(num);
		magnitude += y / Mathf.Tan(num);
		float num2 = Mathf.Sqrt(magnitude * Physics2D.gravity.magnitude / Mathf.Sin(2f * num));
		return num2 * vector.normalized;
	}
}
