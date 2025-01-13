using UnityEngine;

public class ShakeEffect : MonoBehaviour
{
	public Transform objTransform;

	private float shakeDuration;

	private float shakeAmount = 0.7f;

	public float decreaseFactor = 1f;

	private Vector3 originalPos;

	private void Awake()
	{
		if (objTransform == null)
		{
			objTransform = GetComponent(typeof(Transform)) as Transform;
		}
	}

	private void OnEnable()
	{
		originalPos = objTransform.localPosition;
	}

	public void Shake(float duration, float amt)
	{
		shakeDuration = duration;
		shakeAmount = amt;
	}

	private void Update()
	{
		if (shakeDuration > 0f)
		{
			objTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;
			shakeDuration -= Time.deltaTime * decreaseFactor;
		}
		else
		{
			shakeDuration = 0f;
			objTransform.localPosition = originalPos;
		}
	}
}
