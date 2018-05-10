using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialShackReference : MonoBehaviour
{
    public RectTransform superPeche;
    public RectTransform plonger;
    public Button shopButton;
    public RectTransform objectifTitle;

    public Button newTaskButton;
    public RectTransform cash;
    public RectTransform tickets;
    public Shack_CameraController cameraController;
    public QuestPanel questPanel;
    public TaskPanel taskPanel;
    public Button goRightButton;
    public Button goLeftButton;

    public static TutorialShackReference Instance { get; private set; }

    void OnEnable()
    {
        Instance = this;
    }

    void OnDisable()
    {
        Instance = null;
    }
}
