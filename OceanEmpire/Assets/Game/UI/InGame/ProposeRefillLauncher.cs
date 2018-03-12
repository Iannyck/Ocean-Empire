 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ProposeRefillLauncher : MonoBehaviour {

    // Utile pour scripter une demande d'exercice (donc a enlever lorsquon aura le vrai systeme)

    public float proposeUnderDensity = 0.75f;

	void Start ()
    {
        Game.OnGameReady += delegate ()
        {
            FishPopulation.instance.RefreshPopulation();
            if (FishPopulation.FishDensity <= proposeUnderDensity)
            {
                Scenes.LoadAsync(ProposeRefillWindow.SCENE_NAME, LoadSceneMode.Additive,null);
            }
        };
	}
}
