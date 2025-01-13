using System;
using UnityEngine;

[Serializable]
public class PortraitHolder : MonoBehaviour
{
	public Transform targetLocation;

	public virtual void Start()
	{
		base.gameObject.transform.position = targetLocation.position;
	}

	public virtual void Update()
	{
	}
}
