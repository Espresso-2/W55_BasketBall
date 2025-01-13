using System;
using UnityEngine;

[Serializable]
public class DevScreenButton : MonoBehaviour
{
	private int numClicks;

	private string scene;

	public DevScreenButton()
	{
		scene = "Dev";
	}

	public virtual void Start()
	{
	}

	public virtual void OnClick()
	{
		numClicks++;
		if (numClicks >= 3)
		{
			Application.LoadLevel(scene);
		}
	}

	public virtual void Update()
	{
	}
}
