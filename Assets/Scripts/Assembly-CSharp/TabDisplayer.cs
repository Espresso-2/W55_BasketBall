using UnityEngine;
using UnityEngine.UI;

public class TabDisplayer : MonoBehaviour
{
	public Color notActiveColor;

	public Color activeColor;

	public Image[] tabImages;

	private bool initiated;

	public void Start()
	{
		if (!initiated)
		{
			SetActiveTab(0);
		}
	}

	public void SetActiveTab(int tabNum)
	{
		for (int i = 0; i < tabImages.Length; i++)
		{
			Image image = tabImages[i];
			if (tabNum == i)
			{
				image.color = activeColor;
			}
			else
			{
				image.color = notActiveColor;
			}
		}
		initiated = true;
	}
}
