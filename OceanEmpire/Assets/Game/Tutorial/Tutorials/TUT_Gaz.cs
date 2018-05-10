using System;
using System.Collections;
using System.Collections.Generic;
using Tutorial;
using UnityEngine;

// Tutoriel #1 base du jeu
[CreateAssetMenu(fileName = "TUT_Gaz", menuName = "Ocean Empire/Tutorial/Gaz")]
public class TUT_Gaz : BaseTutorial
{

    protected override void OnStart(Action onComplete = null)
    {
        FocusOnSubmarine();
        modules.inputDisabler.DisableInput();
    }

    void FocusOnSubmarine()
    {
        if (Game.Instance.gameOver)
            return;

        Game.Instance.GameRunning.Lock("tut");
        Spotlight spotlight = modules.spotlight;
        spotlight.On(Game.Instance.Recolte_UI.gazSlider.transform.position);

        modules.textDisplay.SetMiddle();
        modules.textDisplay.DisplayText("Voici ta jauge d'essence." +
            "\n<size=55>Lorsqu'elle devient vide, ta récolte de poisson se termine.</size>", true);

        modules.okButton.PromptOk(2f, () =>
        {
            
            modules.textDisplay.DisplayText("Si tu désire remonter à la surface plus tôt, tu peux toujours appuyer sur ce bouton.", true,
                ()=> spotlight.On(Game.Instance.Recolte_UI.optionButton.transform.position));
            modules.okButton.PromptOk(2f, () =>
            {
                InitQueue initQueue = new InitQueue(() =>
                {
                    End(true);
                    Game.Instance.GameRunning.Unlock("tut");
                });
                modules.textDisplay.HideText(initQueue.RegisterTween());
                modules.spotlight.Off(initQueue.RegisterTween());
                initQueue.MarkEnd();
            });
        });
    }

    public override bool LaunchCondition()
    {
        return Game.Instance != null && Game.Instance.ElapsedPlayTime > 16;
    }
}
