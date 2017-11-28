using CCC.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExerciceDemand : MonoBehaviour {

    // Utile pour scripter une demande d'exercice (donc a enlever lorsquon aura le vrai systeme)

	void Start ()
    {
        Game.OnGameReady += delegate ()
        {
            if (FishPopulation.PopulationRate <= 25)
            {
                Scenes.LoadAsync(ProposeRefillWindow.SCENE_NAME, LoadSceneMode.Additive,null);
            }
        };
	}
}
