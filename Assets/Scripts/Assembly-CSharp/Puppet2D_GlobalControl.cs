using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
public class Puppet2D_GlobalControl : MonoBehaviour
{
	public float startRotationY;

	public List<Puppet2D_SplineControl> _SplineControls = new List<Puppet2D_SplineControl>();

	public List<Puppet2D_IKHandle> _Ikhandles = new List<Puppet2D_IKHandle>();

	public List<Puppet2D_ParentControl> _ParentControls = new List<Puppet2D_ParentControl>();

	public List<Puppet2D_FFDLineDisplay> _ffdControls = new List<Puppet2D_FFDLineDisplay>();

	[HideInInspector]
	public List<SpriteRenderer> _Controls = new List<SpriteRenderer>();

	[HideInInspector]
	public List<SpriteRenderer> _Bones = new List<SpriteRenderer>();

	[HideInInspector]
	public List<SpriteRenderer> _FFDControls = new List<SpriteRenderer>();

	public bool ControlsVisiblity = true;

	public bool BonesVisiblity = true;

	public bool FFD_Visiblity = true;

	public bool CombineMeshes;

	public bool flip;

	private bool internalFlip;

	public bool AutoRefresh = true;

	public bool ControlsEnabled = true;

	public bool lateUpdate = true;

	[HideInInspector]
	public int _flipCorrection = 1;

	public void SetFlip(bool f)
	{
		flip = f;
	}

	private void OnEnable()
	{
		if (AutoRefresh)
		{
			_Ikhandles.Clear();
			_SplineControls.Clear();
			_ParentControls.Clear();
			_Controls.Clear();
			_Bones.Clear();
			_FFDControls.Clear();
			_ffdControls.Clear();
			TraverseHierarchy(base.transform);
		}
	}

	public void Refresh()
	{
		_Ikhandles.Clear();
		_SplineControls.Clear();
		_ParentControls.Clear();
		_Controls.Clear();
		_Bones.Clear();
		_FFDControls.Clear();
		_ffdControls.Clear();
		TraverseHierarchy(base.transform);
	}

	private void Awake()
	{
		internalFlip = flip;
		if (Application.isPlaying && CombineMeshes)
		{
			CombineAllMeshes();
		}
	}

	public void Init()
	{
		_Ikhandles.Clear();
		_SplineControls.Clear();
		_ParentControls.Clear();
		_Controls.Clear();
		_Bones.Clear();
		_FFDControls.Clear();
		_ffdControls.Clear();
		TraverseHierarchy(base.transform);
	}

	private void OnValidate()
	{
		if (AutoRefresh)
		{
			_Ikhandles.Clear();
			_SplineControls.Clear();
			_ParentControls.Clear();
			_Controls.Clear();
			_Bones.Clear();
			_FFDControls.Clear();
			_ffdControls.Clear();
			TraverseHierarchy(base.transform);
		}
		foreach (SpriteRenderer control in _Controls)
		{
			if ((bool)control && control.enabled != ControlsVisiblity)
			{
				control.enabled = ControlsVisiblity;
			}
		}
		foreach (SpriteRenderer bone in _Bones)
		{
			if ((bool)bone && bone.enabled != BonesVisiblity)
			{
				bone.enabled = BonesVisiblity;
			}
		}
		foreach (SpriteRenderer fFDControl in _FFDControls)
		{
			if ((bool)fFDControl && (bool)fFDControl.transform.parent && (bool)fFDControl.transform.parent.gameObject && fFDControl.transform.parent.gameObject.activeSelf != FFD_Visiblity)
			{
				fFDControl.transform.parent.gameObject.SetActive(FFD_Visiblity);
			}
		}
	}

	private void Update()
	{
		if (lateUpdate)
		{
			return;
		}
		if (ControlsEnabled)
		{
			Run();
		}
		if (internalFlip != flip)
		{
			if (flip)
			{
				base.transform.localScale = new Vector3(base.transform.localScale.x, base.transform.localScale.y, 0f - base.transform.localScale.z);
				base.transform.localEulerAngles = new Vector3(base.transform.rotation.eulerAngles.x, startRotationY + 180f, base.transform.rotation.eulerAngles.z);
			}
			else
			{
				base.transform.localScale = new Vector3(Mathf.Abs(base.transform.localScale.x), Mathf.Abs(base.transform.localScale.y), Mathf.Abs(base.transform.localScale.z));
				base.transform.localEulerAngles = new Vector3(base.transform.rotation.eulerAngles.x, startRotationY, base.transform.rotation.eulerAngles.z);
			}
			internalFlip = flip;
			Run();
		}
	}

