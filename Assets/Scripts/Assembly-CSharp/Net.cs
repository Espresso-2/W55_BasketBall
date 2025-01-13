using System;
using UnityEngine;

[Serializable]
public class Net : MonoBehaviour
{
	public Animator anim;

	public float timeSinceSwish;

	public Net()
	{
		timeSinceSwish = 1f;
	}

	public virtual void Start()
	{
	}

	public virtual void Update()
	{
		timeSinceSwish += Time.deltaTime;
	}

	public virtual void SwishForward()
	{
		if (timeSinceSwish > 2f)
		{
			anim.SetTrigger("SwishMiddle");
			timeSinceSwish = 0f;
		}
	}

	public virtual void SwishMiddle()
	{
		if (timeSinceSwish > 2f)
		{
			anim.SetTrigger("SwishMiddle");
			timeSinceSwish = 0f;
		}
	}

	public virtual void SwishBack()
	{
		anim.SetTrigger("SwishBack");
		timeSinceSwish = 0f;
	}

	public virtual void Shake()
	{
		if (timeSinceSwish > 2f)
		{
			anim.SetTrigger("Shake");
		}
	}
}
