 
using CCC.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RatingWindow : MonoBehaviour {

    public const string SCENE_NAME = "Rating";

    public List<Button> ratingButtons = new List<Button>();

    public WindowAnimation windowAnim;

    private Button currentButtonSelected;
    private Action<HappyRating> onComplete;

    public static void ShowRatingWindow(Action<HappyRating> onComplete, string exerciceDescription = "Comment avez-vous trouvé l'exercice ?")
    {
        Scenes.LoadAsync(SCENE_NAME, LoadSceneMode.Additive, delegate (Scene scene) {
            scene.FindRootObject<RatingWindow>().InitDisplay(onComplete);
        });
    }

    public void InitDisplay(Action<HappyRating> onComplete)
    {
        windowAnim.Open();
        this.onComplete = onComplete;
        foreach (Button button in ratingButtons)
        {
            currentButtonSelected = button;
            button.onClick.AddListener(RatingSelected);
            
        }
    }

    public void RatingSelected()
    {
        windowAnim.Close(delegate() {
            Scenes.UnloadAsync(SCENE_NAME);
            onComplete.Invoke(currentButtonSelected.gameObject.GetComponent<RatingButtonTag>().happyRatingTag);
        });
    }
}
