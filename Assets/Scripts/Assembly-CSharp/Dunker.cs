using System;
using UnityEngine;

[Serializable]
public class Dunker : MonoBehaviour
{
	public PlayerController playerController;

	public virtual void Start()
	{
	}

	public virtual void Update()
	{
	}

	public virtual void OnTriggerEnter2D(Collider2D theObject)
	{
		if (theObject.tag == "DunkTrigger")
		{
			playerController.shooter.DunkTriggered();
		}
	}
}
