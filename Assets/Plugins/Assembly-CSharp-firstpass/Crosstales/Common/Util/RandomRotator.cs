using UnityEngine;

namespace Crosstales.Common.Util
{
	public class RandomRotator : MonoBehaviour
	{
		[Tooltip("Use intervals to change the rotation (default: true).")]
		public bool UseInterval = true;

		[Tooltip("Random change interval between min (= x) and max (= y) in seconds (default: x = 10, y = 20).")]
		public Vector2 ChangeInterval = new Vector2(10f, 20f);

		[Tooltip("Minimum rotation speed per axis (default: 5 for all axis).")]
		public Vector3 SpeedMin = new Vector3(5f, 5f, 5f);

		[Tooltip("Minimum rotation speed per axis (default: 15 for all axis).")]
		public Vector3 SpeedMax = new Vector3(15f, 15f, 15f);

		[Tooltip("Set the object to a random rotation at Start (default: false).")]
		public bool RandomRotationAtStart;

		private Transform tf;

		private Vector3 speed;

		private float elapsedTime;

		private float changeTime;

		public void Start()
		{
			tf = base.transform;
			elapsedTime = (changeTime = Random.Range(ChangeInterval.x, ChangeInterval.y));
			if (RandomRotationAtStart)
			{
				tf.localRotation = Random.rotation;
			}
		}

		public void Update()
		{
			if (UseInterval)
			{
				elapsedTime += Time.deltaTime;
				if (elapsedTime > changeTime)
				{
					elapsedTime = 0f;
					speed.x = Random.Range(Mathf.Abs(SpeedMin.x), Mathf.Abs(SpeedMax.x)) * (float)((Random.Range(0, 2) == 0) ? 1 : (-1));
					speed.y = Random.Range(Mathf.Abs(SpeedMin.y), Mathf.Abs(SpeedMax.y)) * (float)((Random.Range(0, 2) == 0) ? 1 : (-1));
					speed.z = Random.Range(Mathf.Abs(SpeedMin.z), Mathf.Abs(SpeedMax.z)) * (float)((Random.Range(0, 2) == 0) ? 1 : (-1));
					changeTime = Random.Range(ChangeInterval.x, ChangeInterval.y);
				}
				tf.Rotate(speed.x * Time.deltaTime, speed.y * Time.deltaTime, speed.z * Time.deltaTime);
			}
		}
	}
}
