using System;
using UnityEngine;

[Serializable]
public class HintArrow : MonoBehaviour
{
	public virtual void Start()
	{
		float x = base.gameObject.transform.position.x - 0.35f;
		float y = base.gameObject.transform.position.y + 0.35f;
		LeanTween.move(base.gameObject, new Vector2(x, y), 0.5f).setEase(LeanTweenType.easeInOutSine).setLoopPingPong()
			.setUseEstimatedTime(true);
	}

	public virtual void Update()
	{
	}
}
