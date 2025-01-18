using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InnerSimpleShortcutBlack : MonoBehaviour
{
    private void Awake() => GetComponent<Button>().onClick.AddListener(() => AdTotalManager.Instance.SimpleShortcutBlack());
}
