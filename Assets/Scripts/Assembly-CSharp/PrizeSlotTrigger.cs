using UnityEngine;

public class PrizeSlotTrigger : MonoBehaviour
{
	public PrizeSlotHolder prizeSlotHolder;

	public int slotNum;

	private void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.gameObject.tag == "Ball")
		{
			PrizeBall component = coll.gameObject.GetComponent<PrizeBall>();
			if (!component.DidReachPrizeSlot())
			{
				iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.Failure);
				prizeSlotHolder.BallReachedSlot(slotNum);
			}
			component.OnReachedPrizeSlot();
		}
	}
}
