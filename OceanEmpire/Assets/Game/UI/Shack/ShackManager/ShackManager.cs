using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShackManager : MonoBehaviour
{
    public const string SCENENAME = "Shack";

    public GameObject upgradePannel;
    public GameObject zonePannel;
    public GameObject mapInformationPannel;

    
    void Start()
    {
        upgradePannel.SetActive(false);
        zonePannel.SetActive(false);

        CCC.Manager.MasterManager.Sync();
    }

    public void ToggleUpgradePannel()
    {
        upgradePannel.SetActive(!upgradePannel.activeInHierarchy);
        zonePannel.SetActive(false);
    }

    public void ToggleZonePannel()
    {
        zonePannel.SetActive(!zonePannel.activeInHierarchy);
        upgradePannel.SetActive(false);
    }

    public void ReturnToZonePannel()
    {
        zonePannel.SetActive(true);
        mapInformationPannel.SetActive(false);
    }


    public void LoadDataMapInformation(/*MapDescription map = null*/)
    {
        // if (map != null)
        // Methode de la MapDescription

        mapInformationPannel.SetActive(!mapInformationPannel.activeInHierarchy);
        zonePannel.SetActive(false);
    }




    public void GoToExploration()
    {
        // Load Scene
        LoadingScreen.TransitionTo("Recolte_Map", null);
    }


}
