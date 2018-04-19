using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Questing;

public class QuestPanelEntry : MonoBehaviour
{
    [Header("Components"), SerializeField] Text description;
    [SerializeField] Text status;
    [SerializeField] Image icon;
    [SerializeField] Slider slider;

    public void Fill(Quest quest)
    {
        description.text = quest.Context.description;
        if(quest.state == QuestState.Completed)
        {
            status.text = "COMPLÉTÉ !";
        }
        else
        {
            status.text = "En cours: " + quest.GetDisplayedProgressText();
        }

        Sprite iconSprite = Resources.Load<Sprite>(quest.Context.iconResource);

        icon.sprite = iconSprite;
        icon.enabled = iconSprite != null;

        slider.value = quest.GetProgress01();
    }
}
