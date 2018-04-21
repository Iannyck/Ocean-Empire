using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialShackReference : MonoBehaviour {

    public RectTransform cash;
    public RectTransform superPeche;
    public RectTransform calendrier;
    public Button changeSectionToPlonger;
    public RectTransform plonger;
    public Button changeSectionToMagasin;
    public RectTransform magasin;
    public RectTransform objectifTitle;

    public static TutorialShackReference instance;

    void Start()
    {
        if(instance == null)
            instance = this;
    }
}
