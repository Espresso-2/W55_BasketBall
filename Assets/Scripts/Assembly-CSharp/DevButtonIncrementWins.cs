using System;
using UnityEngine;

[Serializable]
public class DevButtonIncrementWins : MonoBehaviour
{
	public virtual void Start()
	{
	}

	public virtual void OnClick()
	{
		Stats.SetNumWins(Stats.GetNumWins() + 1);
	}

	public virtual void Update()
	{
	}
}
