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
            topNavBar.UpdateCurrencyDisplay();
        }
    }
}