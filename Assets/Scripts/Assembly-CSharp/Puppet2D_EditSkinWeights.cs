using UnityEngine;

[ExecuteInEditMode]
public class Puppet2D_EditSkinWeights : MonoBehaviour
{
	public GameObject Bone0;

	public GameObject Bone1;

	public GameObject Bone2;

	public GameObject Bone3;

	public int boneIndex0;

	public int boneIndex1;

	public int boneIndex2;

	public int boneIndex3;

	public float Weight0;

	public float Weight1;

	public float Weight2;

	public float Weight3;

	public Mesh mesh;

	public SkinnedMeshRenderer meshRenderer;

	public int vertNumber;

	private GameObject[] handles;

	public Vector3[] verts;

	private static Mesh skinnedMesh;

	public bool autoUpdate;

	private void Update()
	{
		Refresh();
	}

	public void Refresh()
	{
		if ((bool)base.transform.parent && (bool)base.transform.parent.GetComponent<SkinnedMeshRenderer>())
		{
			meshRenderer = base.transform.parent.GetComponent<SkinnedMeshRenderer>();
		}
		BoneWeight[] boneWeights = mesh.boneWeights;
		if ((bool)Bone0)
		{
			boneWeights[vertNumber].boneIndex0 = boneIndex0;
		}
		if ((bool)Bone1)
		{
			boneWeights[vertNumber].boneIndex1 = boneIndex1;
		}
		if ((bool)Bone2)
		{
			boneWeights[vertNumber].boneIndex2 = boneIndex2;
		}
		if ((bool)Bone3)
		{
			boneWeights[vertNumber].boneIndex3 = boneIndex3;
		}
		boneWeights[vertNumber].weight0 = Weight0;
		boneWeights[vertNumber].weight1 = Weight1;
		boneWeights[vertNumber].weight2 = Weight2;
		boneWeights[vertNumber].weight3 = Weight3;
		mesh.boneWeights = boneWeights;
		if (meshRenderer != null)
		{
			meshRenderer.sharedMesh = mesh;
		}
	}
}
