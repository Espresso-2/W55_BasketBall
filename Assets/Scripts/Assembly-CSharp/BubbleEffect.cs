using System;
using UnityEngine;

[Serializable]
public class BubbleEffect : MonoBehaviour
{
	private float timeEnabled;

	public virtual void OnEnable()
	{
		timeEnabled = 0f;
		LeanTween.scale(base.gameObject, new Vector3(1.1f, 1.1f, 1f), 0.25f).setEase(LeanTweenType.easeOutExpo);
	}

	public virtual void Start()
	{
	}

	public virtual void Update()
	{
		timeEnabled += 1f;
		if (timeEnabled == 20f)
		{
			LeanTween.scale(base.gameObject, new Vector3(1f, 1f, 1f), 1f).setEase(LeanTweenType.easeOutExpo);
		}
		else if (timeEnabled > 50f)
		{
			base.gameObject.SetActive(false);
		}
	}
}
