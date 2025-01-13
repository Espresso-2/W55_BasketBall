using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class StartMsg : MonoBehaviour
{
	public Text msg;

	private string originalTxt;

	public bool useTweenAnimation;

	public StartMsg()
	{
		useTweenAnimation = true;
	}

	public virtual void Start()
	{
		originalTxt = msg.text;
		msg.text = string.Empty;
	}

	public virtual void OnEnable()
	{
		StartCoroutine(ShowIntro());
	}

	public virtual IEnumerator ShowIntro()
	{
		if (useTweenAnimation)
		{
			LeanTween.scale(base.gameObject, new Vector3(0.001f, 0.001f, 1f), 0.15f).setEase(LeanTweenType.easeInOutExpo);
		}
		yield return new WaitForSeconds(0.45f);
		msg.text = originalTxt;
		if (useTweenAnimation)
		{
			LeanTween.scale(base.gameObject, new Vector3(1f, 1f, 1f), 0.45f).setEase(LeanTweenType.easeOutExpo);
		}
		yield return new WaitForSeconds(0.55f);
		if (useTweenAnimation)
		{
			LeanTween.scale(base.gameObject, new Vector3(0.001f, 0.001f, 1f), 0.55f).setEase(LeanTweenType.easeInExpo);
		}
		GameObject.Find("GameSounds").SendMessage("Play_whistle_02");
		yield return new WaitForSeconds(1f);
		msg.text = string.Empty;
	}

	public virtual void Update()
	{
	}
}
