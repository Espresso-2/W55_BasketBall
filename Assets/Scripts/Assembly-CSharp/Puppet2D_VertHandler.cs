using UnityEngine;

[ExecuteInEditMode]
public class Puppet2D_VertHandler : MonoBehaviour
{
	private Mesh mesh;

	private Vector3[] verts;

	private Vector3 vertPos;

	private GameObject[] handles;

	private void OnEnable()
	{
		mesh = GetComponent<MeshFilter>().sharedMesh;
		verts = mesh.vertices;
		Vector3[] array = verts;
		foreach (Vector3 position in array)
		{
			vertPos = base.transform.TransformPoint(position);
			GameObject gameObject = new GameObject("handle");
			gameObject.transform.position = vertPos;
			gameObject.transform.parent = base.transform;
			gameObject.tag = "handle";
			MonoBehaviour.print(vertPos);
		}
	}

	private void Update()
	{
		handles = GameObject.FindGameObjectsWithTag("handle");
		for (int i = 0; i < verts.Length; i++)
		{
			verts[i] = handles[i].transform.localPosition;
		}
		mesh.vertices = verts;
		mesh.RecalculateBounds();
		mesh.RecalculateNormals();
	}
}
