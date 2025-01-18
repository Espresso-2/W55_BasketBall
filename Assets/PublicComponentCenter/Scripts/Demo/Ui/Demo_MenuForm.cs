//------------------------------------------------------------
// Copyright © 2020-2021 Lefend. All rights reserved.
//------------------------------------------------------------

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace PublicComponentCenter
{
    public class Demo_MenuForm : UGuiForm
    {
        public Image Title;
        [SerializeField] private GameObject m_MenuForm;
        [SerializeField] private GameObject m_SettingForm;
        [SerializeField] private GameObject m_SelectForm;
        [SerializeField] private Text m_Text;
        private int textIndex = 0;

        protected override void Start()
        {
            base.Start();
            //修改标题
            Title.sprite = GameEntry.Localization.GetUiSprite("Title");
        }

        public void OnBtnSettingClick()
        {
            m_SettingForm.SetActive(true);
        }

        public void OnBtnChangeTextClick()
        {
            textIndex++;
            textIndex %= 4;
            m_Text.text = GameEntry.Localization.GetString("Custom.Test" + textIndex);
        }

        public void OnBtnStartClick()
        {
            Debug.Log("开始游戏");
            m_SelectForm.SetActive(true);
            m_MenuForm.SetActive(false);
        }

        public void OnBtnQuitClick()
        {
            Debug.Log("退出游戏");
            GameEntry.Ad.Quit();
        }


        public void OnBtnEventClick()
        {
            SceneManager.LoadScene("Demo_Event");
        }

        public void OnBtnAdClick()
        {
            SceneManager.LoadScene("Demo_Ad");
        }
        public void OnBtnSettingDemoClick()
        {
            SceneManager.LoadScene("Demo_Setting");
        }
    }
}