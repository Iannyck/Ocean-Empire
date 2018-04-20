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
    [SerializeField] Image sliderFill;
    [SerializeField] Image background;

    [Header("Sprites"), SerializeField] Sprite ongoingBackground;
    [SerializeField] Sprite completedBackground;
    [SerializeField] Sprite ongoingSliderFill;
    [SerializeField] Sprite completedSliderFill;
    [SerializeField] float completedIconAlpha;

    public void Fill(Quest quest)
    {
        description.text = quest.Context.description;
        if (quest.state == QuestState.Completed)
        {
            status.text = "COMPLÉTÉ !";
            sliderFill.sprite = completedSliderFill;
            background.sprite = completedBackground;
            icon.color = new Color(1, 1, 1, completedIconAlpha);
        }
        else
        {
            status.text = "En cours: " + quest.GetDisplayedProgressText();
            sliderFill.sprite = ongoingSliderFill;
            background.sprite = ongoingBackground;
            icon.color = new Color(1, 1, 1, 1);
        }
        
        Sprite iconSprite = Resources.Load<Sprite>(quest.Context.iconResource);

        icon.sprite = iconSprite;
        icon.enabled = iconSprite != null;

        slider.value = quest.GetProgress01();
    }
}
