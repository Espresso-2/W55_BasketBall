using System;
using UnityEngine;

[Serializable]
public class Background : MonoBehaviour
{
	public SpriteRenderer fillRenderer;

	public virtual void Start()
	{
	}

	public virtual void SetColor(Color c)
	{
		fillRenderer.color = c;
	}

	public virtual void Update()
	{
	}
}
