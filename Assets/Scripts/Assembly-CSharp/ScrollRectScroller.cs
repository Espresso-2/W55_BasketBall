using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class ScrollRectScroller : MonoBehaviour
{
	public ScrollRect scrollRect;

	public bool scrollingToDest;

	public float scrollingDestX;

	public float scrollingDestY;

	private float scrollTime;

	private float startingPosX;

	private float startingPosY;

	private float changeInValueX;

	private float changeInValueY;

	private float time;

	private float timeAlive;

	public ScrollRectScroller()
	{
		scrollTime = 0.35f;
	}

	public virtual void Start()
	{
	}

	public virtual void Update()
	{
		timeAlive += Time.deltaTime;
		if (scrollingToDest)
		{
			time += Time.deltaTime;
			float num = 0.002f;
			float num2 = EaseInOutQuad(time, startingPosX, changeInValueX, scrollTime);
			if (num2 + num < scrollingDestX)
			{
				scrollRect.horizontalNormalizedPosition = num2;
			}
			else if (num2 - num > scrollingDestX)
			{
				scrollRect.horizontalNormalizedPosition = num2;
			}
			else
			{
				scrollingToDest = false;
			}
			float num3 = 0.002f;
			float num4 = EaseInOutQuad(time, startingPosY, changeInValueY, scrollTime);
			if (num4 + num3 < scrollingDestY)
			{
				scrollRect.verticalNormalizedPosition = num4;
			}
			else if (num4 - num3 > scrollingDestY)
			{
				scrollRect.verticalNormalizedPosition = num4;
			}
		}
	}

	public virtual float LinearTween(float t, float b, float c, float d)
	{
		return c * t / d + b;
	}

	public virtual float EaseOutQuad(float t, float b, float c, float d)
	{
		t /= d;
		return (0f - c) * t * (t - 2f) + b;
	}

	public virtual float EaseInQuad(float t, float b, float c, float d)
	{
		t /= d;
		return c * t * t + b;
	}

	public virtual float EaseInOutQuad(float t, float b, float c, float d)
	{
		t /= d / 2f;
		if (t < 1f)
		{
			return c / 2f * t * t + b;
		}
		t -= 1f;
		return (0f - c) / 2f * (t * (t - 2f) - 1f) + b;
	}

	public virtual void SetHorizontalPos(float pos)
	{
		if (timeAlive <= 0.5f)
		{
			scrollRect.horizontalNormalizedPosition = pos;
			return;
		}
		time = 0f;
		startingPosX = scrollRect.horizontalNormalizedPosition;
		scrollingToDest = true;
		scrollingDestX = pos;
		changeInValueX = pos - startingPosX;
	}

	public virtual void SetPos(float posX, float posY)
	{
		if (timeAlive <= 0.5f)
		{
			scrollRect.horizontalNormalizedPosition = posX;
			scrollRect.verticalNormalizedPosition = posY;
			return;
		}
		time = 0f;
		startingPosX = scrollRect.horizontalNormalizedPosition;
		startingPosY = scrollRect.verticalNormalizedPosition;
		scrollingToDest = true;
		scrollingDestX = posX;
		scrollingDestY = posY;
		changeInValueX = posX - startingPosX;
		changeInValueY = posY - startingPosY;
	}
}
