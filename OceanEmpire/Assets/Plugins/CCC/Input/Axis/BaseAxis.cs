using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CCC.Input.Axis
{
    [System.Serializable]
    public abstract class BaseAxis
    {
        public float sensitivity = 1;

        public BaseAxis(float sensitivity = 1)
        {
            this.sensitivity = sensitivity;
        }

        public float GetValue()
        {
            return _GetRawValue() * sensitivity;
        }
        protected abstract float _GetRawValue();
    }

}