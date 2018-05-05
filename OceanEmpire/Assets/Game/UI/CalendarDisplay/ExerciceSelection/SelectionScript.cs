using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionScript : MonoBehaviour {

    public SceneInfo selectionWindow;

    public GameObject countainer;

    public Button prefabDifficultyButton;
    public int levelToSpawn;

    public Button exerciceButton;

    public delegate void ExerciceSelectionHandler(PossibleExercice.PlannedExercice plannedExercice);

    public event ExerciceSelectionHandler SelectionEvent;

    private int currentLevelSelected = 1;

    public void Init(int startAtLevel = 1, Action<PossibleExercice.PlannedExercice> onSelection = null)
    {
        for (int i = 0; i < levelToSpawn; i++)
        {
            Button newButton = Instantiate(prefabDifficultyButton, countainer.transform);
            newButton.GetComponentInChildren<Text>().text = "Level " + (i + 1);
            int currentIndex = i;
            newButton.onClick.AddListener(delegate()
            {
                DifficultySelection(currentIndex+1);
            });
        }

        if(onSelection != null)
        {
            SelectionEvent += delegate (PossibleExercice.PlannedExercice plannedExercice)
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
        foreach (Transform child in countainer.transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < 3; i++)
        {
            Button newButton = Instantiate(exerciceButton, countainer.transform);
            switch (i)
            {
                case 0:
                    newButton.GetComponentInChildren<Text>().text = "Faire de la Marche\n " + PossibleExercice.GetInfo(PossibleExercice.PlannedExercice.ExerciceType.marche, currentLevelSelected);
                    newButton.onClick.AddListener(delegate ()
                    {
                        ExerciceSelection(PossibleExercice.PlannedExercice.ExerciceType.marche);
                    });
                    break;
                case 1:
                    newButton.GetComponentInChildren<Text>().text = "Faire de la Course\n " + PossibleExercice.GetInfo(PossibleExercice.PlannedExercice.ExerciceType.course, currentLevelSelected);
                    newButton.onClick.AddListener(delegate ()
                    {
                        ExerciceSelection(PossibleExercice.PlannedExercice.ExerciceType.marche);
                    });
                    break;
                case 2:
                    newButton.GetComponentInChildren<Text>().text = "Faire du Bicycle\n " + PossibleExercice.GetInfo(PossibleExercice.PlannedExercice.ExerciceType.bicycle, currentLevelSelected);
                    newButton.onClick.AddListener(delegate ()
                    {
                        ExerciceSelection(PossibleExercice.PlannedExercice.ExerciceType.marche);
                    });
                    break;
                default:
                    break;
            }
        }
    }

    public void ExerciceSelection(PossibleExercice.PlannedExercice.ExerciceType type)
    {
        if(SelectionEvent != null)
            SelectionEvent.Invoke(PossibleExercice.CreateExercice(type,currentLevelSelected));
        Scenes.UnloadAsync(selectionWindow);
    }

    public void Exit()
    {
        Scenes.UnloadAsync(selectionWindow);
    }
}
