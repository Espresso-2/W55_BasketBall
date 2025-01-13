using System;
using UnityEngine;

[Serializable]
public class IkBall : MonoBehaviour
{
	public Transform ballTrans;

	public Transform targetToFollow;

	private float moveSpeed;

	private bool fixedToTarget;

	public IkBall()
	{
		moveSpeed = 2.5f;
	}

	public virtual void Start()
	{
	}

	public virtual void Update()
	{
		UpdateIkBall();
	}

	public virtual void FixToTarget(GameObject target)
	{
		targetToFollow = target.transform;
		fixedToTarget = true;
	}

	public virtual void FollowTarget(GameObject target)
	{
		fixedToTarget = false;
		moveSpeed = 4f;
		targetToFollow = target.transform;
	}

	private void UpdateIkBall()
	{
		if (targetToFollow == null)
		{
			return;
		}
		if (fixedToTarget)
		{
			ballTrans.position = targetToFollow.position;
		}
		else
		{
			moveSpeed += 9f * Time.deltaTime;
			float num = 9f;
			if (moveSpeed > num)
			{
				moveSpeed = num;
			}
			float num2 = Vector2.Distance(ballTrans.position, targetToFollow.position);
			float maxDistanceDelta = moveSpeed * Time.deltaTime;
			ballTrans.position = Vector3.MoveTowards(ballTrans.position, targetToFollow.position, maxDistanceDelta);
		}
		ballTrans.rotation = targetToFollow.rotation;
	}
}
