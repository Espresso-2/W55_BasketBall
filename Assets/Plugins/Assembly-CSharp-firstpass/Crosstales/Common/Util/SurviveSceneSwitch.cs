using UnityEngine;

namespace Crosstales.Common.Util
{
	[DisallowMultipleComponent]
	public class SurviveSceneSwitch : MonoBehaviour
	{
		[Tooltip("Objects which have to survive a scene switch.")]
		public GameObject[] Survivors;

		private Transform tf;

		private const float ensureParentTime = 1.5f;

		private float ensureParentTimer;

		public void Awake()
		{
			tf = base.transform;
			Object.DontDestroyOnLoad(tf.root.gameObject);
		}

		public void Start()
		{
			ensureParentTimer = 1.5f;
		}

		public void Update()
		{
			ensureParentTimer += Time.deltaTime;
			if (Survivors == null || !(ensureParentTimer > 1.5f))
			{
				return;
			}
			ensureParentTimer = 0f;
			GameObject[] survivors = Survivors;
			foreach (GameObject gameObject in survivors)
			{
				if (gameObject != null)
				{
					gameObject.transform.SetParent(tf);
				}
			}
		}
	}
}
