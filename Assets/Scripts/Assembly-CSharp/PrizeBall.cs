using System.Collections;
using UnityEngine;

public class PrizeBall : MonoBehaviour
{
	public TrailRenderer trailRenderer;

	public GameObject ballVisual;

	private GameObject gameSounds;

	private float secondsAlive;

	private float bounceSoundTimer = 0.55f;

	private float bounceSoundLength = 0.075f;

	private int numBouncesInSlot;

	private int maxBouncesInSlot = 7;

	private float couldBeStuck = 10f;

	private bool reachedPrizeSlot;

	private bool scaleBig;

	private void Start()
	{
		gameSounds = GameObject.Find("GameSounds");
	}

	public void Drop()
	{
		GetComponent<Animator>().enabled = false;
		GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
	}

	public void Reset()
	{
		trailRenderer.enabled = false;
		GetComponent<Animator>().enabled = true;
		GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
		numBouncesInSlot = 0;
		secondsAlive = 0f;
		reachedPrizeSlot = false;
		numBouncesInSlot = 0;
		StartCoroutine(EnableTrailRenderer(true));
	}

	private IEnumerator EnableTrailRenderer(bool enable)
	{
		yield return new WaitForSeconds(0.5f);
		trailRenderer.enabled = enable;
	}

	private void Update()
	{
		if (bounceSoundTimer < bounceSoundLength)
		{
			bounceSoundTimer += Time.deltaTime;
		}
		if (secondsAlive > couldBeStuck)
		{
			GetComponent<Rigidbody2D>().AddForce(new Vector2(0.02f, 0f));
		}
		secondsAlive += Time.deltaTime;
	}

	private void OnCollisionEnter2D(Collision2D coll)
	{
		if (coll.gameObject.tag == "Ground" && bounceSoundTimer >= bounceSoundLength && numBouncesInSlot < maxBouncesInSlot)
		{
			if (reachedPrizeSlot)
			{
				float num = 1f - (float)numBouncesInSlot * 0.33f;
				gameSounds.SendMessage("Play_ball_dribble", num);
				//iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.ImpactHeavy);
			}
			else if (Random.Range(0, 100) >= 90)
			{
				gameSounds.SendMessage("Play_ball_dribble");
				//iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.ImpactMedium);
			}
			else if (Random.Range(0, 100) >= 65)
			{
				gameSounds.SendMessage("Play_rattling_hinge");
				//iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.ImpactMedium);
			}
			else
			{
				gameSounds.SendMessage("Play_light_click_2");
				//iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.SelectionChange);
			}
			scaleBig = !scaleBig;
			if (reachedPrizeSlot)
			{
				numBouncesInSlot++;
			}
		}
	}

	public void OnReachedPrizeSlot()
	{
		reachedPrizeSlot = true;
	}

	public bool DidReachPrizeSlot()
	{
		return reachedPrizeSlot;
	}
}
