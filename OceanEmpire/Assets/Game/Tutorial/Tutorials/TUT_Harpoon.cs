using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Tutorial;
using UnityEngine;

// Tutoriel #1 base du jeu
[CreateAssetMenu(fileName = "TUT_Harpoon", menuName = "Ocean Empire/Tutorial/Harpoon")]
public class TUT_Harpoon : BaseTutorial
{
    [Header("ADVANCE MAP TUTORIAL")]
    public CanvasGroup explanationImage;

    [NonSerialized] CanvasGroup imageInstance;

    public const string SHOW_HARPOON_KEY = "itstimetoharpoon";

    protected override void OnStart(Action onComplete = null)
    {
        onComplete();

        modules.delayedAction.Do(1.5f, Step1_YouHaveAHarpoon);
    }

    void Step1_YouHaveAHarpoon()
    {
        Game.Instance.GameRunning.Lock("tut");

        modules.spotlight.OnWorld(Game.Instance.SubmarineMovement.transform.position);

        modules.textDisplay.SetTop();
        modules.textDisplay.DisplayText("Ton sous-marin est maintenant équipé d'un\n<size=100>lance-harpon !</size>", true);

        modules.okButton.PromptOk(1.5f, Step2_UseItLikeThis);
    }

    void Step2_UseItLikeThis()
    {
        modules.textDisplay.DisplayText("Tu peux l'utiliser en glissant ton doigt à partir de ton sous-marin.", true);

        imageInstance = explanationImage.DuplicateGO(modules.transform);
        imageInstance.alpha = 0;
        imageInstance.DOFade(1, 0.5f).SetUpdate(true);

        modules.delayedAction.Do(1.5f, modules.okButton.SetBottomPosition);
        modules.okButton.PromptOk(3f, ()=>
        {
            Step3_ReleaseToFire();
        });
    }

    void Step3_ReleaseToFire()
    {
        modules.textDisplay.DisplayText("C'est en relâchant la poignée que le harpon sera lancé.", true);

        modules.okButton.PromptOk(1.5f, ()=>
        {
            imageInstance.DOFade(0, 0.35f).SetUpdate(true).onComplete = imageInstance.DestroyGO;

            InitQueue initQueue = new InitQueue(() =>
            {
                End(true);
                Game.Instance.GameRunning.Unlock("tut");
            });
            modules.textDisplay.HideText(initQueue.RegisterTween());
            modules.spotlight.Off(initQueue.RegisterTween());
            initQueue.MarkEnd();
        });
    }

    public override bool StartCondition()
    {
        return dataSaver.GetBool(SHOW_HARPOON_KEY, false);
    }
}
