using System;
using UnityEngine;

[Serializable]
public class ExitBox : MonoBehaviour
{
	public virtual void Start()
	{
	}

	public virtual void Update()
	{
	}

	public virtual void ExitGame()
	{
		Application.Quit();
	}
}
