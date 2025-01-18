using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

// using UnityEngine.PlayerLoop;

namespace AdGeneric.AdBox
{
    public class CommonButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler,
        IPointerUpHandler
    {
        private const float FadeTime = 0.3f;
        private const float OnHoverAlpha = 0.7f;
        private const float OnClickAlpha = 0.6f;

        [SerializeField] private UnityEvent m_OnHover = null;

        [SerializeField] private UnityEvent m_OnClick = null;

        [SerializeField] private UnityEvent m_OnlongPress = null;
        [SerializeField] private UnityEvent m_OnTouchUp = null;

        private CanvasGroup m_CanvasGroup = null;

        private bool isDown;


        private PointerEventData _pointerEventData;

    

        private void Awake()
        {
            m_CanvasGroup = gameObject.TryGetComponent<CanvasGroup>(out var comp)?comp:gameObject.AddComponent<CanvasGroup>();
            isDown = false;
        }

    

        private void Update()
        {
            if (isDown)
            {
                m_OnlongPress.Invoke();
                // Debug.Log("按下？");
            }


            // if (this._pointerEventData != null && isDown)
            // {
            //     if (Input.touchCount == 0)
            //     {
            //         OnPointerUp(_pointerEventData);
            //     }
            //     else if (Input.touchCount == 1)
            //     {
            //         if (isDown)
            //         {
            //             float dis = Vector2.Distance(transform.position, Input.GetTouch(0).position);
            //             if (dis > 100)
            //             {
            //                 OnPointerUp(_pointerEventData);
            //             }
            //         }
            //     }
            // }
        }

        private void OnDisable()
        {
            m_CanvasGroup.alpha = 1f;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
            {
                return;
            }

            StopAllCoroutines();
            // StartCoroutine(m_CanvasGroup.FadeToAlpha(OnHoverAlpha, FadeTime));
            m_OnHover.Invoke();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
            {
                return;
            }

            StopAllCoroutines();
            StartCoroutine(FadeToAlpha(m_CanvasGroup,1f, FadeTime));
        }
        public static IEnumerator FadeToAlpha(CanvasGroup canvasGroup, float alpha, float duration)
        {
            float time = 0f;
            float originalAlpha = canvasGroup.alpha;
            while (time < duration)
            {
                time += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(originalAlpha, alpha, time / duration);
                yield return new WaitForEndOfFrame();
            }

            canvasGroup.alpha = alpha;
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
            {
                return;
            }

            // m_CanvasGroup.alpha = OnClickAlpha;
            m_OnClick.Invoke();
            isDown = true;
            this._pointerEventData = eventData;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
            {
                return;
            }
            // m_CanvasGroup.alpha = OnHoverAlpha;
            m_OnTouchUp.Invoke();
            isDown = false;
            _pointerEventData = null;
            // if (eventData.pointerId == this._pointerEventData.pointerId)
            // {
            //     isDown = false;
            //     _pointerEventData = null;
            // }
        }
    }
}