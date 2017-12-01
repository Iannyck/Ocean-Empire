using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderInfo : MonoBehaviour
{
    public enum ParentType
    {
        Player = 1 << 0,
        MiniFish = 1 << 1,
        BigFish = 1 << 2
    }

    public ParentType parentType;

    [SerializeField]
    private GameObject parent;
    public GameObject Parent { get { return parent != null ? parent : gameObject; } }
}
