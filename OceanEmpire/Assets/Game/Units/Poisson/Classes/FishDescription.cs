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

    //Si certain poisson font plus baissé la densité que d'autre, ca varrie en fonction de cette variable.
    public float populationValue;

    public ResourceSprite icon = new ResourceSprite(ImagePathPrefix);
}