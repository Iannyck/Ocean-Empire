using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recolte_UI : MonoBehaviour
{
    public const string SCENENAME = "Recolte_UI";

    public OnScreenFeedback feedbacks;
    public InGameTextPopup textPopups;
    public GazSlider gazSlider;
    public OptionsButton optionButton;
    public GameObject hider;
    
    public void Quit()
    {
        Game.Instance.EndGame();
    }


}
