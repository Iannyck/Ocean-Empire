using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GPComponents
{
    public abstract class SequentialBASE : Nary
    {
        protected int childrenIndex;

        public override void Launch()
        {
            childrenIndex = 0;
            if (children != null && children.Count > 0)
                children[childrenIndex].Launch();
        }

        public override void Reset()
        {
            for (int tempChildrenIndex = 0; tempChildrenIndex < childrenIndex; tempChildrenIndex++)
            {
                children[childrenIndex].Reset();
            }
            childrenIndex = 0;
        }

        public override void Abort()
        {
            for (int tempChildrenIndex = 0; tempChildrenIndex < childrenIndex; tempChildrenIndex++)
            {
                children[childrenIndex].Abort();
            }
        }

    }
}
