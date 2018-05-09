using System;
using System.Collections;
using System.Collections.Generic;
using Tutorial;
using UnityEngine;

// Tutoriel #1 base du jeu
[CreateAssetMenu(fileName = "TUT_Advance", menuName = "Ocean Empire/Tutorial/Advance")]
public class TUT_Advance : BaseTutorial
{
    [Header("ADVANCE MAP TUTORIAL")]
    public GameObject sliderAnim_Prefab;
    private GameObject sliderAnim;

    public const string showHarpoonKey = "itstimetoharpoon";

    protected override void OnStart(Action onComplete = null)
    {
        sliderAnim = Instantiate(sliderAnim_Prefab, Scenes.GetActive(TutorialScene.SCENENAME).FindRootObject<TutorialScene>().transform);
        onComplete();
    }

    // A mettre dans un autre tutoriel eventuellement
    public void FocusOnHarpoon(Action OnComplete)
    {
        if (dataSaver.GetBool(showHarpoonKey, false))
        {
            Debug.Log("FOCUS ON HARPOON!!!");
            Game.Instance.GameRunning.Lock("spotlight");
            Spotlight spotlight = modules.spotlight;
            spotlight.OnWorld(Game.Instance.SubmarineMovement.transform.position);
            sliderAnim.SetActive(true);
            sliderAnim.GetComponent<SlideAnimation>().Init(new Vector3(0, 0, 0)); // centre de l'ecran
            modules.textDisplay.DisplayText("Voici ton harpon! Utilise le pour capturer des gros poissons.", true);
            modules.textDisplay.SetTop();
            spotlight.DelayedCall(delegate ()
            {
                modules.waitForInput.OnTouch(delegate ()
                {
                    sliderAnim.Destroy();
                    modules.textDisplay.HideText();
                    spotlight.Off(() =>
                    {
                        OnComplete();
                        End(true);
                    });
                    Game.Instance.GameRunning.Unlock("spotlight");
                }, TouchPhase.Moved);
            }, 0, true);
        }
    }
}
