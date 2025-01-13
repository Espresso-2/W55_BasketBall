using System.Collections.Generic;
using System.IO;
using UnityEngine;

[ExecuteInEditMode]
public class Puppet2D_SplineControl : MonoBehaviour
{
	public List<Transform> _splineCTRLS = new List<Transform>();

	public int numberBones = 4;

	private List<Vector3> outCoordinates = new List<Vector3>();

	public List<GameObject> bones = new List<GameObject>();

	private static string _puppet2DPath;

	private string GetUniqueName(string name)
	{
		int num = name.Length + 1;
		int num2 = 0;
		Object[] array = Object.FindObjectsOfType(typeof(GameObject));
		for (int i = 0; i < array.Length; i++)
		{
			GameObject gameObject = (GameObject)array[i];
			if (gameObject.name.StartsWith(name))
			{
				string s = gameObject.name.Substring(num, gameObject.name.Length - num);
				int result = 0;
				if (int.TryParse(s, out result) && int.Parse(s) > num2)
				{
					num2 = int.Parse(s);
				}
			}
		}
		num2++;
		return name + "_" + num2;
	}

	public void Run()
	{
		Quaternion quaternion = Quaternion.Euler(new Vector3(0f, base.transform.eulerAngles.y, 0f));
		CatmullRom(_splineCTRLS, out outCoordinates, numberBones);
		for (int i = 0; i < outCoordinates.Count; i++)
		{
			Vector3 position = outCoordinates[i];
			bones[i].transform.position = position;
			if (i < outCoordinates.Count - 1)
			{
				if (i == 0 && outCoordinates.Count > 2)
				{
					bones[i].transform.rotation = _splineCTRLS[1].transform.rotation;
				}
				else
				{
					bones[i].transform.rotation = Quaternion.LookRotation(outCoordinates[i] - outCoordinates[i + 1], Vector3.forward) * Quaternion.AngleAxis(90f, Vector3.left) * quaternion;
				}
			}
			else
			{
				bones[i].transform.rotation = _splineCTRLS[_splineCTRLS.Count - 2].transform.rotation;
			}
		}
	}

	public static bool CatmullRom(List<Transform> inCoordinates, out List<Vector3> outCoordinates, int samples)
	{
		if (inCoordinates.Count < 4)
		{
			outCoordinates = null;
			return false;
		}
		List<Vector3> list = new List<Vector3>();
		for (int i = 1; i < inCoordinates.Count - 2; i++)
		{
			for (int j = 0; j < samples; j++)
			{
				list.Add(PointOnCurve(inCoordinates[i - 1].position, inCoordinates[i].position, inCoordinates[i + 1].position, inCoordinates[i + 2].position, 1f / (float)samples * (float)j));
			}
		}
		list.Add(inCoordinates[inCoordinates.Count - 2].position);
		outCoordinates = list;
		return true;
	}

	public static Vector3 PointOnCurve(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
	{
		Vector3 result = default(Vector3);
		float num = ((0f - t + 2f) * t - 1f) * t * 0.5f;
		float num2 = ((3f * t - 5f) * t * t + 2f) * 0.5f;
		float num3 = ((-3f * t + 4f) * t + 1f) * t * 0.5f;
		float num4 = (t - 1f) * t * t * 0.5f;
		result.x = p0.x * num + p1.x * num2 + p2.x * num3 + p3.x * num4;
		result.y = p0.y * num + p1.y * num2 + p2.y * num3 + p3.y * num4;
		result.z = p0.z * num + p1.z * num2 + p2.z * num3 + p3.z * num4;
		return result;
	}

	private static void RecursivelyFindFolderPath(string dir)
	{
		string[] directories = Directory.GetDirectories(dir);
		string[] array = directories;
		foreach (string text in array)
		{
			if (text.Contains("Puppet2D"))
			{
				_puppet2DPath = text;
				break;
			}
			RecursivelyFindFolderPath(text);
		}
	}
}
