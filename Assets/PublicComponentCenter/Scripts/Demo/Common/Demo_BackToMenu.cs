using UnityEngine;
using UnityEngine.SceneManagement;

namespace PublicComponentCenter
{
    public class Demo_BackToMenu : MonoBehaviour
    {
        public void OnBackToMenu()
        {
            SceneManager.LoadScene("Demo_Test");
        }
    }
}