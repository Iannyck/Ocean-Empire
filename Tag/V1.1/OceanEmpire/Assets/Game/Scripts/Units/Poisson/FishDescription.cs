using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ocean Empire/Fish Description")]
public class FishDescription : ScriptableObject
{
    public const string ImagePathPrefix = "Images/Fish/";

    public string fishName;
    public string description;
    public float baseMonetaryValue;
    public FishId id;
    [BitMask]
    public FishFlags flags;

    public Sprite icon;
}