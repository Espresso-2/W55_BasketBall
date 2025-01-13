using UnityEngine;

public class TransformConstrain : MonoBehaviour
{
	public Transform Target;

	public bool ConstrainPosition;

	public bool ConstrainRotation;

	private void Update()
	{
		if (Target != null)
		{
			if (ConstrainPosition)
			{
				base.transform.position = Target.position;
			}
			if (ConstrainRotation)
			{
				base.transform.rotation = Target.rotation;
			}
		}
	}
}
