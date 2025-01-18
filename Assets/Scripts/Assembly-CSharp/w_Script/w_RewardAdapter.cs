using UnityEngine;

namespace W_Log.w_Script
{
    public class w_RewardAdapter : W_Singleton<w_RewardAdapter>
    {
        public TopNavBar topNavBar;
        public TabChanger tabChanger;

        public void ADDGold(int Amount)
        {
            Currency.AddGold(Amount);
            if (topNavBar == null)
            {
                topNavBar = FindObjectOfType<TopNavBar>();
            }
            topNavBar.UpdateCurrencyDisplay();
        }

        public void onPurchased(int ID)
        {
            Debug.Log("e.PurchasedItem.Id: " + ID);
            int num = -1;
            int num2 = -1;
            int num3 = -1;
            int num4 = -1;
            int num5 = -1;
            switch (ID)
            {
                case 0:
                    num = 0;
                    break;
                case 1:
                    num = 1;
                    break;
                case 2:
                    num = 2;
                    break;
                case 3:
                    num = 3;
                    break;
                case 4:
                    num = 4;
                    break;
                case 5:
                    num = 5;
                    break;
                case 6:
                    num = 6;
                    break;
                case 7:
                    num = 7;
                    break;
                case 8:
                    num2 = 0;
                    break;
                case 9:
                    num2 = 1;
                    break;
                case 10:
                    num3 = 0;
                    break;
                case 11:
                    num4 = 0;
                    break;
                case 12:
                    num5 = 0;
                    break;
                case 13:
                    num5 = 1;
                    break;
            }
            if (num >= 0)
            {
                giveGoldPackage(num, false);
            }
            else if (num3 >= 0)
            {
                giveGoldPackage(num3, true);
            }
            else if (num2 >= 0)
            {
                giveDeal(num2);
            }
            else if (num4 >= 0)
            {
                removeAds();
            }
            else if (num5 >= 0)
            {
                givePrizeBallsPackage(num5);
            }
        }

        private void giveGoldPackage(int num, bool includeCash)
        {
            if (includeCash)
            {
                GameObject.Find("PurchaseGiver").SendMessage("AddGoldAndCashPackage", num);
            }
            else
            {
                GameObject.Find("PurchaseGiver").SendMessage("AddGoldPackage", num);
            }
            GameObject gameObject = GameObject.Find("GetGoldBox");
            if (gameObject == null)
            {
                GameObject gameObject2 = GameObject.Find("PlusButton");
                if (gameObject2 != null)
                {
                    gameObject2.SendMessage("ShowGetGoldBox");
                    gameObject = GameObject.Find("GetGoldBox");
                }
            }
            if (gameObject != null)
            {
                if (includeCash)
                {
                    gameObject.SendMessage("AddedGoldAndCashPackage", num);
                }
                else
                {
                    gameObject.SendMessage("AddedGoldPackage", num);
                }
                GameObject gameObject3 = GameObject.Find("BonusOfferMsgBox");
                if (gameObject3 != null)
                {
                    gameObject3.SendMessage("CompletedPurchase");
                }
            }
            else
            {
                GameObject gameObject4 = GameObject.Find("HotPurchaseBox");
                if (gameObject4 != null && gameObject4.activeInHierarchy)
                {
                    gameObject4.SendMessage("AddedGoldPackage", num);
                }
            }
        }

        private void giveDeal(int num)
        {
            GameObject.Find("PurchaseGiver").SendMessage("RecieveDeal", num);
            GameObject gameObject = GameObject.Find("DealPack");
            if (gameObject != null)
            {
                gameObject.SendMessage("RecievedDeal", num);
            }
        }

        private void givePrizeBallsPackage(int num)
        {
            GameObject.Find("PurchaseGiver").SendMessage("AddPrizeBallsPackage", num);
            GameObject gameObject = GameObject.Find("PrizeBallGame");
            if (gameObject != null)
            {
                PrizeBallGameController component = gameObject.GetComponent<PrizeBallGameController>();
                component.PurchasedPrizeBalls();
            }
        }

        private void removeAds()
        {
            GameObject.Find("PurchaseGiver").SendMessage("RemoveAds");
            GameObject gameObject = GameObject.Find("RemoveAdsMsgBox");
            if (gameObject != null)
            {
                gameObject.SendMessage("RemovedAds");
            }
        }
    }
}