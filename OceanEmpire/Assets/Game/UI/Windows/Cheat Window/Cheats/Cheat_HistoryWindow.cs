using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.UI;

public class Cheat_HistoryWindow : MonoBehaviour
{
    public Text displayText;

    void OnEnable()
    {
        if (History.instance == null)
            return;

        displayText.text = History.instance.GetDataToString();
    }

    void OnDisable()
    {
        displayText.text = "";
    }
}
