using UnityEngine;

public class PrizeSlotUIHolder : MonoBehaviour
{
	public GameObject[] slots;

	public GameObject noPrizePrefab;

	public GameObject smallPrizePrefab;

	public GameObject medPrizePrefab;

	public GameObject largePrizePrefab;

	public GameObject grandPrizePrefab;

	public GameObject gameOverPrefab;

	public GameObject cashPrizePrefab;

	public GameObject goldPrizePrefab;

	public GameObject bagPrizePrefab;

	public void ClearSlots()
	{
		GameObject[] array = slots;
		foreach (GameObject gameObject in array)
		{
			foreach (Transform item in gameObject.transform)
			{
				if (item.gameObject.tag == "PrizeSlotUI")
				{
					Object.Destroy(item.gameObject);
				}
			}
		}
	}

	public void SetupSlot(int num, string label, PrizeSlot.SlotType type)
	{
		GameObject original = null;
		switch (type)
		{
		case PrizeSlot.SlotType.NoPrize:
			original = noPrizePrefab;
			break;
		case PrizeSlot.SlotType.SmallPrize:
			original = smallPrizePrefab;
			break;
		case PrizeSlot.SlotType.MedPrize:
			original = medPrizePrefab;
			break;
		case PrizeSlot.SlotType.LargePrize:
			original = largePrizePrefab;
			break;
		case PrizeSlot.SlotType.GrandPrize:
			original = grandPrizePrefab;
			break;
		case PrizeSlot.SlotType.GameOver:
			original = gameOverPrefab;
			break;
		case PrizeSlot.SlotType.CashPrize:
			original = cashPrizePrefab;
			break;
		case PrizeSlot.SlotType.GoldPrize:
			original = goldPrizePrefab;
			break;
		case PrizeSlot.SlotType.BagPrize:
			original = bagPrizePrefab;
			break;
		}
		PrizeSlotUI prizeSlotUI = (PrizeSlotUI)Object.Instantiate(original, slots[num].transform).GetComponent(typeof(PrizeSlotUI));
		prizeSlotUI.transform.SetAsFirstSibling();
		prizeSlotUI.label.text = label;
	}

	public void ReachedSlot(int num)
	{
		foreach (Transform item in slots[num].transform)
		{
			if (item.gameObject.tag == "PrizeSlotUI")
			{
				PrizeSlotUI component = item.GetComponent<PrizeSlotUI>();
				component.BallReachedSlot();
			}
		}
	}
}
