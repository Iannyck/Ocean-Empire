using System;
using System.Collections;
using System.Collections.Generic;
using Tutorial;
using UnityEngine;

// Tutoriel #1 base du jeu
[CreateAssetMenu(fileName = "TUT_FirstMap", menuName = "Ocean Empire/Tutorial/First Map")]
public class TUT_FirstMap : BaseTutorial
{
    protected override void OnStart(Action onComplete = null)
    {
        onComplete();
        modules.inputDisabler.DisableInput();
        modules.delayedAction.Do(2, FocusOnSubmarine);
    }

    void FocusOnSubmarine()
    {
        Game.Instance.GameRunning.Lock("tut");

        Spotlight spotlight = modules.spotlight;
        spotlight.OnWorld(Game.Instance.SubmarineMovement.transform.position);

        modules.textDisplay.SetTop();
        modules.textDisplay.DisplayText("Voici ton sous-marin!\n<size=55>Déplace le en appuyant sur l'écran.</size>", true);

        modules.okButton.PromptOk(2f, () =>
        {
            modules.textDisplay.DisplayText("Essaie de capturer les poissons en te déplaçant!", true);
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
}
