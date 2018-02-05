using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ChoiceButton : MonoBehaviour {

    [SerializeField]
    private Button button;
    [SerializeField]
    private Text text;

    public void UpdateButtonInfo(Action<string> onClick, string choiceText, string adviceText)
    {
        this.text.text = choiceText;
        button.onClick.AddListener(delegate() {
            onClick.Invoke(adviceText);
        });
    }
}
