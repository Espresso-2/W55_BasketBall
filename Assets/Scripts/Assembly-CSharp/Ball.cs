using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class Ball : MonoBehaviour
{
	public BallVisual ballVisual;

	public bool canScore;

	public bool isAboveRim;

	public bool isDirectlyAboveCylinder;

	public bool didScore;

	public bool isTipoff;

	public bool wasBlocked;

	public bool wasDunked;

	public float secondsAlive;

	public bool beingLayedUp;

	public int numRimHits;

	private bool canBeBlocked = true;

	private bool hitBehindBackBoard;

	private float aboveRimTimer;

	private float aboveRimExpireTime = 1.5f;

	private int aboveRimLayer;

	private int readyToReboundLayer;

	public GameSounds gameSounds;

	public bool aboutToBeDestroyed;

	private float bounceSoundTimer = 0.55f;

	private float bounceSoundLength = 0.75f;

	private int bounceNum;

	private int bounceMaxNum = 5;

	private float couldBeStuckOnRimTime = 10f;

	private bool movedToGetUnstuck;

	public Rigidbody2D rb;

	public virtual void Start()
	{
		aboveRimLayer = LayerMask.NameToLayer("AboveRimBall");
		readyToReboundLayer = LayerMask.NameToLayer("ReadyToRebound");
		rb = GetComponent<Rigidbody2D>();
	}

	public virtual void Update()
	{
		if (bounceSoundTimer < bounceSoundLength)
		{
			bounceSoundTimer += Time.deltaTime;
		}
		if (secondsAlive > couldBeStuckOnRimTime + 10f && !movedToGetUnstuck)
		{
			rb.transform.localPosition = new Vector3(0f, 4f, 0f);
			movedToGetUnstuck = true;
		}
		else if (secondsAlive > couldBeStuckOnRimTime && !movedToGetUnstuck)
		{
			rb.AddForce(new Vector2(0.02f, 0f));
		}
		secondsAlive += Time.deltaTime;
		if (isAboveRim)
		{
			aboveRimTimer += Time.deltaTime;
			if (aboveRimTimer >= aboveRimExpireTime)
			{
				aboveRimTimer = 0f;
				base.gameObject.layer = readyToReboundLayer;
			}
		}
		if (canBeBlocked && rb.velocity.y <= -4f && !wasDunked)
		{
			canBeBlocked = false;
		}
	}

	public virtual void OnTriggerEnter2D(Collider2D theObject)
	{
		if (theObject.gameObject.tag == "AboveRim" && base.gameObject.layer != readyToReboundLayer && !hitBehindBackBoard)
		{
			SetToCanScore();
		}
		else if (theObject.gameObject.tag == "DirectlyAboveCylinder")
		{
			isDirectlyAboveCylinder = true;
		}
	}

	public virtual void OnTriggerExit2D(Collider2D theObject)
	{
		if (theObject.gameObject.tag == "BehindBackBoard")
		{
			SetToAsIfJustShot();
			hitBehindBackBoard = true;
		}
		else if (theObject.gameObject.tag == "DirectlyAboveCylinder")
		{
			isDirectlyAboveCylinder = false;
		}
	}

	public virtual void SetToCanScore()
	{
		canScore = true;
		base.gameObject.layer = LayerMask.NameToLayer("AboveRimBall");
		isAboveRim = true;
	}

	public virtual IEnumerator OnCollisionEnter2D(Collision2D theObject)
	{
		if (beingLayedUp && theObject.gameObject.tag != "Player")
		{
			rb.velocity /= 1.5f;
		}
		if (base.gameObject.layer == aboveRimLayer && !didScore && theObject.gameObject.tag == "Rim")
		{
			canBeBlocked = false;
			numRimHits++;
			yield return new WaitForSeconds(0.012f);
			if (base.gameObject != null)
			{
				base.gameObject.layer = readyToReboundLayer;
			}
		}
		else if (theObject.gameObject.tag == "Ground")
		{
			if (bounceSoundTimer >= bounceSoundLength && bounceNum < bounceMaxNum)
			{
				float num = 1f - (float)bounceNum * 0.25f;
				if (num < 0.25f)
				{
					num = 0.25f;
				}
				if (!didScore)
				{
					//GameVibrations.Instance().PlayBounce(bounceNum);
				}
				gameSounds.Play_ball_dribble(num);
				bounceNum++;
				if (canScore)
				{
					SetToAsIfJustShot();
				}
			}
		}
		else if (base.gameObject.layer == readyToReboundLayer && !didScore && theObject.gameObject.tag == "Rim")
		{
			numRimHits++;
		}
	}

	private void SetToAsIfJustShot()
	{
		base.gameObject.layer = LayerMask.NameToLayer("Ball");
		canScore = false;
		isAboveRim = false;
		canBeBlocked = true;
	}

	public bool CanBeBlocked()
	{
		return canBeBlocked;
	}
}
