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

    public const string SHOW_HARPOON_KEY = "itstimetoharpoon";

    protected override void OnStart(Action onComplete = null)
    {
        sliderAnim = Instantiate(sliderAnim_Prefab, Scenes.GetActive(TutorialScene.SCENENAME).FindRootObject<TutorialScene>().transform);
        onComplete();

        modules.delayedAction.Do(3, FocusOnHarpoon);
    }

    void FocusOnHarpoon()
    {
        Debug.Log("FOCUS ON HARPOON!!!");

        Game.Instance.GameRunning.Lock("spotlight");

        modules.spotlight.OnWorld(Game.Instance.SubmarineMovement.transform.position);

        sliderAnim.SetActive(true);
        sliderAnim.GetComponent<SlideAnimation>().Init(new Vector3(0, 0, 0)); // centre de l'ecran

        modules.textDisplay.SetTop();
        modules.textDisplay.DisplayText("Voici ton harpon! Utilise le pour capturer des gros poissons.", true);

        modules.spotlight.DelayedCall(delegate ()
        {
            modules.waitForInput.OnTouch(delegate ()
            {
                sliderAnim.Destroy();
                modules.textDisplay.HideText();
                modules.spotlight.Off(() =>
                {
                    End(true);
                });
                Game.Instance.GameRunning.Unlock("spotlight");
            }, TouchPhase.Moved);
        }, 0, true);
    }

    public override bool StartCondition()
    {
        return dataSaver.GetBool(SHOW_HARPOON_KEY, false);
    }
}
