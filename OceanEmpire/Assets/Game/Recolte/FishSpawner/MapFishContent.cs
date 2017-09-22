using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullInspector;

public class MapFishContent : BaseBehavior {



        //Fish Density is not an arbitraty number. It's rather the amount of fish per seocond that will spawn.
    public float mapFishDensity;
    public AnimationCurve mapFishReparition = new AnimationCurve(
        new Keyframe(0.0f, 1f), new Keyframe(1f, 1f));


    private float heightMax;
    private float depthMax;
    private float depthScaling;

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
    public Lottery<BaseFish> fishLottery; 
    

    void Start()
    {
        fishLottery = new Lottery<BaseFish>(fishTypeList.Count);
    }


    public BaseFish DrawAtFishLottery(float depth)
    {
        int lSize = fishTypeList.Count;
        for ( int i = 0; i < lSize; i ++)
        {
            FishType fT = fishTypeList[i];
            if (fT.minimumDepth > depth && fT.maximumDepth < depth)
            {
                float depthRatio = (fT.maximumDepth - depth) / (fT.maximumDepth - fT.minimumDepth);
                float fishProportion = fishTypeList[i].repartition.Evaluate(depthRatio);
                fishLottery.Add(fT.fish, fishProportion);
            }

        }
        BaseFish bigBigWinner = fishLottery.Pick();
        fishLottery.Clear();
        return bigBigWinner;
    }

    public float getGeneralDensity(float depthRatio)
    {
        return mapFishReparition.Evaluate(depthRatio) * mapFishDensity;
    }
}
