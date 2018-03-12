using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PalierManagerFollow : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PalierManager palierManager;
    public PalierManager PalierManager { get { return palierManager; } }

    [Header("Settings")]
    public Transform FollowTarget;
    public float VerticalOffset;

    private int lastPalier = int.MinValue;

    void Update()
    {
        if (FollowTarget != null)
        {
            var updatedPalier = PalierManager.PalierPlans.GetClosestPalier(FollowTarget.position.y + VerticalOffset);
            if (updatedPalier != lastPalier)
            {
                PalierManager.CenterPalier = updatedPalier;
                lastPalier = updatedPalier;
            }
        }
    }
}