using System;
using System.Collections;
using System.Collections.Generic;
using Tutorial;
using UnityEngine;

// Tutoriel #2 le shack
[CreateAssetMenu(fileName = "TUT_Shack", menuName = "Ocean Empire/Tutorial/Shack")]
public class TUT_Shack : BaseTutorial
{
    public float focusDuration = 1f;

    private const string showMagasinKey = "itstimetoshop";

    protected override void OnStart(Action onComplete = null)
    {
        onComplete();
    }

    public void FocusOnCash(Action OnComplete)
    {
        if (!dataSaver.GetBool(showMagasinKey, false))
        {
            Debug.Log("On commence le premier tutoriel");
            Spotlight spotlight = modules.spotlight;
            spotlight.On(TutorialShackReference.instance.cash.position);

            modules.textDisplay.DisplayText("Voici tes ressources. Elles sont utiles de pleins de facon, surtout pour débloquer des améliorations.", true);
            modules.textDisplay.SetBottom();
            spotlight.DelayedCall(delegate ()
            {
                modules.waitForInput.OnAnyKeyDown(delegate ()
                {
                    modules.textDisplay.HideText();
                    dataSaver.SetBool(showMagasinKey, true);
                    dataSaver.Save();
                    OnComplete();
                });
            }, focusDuration, true);
        } else
        {
            dataSaver.SetBool(showMagasinKey, true);
            dataSaver.Save();
            OnComplete();
        }
    }

    public void FocusOnSuperPeche(Action OnComplete)
    {
        if (!dataSaver.GetBool(showMagasinKey, false))
        {
            Debug.Log("puis le deuxieme");
            Spotlight spotlight = modules.spotlight;
            spotlight.On(TutorialShackReference.instance.superPeche.position);

            modules.textDisplay.DisplayText("Ici, c'est l'indicateur de super pèche. Quand il est disponible, lors de la prochain récolte les océans seront beaucoup plus remplis que d'habitude.", true);
            modules.textDisplay.SetBottom();
            spotlight.DelayedCall(delegate ()
            {
                modules.waitForInput.OnAnyKeyDown(delegate ()
                {
                    modules.textDisplay.HideText();
                    OnComplete();
                });
            }, focusDuration, true);
        }
    }

    public void FocusOnObjectives(Action OnComplete)
    {
        Spotlight spotlight = modules.spotlight;
        spotlight.On(TutorialShackReference.instance.objectifTitle.position);

        modules.textDisplay.DisplayText("Les objectifs en cours sont affichés dans cet écran. En les complètant on obtient des récompenses.", true);
        modules.textDisplay.SetBottom();
        spotlight.DelayedCall(delegate ()
        {
            modules.waitForInput.OnAnyKeyDown(delegate ()
            {
                modules.textDisplay.HideText();
                OnComplete();
            });
        }, focusDuration, true);
    }

    public void FocusOnCalendrier(Action OnComplete)
    {
        Spotlight spotlight = modules.spotlight;
        spotlight.On(TutorialShackReference.instance.calendrier.position);

        modules.textDisplay.DisplayText("Grâce au calendrier, on peut planifier des exercices. Lorsque ceux-ci sont effectués, ils donneront beaucoup plus de récompense qu'un exercice normal.", true);
        modules.textDisplay.SetBottom();
        spotlight.DelayedCall(delegate ()
        {
            modules.waitForInput.OnAnyKeyDown(delegate ()
            {
                modules.textDisplay.HideText();
                OnComplete();
            });
        }, focusDuration, true);
    }

    public void FocusOnMagasin(Action OnComplete)
    {
        if (!dataSaver.GetBool(showMagasinKey, false))
        {
            dataSaver.SetBool(showMagasinKey, true);
        }
        else
        {
            Spotlight spotlight = modules.spotlight;
            spotlight.On(TutorialShackReference.instance.changeSectionToMagasin.transform.position);

            modules.textDisplay.DisplayText("Il faut appuyer ici pour aller à la section de votre quai dédié au Magasin", true);
            modules.textDisplay.SetBottom();

            modules.proxyButton.Proxy(TutorialShackReference.instance.changeSectionToMagasin.transform.position, delegate ()
            {
                spotlight.Off(delegate ()
                {
                    TutorialShackReference.instance.changeSectionToMagasin.onClick.Invoke();
                    spotlight.On(TutorialShackReference.instance.magasin.position);
                    modules.textDisplay.DisplayText("En appuyant sur ce bouton, on ouvre le magasin. Dans celui-ci, on retrouve plein d'améliorations pouvant aider à progresser dans le jeu", true);
                    modules.textDisplay.SetTop();

                    spotlight.DelayedCall(delegate ()
                    {
                        modules.waitForInput.OnAnyKeyDown(delegate ()
                        {
                            modules.textDisplay.HideText();
                            spotlight.Off(delegate ()
                            {
                                OnComplete();
                                End(true);
                            });
                        });
                    }, focusDuration, true);
                });
            });
        }
    }

    public void FocusOnRecolte(Action OnComplete)
    {
        Spotlight spotlight = modules.spotlight;
        spotlight.On(TutorialShackReference.instance.changeSectionToPlonger.transform.position);

        modules.textDisplay.DisplayText("Pour aller capturer d'autres poissons, il faut passer par ici", true);
        modules.textDisplay.SetBottom();

        modules.proxyButton.Proxy(TutorialShackReference.instance.changeSectionToPlonger.transform.position, delegate ()
        {
            spotlight.Off(delegate ()
            {
                TutorialShackReference.instance.changeSectionToPlonger.onClick.Invoke();
                spotlight.On(TutorialShackReference.instance.plonger.position);
                modules.textDisplay.DisplayText("Il suffit d'appuyer sur ce bouton pour lancer le sous-marin dans l'océan et commencer la pêche!", true);
                modules.textDisplay.SetTop();

                spotlight.DelayedCall(delegate ()
                {
                    modules.waitForInput.OnAnyKeyDown(delegate ()
                    {
                        modules.textDisplay.HideText();
                        spotlight.Off(delegate ()
                        {
                            OnComplete();
                        });
                    });
                }, focusDuration, true);
            });
        });
    }
}
