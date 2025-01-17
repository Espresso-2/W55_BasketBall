using UnityEngine;
using UnityEngine.UI;

namespace AdGeneric.Privacy_Health
{
    public class InnerShortcutBlack : MonoBehaviour
    {
        void Awake() => GetComponent<Button>().onClick.AddListener(() => AdTotalManager.Instance.SimpleShortCurBlack());
    }
}