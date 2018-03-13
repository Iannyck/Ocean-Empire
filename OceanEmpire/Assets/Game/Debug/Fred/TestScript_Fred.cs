using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript_Fred : MonoBehaviour
{
    //public float drawAt = 0.1f;
    //public MapInfo mapInfo;
    //public FishLottery fishLottery;

    void Start()
    {
        Debug.LogWarning("Je suis un test script, ne m'oublie pas (" + gameObject.name + ")");
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.T))
        //{
        //    print(fishLottery.DrawAtPosition01(drawAt));
        //}
        //if (Input.GetKeyDown(KeyCode.M))
        //{
        //    Debug.Log("Metrics: "
        //        + "\nMap Height: " + mapInfo.MapHeight
        //        + "\nMap Top: " + mapInfo.mapTop
        //        + "\nMap Bottom: " + mapInfo.mapBottom
        //        + "\nMap 250: " + mapInfo.GetMapPosition01(250) + "%"
        //        + "\nMap -250: " + mapInfo.GetMapPosition01(-250) + "%"
        //        + "\nMap 50%: " + mapInfo.GetMapHeightFromPosition01(0.5f));
        //}
    }
}