using UnityEngine;
using UnityEngine.SceneManagement;

namespace PublicComponentCenter
{
    public class Demo_SelectForm : UGuiForm
    {
        [SerializeField] private GameObject m_MenuForm;
        [SerializeField] private GameObject m_SelectForm;

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

        public void OnBtnBackClick()
        {
            m_SelectForm.SetActive(false);
            m_MenuForm.SetActive(true);
            //SceneManager.LoadScene("Demo_Test");
        }
    }
}