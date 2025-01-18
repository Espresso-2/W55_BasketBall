using AdGeneric.Operation;
using UnityEngine;

namespace AdGeneric.AdBox
{
    public class BoxManager : MonoBehaviour
    {
        [SerializeField, Header("Box")] 
        private GameObject boxPanel;
        [SerializeField] private float firstTime = 180f, repeatTime = 120f;
        public static BoxManager Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            boxPanel.SetActive(false);
            /*if((AdTotalManager.Instance.Addition & Addition.宝箱)!=0)
                InvokeRepeating(nameof(RepeatBox), firstTime, repeatTime);*/
        }

        private void RepeatBox()
        {
            print(nameof(RepeatBox));
            AdTotalManager.Instance.Show(Addition.宝箱);
        }

        public void ShowBox()
        {
            if ((AdTotalManager.Instance.Addition & Addition.宝箱) == 0) return;
            print(nameof(ShowBox));
            boxPanel.SetActive(true);
            AdTotalManager.Instance.ShowWhiteAd();
        }

        public void HideBox()
        {
            if ((AdTotalManager.Instance.Addition & Addition.宝箱) == 0) return;
            print(nameof(HideBox));
            boxPanel.SetActive(false);
        }
    }
}