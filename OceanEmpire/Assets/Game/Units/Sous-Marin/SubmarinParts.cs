using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmarinParts : MonoBehaviour
{
    [SerializeField]
    private Thruster thruster;

    [SerializeField]
    private HarpoonThrower harpoonThrower;

    [SerializeField]
    private FishContainer fishContainer;

    [SerializeField]
    private GazTank gazTank;

    public Thruster GetThruster() { return thruster; }
    public HarpoonThrower GetHarpoonThrower() { return harpoonThrower; }
    public FishContainer GetFishContainer() { return fishContainer; }
    public GazTank GetGazTank() { return gazTank; }

    void Start()
    {
        //ThrusterDescriptionOLD TD = null;
        //HarpoonThrowerDescriptionOLD HPD = null;
        //FishContainerDescription FC = null;
        //GazTankDescriptionOLD GT = null;

        //if (ItemsList.instance != null)
        //{
        //    TD = ItemsList.GetEquipThruster();
        //    HPD = ItemsList.GetEquipHarpoonThrower();
        //    FC = ItemsList.GetEquipFishContainer();
        //    GT = ItemsList.GetEquipGazTank();
        //}

        //if (TD != null)
        //    thruster = TD.GetItem<Thruster>();
        //if (HPD != null)
        //    harpoonThrower = HPD.GetItem<HarpoonThrower>();
        //if (FC != null)
        //    fishContainer = FC.GetItem<FishContainer>();
        //if (GT != null)
        //    gazTank = GT.GetItem<GazTank>();

        if (gazTank != null)
            gazTank.SetGaz();
        if (fishContainer != null)
            fishContainer.ResetContainedFish();
    }

    private void Update()
    {
        if (gazTank != null)
            gazTank.UpdateTimer();
    }
}
