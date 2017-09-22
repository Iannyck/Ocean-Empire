using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullInspector;

public class MapFishContent : BaseBehavior {

    [System.Serializable]
    public struct FishType
    {
        [InspectorCategory("Type")]
        public BaseFish fish;
        //Fish repartition in map
        //[InspectorCategory("Depth")]
        //public float maxDensity;
        //[InspectorCategory("Depth")]
        //public float highestPoint;
        //[InspectorCategory("Depth")]
        //public float lowestPoint;
        [InspectorCategory("Depth")]
        public AnimationCurve repartition;
        [InspectorCategory("Depth")]
        public float minimumDepth;
        [InspectorCategory("Depth")]
        public float maximumDepth;
        [InspectorButton, InspectorCategory("Depth")]
        public void DefaultRepartition()
        {
            repartition = new AnimationCurve(
            new Keyframe(0, 0), new Keyframe(0.5f, 1f), new Keyframe(1f, 0));
        }
        [InspectorButton, InspectorCategory("Depth")]
        public void SurfaceRepartition()
        {
            repartition = new AnimationCurve(
            new Keyframe(0.0f, 1f), new Keyframe(1f, 0));
        }


        //Fish denstity in zone
        [InspectorCategory("Density")]
        public float fishDensity;
        [InspectorCategory("Density")]
        public float populationInfluence;
    }

    public List<FishType> fishTypeList;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    

}
