using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class DepthMeter : MonoBehaviour {

    private SubmarineMovement submarine;
    //private float depthStart;
    private float depthScaling;
    private Text tx;


    // Use this for initialization
    void Start () {
        tx = GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update () {

        if (Game.Instance != null && (submarine == null))
        {
            if (Game.Instance.map == null)
                return;

            //MapInfo m = Game.instance.map;
            //depthStart = m.depthAtYZero;
            depthScaling = MapInfo.DEPTHSCALING;
            submarine = Game.Instance.submarine;
            return;

        }

        if(submarine != null)
            tx.text = "Depth: " + (( - submarine.transform.position.y) * depthScaling).Rounded(-1); 
    }
}
