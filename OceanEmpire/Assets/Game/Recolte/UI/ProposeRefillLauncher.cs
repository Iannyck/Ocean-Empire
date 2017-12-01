using CCC.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ProposeRefillLauncher : MonoBehaviour {

    // Utile pour scripter une demande d'exercice (donc a enlever lorsquon aura le vrai systeme)

    public float proposeUnderRate = 0.75f;

	void Start ()
    {
        Game.OnGameReady += delegate ()
        {
            if (FishPopulation.PopulationRate <= proposeUnderRate)
            {
                Scenes.LoadAsync(ProposeRefillWindow.SCENE_NAME, LoadSceneMode.Additive,null);
            }
        };
	}
}
