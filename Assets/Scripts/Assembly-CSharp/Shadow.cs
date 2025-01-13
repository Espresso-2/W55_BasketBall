using System;
using UnityEngine;

[Serializable]
public class Shadow : MonoBehaviour
{
	public Transform parentTrans;

	public Transform shadowTrans;

	public bool adjustSizeByY;

	public float adjustSizeByMult;

	public float startingYOffset;

	private float startingY;

	private float startingScaleX;

	private float startingScaleY;

	private float startingScaleZ;

	public virtual void Start()
	{
		startingY = shadowTrans.position.y;
		startingScaleX = shadowTrans.localScale.x;
		startingScaleY = shadowTrans.localScale.y;
		startingScaleZ = shadowTrans.localScale.z;
	}

	public virtual void Update()
	{
		if (parentTrans != null)
		{
			float x = parentTrans.position.x;
			Vector3 position = shadowTrans.position;
			position.x = x;
			shadowTrans.position = position;
			if (shadowTrans.position.y != startingY + startingYOffset)
			{
				float y = startingY + startingYOffset;
				Vector3 position2 = shadowTrans.position;
				position2.y = y;
				shadowTrans.position = position2;
			}
			if (adjustSizeByY)
			{
				float num = parentTrans.position.y - shadowTrans.position.y;
				float num2 = num * adjustSizeByMult;
				Vector3 localScale = new Vector3(startingScaleX - num2, startingScaleY - num2, startingScaleZ - num2);
				shadowTrans.localScale = localScale;
			}
		}
	}
}
