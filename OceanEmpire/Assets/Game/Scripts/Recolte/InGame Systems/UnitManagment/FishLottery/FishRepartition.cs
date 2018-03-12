
using UnityEngine;

[System.Serializable]
//[CreateAssetMenu(menuName = "Ocean Empire/Fish Repartition")]
public class FishRepartition
{
    public GameObject prefab;
    public float weight;
    public AnimationCurve populationCurve;
    public float shallowestSpawn = 0;
    public float deepestSpawn = 1;

#if UNITY_EDITOR
#pragma warning disable 0414  // variable declared but not used.
    [SerializeField] private bool hasBeenInitialized = false;//
#endif

    #region Premade Curves
    public static AnimationCurve CURVE_CONSTANT
    {
        get
        {
            return new AnimationCurve(
                new Keyframe(0, 1),
                new Keyframe(1, 1));
        }
    }
    public static AnimationCurve CURVE_CENTERED
    {
        get
        {
            return new AnimationCurve(
                new Keyframe(0, 0),
                new Keyframe(0.5f, 1f),
                new Keyframe(1f, 0));
        }
    }
    public static AnimationCurve CURVE_SHALLOW
    {
        get
        {
            return new AnimationCurve(
                new Keyframe(0, 1),
                new Keyframe(1f, 0));
        }
    }
    public static AnimationCurve CURVE_DEEP
    {
        get
        {
            return new AnimationCurve(
                new Keyframe(0, 0),
                new Keyframe(1f, 1));
        }
    }
    #endregion
}