	private void LateUpdate()
	{
		if (!lateUpdate)
		{
			return;
		}
		if (ControlsEnabled)
		{
			Run();
		}
		if (internalFlip != flip)
		{
			if (flip)
			{
				base.transform.localScale = new Vector3(base.transform.localScale.x, base.transform.localScale.y, 0f - base.transform.localScale.z);
				base.transform.localEulerAngles = new Vector3(base.transform.rotation.eulerAngles.x, startRotationY + 180f, base.transform.rotation.eulerAngles.z);
			}
			else
			{
				base.transform.localScale = new Vector3(Mathf.Abs(base.transform.localScale.x), Mathf.Abs(base.transform.localScale.y), Mathf.Abs(base.transform.localScale.z));
				base.transform.localEulerAngles = new Vector3(base.transform.rotation.eulerAngles.x, startRotationY, base.transform.rotation.eulerAngles.z);
			}
			internalFlip = flip;
			Run();
		}
	}

	public void Run()
	{
		foreach (Puppet2D_SplineControl splineControl in _SplineControls)
		{
			if ((bool)splineControl)
			{
				splineControl.Run();
			}
		}
		foreach (Puppet2D_ParentControl parentControl in _ParentControls)
		{
			if ((bool)parentControl)
			{
				parentControl.ParentControlRun();
			}
		}
		FaceCamera();
		foreach (Puppet2D_IKHandle ikhandle in _Ikhandles)
		{
			if ((bool)ikhandle)
			{
				ikhandle.CalculateIK();
			}
		}
		foreach (Puppet2D_FFDLineDisplay ffdControl in _ffdControls)
		{
			if ((bool)ffdControl)
			{
				ffdControl.Run();
			}
		}
	}

	public void TraverseHierarchy(Transform root)
	{
		foreach (Transform item in root)
		{
			GameObject gameObject = item.gameObject;
			SpriteRenderer component = gameObject.transform.GetComponent<SpriteRenderer>();
			if ((bool)component && (bool)component.sprite)
			{
				if (component.sprite.name.Contains("Control"))
				{
					_Controls.Add(component);
				}
				else if (component.sprite.name.Contains("ffd"))
				{
					_FFDControls.Add(component);
				}
				else if (component.sprite.name.Contains("Bone"))
				{
					_Bones.Add(component);
				}
			}
			Puppet2D_ParentControl component2 = gameObject.transform.GetComponent<Puppet2D_ParentControl>();
			if ((bool)component2)
			{
				_ParentControls.Add(component2);
			}
			Puppet2D_IKHandle component3 = gameObject.transform.GetComponent<Puppet2D_IKHandle>();
			if ((bool)component3)
			{
				_Ikhandles.Add(component3);
			}
			Puppet2D_FFDLineDisplay component4 = gameObject.transform.GetComponent<Puppet2D_FFDLineDisplay>();
			if ((bool)component4)
			{
				_ffdControls.Add(component4);
			}
			Puppet2D_SplineControl component5 = gameObject.transform.GetComponent<Puppet2D_SplineControl>();
			if ((bool)component5)
			{
				_SplineControls.Add(component5);
			}
			TraverseHierarchy(item);
		}
	}

