using System;
using UnityEngine;

[Serializable]
public class MapHintArrow : MonoBehaviour
{
	public virtual void Start()
	{
		float num = base.gameObject.transform.position.x - 10.5f;
		float num2 = base.gameObject.transform.position.y + 10.5f;
		LeanTween.scale(base.gameObject, new Vector3(1.1f, 1f, 1f), 0.5f).setEase(LeanTweenType.easeInOutSine).setLoopPingPong();
	}

	public virtual void Update()
	{
	}
}
