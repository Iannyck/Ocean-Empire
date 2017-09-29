using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( menuName = "Ocean Empire/Map Description")]
public class MapDescription : ScriptableObject
{
    public string zoneName;
    public string sceneToLoad;

    public List<FishDescription> fishList = new List<FishDescription>();


    public string GetName()
    {
        return zoneName;
    }
	
}
