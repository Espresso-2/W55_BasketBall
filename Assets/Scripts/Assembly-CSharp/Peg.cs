using UnityEngine;

public class Peg : MonoBehaviour
{
	public bool isLong;

	private ShakeEffect shakeEffect;

	private SpriteRenderer spriteRenderer;

	private Color origColor;

	public Color hitColor1;

	public Color hitColor2;

	public Color hitColor3;

	private GameObject gameSounds;

	private float secondsSinceBeingHit = 99f;

	private float secondsSinceChangingRandomColor = 99f;

	private void Awake()
	{
		shakeEffect = base.gameObject.GetComponent<ShakeEffect>();
		spriteRenderer = base.gameObject.GetComponent<SpriteRenderer>();
		origColor = spriteRenderer.color;
		gameSounds = GameObject.Find("GameSounds");
	}

	private void Start()
	{
		secondsSinceChangingRandomColor = Random.Range(0f, 3f);
	}

	private void FixedUpdate()
	{
		secondsSinceBeingHit += Time.deltaTime;
		if (secondsSinceBeingHit >= 0.75f)
		{
			SetRandomColor();
		}
		else if (secondsSinceBeingHit >= 0.25f)
		{
			spriteRenderer.color = hitColor3;
		}
		else if (secondsSinceBeingHit >= 0.1f)
		{
			spriteRenderer.color = hitColor2;
		}
	}

	private void SetRandomColor()
	{
		secondsSinceChangingRandomColor += Time.deltaTime;
		if (secondsSinceChangingRandomColor >= 0.15f)
		{
			if (Random.Range(0, 100) >= 50)
			{
				spriteRenderer.color = hitColor3;
			}
			else
			{
				spriteRenderer.color = origColor;
			}
			secondsSinceChangingRandomColor = 0f;
		}
	}

	private void OnCollisionEnter2D(Collision2D coll)
	{
		secondsSinceBeingHit = 0f;
		PlayHitEffect();
	}

	private void PlayHitEffect()
	{
		if (isLong)
		{
			shakeEffect.Shake(0.95f, 0.275f);
			gameSounds.SendMessage("Play_ball_dribble");
			return;
		}
		shakeEffect.Shake(0.65f, 0.045f);
		if (spriteRenderer != null)
		{
			spriteRenderer.color = hitColor1;
		}
	}
}
