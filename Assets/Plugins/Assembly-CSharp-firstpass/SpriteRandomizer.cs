using System.Collections.Generic;
using UnityEngine;

public class SpriteRandomizer : MonoBehaviour
{
	public GameObject[] RandomObjects;

	[SerializeField]
	private int _minObjects;

	[SerializeField]
	private int _maxObjects;

	private void Start()
	{
		SetInitialObjects();
	}

	private void SetInitialObjects()
	{
		int num = Random.Range(_minObjects, _maxObjects);
		List<int> list = new List<int>();
		for (int i = 0; i < RandomObjects.Length; i++)
		{
			RandomObjects[i].SetActive(false);
			list.Add(i);
		}
		for (int j = 0; j < num; j++)
		{
			int num2 = Random.Range(0, list.Count);
			RandomObjects[list[num2]].SetActive(true);
			list.Remove(num2);
		}
	}

	private void Update()
	{
	}
}
