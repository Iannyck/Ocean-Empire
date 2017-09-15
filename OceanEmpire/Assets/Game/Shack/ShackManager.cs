using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShackManager : MonoBehaviour {

    public GameObject upgradePannel;

    // Use this for initialization
    void Start()
    {
        upgradePannel.SetActive(false);
    }

    // Update is called once per frame
    void Update() {

    }

    public void ToggleUpgradePannel()
    {
        upgradePannel.SetActive(!upgradePannel.activeInHierarchy);
    }

    public void GoToExploration()
    {
        // Load Scene
    }


}
