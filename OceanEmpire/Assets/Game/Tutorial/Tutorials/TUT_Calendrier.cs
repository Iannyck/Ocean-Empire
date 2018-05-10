using System;
using System.Collections;
using System.Collections.Generic;
using Tutorial;
using UnityEngine;

[CreateAssetMenu(fileName = "TUT_Calendrier", menuName = "Ocean Empire/Tutorial/Calendrier")]
public class TUT_Calendrier : BaseTutorial
{
    public float focusDuration = 1f;

    private const string showCalendrierKey = "itstimetoexercice";

    protected override void OnStart(Action onComplete = null)
    {
        onComplete();
    }

    public void FocusOnCalendrier(Action OnComplete)
    {
        if (dataSaver.GetBool(showCalendrierKey, false))
        {
            Spotlight spotlight = modules.spotlight;
            spotlight.On(TutorialShackReference.Instance.calendrier.position);

            modules.textDisplay.DisplayText("Grâce au calendrier, on peut planifier des exercices. Lorsque ceux-ci sont effectués, ils donneront beaucoup plus de récompense qu'un exercice normal.", true);
            modules.textDisplay.SetBottom();
            spotlight.DelayedCall(delegate ()
            {
                modules.waitForInput.OnAnyKeyDown(delegate ()
                {
                    modules.textDisplay.HideText();
                    OnComplete();
                    End(true); // Temporarire, il faut faire le reste du tutoriel
                    Debug.Log("Faire le reste du tutoriel du calendrier");
                });
            }, focusDuration, true);
        }
    }
}
