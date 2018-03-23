using UnityEngine;

[CreateAssetMenu(menuName = "Ocean Empire/Map/Fish Repartition")]
public class FishRepartition : ScriptableObject
{
    public GameObject prefab;
    public float weight = 1;
    public AnimationCurve populationCurve = new AnimationCurve(new Keyframe(0, 1), new Keyframe(1, 1));

    /// <summary>
    /// Entre 0 et 1
    /// </summary>
    public float shallowestSpawn = 0;
    /// <summary>
    /// Entre 0 et 1
    /// </summary>
    public float deepestSpawn = 1;

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