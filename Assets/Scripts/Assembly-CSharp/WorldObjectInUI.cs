using System;
using UnityEngine;

[Serializable]
public class WorldObjectInUI : MonoBehaviour
{
	public Transform uiParent;

	public virtual void Start()
	{
		base.gameObject.transform.position = uiParent.position;
	}

	public virtual void Update()
	{
	}
}
