using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderInfo : MonoBehaviour
{
    public enum ParentType { Player = 0, MiniFish = 1, BigFish = 2 }

    public ParentType parentType;
}
