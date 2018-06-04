using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CCC.Input.Axis
{
    [System.Serializable]
    public class KeyAxis : BaseAxis
    {
        public KeyCode positive;
        public KeyCode negative;

        public KeyAxis(KeyCode positive, KeyCode negative, float sensitivity = 1) : base(sensitivity)
        {
            this.positive = positive;
            this.negative = negative;
        }

        protected override float _GetRawValue()
        {
            float value = 0;
            if (UnityEngine.Input.GetKey(positive))
                value++;
            if (UnityEngine.Input.GetKey(negative))
                value--;
            return value;
        }

        public override string ToString()
        {
            return "(-) " + negative.ToString() + "   (+) " + positive.ToString();
        }
    }
}