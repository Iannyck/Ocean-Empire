using System;
using System.Collections;
using System.Collections.Generic;
using Tutorial;
using UnityEngine;

// Tutoriel #2 le shack
[CreateAssetMenu(fileName = "TUT_Shack1", menuName = "Ocean Empire/Tutorial/Shack 1")]
public class TUT_Shack1 : BaseTutorial
{
    public override bool StartCondition()
    {
        return Scenes.IsActive("Shack");
    }

    protected override void OnStart(Action onComplete = null)
    {
        onComplete();

        TutorialShackReference.Instance.cameraController.GoTo(Shack_CameraController.Section.Recolte, true);
        modules.inputDisabler.DisableInput();

        TutorialShackReference.Instance.cameraController.SECTION_MAX = 0;
        TutorialShackReference.Instance.cameraController.UpdateButtons();

        modules.delayedAction.Do(2f, Step1_Bravo);
    }

    void Step1_Bravo()
    {
        modules.textDisplay.SetTop();
        modules.textDisplay.DisplayText("Nous voilà de retour sur ton quai.");
        modules.spotlight.OnButOffScreenLeft();

        modules.okButton.SetMiddlePosition();
        modules.okButton.PromptOk(1, Step2_HeresYourCash);
    }

    void Step2_HeresYourCash()
    {
        modules.textDisplay.DisplayText("Tous les jetons que tu as obtenue au cours de ta récolte se retrouvent ici.",
            position: TextDisplay.Position.Middle);
        modules.spotlight.On(TutorialShackReference.Instance.cash.position);
        modules.okButton.PromptOk(2f, Step3_LetsGoRight);
    }

    void Step3_LetsGoRight()
    {
        modules.textDisplay.DisplayText("Allons voir le reste du quai.\nAppuie sur ce bouton te déplacer.",
            position: TextDisplay.Position.Top);

        modules.delayedAction.Do(1, () => modules.spotlight.On(TutorialShackReference.Instance.goRightButton.transform.position));
        modules.proxyButton.Proxy(TutorialShackReference.Instance.goRightButton, () =>
        {
            modules.spotlight.OnButOffScreenRight();
            modules.spotlight.Off();
            modules.textDisplay.HideText();
            modules.delayedAction.Do(2f, Step4_HereAreYourObjectives);
        });
    }

    void Step4_HereAreYourObjectives()
    {
        modules.textDisplay.DisplayText("Voici ton panneau d'objectif.",
            position: TextDisplay.Position.Bottom);

        modules.spotlight.On();
        modules.okButton.SetBottomPosition();
        modules.okButton.PromptOk(1.5f, Step5_HereAreYourObjectives);
    }

    void Step5_HereAreYourObjectives()
    {
        modules.textDisplay.DisplayText("En complétant tous les objectifs, tu auras" +
            " accès à un nouvel océan avec plus de poissons et de richesses.",
            position: TextDisplay.Position.Bottom);

        modules.okButton.PromptOk(2f, Step6_LetsGoToTheShop);
    }

    void Step6_LetsGoToTheShop()
    {
        TutorialShackReference.Instance.cameraController.SECTION_MAX = 1;
        TutorialShackReference.Instance.cameraController.UpdateButtons();

        modules.textDisplay.DisplayText("Allons voir la dernière section du quai.",
            position: TextDisplay.Position.Top);


        modules.delayedAction.Do(1, () => modules.spotlight.On(TutorialShackReference.Instance.goRightButton.transform.position));
        modules.proxyButton.Proxy(TutorialShackReference.Instance.goRightButton, () =>
        {
            modules.spotlight.OnButOffScreenRight();
            modules.spotlight.Off();
            modules.textDisplay.HideText();
            modules.delayedAction.Do(2f, Step7_ThatsYoShop);
        });
    }

    void Step7_ThatsYoShop()
    {
        modules.spotlight.On();
        modules.textDisplay.DisplayText("Voici ton magasin!\n<size=50>" +
            "C'est ici que tu pourras dépenser tes jetons et tes tickets pour améliorer ton sous-marin.</size>",
            position: TextDisplay.Position.Top);
        modules.okButton.SetTopPosition();

        modules.okButton.PromptOk(3, Step8_YourGoal);
    }

    void Step8_YourGoal()
    {
        modules.textDisplay.DisplayText("Tu es maintenant libre!<size=50>\n" +
            "\nRécolte des poissons, améliore ton sous-marin et deviens le pêcheur le plus riche de l'océan!</size>",
            position: TextDisplay.Position.Top);
        modules.okButton.PromptOk(4, Step_End);
    }

    void Step_End()
    {
        InitQueue initQueue = new InitQueue(() =>
        {
            End(true);
        });
        modules.textDisplay.HideText(initQueue.RegisterTween());
        modules.spotlight.Off(initQueue.RegisterTween());
        initQueue.MarkEnd();
    }
}
