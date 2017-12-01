using UnityEngine;
using FullInspector;

namespace CCC.Animation
{
    public abstract class BaseAnimator : BaseBehavior
    {
        public abstract void Animate(GameObject target, params object[] extra);
    }
}
