using UnityEngine;
using UnityEngine.EventSystems;

namespace Crosstales.UI
{
	public class UIResize : MonoBehaviour, IPointerDownHandler, IDragHandler, IEventSystemHandler
	{
		[Tooltip("Minimum size of the UI element.")]
		public Vector2 MinSize = new Vector2(300f, 160f);

		[Tooltip("Maximum size of the UI element.")]
		public Vector2 MaxSize = new Vector2(800f, 600f);

		private RectTransform panelRectTransform;

		private Vector2 originalLocalPointerPosition;

		private Vector2 originalSizeDelta;

		private Vector2 originalSize;

		public void Awake()
		{
			panelRectTransform = base.transform.parent.GetComponent<RectTransform>();
			originalSize = new Vector2(panelRectTransform.rect.width, panelRectTransform.rect.height);
		}

		public void OnPointerDown(PointerEventData data)
		{
			originalSizeDelta = panelRectTransform.sizeDelta;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(panelRectTransform, data.position, data.pressEventCamera, out originalLocalPointerPosition);
		}

		public void OnDrag(PointerEventData data)
		{
			if (!(panelRectTransform == null))
			{
				Vector2 localPoint;
				RectTransformUtility.ScreenPointToLocalPointInRectangle(panelRectTransform, data.position, data.pressEventCamera, out localPoint);
				Vector3 vector = localPoint - originalLocalPointerPosition;
				Vector2 sizeDelta = originalSizeDelta + new Vector2(vector.x, 0f - vector.y);
				if (originalSize.x + sizeDelta.x < MinSize.x)
				{
					sizeDelta.x = 0f - (originalSize.x - MinSize.x);
				}
				else if (originalSize.x + sizeDelta.x > MaxSize.x)
				{
					sizeDelta.x = MaxSize.x - originalSize.x;
				}
				if (originalSize.y + sizeDelta.y < MinSize.y)
				{
					sizeDelta.y = 0f - (originalSize.y - MinSize.y);
				}
				else if (originalSize.y + sizeDelta.y > MaxSize.y)
				{
					sizeDelta.y = MaxSize.y - originalSize.y;
				}
				panelRectTransform.sizeDelta = sizeDelta;
			}
		}
	}
}
