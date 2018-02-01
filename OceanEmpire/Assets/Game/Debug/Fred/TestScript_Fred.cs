using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TestScript_Fred : MonoBehaviour
{
    public DayInspector g;

    void Start()
    {
        PersistentLoader.LoadIfNotLoaded();
        Debug.LogWarning("Je suis un test script, ne m'oublie pas (" + gameObject.name + ")");

        Update();
        //InstantExerciseChoice.ProposeTasks();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            char e = TextCharacters.E_Aigue;
            char a = TextCharacters.A_Grave;
            MessagePopup.DisplayMessage("La plage horaire est d" + e + "j" + a + " utilis" + e + ".");
        }
    }
}
