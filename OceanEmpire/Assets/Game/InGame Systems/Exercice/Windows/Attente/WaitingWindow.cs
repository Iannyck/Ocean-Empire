using CCC.Manager;
using CCC.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaitingWindow : MonoBehaviour {

    public const string SCENE_NAME = "WaitWindow";

    public Text title;
    public Text exercice;
    public Text enAttente;
    public WaitAnimation waitAnimation;
    public WindowAnimation windowAnim;
    public bool debug = false;
    public GameObject debugInfoPrefab;
    public Transform debugInfoCountainer;

    private Action onCompleteEvent;

    void Start()
    {
        Time.timeScale = 0;
        if (debug)
            InitDisplay("À faire: Courrir ta vie");
    }

    public void InitDisplay(string exerciceDescription, Action onComplete = null, string enAttente = "En Attente...", string title = "Faire l'exercice")
    {
        this.title.text = title;
        exercice.text = exerciceDescription;
        this.enAttente.text = enAttente;
        onCompleteEvent = onComplete;
        windowAnim.Open(delegate() {
            waitAnimation.DoAnimation();
        });
    }

    public void AddDebugInfo(List<string> infos)
    {
        foreach (string info in infos)
        {
            GameObject newDebugInfo = Instantiate(debugInfoPrefab, debugInfoCountainer);
            //newDebugInfo.GetComponent<>
        }
    }

    public void Hide()
    {
        Time.timeScale = 1;
        windowAnim.Close();
        Scenes.UnloadAsync(SCENE_NAME);
        if(onCompleteEvent != null)
            onCompleteEvent.Invoke();
    }
    
	public void DebugComplete()
    {
        Hide();
    }
}
