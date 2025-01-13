using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class CustomItemVisual : MonoBehaviour
{
	public GameObject[] items;

	public Image ball;

	public Image ballFill;

	public Image jersey;

	public Image jerseyGraphics;

	public Image[] armbands;

	public Image[] pants;

	public Image leftShoe;

	public Image leftShoeFill;

	public virtual void SetBallColor(Color color)
	{
		ball.color = color;
	}

	public virtual void SetBallFillSprite(Sprite sprite)
	{
		ballFill.sprite = sprite;
	}

	public virtual void SetBallFillColor(Color color)
	{
		ballFill.color = color;
	}

	public virtual void SetJerseySprite(Sprite sprite)
	{
		jersey.sprite = sprite;
	}

	public virtual void SetJerseyColor(Color color)
	{
		jersey.color = color;
	}

	public virtual void SetJerseyGraphicsSprite(Sprite sprite)
	{
		jerseyGraphics.sprite = sprite;
	}

	public virtual void SetJerseyGraphicsColor(Color color)
	{
		jerseyGraphics.color = color;
	}

	public virtual void SetArmBandColor(Color color)
	{
		Image[] array = armbands;
		foreach (Image image in array)
		{
			image.color = color;
		}
	}

	public virtual void SetPantsColor(Color color)
	{
		Image[] array = pants;
		foreach (Image image in array)
		{
			image.color = color;
		}
	}

	public virtual void SetShoesColor(Color color)
	{
		leftShoe.color = color;
	}

	public virtual void SetShoesFillColor(Color color)
	{
		leftShoeFill.color = color;
	}

	public virtual void SetVisual(CustomItem customItem)
	{
		for (int i = 0; i < items.Length; i++)
		{
			items[i].SetActive(i == customItem.type);
		}
		CharacterSprites characterSprites = (CharacterSprites)GameObject.Find("CharacterSprites").GetComponent(typeof(CharacterSprites));
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		int num5 = 0;
		int num6 = 0;
		int num7 = 0;
		int num8 = 1;
		int num9 = 0;
		int num10 = 0;
		if (customItem.type == CustomItems.BALL)
		{
			num = customItem.color;
			num2 = customItem.overlaySprite;
			num3 = customItem.overlayColor;
			SetBallColor(characterSprites.clothingColors[num]);
			SetBallFillSprite(characterSprites.ballFills[num2]);
			SetBallFillColor(characterSprites.clothingColors[num3]);
		}
		else if (customItem.type == CustomItems.JERSEY)
		{
			num4 = customItem.color;
			num5 = customItem.overlaySprite;
			num6 = customItem.overlayColor;
			SetJerseyColor(characterSprites.clothingColors[num4]);
			SetJerseyGraphicsSprite(characterSprites.jerseyGraphics[num5]);
			SetJerseyGraphicsColor(characterSprites.clothingColors[num6]);
		}
		else if (customItem.type == CustomItems.ARM_BAND)
		{
			num7 = customItem.color;
			SetArmBandColor(characterSprites.clothingColors[num7]);
		}
		else if (customItem.type == CustomItems.PANTS)
		{
			num8 = customItem.color;
			SetPantsColor(characterSprites.clothingColors[num8]);
		}
		else if (customItem.type == CustomItems.SHOES)
		{
			num9 = customItem.overlayColor;
			num10 = customItem.color;
			SetShoesColor(characterSprites.clothingColors[num9]);
			SetShoesFillColor(characterSprites.clothingColors[num10]);
		}
	}
}
