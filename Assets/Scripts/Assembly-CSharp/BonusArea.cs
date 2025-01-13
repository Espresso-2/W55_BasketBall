using System;
using UnityEngine;

[Serializable]
public class BonusArea : MonoBehaviour
{
	private float posX;

	public bool isTouchingPlayer;

	public BonusArea()
	{
		posX = 4f;
		isTouchingPlayer = true;
	}

	public virtual void Start()
	{
		posX = 3.3f;
		float x = posX;
		Vector3 position = base.gameObject.transform.position;
		position.x = x;
		base.gameObject.transform.position = position;
	}

	public virtual void NewPosition()
	{
		posX = UnityEngine.Random.Range(-4.1f, 5.8f);
		float x = posX;
		Vector3 position = base.gameObject.transform.position;
		position.x = x;
		base.gameObject.transform.position = position;
	}

	public virtual void OnTriggerEnter2D(Collider2D @object)
	{
		if (@object.gameObject.tag == "Player")
		{
			isTouchingPlayer = true;
		}
	}

	public virtual void OnTriggerExit2D(Collider2D @object)
	{
		if (@object.gameObject.tag == "Player")
		{
			isTouchingPlayer = false;
		}
	}

	public virtual void Update()
	{
	}
}
