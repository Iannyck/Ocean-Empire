using UnityEngine;
using System.Collections;

namespace CCC.Utility
{
    /// <summary>
    /// Monobehavior that skips updates. Use the local variables 'deltaTime' and 'fixedDeltaTime' to mesure time.
    /// </summary>
    public class SlowBehaviour : MonoBehaviour
    {
        public int skippedUpdates = 5;
        public int skippedFixedUpdates = 5;

        protected float deltaTime;
        protected float fixedDeltaTime;

        private int counter = 0;
        private int fixedCounter = 0;

        protected virtual void Update()
        {
            if (skippedUpdates == 0)
            {
                deltaTime = Time.deltaTime * (skippedUpdates + 1);
                SlowUpdate();
                return;
            }

            if (counter <= 0)
            {
                deltaTime = Time.deltaTime * (skippedUpdates + 1);
                SlowUpdate();
                counter = skippedUpdates;
            }
            else counter--;
        }


        void FixedUpdate()
        {
            if (skippedFixedUpdates <= 0)
            {
                fixedDeltaTime = Time.fixedDeltaTime * (skippedFixedUpdates + 1);
                SlowFixedUpdate();
                return;
            }

            if (fixedCounter <= 0)
            {
                fixedDeltaTime = Time.fixedDeltaTime * (skippedFixedUpdates + 1);
                SlowFixedUpdate();
                fixedCounter = skippedFixedUpdates;
            }
            else fixedCounter--;
        }

        protected virtual void SlowUpdate() { }
        protected virtual void SlowFixedUpdate() { }
    }
}
