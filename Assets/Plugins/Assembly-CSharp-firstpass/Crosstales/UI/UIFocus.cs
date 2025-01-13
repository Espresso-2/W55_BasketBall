using UnityEngine;
using UnityEngine.UI;

namespace Crosstales.UI
{
	public class UIFocus : MonoBehaviour
	{
		[Tooltip("Name of the gameobject containing the UIWindowManager.")]
		public string ManagerName = "Canvas";

		private UIWindowManager manager;

		private Image image;

		private Transform tf;

		public void Start()
		{
			tf = base.transform;
			manager = GameObject.Find(ManagerName).GetComponent<UIWindowManager>();
			image = tf.Find("Panel/Header").GetComponent<Image>();
		}

		public void OnPanelEnter()
		{
			manager.ChangeState(base.gameObject);
			Color color = image.color;
			color.a = 255f;
			image.color = color;
			tf.SetAsLastSibling();
			tf.SetAsFirstSibling();
			tf.SetSiblingIndex(-1);
			tf.GetSiblingIndex();
		}
	}
}
