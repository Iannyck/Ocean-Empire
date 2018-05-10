using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapUnlocks : MonoBehaviour
{
    [SerializeField] FishingFrenzyCategory fishingFrenzyCategory;
    [SerializeField] GazTankCategory gazTankCategory;
    [SerializeField] ThrusterCategory thrusterCategory;


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
        if (arg1 == 1)
        {
            if (!fishingFrenzyCategory.IsAvailable)
                fishingFrenzyCategory.MakeAvailable(true);
            if (!gazTankCategory.IsAvailable)
                gazTankCategory.MakeAvailable(true);
            if (!thrusterCategory.IsAvailable)
                thrusterCategory.MakeAvailable(true);
        }
    }
}
