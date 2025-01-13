using UnityEngine;

public class HolidayItem : MonoBehaviour
{
	public enum HolidayType
	{
		None = 0,
		Halloween = 1,
		Easter = 2,
		Christmas = 3
	}

	public HolidayType type;

	public void Start()
	{
		if (GetCurrentHoliday() != type)
		{
			base.gameObject.SetActive(false);
		}
	}

	public static HolidayType GetCurrentHoliday()
	{
		return (HolidayType)PlayerPrefs.GetInt("CURRENT_HOLIDAY");
	}
}
