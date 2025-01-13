using System;
using UnityEngine;

[Serializable]
public class PlayerVisual : MonoBehaviour
{
	public BallVisual ballVisual;

	public HeadVisual headVisual;

	public SpriteRenderer spriteHead;

	public SpriteRenderer[] skin;

	public SpriteRenderer jersey;

	public SpriteRenderer jerseyGraphics;

	public SpriteRenderer[] armbands;

	public SpriteRenderer[] pants;

	public SpriteRenderer leftShoe;

	public SpriteRenderer rightShoe;

	public SpriteRenderer leftShoeFill;

	public SpriteRenderer rightShoeFill;

	public virtual void Start()
	{
	}

	public virtual void SetSkinTone(Color color)
	{
		SpriteRenderer[] array = skin;
		foreach (SpriteRenderer spriteRenderer in array)
		{
			spriteRenderer.color = color;
		}
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
		SpriteRenderer[] array = armbands;
		foreach (SpriteRenderer spriteRenderer in array)
		{
			spriteRenderer.color = color;
		}
	}

	public virtual void SetPantsColor(Color color)
	{
		SpriteRenderer[] array = pants;
		foreach (SpriteRenderer spriteRenderer in array)
		{
			spriteRenderer.color = color;
		}
	}

	public virtual void SetShoesColor(Color color)
	{
		leftShoe.color = color;
		rightShoe.color = color;
	}

	public virtual void SetShoesFillColor(Color color)
	{
		leftShoeFill.color = color;
		rightShoeFill.color = color;
	}

	public virtual void SetVisual(Player player, CharacterSprites characterSprites, CustomItems customItems, int arenaNum)
	{
		ballVisual.SetVisual(characterSprites, customItems);
		headVisual.SetVisual(player, characterSprites, arenaNum);
		Color skinToneColor = player.GetSkinToneColor(characterSprites);
		SetSkinTone(skinToneColor);
		if (player.isFemale)
		{
			SetJerseySprite(characterSprites.jerseyFemale);
		}
		else
		{
			SetJerseySprite(characterSprites.jersey);
		}
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		int num5 = 1;
		int num6 = 0;
		int num7 = 0;
		CustomItem activeItem = customItems.GetActiveItem(CustomItems.JERSEY);
		num = activeItem.color;
		num2 = activeItem.overlaySprite;
		num3 = activeItem.overlayColor;
		CustomItem activeItem2 = customItems.GetActiveItem(CustomItems.ARM_BAND);
		num4 = activeItem2.color;
		CustomItem activeItem3 = customItems.GetActiveItem(CustomItems.PANTS);
		num5 = activeItem3.color;
		CustomItem activeItem4 = customItems.GetActiveItem(CustomItems.SHOES);
		num6 = activeItem4.overlayColor;
		num7 = activeItem4.color;
		SetJerseyColor(characterSprites.clothingColors[num]);
		SetJerseyGraphicsSprite(characterSprites.jerseyGraphics[num2]);
		SetJerseyGraphicsColor(characterSprites.clothingColors[num3]);
		SetArmBandColor(characterSprites.clothingColors[num4]);
		SetPantsColor(characterSprites.clothingColors[num5]);
		SetShoesColor(characterSprites.clothingColors[num6]);
		SetShoesFillColor(characterSprites.clothingColors[num7]);
	}
}
