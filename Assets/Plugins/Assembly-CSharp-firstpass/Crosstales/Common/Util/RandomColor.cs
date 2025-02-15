using UnityEngine;

namespace Crosstales.Common.Util
{
	public class RandomColor : MonoBehaviour
	{
		[Tooltip("Use intervals to change the color (default: true).")]
		public bool UseInterval = true;

		[Tooltip("Random change interval between min (= x) and max (= y) in seconds (default: x = 5, y = 10).")]
		public Vector2 ChangeInterval = new Vector2(5f, 10f);

		[Tooltip("Random hue range between min (= x) and max (= y) (default: x = 0, y = 1).")]
		public Vector2 HueRange = new Vector2(0f, 1f);

		[Tooltip("Random saturation range between min (= x) and max (= y) (default: x = 1, y = 1).")]
		public Vector2 SaturationRange = new Vector2(1f, 1f);

		[Tooltip("Random value range between min (= x) and max (= y) (default: x = 1, y = 1).")]
		public Vector2 ValueRange = new Vector2(1f, 1f);

		[Tooltip("Random alpha range between min (= x) and max (= y) (default: x = 1, y = 1).")]
		public Vector2 AlphaRange = new Vector2(1f, 1f);

		[Tooltip("Use gray scale colors (default: false).")]
		public bool GrayScale;

		[Tooltip("Modify the color of a material instead of the Renderer (default: not set, optional).")]
		public Material Material;

		[Tooltip("Set the object to a random color at Start (default: false).")]
		public bool RandomColorAtStart;

		private float elapsedTime;

		private float changeTime;

		private Renderer currentRenderer;

		private Color32 startColor;

		private Color32 endColor;

		private float lerpProgress;

		public void Start()
		{
			elapsedTime = (changeTime = Random.Range(ChangeInterval.x, ChangeInterval.y));
			if (RandomColorAtStart)
			{
				if (GrayScale)
				{
					float num = Random.Range(HueRange.x, HueRange.y);
					startColor = new Color(num, num, num, Random.Range(AlphaRange.x, AlphaRange.y));
				}
				else
				{
					startColor = Random.ColorHSV(HueRange.x, HueRange.y, SaturationRange.x, SaturationRange.y, ValueRange.x, ValueRange.y, AlphaRange.x, AlphaRange.y);
				}
				if (Material != null)
				{
					Material.SetColor("_Color", startColor);
					return;
				}
				currentRenderer = GetComponent<Renderer>();
				currentRenderer.material.color = startColor;
			}
			else if (Material != null)
			{
				startColor = Material.GetColor("_Color");
			}
			else
			{
				currentRenderer = GetComponent<Renderer>();
				startColor = currentRenderer.material.color;
			}
		}

		public void Update()
		{
			if (!UseInterval)
			{
				return;
			}
			elapsedTime += Time.deltaTime;
			if (elapsedTime > changeTime)
			{
				lerpProgress = (elapsedTime = 0f);
				if (GrayScale)
				{
					float num = Random.Range(HueRange.x, HueRange.y);
					endColor = new Color(num, num, num, Random.Range(AlphaRange.x, AlphaRange.y));
				}
				else
				{
					endColor = Random.ColorHSV(HueRange.x, HueRange.y, SaturationRange.x, SaturationRange.y, ValueRange.x, ValueRange.y, AlphaRange.x, AlphaRange.y);
				}
				changeTime = Random.Range(ChangeInterval.x, ChangeInterval.y);
			}
			if (Material != null)
			{
				Material.SetColor("_Color", Color.Lerp(startColor, endColor, lerpProgress));
			}
			else
			{
				currentRenderer.material.color = Color.Lerp(startColor, endColor, lerpProgress);
			}
			if (lerpProgress < 1f)
			{
				lerpProgress += Time.deltaTime / (changeTime - 0.1f);
			}
			else if (Material != null)
			{
				startColor = Material.GetColor("_Color");
			}
			else
			{
				startColor = currentRenderer.material.color;
			}
		}
	}
}
