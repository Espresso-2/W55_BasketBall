using UnityEngine;

public class SimpleSpriteTranslate : MonoBehaviour
{
	[SerializeField]
	private float _minSpeed;

	[SerializeField]
	private float _maxSpeed;

	[SerializeField]
	private float _rotateSpeed;

	[SerializeField]
	private float _verticalSpeed;

	[SerializeField]
	private float _maxSecondsAlive;

	private float _speed;

	private float _secondsAlive;

	[SerializeField]
	private bool _animate = true;

	private void Start()
	{
		_speed = Random.Range(_minSpeed, _maxSpeed);
	}

	private void Update()
	{
		_secondsAlive += Time.deltaTime;
		if (_secondsAlive >= _maxSecondsAlive && _maxSecondsAlive > 0f)
		{
			Object.Destroy(base.gameObject);
		}
		else
		{
			MoveMe();
		}
	}

	private void MoveMe()
	{
		if (_animate)
		{
			if (Mathf.Abs(_speed) > 0f || Mathf.Abs(_verticalSpeed) > 0f)
			{
				base.transform.Translate(_speed * Time.deltaTime, _verticalSpeed * Time.deltaTime, 0f);
			}
			if (Mathf.Abs(_rotateSpeed) > 0f)
			{
				base.transform.Rotate(0f, 0f, _rotateSpeed * Time.deltaTime);
			}
		}
	}
}
