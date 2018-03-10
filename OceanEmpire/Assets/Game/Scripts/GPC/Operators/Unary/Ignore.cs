using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GPComponents
{
    /// <summary>
    /// Ignore the end state of your child by always returning true when the child reaches it.
    /// </summary>
    public class Ignore : Unary
    {
        public Ignore(IGPComponent child) : base(child) { }

        public override GPCState Eval()
        {
            GPCState state = child.Eval();
            if (state != GPCState.RUNNING)
            {
                return GPCState.SUCCESS;
            }
            return GPCState.RUNNING;
        }

        public override void Launch()
        {
            child.Launch();
        }

        public override void Reset()
        {
            child.Reset();
        }

        public override void Abort()
        {
            child.Abort();
        }
    }
}
