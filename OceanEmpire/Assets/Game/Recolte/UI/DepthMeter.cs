using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class DepthMeter : MonoBehaviour {

    private SubmarineMovement submarine;
    private float heightMax;
    private float depthScaling;
    private Text tx;


    // Use this for initialization
    void Start () {
        tx = GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update () {

        if (Game.instance != null && (submarine == null))
        {
            MapInfo m = Game.instance.map;
            heightMax = m.heightMax;
            depthScaling = MapInfo.DEPTHSCALING;
            submarine = Game.instance.submarine;
            print("out");
            return;

        }
        print("in");
        tx.text = "Depth: " + (heightMax - submarine.transform.position.y) * depthScaling; 
    }
}
