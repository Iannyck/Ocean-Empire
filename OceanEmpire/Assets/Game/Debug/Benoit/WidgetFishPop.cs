using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WidgetFishPop : MonoBehaviour {

    public Image gageMeter;

	// Update is called once per frame
	void Update () {

        FishPopulation.instance.RefreshPopulation();
       gageMeter.rectTransform.localScale = new Vector3(FishPopulation.PopulationRate, 1 ,1) ;

        if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            print("mad");
            FishPopulation.instance.AddRate(0.2f);
        }

        if (Input.GetKeyDown(KeyCode.KeypadMinus))
        { 
            FishPopulation.instance.AddRate(-0.2f);
        }
    }

}
