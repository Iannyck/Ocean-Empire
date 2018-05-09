using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ChoiceButton : MonoBehaviour
{
    [Serializable]
    public class Choice
    {
        public string choiceText;
        public string advice;
    }

    [Header("UI"), SerializeField] Button button;
    [SerializeField] Text text;

    [Header("Data")]
    public Choice choice;

    public Action<ChoiceButton> OnClick { get; set; }

    void Awake()
    {
        button.onClick.AddListener(delegate ()
        {
            if (OnClick != null)
                OnClick(this);
        });
    }

    void FillContent(Choice choice)
    {
        if (text)
            text.text = choice.choiceText;
    }

    void OnValidate()
    {
        if (choice != null)
            FillContent(choice);
    }
}
