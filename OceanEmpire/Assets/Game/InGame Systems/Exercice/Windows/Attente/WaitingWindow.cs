using CCC.Manager;
using CCC.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WaitingWindow : MonoBehaviour {

    public const string SCENE_NAME = "WaitWindow";

    public Text title;
    public Text exercice;
    public Text enAttente;
    public WaitAnimation waitAnimation;
    public WindowAnimation windowAnim;
    public GameObject debugInfoPrefab;
    public Transform debugInfoCountainer;
    private List<GameObject> infoObjects = new List<GameObject>();

    private Action onCompleteEvent;

    public static void ShowWaitingWindow(string exerciceDescription, Action onComplete = null, string enAttente = "En Attente...", string title = "Faire l'exercice")
    {
        Scenes.LoadAsync(SCENE_NAME, LoadSceneMode.Additive, delegate (Scene scene) {
            scene.FindRootObject<WaitingWindow>().InitDisplay(exerciceDescription,onComplete,enAttente,title);
        });
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

    public void AddDebugInfos(List<string> infos)
    {
        foreach (string info in infos)
        {
            GameObject newDebugInfo = Instantiate(debugInfoPrefab, debugInfoCountainer);
            infoObjects.Add(newDebugInfo);
            newDebugInfo.GetComponent<Text>().text = info;
        }
    }

    public void AddDebugInfo(string info)
    {
        GameObject newDebugInfo = Instantiate(debugInfoPrefab, debugInfoCountainer);
        newDebugInfo.GetComponent<Text>().text = info;
    }

    public void UpdateInfo(int index, string info)
    {
        infoObjects[index].GetComponent<Text>().text = info;
    }

    public void Hide()
    {
        windowAnim.Close(delegate() {
            Scenes.UnloadAsync(SCENE_NAME);
            if (onCompleteEvent != null)
                onCompleteEvent.Invoke();
        });
    }
    
	public void DebugComplete()
    {
        Hide();
    }
}
