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
    public SceneInfo shackScene;

    protected override void OnStart(Action onComplete = null)
    {
        if(Game.Instance != null)
            Game.Instance.SubmarineMovement.GetComponent<SlingshotControl>().enabled = false;
        else
        {
            modules.DelayedCall(delegate ()
            {
                Game.Instance.SubmarineMovement.GetComponent<SlingshotControl>().enabled = false;
            }, 1);
        }

        if (onComplete != null)
        {
            onComplete();
        }
    }

    public void FocusOnSubmarine(Action OnComplete)
    {
        Game.Instance.GameRunning.Lock("spotlight");
        Spotlight spotlight = modules.spotlight;
        spotlight.OnWorld(Game.Instance.SubmarineMovement.transform.position);
        
        modules.textDisplay.DisplayText("Voici ton sous-marin! Contrôle le en tenant ton doigt enfoncer sur l'écran", true);
        modules.textDisplay.SetTop();
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
        Spotlight spotlight = modules.spotlight;
        spotlight.On(Game.Instance.Recolte_UI.gazSlider.transform.position);
        modules.textDisplay.DisplayText("Voici ton essence. Quand tu n'en as plus, la récolte est terminé.", true);
        modules.textDisplay.SetTop();
        spotlight.DelayedCall(delegate ()
        {
            modules.textDisplay.HideText();
            OnComplete();
        }, focusDuration, true);
    }

    public void FocusOnOption(Action OnComplete)
    {
        Spotlight spotlight = modules.spotlight;
        spotlight.On(Game.Instance.Recolte_UI.optionButton.transform.position);
        modules.textDisplay.DisplayText("Voici le menu option. Par là, tu peux quitter la partie ou changer les paramètres du jeux comme le volume.", true);
        modules.textDisplay.SetTop();
        spotlight.DelayedCall(delegate ()
        {
            modules.textDisplay.HideText();
            spotlight.Off(() => {
                OnComplete();
                End(true);
            });
            Game.Instance.GameRunning.Unlock("spotlight");
        }, focusDuration, true);
    }

    public void FocusOnFishPognable(Action OnComplete)
    {
        Debug.Log("a faire eventuellement si possible");
    }
}
