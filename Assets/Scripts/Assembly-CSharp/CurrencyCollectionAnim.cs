using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class CurrencyCollectionAnim : MonoBehaviour
{
	public Transform trans;

	public Transform destination;

	public float moveSpeed;

	public float moveDelay;

	private float timeSinceEnabled;

	private static GameSounds gameSounds;

	public CurrencyCollectionAnim()
	{
		moveSpeed = 10f;
		moveDelay = 1f;
	}

	public virtual void Start()
	{
		gameSounds = GameSounds.GetInstance();
		StartCoroutine(Play());
	}

	public virtual void Update()
	{
		timeSinceEnabled += Time.deltaTime;
		if (timeSinceEnabled > moveDelay)
		{
			float maxDistanceDelta = moveSpeed * Time.deltaTime;
			trans.position = Vector3.MoveTowards(trans.position, destination.position, maxDistanceDelta);
		}
	}

	private IEnumerator Play()
	{
		gameSounds.Play_ascend_chime_bright_2();
		yield return new WaitForSeconds(0.75f);
		gameSounds.Play_chime_2_beeps_2();
		gameSounds.Play_ascend_chime_bright_2();
		yield return new WaitForSeconds(0.25f);
		gameSounds.Play_chime_2_beeps_2();
		yield return new WaitForSeconds(0.25f);
		gameSounds.Play_chime_2_beeps_2();
		yield return new WaitForSeconds(1f);
		UnityEngine.Object.Destroy(base.gameObject);
	}
}
