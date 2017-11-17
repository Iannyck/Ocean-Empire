using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegionSelectionDisplayMaps : MonoBehaviour {

    public GameObject mapLayout;
    public RegionDisplay mapDisplayPrefab;


    void Start () {

        List<MapDescription> maps = ItemsList.GetAllOwnedMaps();

        for (int i = 0; i < maps.Count; ++i)
        {
            RegionDisplay newRegion = Instantiate(mapDisplayPrefab, mapLayout.transform);
            newRegion.Init(maps[i]);
        }
    }
}
