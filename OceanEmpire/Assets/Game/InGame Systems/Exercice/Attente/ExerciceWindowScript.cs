using CCC.Manager;
using CCC.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExerciceWindowScript : MonoBehaviour {

    public const string SCENE_NAME = "WaitExerciceWindow";

    public Text title;
    public Text exercice;
    public Text enAttente;
    public WaitAnimation waitAnimation;
    public WindowAnimation windowAnim;
    public bool debug = false;

    void Start()
    {
        if (debug)
            InitDisplay("À faire: Courrir ta vie");
    }

    public void InitDisplay(string exerciceDescription, string enAttente = "En Attente...", string title = "Faire l'exercice")
    {
        this.title.text = title;
        exercice.text = exerciceDescription;
        this.enAttente.text = enAttente;
        windowAnim.Open(delegate() {
            waitAnimation.DoAnimation();
        });
    }

    public void Hide()
    {
        windowAnim.Close();
        Scenes.UnloadAsync(SCENE_NAME);
    }
    
	public void DebugComplete()
    {
        Hide();
    }
}
