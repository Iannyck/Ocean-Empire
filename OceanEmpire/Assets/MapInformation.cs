using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapInformation : MonoBehaviour
{
    public Button cancelButton;
    public Button explorationButton;
    public Text nameZone;

    public List<GameObject> fishToDisplay = new List<GameObject>();

    public void Init(MapDescription a)
    {
        nameZone.text = a.GetName();
        int count = 0;
        
        foreach (FishDescription fd in a.fishList)
        {
            GameObject actualFishDisplay = fishToDisplay[count];

            actualFishDisplay.transform.Find("FishName").GetComponent<Text>().text = fd.fishName;
            actualFishDisplay.transform.Find("FishDescription").GetComponent<Text>().text = fd.description;
            actualFishDisplay.transform.Find("Image").GetComponent<Image>().sprite = fd.icon.GetSprite();
            Debug.Log(fd.icon.GetSprite());
            actualFishDisplay.SetActive(true);

            count++;
        }
    }

    public void Close()
    {
        for (int i = 0; i< fishToDisplay.Count;i++)
            fishToDisplay[i].SetActive(false);
    }
}