	private void CombineAllMeshes()
	{
		Vector3 localScale = base.transform.localScale;
		Quaternion rotation = base.transform.rotation;
		Vector3 position = base.transform.position;
		base.transform.localScale = Vector3.one;
		base.transform.rotation = Quaternion.identity;
		base.transform.position = Vector3.zero;
		SkinnedMeshRenderer[] componentsInChildren = GetComponentsInChildren<SkinnedMeshRenderer>();
		List<Transform> list = new List<Transform>();
		List<BoneWeight> list2 = new List<BoneWeight>();
		List<CombineInstance> list3 = new List<CombineInstance>();
		List<Texture2D> list4 = new List<Texture2D>();
		Material material = null;
		int num = 0;
		Dictionary<SkinnedMeshRenderer, float> dictionary = new Dictionary<SkinnedMeshRenderer, float>(componentsInChildren.Length);
		bool updateWhenOffscreen = false;
		SkinnedMeshRenderer[] array = componentsInChildren;
		foreach (SkinnedMeshRenderer skinnedMeshRenderer in array)
		{
			dictionary.Add(skinnedMeshRenderer, skinnedMeshRenderer.transform.position.z);
			updateWhenOffscreen = skinnedMeshRenderer.updateWhenOffscreen;
		}
		IOrderedEnumerable<KeyValuePair<SkinnedMeshRenderer, float>> source = dictionary.OrderBy((KeyValuePair<SkinnedMeshRenderer, float> pair) => pair.Key.sortingOrder);
		source = source.OrderByDescending((KeyValuePair<SkinnedMeshRenderer, float> pair) => pair.Value);
		foreach (KeyValuePair<SkinnedMeshRenderer, float> item4 in source)
		{
			num += item4.Key.sharedMesh.subMeshCount;
		}
		int[] array2 = new int[num];
		int num2 = 0;
		int num3 = 0;
		foreach (KeyValuePair<SkinnedMeshRenderer, float> item5 in source)
		{
			SkinnedMeshRenderer key = item5.Key;
			if (material == null)
			{
				material = key.sharedMaterial;
			}
			else if ((bool)material.mainTexture && (bool)key.sharedMaterial.mainTexture && material.mainTexture != key.sharedMaterial.mainTexture)
			{
				continue;
			}
			bool flag = false;
			Transform[] bones = key.bones;
			foreach (Transform transform in bones)
			{
				Puppet2D_FFDLineDisplay component = transform.GetComponent<Puppet2D_FFDLineDisplay>();
				if ((bool)component && component.outputSkinnedMesh != key)
				{
					flag = true;
				}
			}
			if (!flag)
			{
				BoneWeight[] boneWeights = key.sharedMesh.boneWeights;
				BoneWeight[] array3 = boneWeights;
				foreach (BoneWeight boneWeight in array3)
				{
					BoneWeight item = boneWeight;
					item.boneIndex0 += num2;
					item.boneIndex1 += num2;
					item.boneIndex2 += num2;
					item.boneIndex3 += num2;
					list2.Add(item);
				}
				num2 += key.bones.Length;
				Transform[] bones2 = key.bones;
				Transform[] array4 = bones2;
				foreach (Transform item2 in array4)
				{
					list.Add(item2);
				}
				if (key.material.mainTexture != null)
				{
					list4.Add(key.GetComponent<Renderer>().material.mainTexture as Texture2D);
				}
				CombineInstance item3 = default(CombineInstance);
				item3.mesh = key.sharedMesh;
				array2[num3] = item3.mesh.vertexCount;
				item3.transform = key.transform.localToWorldMatrix;
				list3.Add(item3);
				Object.Destroy(key.gameObject);
				num3++;
			}
		}
		List<Matrix4x4> list5 = new List<Matrix4x4>();
		for (int m = 0; m < list.Count; m++)
		{
			if ((bool)list[m].GetComponent<Puppet2D_FFDLineDisplay>())
			{
				Vector3 position2 = list[m].transform.parent.parent.position;
				Quaternion rotation2 = list[m].transform.parent.parent.rotation;
				list[m].transform.parent.parent.position = Vector3.zero;
				list[m].transform.parent.parent.rotation = Quaternion.identity;
				list5.Add(list[m].worldToLocalMatrix * base.transform.worldToLocalMatrix);
				list[m].transform.parent.parent.position = position2;
				list[m].transform.parent.parent.rotation = rotation2;
			}
			else
			{
				list5.Add(list[m].worldToLocalMatrix * base.transform.worldToLocalMatrix);
			}
		}
		SkinnedMeshRenderer skinnedMeshRenderer2 = base.gameObject.AddComponent<SkinnedMeshRenderer>();
		skinnedMeshRenderer2.updateWhenOffscreen = updateWhenOffscreen;
		skinnedMeshRenderer2.sharedMesh = new Mesh();
		skinnedMeshRenderer2.sharedMesh.CombineMeshes(list3.ToArray(), true, true);
		Material material2 = ((!(material != null)) ? new Material(Shader.Find("Unlit/Transparent")) : material);
		material2.mainTexture = list4[0];
		skinnedMeshRenderer2.sharedMesh.uv = skinnedMeshRenderer2.sharedMesh.uv;
		skinnedMeshRenderer2.sharedMaterial = material2;
		skinnedMeshRenderer2.bones = list.ToArray();
		skinnedMeshRenderer2.sharedMesh.boneWeights = list2.ToArray();
		skinnedMeshRenderer2.sharedMesh.bindposes = list5.ToArray();
		skinnedMeshRenderer2.sharedMesh.RecalculateBounds();
		base.transform.localScale = localScale;
		base.transform.rotation = rotation;
		base.transform.position = position;
	}

	private void FaceCamera()
	{
		foreach (Puppet2D_IKHandle ikhandle in _Ikhandles)
		{
			ikhandle.AimDirection = base.transform.forward.normalized * _flipCorrection;
		}
	}
}
