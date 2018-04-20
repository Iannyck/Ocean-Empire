using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Questing
{
    public interface IQuestBuilder
    {
        Quest BuildQuest(DateTime questStartTime);
    }
}
