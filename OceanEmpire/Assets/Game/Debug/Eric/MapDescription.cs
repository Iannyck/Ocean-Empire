using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDescription : MonoBehaviour {

    private string zoneName;
    private string sceneToLoad;

    private List<FishDescription> fishList = new List<FishDescription>();

	// Use this for initialization
	void Start ()
    {
		
	}

    public string GetName()
    {
        return zoneName;
    }
	
}
