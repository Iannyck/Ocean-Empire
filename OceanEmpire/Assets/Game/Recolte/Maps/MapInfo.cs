using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullInspector;

public class MapInfo : BaseBehavior
{
        //contain distribution of a fish species
    [System.Serializable]
    public struct FishType
    {
            //Fish type
        [InspectorCategory("Type")]
        public BaseFish fish;

            //Distribution by Depth
        [InspectorCategory("Depth")]
        public AnimationCurve repartition;
        [InspectorCategory("Depth")]
        public float highestSpawn;
        [InspectorCategory("Depth")]
        public float lowestSpawn;

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


            //Fish denstity
        [InspectorCategory("Density")]
        public float fishDensity;
        [InspectorCategory("Density")]
        public float populationInfluence;
    }





    public const float MAP_WIDTH = 5;
    public const float MAP_RIGHT = MAP_WIDTH / 2;
    public const float MAP_LEFT = MAP_WIDTH / -2;

    /// <summary>
    /// À L'horizontal seulement
    /// </summary>
    public static bool IsOutOfBounds(Vector2 v) { return v.x > MAP_RIGHT || v.x < MAP_LEFT; }
    [InspectorCategory("General")]
    public string regionName = "YourRegionName";
    [InspectorCategory("General")]
    public float mapTop = 0;
    [InspectorCategory("General")]
    public float mapBottom = -100;

    [InspectorCategory("General")]
    public float depthAtYZero = 0;

    public const float DEPTHSCALING = 100;




    //Overall map Fish density
    [InspectorCategory("fish")]
    public float mapFishDelay;
    //Overall Distribution by depth 
    [InspectorCategory("fish")]
    public AnimationCurve mapFishReparition = new AnimationCurve(
        new Keyframe(0.0f, 1f), new Keyframe(1f, 1f));

    //Array of fish distributions
    [InspectorCategory("fish")]
    public List<FishType> fishTypeList;

        //Fish Lottery
    private Lottery<BaseFish> fishLottery;


        //Set lottery
    void Start()
    {
        fishLottery = new Lottery<BaseFish>(fishTypeList.Count);
    }

        //Each fish draw a straw, the fish with the shortest straw spawns
    public BaseFish DrawAtFishLottery(float yPos)
    {
   
        int lSize = fishTypeList.Count;
        print("size :" + lSize);
        int nbStraws = 0;

        for (int i = 0; i < lSize; i++)
        {
            FishType fT = fishTypeList[i];
            if (fT.highestSpawn > yPos && fT.lowestSpawn < yPos)
            {
               
                nbStraws++;
                float depthRatio = (fT.highestSpawn - yPos) / (fT.highestSpawn - fT.lowestSpawn);
                float fishProportion = fishTypeList[i].repartition.Evaluate(depthRatio);
                fishLottery.Add(fT.fish, fishProportion.Clamped(0f,1f));
            }
        }

        if (nbStraws != 0)
        { 
            BaseFish bigBigWinner = fishLottery.Pick();
            fishLottery.Clear();
            
            return bigBigWinner;
        }
        else
            
        return null;
    }

        //Get the spawn rate for this specific depth
    public float GetGeneralDensity(float position)
    {
        float deRatio = (mapTop - position) / (mapTop - mapBottom);
        return mapFishReparition.Evaluate(deRatio) * mapFishDelay;
    }



}
