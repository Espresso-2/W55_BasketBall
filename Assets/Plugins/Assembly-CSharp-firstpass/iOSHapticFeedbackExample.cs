/*using UnityEngine;

public class iOSHapticFeedbackExample : MonoBehaviour
{
    private void OnGUI()
    {
        GUI.skin.button.fontSize = Mathf.RoundToInt(0.012f * (float)Screen.width);
        for (int i = 0; i < 7; i++)
        {
            if (GUI.Button(
                    new Rect(0.25f * (float)Screen.width, 0.12f * (float)Screen.height + (float)i * 0.06f * (float)Screen.height,
                        0.1f * (float)Screen.width, 0.05f * (float)Screen.height), "Haptic\n" + (iOSHapticFeedback.iOSFeedbackType)i))
            {
                iOSHapticFeedback.Instance.Trigger((iOSHapticFeedback.iOSFeedbackType)i);
            }
        }
    }
}*/