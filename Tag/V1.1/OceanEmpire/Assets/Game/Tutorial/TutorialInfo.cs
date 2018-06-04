using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tutorial
{
    public abstract class TutorialInfo : MonoBehaviour
    {
        public abstract void DisplayInfo(string text);

        public abstract void OnEnd();
    }
}
