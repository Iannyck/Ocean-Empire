﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToRecolte_Temporaire : MonoBehaviour
{
    public MapDescription selectedMap;

    private void Start()
    {
        PersistentLoader.LoadIfNotLoaded();
    }

    public void Go()
    {
        if (selectedMap != null)
            LoadingScreen.TransitionTo(GameBuilder.SCENENAME, new ToRecolteMessage(selectedMap.sceneToLoad), true);
    }
}
