using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Questing
{
    [CreateAssetMenu(menuName = "Ocean Empire/Prebuilt Quests/Capture/By Tag", fileName = "New Quest")]
    public class PBCaptureByTagQC : ScriptableObject
    {
        [SerializeField] CaptureByTagQC questContext;

        void OnValidate()
        {
            questContext.trackingFlags = TrackingFlags.Recolte;
        }
    }
}
