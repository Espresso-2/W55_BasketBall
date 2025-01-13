using System;
using UnityEngine;

[Serializable]
public class ScaleEffect : MonoBehaviour
{
	public bool baseOnOrigScale;

	public virtual void Start()
	{
		if (baseOnOrigScale)
		{
			Vector3 to = new Vector3(base.transform.localScale.x * 1.2f, base.transform.localScale.y * 1.2f, 1f);
			LeanTween.scale(base.gameObject, to, 0.35f).setEase(LeanTweenType.easeInOutSine).setLoopPingPong()
				.setIgnoreTimeScale(true);
		}
		else
		{
			LeanTween.scale(base.gameObject, new Vector3(1.1f, 1.1f, 1f), 0.35f).setEase(LeanTweenType.easeInOutSine).setLoopPingPong()
				.setIgnoreTimeScale(true);
		}
	}
}
