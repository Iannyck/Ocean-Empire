using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace CCC.Utility
{
    public class FPSCounter : SlowBehaviour
    {
        public Text display;
        public bool cummulateOverSkippedFrames = true;

        private float cummulatedFPS;

        protected override void Update()
        {
            base.Update();

            if (cummulateOverSkippedFrames)
                cummulatedFPS += GetFPS() / (skippedUpdates + 1);
        }

        protected override void SlowUpdate()
        {
            base.SlowUpdate();
            if (display != null)
            {
                int fps = 0;
                if (cummulateOverSkippedFrames)
                {
                    fps = cummulatedFPS.RoundedToInt();
                    cummulatedFPS = 0;
                }
                else
                    fps = (int)GetFPS();


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
