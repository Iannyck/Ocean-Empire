using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegionDisplay : MonoBehaviour {

    public Text titleName;
    public MapDescription selectedMap;

    public void Init(MapDescription map)
    {
        titleName.text = map.GetName();
        selectedMap = map;
    }

    private void Start()
    {
        CCC.Manager.MasterManager.Sync();
    }

    public void Go()
    {
        if (selectedMap != null)
            LoadingScreen.TransitionTo(GameBuilder.SCENENAME, new ToRecolteMessage(selectedMap), true);
    }
}
