using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Questing
{
    [CreateAssetMenu(menuName = "Ocean Empire/Prebuilt Quests/Capture/By Id", fileName = "New Quest")]
    public class PBCaptureByIdQC : ScriptableObject
    {
        [SerializeField] CaptureByIdQC questContext;

        void OnValidate()
        {
            questContext.trackingFlags = TrackingFlags.Recolte;
        }
    }
}
