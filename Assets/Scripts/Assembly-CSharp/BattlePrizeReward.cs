using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BattlePrizeReward : MonoBehaviour
{
	public Text amount;

	public GameObject cashIcon;

	public GameObject goldIcon;

	public GameObject bagIcon;

	private GameSounds gameSounds;

	public virtual void Start()
	{
		gameSounds = GameSounds.GetInstance();
	}

	public void SetReward(PrizeSlot prizeSlot)
	{
		PrizeSlot.SlotType type = prizeSlot.type;
		SaveReward(type, prizeSlot.amount);
		cashIcon.SetActive(type == PrizeSlot.SlotType.CashPrize);
		goldIcon.SetActive(type == PrizeSlot.SlotType.GoldPrize);
		bagIcon.SetActive(type == PrizeSlot.SlotType.BagPrize);
		amount.text = prizeSlot.label;
	}

	public void CloseOnClick()
	{
		StartCoroutine(Close());
	}

	private IEnumerator Close()
	{
		gameSounds.Play_select();
		yield return new WaitForSeconds(0.15f);
		base.gameObject.SetActive(false);
	}

	private void SaveReward(PrizeSlot.SlotType t, int amt)
	{
		switch (t)
		{
		case PrizeSlot.SlotType.CashPrize:
			Currency.AddCash(amt);
			break;
		case PrizeSlot.SlotType.GoldPrize:
			TournamentView.showGoldAnim = true;
			Currency.AddGold(amt);
			break;
		case PrizeSlot.SlotType.BagPrize:
			DealBag.AddStandardBags(amt, "prizeBall");
			break;
		}
	}
}
