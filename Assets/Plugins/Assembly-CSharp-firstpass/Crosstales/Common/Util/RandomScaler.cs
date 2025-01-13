using UnityEngine;

namespace Crosstales.Common.Util
{
	public class RandomScaler : MonoBehaviour
	{
		[Tooltip("Use intervals to change the scale (default: true).")]
		public bool UseInterval = true;

		[Tooltip("Random change interval between min (= x) and max (= y) in seconds (default: x = 10, y = 20).")]
		public Vector2 ChangeInterval = new Vector2(10f, 20f);

		[Tooltip("Minimum rotation speed per axis (default: 5 for all axis).")]
		public Vector3 ScaleMin = new Vector3(0.1f, 0.1f, 0.1f);

		[Tooltip("Maximum scale per axis (default: 0.1 for all axis).")]
		public Vector3 ScaleMax = new Vector3(3f, 3f, 3f);

		[Tooltip("Uniform scaling for all axis (x-axis values will be used, default: true).")]
		public bool Uniform = true;

		[Tooltip("Set the object to a random scale at Start (default: false).")]
		public bool RandomScaleAtStart;

		private Transform tf;

		private Vector3 startScale;

		private Vector3 endScale;

		private float elapsedTime;

		private float changeTime;

		private float lerpTime;

		public void Start()
		{
			tf = base.transform;
			elapsedTime = (changeTime = Random.Range(ChangeInterval.x, ChangeInterval.y));
			if (RandomScaleAtStart)
			{
				if (Uniform)
				{
					startScale.x = (startScale.y = (startScale.z = Random.Range(ScaleMin.x, Mathf.Abs(ScaleMax.x))));
				}
				else
				{
					startScale.x = Random.Range(ScaleMin.x, Mathf.Abs(ScaleMax.x));
					startScale.y = Random.Range(ScaleMin.y, Mathf.Abs(ScaleMax.y));
					startScale.z = Random.Range(ScaleMin.z, Mathf.Abs(ScaleMax.z));
				}
				tf.localScale = startScale;
			}
			else
			{
				startScale = tf.localScale;
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
				lerpTime = (elapsedTime = 0f);
				if (Uniform)
				{
					endScale.x = (endScale.y = (endScale.z = Random.Range(ScaleMin.x, Mathf.Abs(ScaleMax.x))));
				}
				else
				{
					endScale.x = Random.Range(ScaleMin.x, Mathf.Abs(ScaleMax.x));
					endScale.y = Random.Range(ScaleMin.y, Mathf.Abs(ScaleMax.y));
					endScale.z = Random.Range(ScaleMin.z, Mathf.Abs(ScaleMax.z));
				}
				changeTime = Random.Range(ChangeInterval.x, ChangeInterval.y);
			}
			tf.localScale = Vector3.Lerp(startScale, endScale, lerpTime);
			if (lerpTime < 1f)
			{
				lerpTime += Time.deltaTime / (changeTime - 0.1f);
			}
			else
			{
				startScale = tf.localScale;
			}
		}
	}
}
