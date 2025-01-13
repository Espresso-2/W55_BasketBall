using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class ChampAward : MonoBehaviour
{
	public GameObject cheerLeader;

	public GameObject award1;

	public GameObject award2;

	private GameSounds gameSounds;

	public virtual void Start()
	{
	}

	public virtual void ShowAward(bool won)
	{
	}

	private void FirstPlace()
	{
		award1.SetActive(true);
		award2.SetActive(false);
		StartCoroutine(TweenAward(award1));
	}

	private void SecondPlace()
	{
		award1.SetActive(false);
		award2.SetActive(true);
		StartCoroutine(TweenAward(award2));
	}

	private IEnumerator TweenAward(GameObject award)
	{
		yield return new WaitForSeconds(0.05f);
		LeanTween.move(cheerLeader, cheerLeader.transform.position + new Vector3(-1.45f, -0.1f, 0f), 5.5f).setEase(LeanTweenType.easeOutExpo);
		LeanTween.scale(award, new Vector3(1.45f, 1.45f, 1f), 5.5f).setEase(LeanTweenType.easeOutExpo);
	}

	public virtual void Update()
	{
	}
}
