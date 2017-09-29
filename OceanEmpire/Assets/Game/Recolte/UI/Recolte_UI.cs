using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recolte_UI : MonoBehaviour
{
    public const string SCENENAME = "Recolte_UI";
    
    public void Quit()
    {
        Game.instance.EndGame();
    }
}
