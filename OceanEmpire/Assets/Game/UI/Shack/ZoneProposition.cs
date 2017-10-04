using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZoneProposition : MonoBehaviour {

    public MapDescription zone;
    private ShackManager manager;
    private Text zoneName;
    private Text zoneDescription;

    public void LinkMapDescription(MapDescription a, ShackManager b)
    {
        zone = a;
        manager = b;

        zoneName = this.transform.Find("ZoneName").GetComponent<Text>();
        zoneDescription = this.transform.Find("ZoneDescription").GetComponent<Text>();
        UpdateInfos();

        this.GetComponent<Button>().onClick.AddListener(DoOnClick);

    }

    void UpdateInfos()
    {
        zoneName.text = zone.GetName();
        zoneDescription.text = zone.GetDescription();
    }

    void DoOnClick()
    {
        manager.LoadDataMapInformation(zone);
    }
}
