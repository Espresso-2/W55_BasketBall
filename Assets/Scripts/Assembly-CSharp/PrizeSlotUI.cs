using UnityEngine;
using UnityEngine.UI;

public class PrizeSlotUI : MonoBehaviour
{
	public Text label;

	private Color labelOrigColor;

	public Color labelFlickerColor;

	private Image bg;

	public Color bgFlickerColor;

	private Color bgOrigColor;

	private bool reachedSlot;

	private float secondsSinceFlicker;

	private bool showFlickerColor;

	private float flickerSpeed = 0.15f;

	private void Awake()
	{
		labelOrigColor = label.color;
		bg = base.gameObject.GetComponent<Image>();
		bgOrigColor = bg.color;
	}

	private void FixedUpdate()
	{
		secondsSinceFlicker += Time.deltaTime;
		if (secondsSinceFlicker > flickerSpeed && reachedSlot)
		{
			secondsSinceFlicker = 0f;
			if (showFlickerColor)
			{
				label.color = labelFlickerColor;
				bg.color = bgFlickerColor;
			}
			else
			{
				label.color = labelOrigColor;
				bg.color = bgOrigColor;
			}
			showFlickerColor = !showFlickerColor;
		}
	}

	public void BallReachedSlot()
	{
		reachedSlot = true;
		showFlickerColor = true;
		label.color = labelFlickerColor;
		bg.color = bgFlickerColor;
	}
}
