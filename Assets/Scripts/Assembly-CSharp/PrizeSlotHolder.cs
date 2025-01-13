using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrizeSlotHolder : MonoBehaviour
{
	public PrizeBallGameController gameController;

	public PrizeSlotUIHolder prizeSlotHolderUI;

	private List<PrizeSlot> slots = new List<PrizeSlot>();

	public void NextBall(int ballNum)
	{
		StartCoroutine(AnimateGenerateSlots(ballNum));
	}

	private IEnumerator AnimateGenerateSlots(int ballNum)
	{
		for (int i = 0; i < 20; i++)
		{
			GenerateSlots(ballNum);
			yield return new WaitForSeconds(0.025f);
		}
	}

	private void GenerateSlots(int ballNum)
	{
		slots.Clear();
		if (PrizeBallGameController.basketballBattleBuild)
		{
			SetupSlotsForBasketballBattle();
		}
		else
		{
			SetupSlotsForPrizeBall(ballNum);
		}
		slots.Shuffle();
		prizeSlotHolderUI.ClearSlots();
		for (int i = 0; i < slots.Count; i++)
		{
			PrizeSlot prizeSlot = slots[i];
			prizeSlotHolderUI.SetupSlot(i, prizeSlot.label, prizeSlot.type);
		}
	}

	private void SetupSlotsForBasketballBattle()
	{
		slots.Add(new PrizeSlot(100, "100", PrizeSlot.SlotType.CashPrize));
		slots.Add(new PrizeSlot(100, "100", PrizeSlot.SlotType.CashPrize));
		slots.Add(new PrizeSlot(100, "100", PrizeSlot.SlotType.CashPrize));
		slots.Add(new PrizeSlot(200, "200", PrizeSlot.SlotType.CashPrize));
		slots.Add(new PrizeSlot(500, "500", PrizeSlot.SlotType.CashPrize));
		slots.Add(new PrizeSlot(900, "900", PrizeSlot.SlotType.CashPrize));
		slots.Add(new PrizeSlot(900, "900", PrizeSlot.SlotType.CashPrize));
		slots.Add(new PrizeSlot(10, "10", PrizeSlot.SlotType.GoldPrize));
		slots.Add(new PrizeSlot(25, "25", PrizeSlot.SlotType.GoldPrize));
		slots.Add(new PrizeSlot(1, "1", PrizeSlot.SlotType.BagPrize));
		slots.Add(new PrizeSlot(1, "1", PrizeSlot.SlotType.BagPrize));
	}

	private void SetupSlotsForPrizeBall(int ballNum)
	{
		if (ballNum >= 4)
		{
			slots.Add(new PrizeSlot(0, "GAME OVER", PrizeSlot.SlotType.GameOver));
		}
		else
		{
			slots.Add(new PrizeSlot(0, "0", PrizeSlot.SlotType.NoPrize));
		}
		slots.Add(new PrizeSlot(75, "75", PrizeSlot.SlotType.SmallPrize));
		slots.Add(new PrizeSlot(500, "500", PrizeSlot.SlotType.MedPrize));
		slots.Add(new PrizeSlot(1000, "1000", PrizeSlot.SlotType.LargePrize));
		if (ballNum >= 2)
		{
			slots.Add(new PrizeSlot(0, "GAME OVER", PrizeSlot.SlotType.GameOver));
		}
		else
		{
			slots.Add(new PrizeSlot(0, "0", PrizeSlot.SlotType.NoPrize));
		}
		slots.Add(new PrizeSlot(5000, "2000", PrizeSlot.SlotType.GrandPrize));
		if (ballNum >= 3)
		{
			slots.Add(new PrizeSlot(0, "GAME OVER", PrizeSlot.SlotType.GameOver));
		}
		else
		{
			slots.Add(new PrizeSlot(0, "0", PrizeSlot.SlotType.NoPrize));
		}
		slots.Add(new PrizeSlot(1000, "1000", PrizeSlot.SlotType.LargePrize));
		slots.Add(new PrizeSlot(500, "500", PrizeSlot.SlotType.MedPrize));
		slots.Add(new PrizeSlot(75, "75", PrizeSlot.SlotType.SmallPrize));
		if (ballNum >= 5)
		{
			slots.Add(new PrizeSlot(0, "GAME OVER", PrizeSlot.SlotType.GameOver));
		}
		else
		{
			slots.Add(new PrizeSlot(0, "0", PrizeSlot.SlotType.NoPrize));
		}
	}

	public void BallReachedSlot(int num)
	{
		PrizeSlot prizeSlot = slots[num];
		if (prizeSlot.type == PrizeSlot.SlotType.GameOver)
		{
			gameController.GameOver();
		}
		else
		{
			prizeSlotHolderUI.ReachedSlot(num);
			StartCoroutine(gameController.ReachedSlot(prizeSlot));
		}
		/*FlurryAnalytics.Instance().LogEvent("REACHED_SLOT", new string[3]
		{
			"slotNum:" + num,
			"prizeAmount:" + prizeSlot.amount,
			"wins:" + PlayerPrefs.GetInt("NUM_WINS") + string.Empty
		}, false);*/
	}
}
