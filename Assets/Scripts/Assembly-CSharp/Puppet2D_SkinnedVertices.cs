using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Puppet2D_SkinnedVertices : MonoBehaviour
{
	private class Bone
	{
		internal Transform bone;

		internal float weight;

		internal Vector3 delta;
	}

	private Mesh mesh;

	private List<List<Bone>> allBones = new List<List<Bone>>();

	private void Start()
	{
		SkinnedMeshRenderer skinnedMeshRenderer = GetComponent(typeof(SkinnedMeshRenderer)) as SkinnedMeshRenderer;
		mesh = skinnedMeshRenderer.sharedMesh;
		for (int i = 0; i < mesh.vertexCount; i++)
		{
			Vector3 position = mesh.vertices[i];
			position = base.transform.TransformPoint(position);
			BoneWeight boneWeight = mesh.boneWeights[i];
			int[] array = new int[4] { boneWeight.boneIndex0, boneWeight.boneIndex1, boneWeight.boneIndex2, boneWeight.boneIndex3 };
			float[] array2 = new float[4] { boneWeight.weight0, boneWeight.weight1, boneWeight.weight2, boneWeight.weight3 };
			List<Bone> list = new List<Bone>();
			allBones.Add(list);
			for (int j = 0; j < 4; j++)
			{
				if (array2[j] > 0f)
				{
					Bone bone = new Bone();
					list.Add(bone);
					bone.bone = skinnedMeshRenderer.bones[array[j]];
					bone.weight = array2[j];
					bone.delta = bone.bone.InverseTransformPoint(position);
				}
			}
		}
	}

	private void OnDrawGizmos()
	{
		if (!Application.isPlaying || !base.enabled)
		{
			return;
		}
		for (int i = 0; i < mesh.vertexCount; i++)
		{
			List<Bone> list = allBones[i];
			Vector3 zero = Vector3.zero;
			foreach (Bone item in list)
			{
				zero += item.bone.TransformPoint(item.delta) * item.weight;
			}
			int count = list.Count;
			Color color;
			switch (count)
			{
			case 4:
				color = Color.red;
				break;
			case 3:
				color = Color.blue;
				break;
			case 2:
				color = Color.green;
				break;
			default:
				color = Color.black;
				break;
			}
			Gizmos.color = color;
			Gizmos.DrawWireCube(zero, (float)count * 0.05f * Vector3.one);
			Vector3 zero2 = Vector3.zero;
			foreach (Bone item2 in list)
			{
				zero2 += item2.bone.TransformDirection(mesh.normals[i]) * item2.weight;
			}
		}
	}
}
