using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class ScoreboardTimeouts : MonoBehaviour
{
	public Image[] lights;

	public Sprite offImage;

	public Sprite onImage;

	public virtual void Start()
	{
	}

	public virtual void Update()
	{
	}

	public virtual void SetNumTimeouts(int n)
	{
		for (int i = 0; i < lights.Length; i++)
		{
			Image image = lights[i];
			if (n > i)
			{
				image.sprite = onImage;
			}
			else
			{
				image.sprite = offImage;
			}
		}
	}
}
