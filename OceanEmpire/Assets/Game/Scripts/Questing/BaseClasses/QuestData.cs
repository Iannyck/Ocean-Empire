using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Questing
{
    /// <summary>
    /// Toutes les information dynamiques de la quest (qui ne sont pas prédéterminés)
    /// </summary>
    [Serializable]
    public class QuestData
    {
        public TimeSlot timeSlot;
        public QuestState state;
        public int processorIndex;
    }
}