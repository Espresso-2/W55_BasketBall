using System;
using UnityEngine;

[Serializable]
public class BballWorldCanvas : MonoBehaviour
{
	private SessionVars sessionVars;

	public virtual void Awake()
	{
		sessionVars = SessionVars.GetInstance();
		float num = (float)Screen.height * 1f / (float)Screen.width * 1f;
		bool flag = num >= 0.73f;
		bool flag2 = num >= 0.7f;
		if (flag || flag2)
		{
			Vector3 localScale = base.gameObject.transform.localScale;
			float num2 = 1f;
			if (sessionVars.twoPlayerMode)
			{
				num2 = 0.93f;
			}
			else if (flag)
			{
				num2 = 1.12f;
			}
			else if (flag2)
			{
				num2 = 1.07f;
			}
			base.gameObject.transform.localScale = new Vector3(localScale.x * num2, localScale.y * num2, localScale.z * num2);
		}
	}
}
