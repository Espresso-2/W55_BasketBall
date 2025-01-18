using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class CurrencyExchangeBox : MonoBehaviour
{
    public GameObject panel;

    public Text cashAmount;

    public Text goldAmount;

    public DealBag dealBag;

    public UpgradeScreen upgradeScreen;

    public PlayerDetails playersScreenPlayerDetails;

    private GameSounds gameSounds;

    public virtual void Start()
    {
        gameSounds = GameSounds.GetInstance();
    }

    public virtual void SetCashPrice(int price, int goldPrice)
    {
        int currentCash = Currency.GetCurrentCash();
        int cash = price - currentCash;
        int cashGoldPrice = Currency.GetCashGoldPrice(cash, goldPrice);
        cashAmount.text = cash.ToString("n0");
        goldAmount.text = cashGoldPrice.ToString("n0");
    }

    public virtual void ExchangeOnClick()
    {
        if (upgradeScreen.gameObject.activeInHierarchy)
        {
            upgradeScreen.ExchangeGoldForCashAndBuyUpgrade();
        }
        else if (playersScreenPlayerDetails.gameObject.activeInHierarchy)
        {
            playersScreenPlayerDetails.ExchangeGoldForCashAndBuyPlayer();
        }
        else
        {
            dealBag.ExchangeGoldForCashAndBuy();
        }
    }

    public virtual void CloseCurrencyExchangeBox()
    {
        gameObject.SetActive(false);
    }
}