using System;
using UnityEngine;

[Serializable]
public class BallVisual : MonoBehaviour
{
	public SpriteRenderer ball;

	public SpriteRenderer ballFill;

	private Color nonFlickeringColor;

	private Sprite nonFlickeringSprite;

	private Color nonFlickeringFillColor;

	private Sprite nonFlickeringFillSprite;

	private Color flickeringColor;

	private Color flickeringColorRed;

	public Sprite ballBlocked;

	private float secondsToFlicker;

	private bool flickerStopped;

	private bool flickerRed;

	private float flickerTimer;

	private float flickerLength;

	private bool flickerBool;

	public BallVisual()
	{
		flickeringColor = Color.white;
		flickeringColorRed = new Color(77f / 85f, 0.29803923f, 0.23529412f, 1f);
		flickerStopped = true;
		flickerLength = 0.1f;
		flickerBool = true;
	}

	public virtual void OnDisable()
	{
		SetToNonFlickering();
	}

	public virtual void Update()
	{
		if (secondsToFlicker > 0f)
		{
			secondsToFlicker -= Time.deltaTime;
			flickerStopped = false;
			flickerTimer += Time.deltaTime;
			if (flickerTimer >= flickerLength)
			{
				if (flickerBool)
				{
					ball.sprite = ballBlocked;
					ball.color = flickeringColor;
				}
				else if (flickerRed)
				{
					ball.sprite = ballBlocked;
					ball.color = flickeringColorRed;
				}
				else
				{
					ball.sprite = nonFlickeringSprite;
					ball.color = nonFlickeringColor;
				}
				flickerTimer = 0f;
				flickerBool = !flickerBool;
			}
		}
		else
		{
			StopFlicker();
		}
	}

	public virtual void SetVisual(CharacterSprites characterSprites, CustomItems customItems)
	{
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		CustomItem activeItem = customItems.GetActiveItem(CustomItems.BALL);
		num = activeItem.color;
		num2 = activeItem.overlaySprite;
		num3 = activeItem.overlayColor;
		nonFlickeringColor = characterSprites.clothingColors[num];
		nonFlickeringSprite = ball.sprite;
		nonFlickeringFillSprite = characterSprites.ballFills[num2];
		nonFlickeringFillColor = characterSprites.clothingColors[num3];
		SetToNonFlickering();
	}

	public virtual void SetSortingLayerName(string name)
	{
		((SpriteRenderer)ball.GetComponent(typeof(SpriteRenderer))).sortingLayerName = name;
		((SpriteRenderer)ballFill.GetComponent(typeof(SpriteRenderer))).sortingLayerName = name;
	}

	public virtual void FlickerRed()
	{
		flickerRed = true;
		secondsToFlicker += 0.75f;
	}

	public virtual void Flicker()
	{
		flickerRed = false;
		secondsToFlicker += 0.75f;
	}

	public virtual void ShortFlicker()
	{
		flickerRed = false;
		secondsToFlicker += 0.45f;
	}

	public virtual void StopFlicker()
	{
		if (!flickerStopped)
		{
			secondsToFlicker = 0f;
			flickerTimer = 99f;
			flickerBool = true;
			flickerStopped = true;
			SetToNonFlickering();
		}
	}

	private void SetToNonFlickering()
	{
		if (!(ball == null))
		{
			ball.color = nonFlickeringColor;
			if (nonFlickeringSprite != null)
			{
				ball.sprite = nonFlickeringSprite;
			}
			ballFill.sprite = nonFlickeringFillSprite;
			ballFill.color = nonFlickeringFillColor;
		}
	}
}
