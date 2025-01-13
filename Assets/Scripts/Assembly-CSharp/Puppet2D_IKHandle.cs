using System.Collections.Generic;
using UnityEngine;

public class Puppet2D_IKHandle : MonoBehaviour
{
	public bool Flip;

	public bool SquashAndStretch;

	public bool Scale;

	[HideInInspector]
	public Vector3 AimDirection;

	[HideInInspector]
	public Transform poleVector;

	[HideInInspector]
	public Vector3 UpDirection;

	[HideInInspector]
	public Vector3[] scaleStart = new Vector3[2];

	[HideInInspector]
	public Transform topJointTransform;

	[HideInInspector]
	public Transform middleJointTransform;

	[HideInInspector]
	public Transform bottomJointTransform;

	[HideInInspector]
	public Vector3 OffsetScale = new Vector3(1f, 1f, 1f);

	private Transform IK_CTRL;

	private Vector3 root2IK;

	private Vector3 root2IK2MiddleJoint;

	private bool LargerMiddleJoint;

	[HideInInspector]
	public int numberIkBonesIndex;

	public int numberOfBones = 4;

	public int iterations = 10;

	public float damping = 1f;

	public Transform IKControl;

	public Transform endTransform;

	public Transform startTransform;

	public List<Vector3> bindPose;

	public List<Transform> bindBones;

	public bool limitBones = true;

	public List<Transform> angleLimitTransform = new List<Transform>();

	public List<Vector2> angleLimits = new List<Vector2>();

	public void CalculateIK()
	{
		if (numberIkBonesIndex == 1)
		{
			CalculateMultiIK();
			return;
		}
		int num = (Flip ? 1 : (-1));
		IK_CTRL = base.transform;
		root2IK = (topJointTransform.position + IK_CTRL.position) / 2f;
		Vector3 vector = IK_CTRL.position - topJointTransform.position;
		Quaternion quaternion = Quaternion.AngleAxis(num * 90, Vector3.forward);
		root2IK2MiddleJoint = quaternion * vector;
		poleVector.position = root2IK - root2IK2MiddleJoint;
		float angle = GetAngle();
		Quaternion quaternion2 = Quaternion.AngleAxis(angle * (float)num, Vector3.forward);
		if (!IsNaN(quaternion2))
		{
			topJointTransform.rotation = Quaternion.LookRotation(IK_CTRL.position - topJointTransform.position, AimDirection) * Quaternion.AngleAxis(90f, UpDirection) * quaternion2;
		}
		else if (LargerMiddleJoint)
		{
			topJointTransform.rotation = Quaternion.LookRotation(IK_CTRL.position - topJointTransform.position, AimDirection) * Quaternion.AngleAxis(-90f, UpDirection);
		}
		else
		{
			topJointTransform.rotation = Quaternion.LookRotation(IK_CTRL.position - topJointTransform.position, AimDirection) * Quaternion.AngleAxis(90f, UpDirection);
		}
		middleJointTransform.rotation = Quaternion.LookRotation(IK_CTRL.position - middleJointTransform.position, AimDirection) * Quaternion.AngleAxis(90f, UpDirection);
		bottomJointTransform.rotation = IK_CTRL.rotation;
		if (Scale)
		{
			bottomJointTransform.localScale = new Vector3(IK_CTRL.localScale.x * OffsetScale.x, IK_CTRL.localScale.y * OffsetScale.y, IK_CTRL.localScale.z * OffsetScale.z);
		}
	}

	private bool IsNaN(Quaternion q)
	{
		return float.IsNaN(q.x) || float.IsNaN(q.y) || float.IsNaN(q.z) || float.IsNaN(q.w);
	}

	private float GetAngle()
	{
		if (SquashAndStretch)
		{
			topJointTransform.localScale = scaleStart[0];
			middleJointTransform.localScale = scaleStart[1];
		}
		float num = Vector3.Distance(topJointTransform.position, middleJointTransform.position);
		float num2 = Vector3.Distance(middleJointTransform.position, bottomJointTransform.position);
		float num3 = num + num2;
		float num4 = Vector3.Distance(topJointTransform.position, IK_CTRL.position);
		if (num2 > num)
		{
			LargerMiddleJoint = true;
		}
		else
		{
			LargerMiddleJoint = false;
		}
		if (SquashAndStretch && num4 > num3)
		{
			topJointTransform.localScale = new Vector3(scaleStart[0].x, num4 / num3 * scaleStart[0].y, scaleStart[0].z);
		}
		num4 = Mathf.Min(num4, num3 - 0.0001f);
		float num5 = (num * num - num2 * num2 + num4 * num4) / (2f * num4);
		return Mathf.Acos(num5 / num) * 57.29578f;
	}

	private void OnValidate()
	{
		for (int i = 0; i < angleLimits.Count; i++)
		{
			angleLimits[i] = new Vector2(Mathf.Clamp(angleLimits[i].x, -360f, 360f), Mathf.Clamp(angleLimits[i].y, -360f, 360f));
		}
	}

	public void CalculateMultiIK()
	{
		if (!(base.transform == null) && !(endTransform == null))
		{
			for (int i = 0; i < iterations; i++)
			{
				CalculateMultiIK_run();
			}
			endTransform.rotation = base.transform.rotation;
		}
	}

	private void CalculateMultiIK_run()
	{
		Transform parent = endTransform.parent;
		while (true)
		{
			RotateTowardsTarget(parent);
			if (parent == startTransform)
			{
				break;
			}
			parent = parent.parent;
		}
	}

	private void RotateTowardsTarget(Transform startTransform)
	{
		if (!(startTransform == null))
		{
			Vector2 vector = base.transform.position - startTransform.position;
			Vector2 vector2 = endTransform.position - startTransform.position;
			float num = SignedAngle(vector2, vector);
			if (startTransform.eulerAngles.y % 360f > 90f && startTransform.eulerAngles.y % 360f < 275f)
			{
				num *= -1f;
			}
			num *= damping;
			num = 0f - (num - startTransform.localEulerAngles.z);
			if (angleLimits != null && angleLimitTransform.Contains(startTransform))
			{
				Vector2 vector3 = angleLimits[angleLimitTransform.IndexOf(startTransform)];
				num = ClampAngle(num, vector3.x, vector3.y);
			}
			startTransform.localRotation = Quaternion.Euler(0f, 0f, num);
		}
	}

	public static float SignedAngle(Vector3 a, Vector3 b)
	{
		float num = Vector3.Angle(a, b);
		float num2 = Mathf.Sign(Vector3.Dot(Vector3.back, Vector3.Cross(a, b)));
		return num * num2;
	}

	private float ClampAngle(float angle, float min, float max)
	{
		return Mathf.Clamp(angle, min, max);
	}
}
