using AdGeneric.Operation;
using UnityEngine;

namespace AdGeneric.AdEye
{
    public class EyeManager : MonoBehaviour
    {
        [SerializeField,Header("Eye")] 
        private GameObject eyePanel;
        [SerializeField] private float firstTime = 180f, repeatTime = 120f;
        public static EyeManager Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            eyePanel.SetActive(false);
            if ((AdTotalManager.Instance.Addition & Addition.护眼) != 0)
                InvokeRepeating(nameof(RepeatEye), firstTime, repeatTime);
        }

        private void RepeatEye()
        {
            print(nameof(RepeatEye));
            AdTotalManager.Instance.Show(Addition.护眼);
        }

        public void ShowEye()
        {
            if ((AdTotalManager.Instance.Addition & Addition.护眼) == 0) return;
            print(nameof(ShowEye));
            eyePanel.SetActive(true);
            AdTotalManager.Instance.ShowWhiteAd();
        }

        public void HideEye()
        {
            if ((AdTotalManager.Instance.Addition & Addition.护眼) == 0) return;
            print(nameof(HideEye));
            eyePanel.SetActive(false);
        }
    }
}