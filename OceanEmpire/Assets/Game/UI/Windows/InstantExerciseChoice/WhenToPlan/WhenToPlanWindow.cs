using CCC.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WhenToPlanWindow : MonoBehaviour
{
    [SerializeField] Button nowButton;
    [SerializeField] Button laterButton;
    [SerializeField] Button cancelButton;


    public Action OnNowClick { get; set; }
    public Action OnLaterClick { get; set; }
    public Action OnCancelClick { get; set; }

    void Awake()
    {
        nowButton.onClick.AddListener(() =>
        {
            Quit();
            if (OnNowClick != null)
                OnNowClick();
        });
        laterButton.onClick.AddListener(() =>
        {
            Quit();
            if (OnLaterClick != null)
                OnLaterClick();
        });
        cancelButton.onClick.AddListener(() =>
        {
            Quit();
            if (OnCancelClick != null)
                OnCancelClick();
        });
    }

    void Quit()
    {
        GetComponent<WindowAnimation>().InstantClose();
    }
}
