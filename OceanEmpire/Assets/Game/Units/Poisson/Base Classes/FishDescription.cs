using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ocean Empire/Fish Description")]
public class FishDescription : ScriptableObject
{
    public const string ImagePathPrefix = "Images/Fish/";

    public string fishName;
    public string description;
    public float baseValue;

    public ResourceSprite icon = new ResourceSprite(ImagePathPrefix);
}