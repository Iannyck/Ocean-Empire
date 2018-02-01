 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenExercicePanel : MonoBehaviour
{
    private void Start()
    {
        PersistentLoader.LoadIfNotLoaded();
    }

    public void Open()
    {
        InstantExerciseChoice.ProposeTasks(RewardType.Tickets);
    }

    void OnSceneLoaded(Scene scene)
    {
        //...
    }
}
