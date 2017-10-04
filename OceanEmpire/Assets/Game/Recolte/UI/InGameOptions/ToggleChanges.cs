using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class ToggleChanges : MonoBehaviour
{
    [Header("Links")]
    public Image bg;
    public Image graphic;

    [Header("BG")]
    public Sprite bgOn;
    public Sprite bgOff;

    [Header("Graphic")]
    public Sprite graphicOn;
    public Sprite graphicOff;

    private void Awake()
    {
        GetComponent<Toggle>().onValueChanged.AddListener(OnValueChange);
    }

    void OnValueChange(bool value)
    {
        if (ChangeBG())
        {
            bg.sprite = value ? bgOn : bgOff;
        }

        if (ChangeGraphic())
        {
            graphic.sprite = value ? graphicOn : graphicOff;
        }
    }

    bool ChangeBG() { return bg != null; }
    bool ChangeGraphic() { return graphic != null; }
}
