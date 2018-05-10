using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapUnlocks : MonoBehaviour
{
    [SerializeField] UpgradeCategory fishingFrenzyCategory;


    void OnEnable()
    {
        PersistentLoader.LoadIfNotLoaded(() =>
        {
            MapManager.Instance.OnMapSet += OnMapSet;
        });
    }

    void OnDisable()
    {
        if (MapManager.Instance)
            MapManager.Instance.OnMapSet -= OnMapSet;
    }

    private void OnMapSet(int arg1, MapData arg2)
    {
        if (arg1 == 1 && !fishingFrenzyCategory.IsAvailable)
            fishingFrenzyCategory.MakeAvailable(true);
    }
}
