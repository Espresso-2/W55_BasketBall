using Crosstales.Common.Util;
using UnityEngine;
using UnityEngine.UI;

namespace Crosstales.UI.Util
{
	public class ScrollRectHandler : MonoBehaviour
	{
		public ScrollRect Scroll;

		private float WindowsSensitivity = 35f;

		private float MacSensitivity = 25f;

		public void Start()
		{
			if (BaseHelper.isWindowsPlatform)
			{
				Scroll.scrollSensitivity = WindowsSensitivity;
			}
			else if (BaseHelper.isMacOSPlatform)
			{
				Scroll.scrollSensitivity = MacSensitivity;
			}
		}
	}
}
