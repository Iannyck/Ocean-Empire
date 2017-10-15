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
            if (FishPopulation.PopulationRate < 0.50)
            {
                Scenes.LoadAsync(AskWindow.SCENE_NAME, LoadSceneMode.Additive,delegate(Scene scene) {
                    scene.FindRootObject<AskWindow>().InitDisplay("Votre densité de poisson est faible. Vous pouvez la remettre pleine via ces deux options: ");
                });
            }
        };
	}
}
