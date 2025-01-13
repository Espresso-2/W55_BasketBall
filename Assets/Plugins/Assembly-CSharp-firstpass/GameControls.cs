using UnityEngine;

public class GameControls : MonoBehaviour
{
	public GameObject[] moveButtons;

	public GameObject[] jumpButtons;

	private void Start()
	{
		int num = 1;
		for (int i = 0; i < moveButtons.Length; i++)
		{
			moveButtons[i].SetActive(i == num);
		}
		for (int j = 0; j < jumpButtons.Length; j++)
		{
			jumpButtons[j].SetActive(j == num);
		}
	}

	public void SetScales(float scale)
	{
		int num = 1;
		Transform transform = moveButtons[num].transform;
		transform.localScale = new Vector3(transform.localScale.x * scale, transform.localScale.y * scale, 1f);
		Transform transform2 = jumpButtons[num].transform;
		transform2.localScale = new Vector3(transform2.localScale.x * scale, transform2.localScale.y * scale, 1f);
	}
}
