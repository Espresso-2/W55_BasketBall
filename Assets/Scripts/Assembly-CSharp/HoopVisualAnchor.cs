using System;
using UnityEngine;

[Serializable]
public class HoopVisualAnchor : MonoBehaviour
{
	public bool isEnemy;

	private bool isPulledDown;

	private float pulledDownTime;

	public virtual void Start()
	{
	}

	public virtual void Update()
	{
	}

	public virtual void FixedUpdate()
	{
		if (isPulledDown)
		{
			pulledDownTime += Time.deltaTime;
			if (pulledDownTime >= 0.7f)
			{
				LeanTween.rotate(base.gameObject, new Vector3(0f, 0f, 0f), 0.25f).setEase(LeanTweenType.easeOutQuad).setUseEstimatedTime(true);
				isPulledDown = false;
				pulledDownTime = 0f;
			}
		}
	}

	public virtual void PullDownHoop()
	{
		float num = 4f;
		if (isEnemy)
		{
			num *= -1f;
		}
		LeanTween.rotate(base.gameObject, new Vector3(0f, 0f, num), 0.35f).setEase(LeanTweenType.easeOutQuad).setUseEstimatedTime(true);
		isPulledDown = true;
	}
}
