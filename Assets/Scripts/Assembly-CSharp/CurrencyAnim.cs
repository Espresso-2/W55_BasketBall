using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class CurrencyAnim : MonoBehaviour
{
	public Image img;

	public float destX;

	public float destY;

	public int num;

	public bool playExtraChime;

	public GameObject dontShowUntilThisObjectIsDeactivated;

	private static GameSounds gameSounds;

	private bool startedMove;

	public float delayTime;

	private float delayTimer;

	public virtual void OnEnable()
	{
		if (gameSounds == null)
		{
			gameSounds = GameSounds.GetInstance();
		}
		img.enabled = false;
		delayTimer = 0f;
		startedMove = false;
	}

	private IEnumerator Move()
	{
		startedMove = true;
		yield return new WaitForSeconds(0.25f * (float)num);
		img.enabled = true;
		gameSounds.Play_chime_2_beeps_2();
		if (playExtraChime)
		{
			gameSounds.Play_coin_glow_2();
		}
		float time = 0.5f;
		LeanTween.move(base.gameObject, new Vector2(destX, destY), time);
		yield return new WaitForSeconds(time);
		base.gameObject.SetActive(false);
		yield return new WaitForSeconds(2f);
		num = 0;
	}

	public virtual void FixedUpdate()
	{
		if (!startedMove)
		{
			delayTimer += Time.deltaTime;
			if (delayTimer >= delayTime && (dontShowUntilThisObjectIsDeactivated == null || !dontShowUntilThisObjectIsDeactivated.activeInHierarchy))
			{
				StartCoroutine(Move());
			}
		}
	}
}
