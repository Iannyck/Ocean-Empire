using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionScript : MonoBehaviour
{

    public SceneInfo selectionWindow;

    public GameObject countainer;

    public Button prefabDifficultyButton;
    public int levelToSpawn;

    public Button exerciceButton;

    public event Action<Task> SelectionEvent;

    private int currentLevelSelected = 1;

    public void Init(int startAtLevel = 1, Action<Task> onSelection = null)
    {
        for (int i = 0; i < levelToSpawn; i++)
        {
            Button newButton = Instantiate(prefabDifficultyButton, countainer.transform);
            newButton.GetComponentInChildren<Text>().text = "Niveau " + (i + 1);
            int currentIndex = i;
            newButton.onClick.AddListener(delegate ()
            {
                DifficultySelection(currentIndex + 1);
            });
        }

        if (onSelection != null)
        {
            SelectionEvent += delegate (Task plannedExercice)
            {
                onSelection.Invoke(plannedExercice);
            };
        }
    }

    public void DifficultySelection(int level)
    {
        currentLevelSelected = level;
        SwapToExerciceSelection();
    }

    public void SwapToExerciceSelection()
    {
        //foreach (Transform child in countainer.transform)
        //{
        //    Destroy(child.gameObject);
        //}

        //for (int i = 0; i < 3; i++)
        //{
        //    Button newButton = Instantiate(exerciceButton, countainer.transform);
        //    switch (i)
        //    {
        //        case 0:
        //            newButton.GetComponentInChildren<Text>().text = "Faire de la Marche\n " + PossibleTasks.GetInfo(PossibleTasks.Task.ExerciseType.marche, currentLevelSelected);
        //            newButton.onClick.AddListener(delegate ()
        //            {
        //                ExerciceSelection(PossibleTasks.Task.ExerciseType.marche);
        //            });
        //            break;
        //        case 1:
        //            newButton.GetComponentInChildren<Text>().text = "Faire de la Course\n " + PossibleTasks.GetInfo(PossibleTasks.Task.ExerciseType.course, currentLevelSelected);
        //            newButton.onClick.AddListener(delegate ()
        //            {
        //                ExerciceSelection(PossibleTasks.Task.ExerciseType.marche);
        //            });
        //            break;
        //        case 2:
        //            newButton.GetComponentInChildren<Text>().text = "Faire du Bicycle\n " + PossibleTasks.GetInfo(PossibleTasks.Task.ExerciseType.bicycle, currentLevelSelected);
        //            newButton.onClick.AddListener(delegate ()
        //            {
        //                ExerciceSelection(PossibleTasks.Task.ExerciseType.marche);
        //            });
        //            break;
        //        default:
        //            break;
        //    }
        //}
    }

    public void ExerciceSelection(Task type)
    {
        //if(SelectionEvent != null)
        //    SelectionEvent.Invoke(PossibleTasks.CreateExercice(type,currentLevelSelected));
        Scenes.UnloadAsync(selectionWindow);
    }

    public void Exit()
    {
        Scenes.UnloadAsync(selectionWindow);
    }
}
