using UnityEngine;

public class InterfacePositionAdjuster : MonoBehaviour
{
	public bool wideScreen;

	public bool iPhoneX;

	public bool superWideScreen;

	public float addX;

	public float addY;

	public float scaleX;

	public float scaleY;

	private void Start()
	{
		bool flag = false;
		if (iPhoneX && DtUtils.IsIosDeviceWithiPhoneXStyleScreen())
		{
			flag = true;
		}
		else if (superWideScreen && DtUtils.IsSuperWideScreenDevice())
		{
			flag = true;
		}
		else if (wideScreen && DtUtils.IsWideScreenDevice())
		{
			flag = true;
		}
		if (flag)
		{
			RectTransform component = base.gameObject.GetComponent<RectTransform>();
			if (component != null)
			{
				component.anchoredPosition = new Vector2(component.anchoredPosition.x + addX, component.anchoredPosition.y + addY);
			}
			else if (base.gameObject.transform != null)
			{
				Transform transform = base.gameObject.transform;
				transform.localPosition = new Vector3(transform.localPosition.x + addX, transform.localPosition.y + addY, transform.localPosition.z);
			}
			if (scaleX != 0f && scaleY != 0f)
			{
				component.localScale = new Vector3(scaleX, scaleY, 1f);
			}
		}
	}
}
