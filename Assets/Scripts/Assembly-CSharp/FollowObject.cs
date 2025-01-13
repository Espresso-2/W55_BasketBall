using System;
using UnityEngine;

[Serializable]
public class FollowObject : MonoBehaviour
{
	public Transform objectToFollow;

	public virtual void Start()
	{
	}

	public virtual void Update()
	{
		if (objectToFollow != null)
		{
			base.gameObject.transform.position = new Vector3(objectToFollow.position.x, objectToFollow.position.y, base.gameObject.transform.position.z);
		}
	}
}
