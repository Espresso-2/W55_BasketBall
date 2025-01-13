using System.Collections;
using UnityEngine;

public class TestSkin : MonoBehaviour
{
	public int SkinToTest = 1;

	public float Delay = 1f;

	public bool UseRandomLoop;

	public int RandomMin = 1;

	public int RandomMax = 5;

	private IEnumerator Start()
	{
		yield return new WaitForSeconds(Delay);
		if (!UseRandomLoop)
		{
			ArenaSkinController.Instance.UpdateArenaSkin(SkinToTest);
		}
	}

	private void OnEnable()
	{
		if (UseRandomLoop)
		{
			StartCoroutine(RandomLooper());
		}
	}

	private IEnumerator RandomLooper()
	{
		while (base.enabled)
		{
			yield return new WaitForSeconds(Delay);
			int randomSkinIndex = Random.Range(RandomMin, RandomMax);
			ArenaSkinController.Instance.UpdateArenaSkin(randomSkinIndex);
		}
	}
}
