using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace CCC.Utility
{
    public class FPSCounter : SlowBehaviour
    {
        public Text display;
        
        protected override void SlowUpdate()
        {
            base.SlowUpdate();
            if (display != null)
            {
                int fps = (int)GetFPS();

                display.text = fps.ToString();
            }
        }

        public static float GetFPS()
        {
            return 1f / Time.deltaTime;
        }

        public static float GetFixedFPS()
        {
            return 1f / Time.fixedDeltaTime;
        }
    }
}
