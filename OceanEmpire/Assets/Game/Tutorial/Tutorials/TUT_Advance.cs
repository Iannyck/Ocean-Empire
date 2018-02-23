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

    protected override void OnStart()
    {
        sliderAnim = Instantiate(sliderAnim_Prefab, Scenes.GetActive(TutorialScene.SCENENAME).FindRootObject<TutorialScene>().transform);
        Game.Instance.submarine.GetComponent<SlingshotControl>().enabled = true;
    }

    public void FocusOnFishInvincible(Action OnComplete)
    {
        Debug.Log("a faire eventuellement si possible");
    }

    // A mettre dans un autre tutoriel eventuellement
    public void FocusOnHarpoon(Action OnComplete)
    {
        Debug.Log("FOCUS ON HARPOON!!!");
        Game.Instance.gameRunning.Lock("spotlight");
        Spotlight spotlight = modules.spotlight;
        spotlight.OnWorld(Game.Instance.submarine.transform.position);
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
                spotlight.Off(() => { OnComplete(); });
                Game.Instance.gameRunning.Unlock("spotlight");
            }, TouchPhase.Moved);
        }, 0, true);
    }
}
