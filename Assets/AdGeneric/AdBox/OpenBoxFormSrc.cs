using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace AdGeneric.AdBox
{
    public class OpenBoxFormSrc : MonoBehaviour
    {
        public GameObject btnOpen;
        public Image bar2;
        public GameObject boxImg;
        // public GameObject btnClose;

        private bool isUp = false;

        private float value1 = 0;
        private float value2 = 0;

        private void Awake()
        {
            bar2.type = Image.Type.Filled;
            bar2.fillMethod = Image.FillMethod.Horizontal;
            bar2.fillOrigin = 0;
            bar2.fillAmount = 0;
            // boxImg.transform.localScale = Vector3.one;
            // btnClose.SetActive(false);
        }


        private void OnEnable()
        {
            bar2.type = Image.Type.Filled;
            bar2.fillMethod = Image.FillMethod.Horizontal;
            bar2.fillOrigin = 0;
            bar2.fillAmount = 0;
            // Random.Range(0f,1f);
            // Time.timeScale = 0;

            value1 = Random.Range(0f, 1f);
            value2 = Random.Range(0.05f, 0.3f);
            // Debug.Log(value1);
            // Debug.Log(value2);
        
        }

        // private float RandomNum()
        // {
        //     Random.Range(0, 1);
        //     return;
        // }

        private void OnDisable()
        {
            // Time.timeScale = 1;
        }

        private void ShowClose()
        {
            // btnClose.SetActive(true);
        }

        public void OnClickOpenBox()
        {
            // Debug.Log("箱子点击");
            Vector3 scalVector3_1 = btnOpen.transform.localScale;
            Vector3 scalVector3_2 = new Vector3(1.3f, 1.3f, 1.3f);
            Vector3 scalVector3_3 = new Vector3(1.5f, 1.5f, 1.5f);
            btnOpen.transform.localScale = Vector3.Lerp(scalVector3_1, scalVector3_2, 0.2f);
            boxImg.transform.localScale = Vector3.Lerp(boxImg.transform.localScale, scalVector3_3, 0.2f);
            bar2.fillAmount += value2;
            isUp = false;
            // Debug.Log(bar2.fillAmount);
            if (bar2.fillAmount >= value1)
            {
                Debug.Log("弹出激励");
                AdTotalManager.Instance.ShowRewardAd(gameObject.name,null);
                gameObject.SetActive(false);
            }
        }

        public void OnUpOpenBox()
        {
            // Debug.Log("箱子按钮点击抬起");
            Vector3 scalVector3_1 = btnOpen.transform.localScale;
            Vector3 scalVector3_2 = new Vector3(1f, 1f, 1f);
            btnOpen.transform.localScale = Vector3.Lerp(scalVector3_1, scalVector3_2, 0.2f);
            boxImg.transform.localScale = Vector3.Lerp(boxImg.transform.localScale, scalVector3_2, 0.2f);
            isUp = true;
        }

        private void Update()
        {
            if (isUp)
            {
                bar2.fillAmount -= 0.15f * Time.deltaTime;
            }
        }
    }
}