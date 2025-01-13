using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class ChimeEffect : MonoBehaviour
{
	private static GameSounds gameSounds;

	public virtual void OnEnable()
	{
		gameSounds = GameSounds.GetInstance();
		StartCoroutine(PlayChime());
	}

	private IEnumerator PlayChime()
	{
		yield return new WaitForSeconds(0.05f);
		if (base.gameObject.activeInHierarchy)
		{
			gameSounds.Play_coin_glow_2();
		}
	}
}
