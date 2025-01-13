using UnityEngine;

[ExecuteInEditMode]
public class Puppet2D_HiddenBone : MonoBehaviour
{
	public Transform boneToAimAt;

	public bool InEditBoneMode;

	public GameObject[] _newSelection;

	private void LateUpdate()
	{
		if (!GetComponent<Renderer>().enabled)
		{
			return;
		}
		if ((bool)boneToAimAt && (bool)base.transform.parent)
		{
			Transform parent = base.transform.parent;
			base.transform.parent = null;
			float num = Vector3.Distance(boneToAimAt.position, base.transform.position);
			if (num > 0f)
			{
				base.transform.rotation = Quaternion.LookRotation(boneToAimAt.position - base.transform.position, Vector3.forward) * Quaternion.AngleAxis(90f, Vector3.right);
			}
			float magnitude = (boneToAimAt.position - base.transform.position).magnitude;
			base.transform.localScale = new Vector3(magnitude, magnitude, magnitude);
			if ((bool)parent)
			{
				base.transform.parent = parent;
				base.transform.position = parent.position;
				if ((bool)parent.GetComponent<SpriteRenderer>())
				{
					base.transform.GetComponent<SpriteRenderer>().sortingLayerName = parent.GetComponent<SpriteRenderer>().sortingLayerName;
				}
			}
			base.transform.hideFlags = HideFlags.HideInHierarchy | HideFlags.NotEditable;
		}
		else
		{
			Object.DestroyImmediate(base.gameObject);
		}
	}

	public void Refresh()
	{
		if (!boneToAimAt)
		{
			return;
		}
		Transform parent = base.transform.parent;
		base.transform.parent = null;
		float num = Vector3.Distance(boneToAimAt.position, base.transform.position);
		if (num > 0f)
		{
			base.transform.rotation = Quaternion.LookRotation(boneToAimAt.position - base.transform.position, Vector3.forward) * Quaternion.AngleAxis(90f, Vector3.right);
		}
		float magnitude = (boneToAimAt.position - base.transform.position).magnitude;
		base.transform.localScale = new Vector3(magnitude, magnitude, magnitude);
		if ((bool)parent)
		{
			base.transform.parent = parent;
			base.transform.position = parent.position;
			if ((bool)parent.GetComponent<SpriteRenderer>())
			{
				base.transform.GetComponent<SpriteRenderer>().sortingLayerName = parent.GetComponent<SpriteRenderer>().sortingLayerName;
			}
		}
	}
}
