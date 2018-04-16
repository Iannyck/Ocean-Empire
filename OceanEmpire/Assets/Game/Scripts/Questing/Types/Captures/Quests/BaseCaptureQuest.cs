using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Questing
{
    public abstract class BaseCaptureQuest<T> : Quest<T> where T : QuestContext
    {
        public override void StartTracking()
        {
            throw new System.NotImplementedException();
        }

        public override QuestState UpdateState()
        {
            throw new System.NotImplementedException();
        }
    }
}
