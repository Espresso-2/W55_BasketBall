using System;
using UnityEngine;
using UnityEngine.UI;

namespace AdGeneric
{
    public class InnerPrivacyButton : MonoBehaviour
    {
        private void Awake()
        {
            var onClick = new Button.ButtonClickedEvent();
            onClick.AddListener(()=>AdHealthPrivacyManager.Instance.ShowPrivacy(PrivacyOption.Agree));
            GetComponent<Button>().onClick = onClick;
        }
    }
}