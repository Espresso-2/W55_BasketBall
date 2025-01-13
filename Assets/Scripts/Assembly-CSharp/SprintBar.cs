using System;
using UnityEngine;

[Serializable]
public class SprintBar : MonoBehaviour
{
	public bool isFull;

	public bool isMedium;

	public bool isTired;

	private bool displayTired;

	public bool isOpponent;

	public float generate;

	public GameObject fill;

	public SpriteRenderer fillRenderer;

	public SpriteRenderer leftEdgeFillRenderer;

	public Color fullColor;

	public Color mediumColor;

	public Color lowColor;

	private float full;

	private float tired;

	private float fillPercent;

	private float decPercentPerSecond;

	public GameRoster gameRoster;

	public GameComputer gameComputer;

	public GameObject GreenParticles;

	public GameObject YellowParticles;

	public GameObject RedParticles;

	public SprintBar()
	{
		full = 0.65f;
		tired = 0.42f;
		fillPercent = 1f;
		decPercentPerSecond = 0.1f;
	}

	public virtual void Start()
	{
		if (PlayerPrefs.GetInt("SPRINTBARS_OFF") == 1)
		{
			base.gameObject.SetActive(false);
		}
		Reset();
	}

	public virtual void Reset()
	{
		fillRenderer.color = fullColor;
		leftEdgeFillRenderer.color = fullColor;
		fillPercent = 1f;
		fill.transform.localScale = new Vector3(fillPercent, 1f, 1f);
	}

	public virtual void Update()
	{
		bool flag = false;
		bool active = true;
		flag = ((!isOpponent || !(gameComputer != null)) ? gameRoster.IsLowOnHydration() : gameComputer.IsLowOnHydration());
		if (fillPercent >= 0.1f && fillPercent <= 1f)
		{
			float num = decPercentPerSecond;
			if (flag && generate > 0.2f)
			{
				num *= 0.15f;
			}
			float num2 = num * generate;
			fillPercent += Time.deltaTime * num2;
			if (fillPercent < 0.1f)
			{
				fillPercent = 0.1f;
			}
			else if (fillPercent > 1f)
			{
				fillPercent = 1f;
			}
			active = num2 >= 0.11f && fillPercent < 1f;
		}
		bool flag2 = false;
		bool flag3 = false;
		bool flag4 = false;
		if (fillPercent > full)
		{
			flag2 = true;
		}
		else if (fillPercent < tired)
		{
			flag3 = true;
		}
		else
		{
			flag4 = true;
		}
		if (!isFull && flag2)
		{
			isTired = false;
			isFull = true;
			isMedium = false;
			if (!isOpponent)
			{
				gameRoster.SprintBarInRed(false);
			}
			fillRenderer.color = fullColor;
			leftEdgeFillRenderer.color = fullColor;
		}
		else if (!isTired && flag3)
		{
			isTired = true;
			isFull = false;
			isMedium = false;
			fillRenderer.color = lowColor;
			leftEdgeFillRenderer.color = lowColor;
			if (!isOpponent)
			{
				gameRoster.SprintBarInRed(true);
			}
		}
		else if (!isMedium && flag4)
		{
			isTired = false;
			isFull = false;
			isMedium = true;
			if (!isOpponent)
			{
				gameRoster.SprintBarInRed(false);
			}
			fillRenderer.color = mediumColor;
			leftEdgeFillRenderer.color = mediumColor;
		}
		if (GreenParticles != null)
		{
			GreenParticles.SetActive(active);
		}
		fill.transform.localScale = new Vector3(fillPercent, 1f, 1f);
	}

	public virtual float GetFillPercent()
	{
		return fillPercent;
	}
}
