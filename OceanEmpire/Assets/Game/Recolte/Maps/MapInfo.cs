using System.Collections.Generic;
using UnityEngine;

public class MapInfo : MonoBehaviour
{
    //[InspectorCategory("General")]
    public Transform PlayerSpawn;
    //[InspectorCategory("General")]
    public Transform PlayerStart_;

    //contain distribution of a fish species
    [System.Serializable]
    public struct FishType
    {
        //Fish type
        [Header("Type")]
        public GameObject fish;

        [Header("Depth")]
        public AnimationCurve repartition;
        [Header("Depth")]
        public float highestSpawn;
        [Header("Depth")]
        public float lowestSpawn;

        [Header("Density")]
        public float fishDensity;

        // BOUTONS
        //public void DefaultRepartition()
        //{
        //    repartition = new AnimationCurve(
        //    new Keyframe(0, 0), new Keyframe(0.5f, 1f), new Keyframe(1f, 0));
        //}
        //public void SurfaceRepartition()
        //{
        //    repartition = new AnimationCurve(
        //    new Keyframe(0.0f, 1f), new Keyframe(1f, 0));
        //}
        //public void DeepRepartition()
        //{
        //    repartition = new AnimationCurve(
        //    new Keyframe(1f, 0f), new Keyframe(1f, 1));
        //}
    }

    public const float MAP_WIDTH = 5;
    public const float MAP_RIGHT = MAP_WIDTH / 2;
    public const float MAP_LEFT = MAP_WIDTH / -2;

    /// <summary>
    /// À L'horizontal seulement
    /// </summary>
    public static bool IsOutOfBounds(Vector2 v) { return v.x > MAP_RIGHT || v.x < MAP_LEFT; }
    [Header("General")]
    public string regionName = "YourRegionName";
    [Header("General")]
    public float mapTop = 0;
    [Header("General")]
    public float mapBottom = -100;

    [Header("General")]
    public float depthAtYZero = 0;

    public const float DEPTHSCALING = 100;

    //Overall map Fish density
    [Header("fish")]
    public float mapFishDensity;
    //Overall Distribution by depth 
    [Header("fish")]
    public AnimationCurve mapFishReparition = new AnimationCurve(
        new Keyframe(0.0f, 1f), new Keyframe(1f, 1f));

    //Array of fish distributions
    [Header("fish")]
    public List<FishType> fishTypeList;

    //Fish Lottery
    private Lottery<GameObject> fishLottery;

    void Start()
    {
        Game.OnGameReady += Init;
    }

    //Set lottery
    public void Init()
    {
        fishLottery = new Lottery<GameObject>(fishTypeList.Count);
    }

    //Each fish draw a straw, the fish with the shortest straw spawns
    public GameObject DrawAtFishLottery(float yPos)
    {
        int lSize = fishTypeList.Count;

        int nbStraws = 0;

        for (int i = 0; i < lSize; i++)
        {
            FishType fT = fishTypeList[i];
            if (fT.highestSpawn > yPos && fT.lowestSpawn < yPos)
            {

                nbStraws++;
                float depthRatio = (fT.highestSpawn - yPos) / (fT.highestSpawn - fT.lowestSpawn);
                float fishProportion = fishTypeList[i].repartition.Evaluate(depthRatio) * fishTypeList[i].fishDensity;

                fishLottery.Add(fT.fish, fishProportion.Clamped(0f, 1f));
            }
        }

        if (nbStraws != 0)
        {
            var bigBigWinner = fishLottery.Pick();
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
        return mapFishReparition.Evaluate(deRatio) * mapFishDensity * FishPopulation.FishDensity;
    }
}
