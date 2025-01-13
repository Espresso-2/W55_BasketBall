using UnityEngine;

[ExecuteInEditMode]
public class Puppet2D_Bakedmesh : MonoBehaviour
{
	public SkinnedMeshRenderer skin;

	private void Start()
	{
		skin = base.transform.GetComponent<SkinnedMeshRenderer>();
	}

	private void Update()
	{
		if (!skin)
		{
			return;
		}
		Mesh mesh = new Mesh();
		skin.BakeMesh(mesh);
		int num = 0;
		foreach (Transform item in base.transform)
		{
			if (!float.IsNaN(mesh.vertices[num].x))
			{
				item.localPosition = mesh.vertices[num];
			}
			else
			{
				Debug.LogWarning("vertex " + num + " is corrupted");
			}
			num++;
		}
		Object.DestroyImmediate(mesh);
	}
}
