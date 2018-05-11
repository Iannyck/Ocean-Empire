using UnityEngine;
using UnityEngine.UI;

public class Cheat_LogsWindow : MonoBehaviour
{
    public Text displayText;

    void OnEnable()
    {
        displayText.text = Logger.Instance.GetTotalLogWithHeader();
    }

    void OnDisable()
    {
        displayText.text = "";
    }
}
