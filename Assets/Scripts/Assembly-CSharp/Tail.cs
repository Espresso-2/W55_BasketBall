using System;
using UnityEngine;

[Serializable]
public class Tail : MonoBehaviour
{
	public GameObject parent;

	public TrailRenderer trailRenderer;

	public virtual void Start()
	{
	}

	public virtual void Update()
	{
		if (parent != null)
		{
			base.gameObject.transform.position = parent.transform.position;
		}
	}
}
