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
    }

    public void FocusOnFishInvincible(Action OnComplete)
    {
        Debug.Log("s tu vois ca, c vraiment pas normal");
    }

    // A mettre dans un autre tutoriel eventuellement
    public void FocusOnHarpoon(Action OnComplete)
    {
        Debug.Log("FOCUS ON HARPOON!!!");
        sliderAnim.SetActive(true);
        sliderAnim.GetComponent<SlideAnimation>().Init(new Vector3(0, 0, 0)); // centre de l'ecran
        // A remplacer par la detection du slide
        modules.waitForInput.OnAnyKeyDown(delegate ()
        {
            Destroy(sliderAnim);
            OnComplete();
        });
    }
}
