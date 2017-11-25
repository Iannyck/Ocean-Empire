using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmarinParts : MonoBehaviour
{
    [SerializeField, ReadOnly]
    private Thruster thruster;

    [SerializeField, ReadOnly]
    private HarpoonThrower harpoonThrower;

    [SerializeField, ReadOnly]
    private FishContainer fishContainer;


    [SerializeField, ReadOnly]
    private GazTank gazTank;

    public Thruster GetThruster() { return thruster; }
    public HarpoonThrower GetHarpoonThrower() { return harpoonThrower; }
    public FishContainer GetFishContainer() { return fishContainer;  }
    public GazTank GetGazTank() { return gazTank; }

    void Start()
    {     
        ThrusterDescription TD = ItemsList.GetEquipThruster();
        HarpoonThrowerDescription HPD = ItemsList.GetEquipHarpoonThrower();
        FishContainerDescription FC = ItemsList.GetEquipFishContainer();
        GazTankDescription GT = ItemsList.GetEquipGazTank();

        thruster = TD.GetItem<Thruster>();
        if (HPD != null)
            harpoonThrower = HPD.GetItem<HarpoonThrower>();
        fishContainer = FC.GetItem<FishContainer>();
        gazTank = GT.GetItem<GazTank>();

        fishContainer.ResetContainedFish();
        gazTank.SetGaz();
    }

    private void Update()
    {
        gazTank.UpdateTimer();
    }
}
