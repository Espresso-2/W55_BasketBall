public class PrizeSlot
{
	public enum SlotType
	{
		NoPrize = 0,
		SmallPrize = 1,
		MedPrize = 2,
		LargePrize = 3,
		GrandPrize = 4,
		GameOver = 5,
		CashPrize = 6,
		GoldPrize = 7,
		BagPrize = 8
	}

	public int amount;

	public SlotType type;

	public string label;

	public PrizeSlot(int amount, string label, SlotType type)
	{
		this.amount = amount;
		this.label = label;
		this.type = type;
	}
}
