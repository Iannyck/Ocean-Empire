using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tutorial;
using System;
using DG.Tweening;

[CreateAssetMenu(fileName = "TUT_Exercise1", menuName = "Ocean Empire/Tutorial/Exercise 1")]
public class TUT_Exercise1 : BaseTutorial
{
    [SerializeField] CanvasGroup ticketDisplay;

    [NonSerialized] CanvasGroup ticketDisplayInstance;

    public override bool LaunchCondition()
    {
        return MapManager.Instance.MapIndex == 1 && Scenes.IsActive("Shack");
    }

    protected override void OnStart(Action onComplete = null)
    {
        onComplete();

        modules.inputDisabler.DisableInput();


        TutorialShackReference.Instance.taskPanel.gameObject.SetActive(false);

        TutorialShackReference.Instance.questPanel.showOnStart = false;
        TutorialShackReference.Instance.questPanel.HideInstant();
        TutorialShackReference.Instance.cameraController.GoTo(Shack_CameraController.Section.Hub);

        modules.delayedAction.Do(1, Step1_Bienvenido);
    }

    void Step1_Bienvenido()
    {
        modules.spotlight.OnButOffScreenDown();

        modules.textDisplay.SetMiddle();
        modules.textDisplay.DisplayText("Nous voilà au golfe du Mexique!<size=50>\n" +
            "De nouveaux poissons et de nouveaux objectifs t'attendent.</size>");

        modules.okButton.PromptOk(1.5f, Step2_Ticket);
    }

    void Step2_Ticket()
    {
        modules.spotlight.On(TutorialShackReference.Instance.tickets.position);
        modules.textDisplay.DisplayText("Si tu veux améliorer ton sous-marin plus rapidement, tu devrais utiliser" +
            " tes <size=100>tickets</size>.");

        ticketDisplayInstance = ticketDisplay.DuplicateGO(modules.transform);
        ticketDisplayInstance.alpha = 0;
        ticketDisplayInstance.DOFade(1, 0.5f).SetDelay(0.5f);
        modules.okButton.PromptOk(3, Step3_WhatTheyAre);
    }
    void Step3_WhatTheyAre()
    {
        modules.textDisplay.DisplayText("Il s'agit d'une monnaie de luxe que tu peux obtenir en réalisant de l'exercice physique.");
        modules.okButton.PromptOk(3, Step4_HowToGain);
    }
    void Step4_HowToGain()
    {
        modules.textDisplay.DisplayText("Pour chaque minute de  marche que tu fais avec ton téléphone, tu seras récompensé" +
            " avec des tickets!<size=50>\n\n" +
            "L'application n'a pas besoin d'être ouverte.</size>");

        modules.okButton.PromptOk(3, Step_YouCanAlsoPlan);
    }

    void Step_YouCanAlsoPlan()
    {
        ticketDisplayInstance.DOFade(0, 0.5f).OnComplete(ticketDisplayInstance.DestroyGO);
        modules.textDisplay.DisplayText("Pour obtenir encore plus de tickets, tu peux te planifier un exercice!",
            position: TextDisplay.Position.Middle);


        modules.delayedAction.Do(0.5f, ()=> TutorialShackReference.Instance.taskPanel.gameObject.SetActive(true));
        modules.spotlight.On(TutorialShackReference.Instance.newTaskButton.transform.position);

        //modules.delayedAction.Do(1, () => modules.okButton.SetTopPosition());
        //modules.okButton.PromptOk(3, Step_Conclusion);
        modules.proxyButton.Proxy(TutorialShackReference.Instance.newTaskButton, () =>
        {
            modules.spotlight.OnButOffScreenDown();
            modules.spotlight.Off();
            modules.textDisplay.HideText();
            modules.delayedAction.Do(2, Step_Conclusion);
        });
    }

    void Step_Conclusion()
    {
        modules.spotlight.On();
        modules.textDisplay.DisplayText("Nous t'invitons à jumeler tes sessions de jeu avec de l'activité physique pour" +
            " progresser plus vite :)");

        modules.okButton.PromptOk(1.5f, () =>
        {
            Step_End();
        });
    }

    void Step_End()
    {
        InitQueue initQueue = new InitQueue(() =>
        {
            End(true);
            TutorialShackReference.Instance.questPanel.Show();
        });
        modules.textDisplay.HideText(initQueue.RegisterTween());
        modules.spotlight.Off(initQueue.RegisterTween());
        initQueue.MarkEnd();
    }

    protected override void Cleanup()
    {
        base.Cleanup();
        ticketDisplayInstance = null;
    }
}
