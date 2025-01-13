using System;
using UnityEngine;

[Serializable]
public class NetTrigger : MonoBehaviour
{
	public Net net;

	public bool isRim;

	public bool isForward;

	public bool isMiddle;

	public bool isBack;

	public virtual void Start()
	{
	}

	public virtual void Update()
	{
	}

	public virtual void OnTriggerEnter2D(Collider2D theObject)
	{
		if (!(theObject.tag == "Ball"))
		{
			return;
		}
		Ball ball = (Ball)theObject.GetComponent(typeof(Ball));
		if (ball.canScore || !ball.didScore)
		{
			if (isForward)
			{
				net.SwishForward();
			}
			else if (isMiddle)
			{
				net.SwishMiddle();
			}
			else if (isBack)
			{
				net.SwishBack();
			}
			else if (isRim)
			{
				net.Shake();
			}
		}
		else if (ball.canScore && isRim)
		{
			net.Shake();
		}
	}
}
