using CCC.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YesNoWindow : MonoBehaviour
{
    public Text text;
    public Button noButton;
    public Button yesButton;

    public Action<bool> chooseCallback;

    void Awake()
    {
        yesButton.onClick.AddListener(OnYesClick);
        noButton.onClick.AddListener(OnNoClick);
    }

    public static void AskYesNoQuestion(string question, Action<bool> resultCallback)
    {
        Scenes.Load("YesNoWindow", UnityEngine.SceneManagement.LoadSceneMode.Additive, (s) =>
        {
            var controller = s.FindRootObject<YesNoWindow>();
            controller.chooseCallback = resultCallback;
            controller.text.text = question;
        });
    }

    void OnYesClick()
    {
        if (chooseCallback != null)
            chooseCallback(true);
        Exit();
    }

    void OnNoClick()
    {
        if (chooseCallback != null)
            chooseCallback(false);
        Exit();
    }

    void Exit()
    {
        GetComponent<WindowAnimation>().Close();
    }
}
