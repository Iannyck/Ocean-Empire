using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FullInspector;

public class ShackManager : BaseBehavior
{
    //public const string SCENENAME = "Shack";

    //public GameObject upgradePannel;
    //public GameObject zonePannel;
    //public GameObject mapInformationPannel;
    //public GameObject upgradeButton;
    //public GameObject zoneButton;

    //public MapDescription[] arrayMapDescription;

    //public GameObject zonePropositionPrefab;

    //public MapDescription selectedMap;

    ////private float spaceBetweenZoneProposition = 85;

    //void Start()
    //{
    //    upgradeButton.SetActive(true);
    //    zoneButton.SetActive(true);
    //    upgradePannel.SetActive(false);
    //    zonePannel.SetActive(false);
    //    mapInformationPannel.SetActive(false);

    //    CCC.Manager.MasterManager.Sync();

    //    LoadMapDescription();
    //}

    //public void ToggleUpgradePannel()
    //{
    //    upgradePannel.SetActive(!upgradePannel.activeInHierarchy);
    //    zonePannel.SetActive(false);
    //}

    //public void ToggleZonePannel()
    //{
    //    zonePannel.SetActive(!zonePannel.activeInHierarchy);
    //    upgradePannel.SetActive(false);
    //}

    //public void ReturnToZonePannel()
    //{
    //    zonePannel.SetActive(true);
    //    zoneButton.SetActive(true);
    //    upgradeButton.SetActive(true);
    //    mapInformationPannel.GetComponent<MapInformation>().Close();
    //    mapInformationPannel.SetActive(false);
    //}


    //public void LoadDataMapInformation(MapDescription map)
    //{
    //    // if (map != null)
    //    // Methode de la MapDescription

    //    mapInformationPannel.GetComponent<MapInformation>().Init(map);
    //    mapInformationPannel.SetActive(true);
    //    upgradeButton.SetActive(false);
    //    zoneButton.SetActive(false);
    //    zonePannel.SetActive(false);
    //}

    //public void GoToExploration()
    //{
    //    // Load Scene
    //    if (selectedMap != null)
    //        LoadingScreen.TransitionTo(GameBuilder.SCENENAME, new ToRecolteMessage(selectedMap), true);
    //}

    //public void LoadMapDescription()
    //{
    //    arrayMapDescription = Resources.FindObjectsOfTypeAll<MapDescription>() as MapDescription[];

    //    foreach(MapDescription a in arrayMapDescription)
    //    {
    //        GameObject b = Instantiate(zonePropositionPrefab, zonePannel.transform);
    //        b.GetComponent<ZoneProposition>().LinkMapDescription(a,this);
    //    } 
    //}


}
