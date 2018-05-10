using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tutorial;
using System;

[CreateAssetMenu(fileName = "TUT_FishingFrenzy", menuName = "Ocean Empire/Tutorial/Fishing Frenzy")]
public class TUT_FishingFrenzy : BaseTutorial
{
    public override bool LaunchCondition()
    {
        return Scenes.IsActive("Shack") && FishingFrenzy.Instance.IsUnlocked;
    }
    protected override void OnStart(Action onComplete = null)
    {
        onComplete();

        TutorialShackReference.Instance.cameraController.ForceHideButtons = true;
        modules.inputDisabler.DisableInput();
        modules.spotlight.OnButOffScreenDown();
        modules.spotlight.Off();
        modules.delayedAction.Do(1.5f, Step1_LookWhatYouveBought);
    }

    void Step1_LookWhatYouveBought()
    {
        TutorialShackReference.Instance.shack_Canvas.HideAll(Shack_Canvas.Filter.HUD);

        modules.textDisplay.DisplayText("Tu viens de débloquer la <size=100>Super-pêche</size>!",
            //"Lorsqu'elle est active, elle remplira l'océan d'un multitude de poissons additionels.",
            position: TextDisplay.Position.Middle);

        modules.okButton.PromptOk(1.5f, Step2_HeresThePanel);
    }

    void Step2_HeresThePanel()
    {
        modules.spotlight.On(TutorialShackReference.Instance.superPeche.position);
        modules.textDisplay.DisplayText("Tu peux voir si elle est disponible à l'aide de ce panneau.");

        modules.okButton.PromptOk(3f, Step_WhatIsDoes);
    }
    void Step_WhatIsDoes()
    {
        TutorialShackReference.Instance.cameraController.GoTo(Shack_CameraController.Section.Recolte);
        TutorialShackReference.Instance.shack_Canvas.ShowAll(Shack_Canvas.Filter.None);

        modules.spotlight.OnButOffScreenUp();
        modules.textDisplay.DisplayText("Lorsqu'elle est active, elle remplira l'océan d'une multitude de poissons additionels lors" +
            " de ta prochaine pêche.", position: TextDisplay.Position.Middle);

        modules.delayedAction.Do(1, modules.okButton.SetMiddlePosition);
        modules.okButton.PromptOk(3f, Step_End);
    }


    void Step_End()
    {
        TutorialShackReference.Instance.cameraController.ForceHideButtons = false;

        InitQueue initQueue = new InitQueue(() =>
        {
            End(true);
        });
        modules.textDisplay.HideText(initQueue.RegisterTween());
        modules.spotlight.Off(initQueue.RegisterTween());
        initQueue.MarkEnd();
    }
}
