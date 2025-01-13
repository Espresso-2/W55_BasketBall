using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Puppet2D_FFDStoreData : MonoBehaviour
{
	public List<Transform> FFDCtrls = new List<Transform>();

	public List<int> FFDPathNumber = new List<int>();

	[HideInInspector]
	public bool Editable = true;

	private void Update()
	{
		if (!Editable)
		{
			return;
		}
		for (int num = FFDCtrls.Count - 1; num >= 0; num--)
		{
			if (FFDCtrls[num] == null)
			{
				FFDCtrls.RemoveAt(num);
			}
		}
	}
}
