using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToRecolte_Temporaire : MonoBehaviour
{
    public MapDescription selectedMap;

    private void Start()
    {
        CCC.Manager.MasterManager.Sync();
    }

    public void Go()
    {
        if (selectedMap != null)
            LoadingScreen.TransitionTo(Game.SCENENAME, new ToRecolteMessage(selectedMap), true);
    }
}
