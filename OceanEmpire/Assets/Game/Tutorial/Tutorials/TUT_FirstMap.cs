using System;
using System.Collections;
using System.Collections.Generic;
using Tutorial;
using UnityEngine;

// Tutoriel #1 base du jeu
[CreateAssetMenu(fileName = "TUT_FirstMap", menuName = "Ocean Empire/Tutorial/First Map")]
public class TUT_FirstMap : BaseTutorial {

    [Header("FIRST MAP TUTORIAL")]
    public float focusDuration = 5;

    protected override void OnStart()
    {

    }

    public void FocusOnSubmarine(Action OnComplete)
    {
        Debug.Log("FOCUS ON SUBAMARINUUUU");
        Game.Instance.gameRunning.Lock("spotlight");
        Spotlight spotlight = modules.spotlight;
        spotlight.OnWorld(Game.Instance.submarine.transform.position);
        //modules.textDisplay.SetBottom();
        modules.textDisplay.DisplayText("Voici ton sous-marin! Contrôle le en tenant ton doigt enfoncer sur l'écran", false);

        spotlight.DelayedCall(delegate ()
        {
            modules.waitForInput.OnAnyKeyDown(delegate ()
            {
                modules.textDisplay.HideText();
                OnComplete();
            });
        }, focusDuration/2, true);
    }

    public void FocusOnGaz(Action OnComplete)
    {
        Debug.Log("FOCUS ON gaz desu");
        Spotlight spotlight = modules.spotlight;
        spotlight.On(Game.Instance.ui.gazSlider.transform.localPosition);
        modules.textDisplay.DisplayText("Voici ton essence. Quand tu n'en as plus, la récolte est terminé.", false);
        spotlight.DelayedCall(delegate ()
        {
            modules.textDisplay.HideText();
            OnComplete();
        }, focusDuration, true);
    }

    public void FocusOnOption(Action OnComplete)
    {
        Debug.Log("FOCUS ON gaz desu");
        Spotlight spotlight = modules.spotlight;
        spotlight.On(Game.Instance.ui.optionButton.transform.localPosition);
        modules.textDisplay.DisplayText("Voici le menu option. Par là, tu peux quitter la partie ou changer les paramètres du jeux comme le volume.", false);
        spotlight.DelayedCall(delegate ()
        {
            modules.textDisplay.HideText();
            spotlight.Off(() => { OnComplete(); });
            Game.Instance.gameRunning.Unlock("spotlight");
        }, focusDuration, true);
    }

    public void FocusOnFishPognable(Action OnComplete)
    {
        Debug.Log("si tu vois ca, c pas normal");
    }
}
