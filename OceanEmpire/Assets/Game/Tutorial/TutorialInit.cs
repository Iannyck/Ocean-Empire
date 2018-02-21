using System.Collections;
using System.Collections.Generic;
using Tutorial;
using UnityEngine;

public class TutorialInit : MonoBehaviour {

    public bool onStart = true;
    public BaseTutorial tutorial;

    void Start ()
    {
        if(onStart)
            Init();
    }

    private void Init()
    {
        TutorialScene.StartTutorial(tutorial.name, tutorial.dataSaver);
    }
}
