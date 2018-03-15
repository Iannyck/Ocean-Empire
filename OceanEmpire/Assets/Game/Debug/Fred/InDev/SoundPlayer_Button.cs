using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundPlayer_Button : BaseSoundPlayer
{
    [Header("Button")]
    [SerializeField] Button _button;

    void OnEnable()
    {
        if (_button != null)
            _button.onClick.AddListener(Play);
    }

    void OnDisable()
    {
        if (_button != null)
            _button.onClick.RemoveListener(Play);
    }
}