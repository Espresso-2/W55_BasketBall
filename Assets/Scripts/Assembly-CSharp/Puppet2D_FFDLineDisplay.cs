using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Puppet2D_FFDLineDisplay : MonoBehaviour
{
	[HideInInspector]
	public Transform target;

	[HideInInspector]
	public Transform target2;

	[HideInInspector]
	public SkinnedMeshRenderer skinnedMesh;

	public SkinnedMeshRenderer outputSkinnedMesh;

	public int vertNumber;

	public List<Transform> bones = new List<Transform>();

	public List<float> weight = new List<float>();

	public List<Vector3> delta = new List<Vector3>();

	private List<float> internalWeights = new List<float>();

	public void Init()
	{
		bones.Clear();
		weight.Clear();
		delta.Clear();
		internalWeights.Clear();
		Mesh sharedMesh = skinnedMesh.sharedMesh;
		Vector3 position = sharedMesh.vertices[vertNumber];
		BoneWeight boneWeight = sharedMesh.boneWeights[vertNumber];
		int[] array = new int[4] { boneWeight.boneIndex0, boneWeight.boneIndex1, boneWeight.boneIndex2, boneWeight.boneIndex3 };
		float[] array2 = new float[4] { boneWeight.weight0, boneWeight.weight1, boneWeight.weight2, boneWeight.weight3 };
		array2[1] = 1f - array2[0];
		for (int i = 0; i < 4; i++)
		{
			if (array2[i] > 0f)
			{
				bones.Add(skinnedMesh.bones[array[i]]);
				weight.Add(array2[i]);
				internalWeights.Add(array2[i]);
				delta.Add(bones[bones.Count - 1].InverseTransformPoint(position));
			}
		}
	}

	private void OnValidate()
	{
		float num = 0f;
		float num2 = 1f;
		for (int i = 0; i < weight.Count; i++)
		{
			if (internalWeights.Count > i)
			{
				if (internalWeights[i] == weight[i])
				{
					num += weight[i];
				}
				else
				{
					num2 -= weight[i];
				}
			}
		}
		for (int j = 0; j < weight.Count; j++)
		{
			if (internalWeights.Count <= j)
			{
				continue;
			}
			if (internalWeights[j] == weight[j])
			{
				if (num <= 0f)
				{
					weight[j] = 0f;
				}
				else
				{
					weight[j] = weight[j] / num * num2;
				}
			}
			internalWeights[j] = weight[j];
		}
	}

	public void Run()
	{
		if (bones.Count > 0)
		{
			Vector3 zero = Vector3.zero;
			for (int i = 0; i < bones.Count; i++)
			{
				zero += bones[i].TransformPoint(delta[i]) * weight[i];
			}
			base.transform.parent.position = zero;
		}
	}

	private void OnDrawGizmos()
	{
		if (GetComponent<Renderer>().enabled)
		{
			base.transform.GetComponent<SpriteRenderer>().color = Color.white;
			if (target != null)
			{
				Gizmos.color = Color.white;
				Gizmos.DrawLine(base.transform.position, target.position);
			}
			if (target2 != null)
			{
				Gizmos.color = Color.white;
				Gizmos.DrawLine(base.transform.position, target2.position);
			}
		}
	}
}
