using System;
using UnityEngine;

[Serializable]
public class SkinSprite
{
	public Sprite SpriteImage;

	public Color SpriteColor = Color.white;

	public void Apply(SpriteRenderer target)
	{
		target.sprite = SpriteImage;
		target.color = SpriteColor;
	}
}
