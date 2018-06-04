using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GPComponents
{
    public class ParallelAND : ParallelBASE
    {
        public ParallelAND(IGPComponent child) : base(child) { }
        public ParallelAND(params IGPComponent[] children) : base(children) { }
        public ParallelAND(IEnumerable<IGPComponent> children) : base(children) { }

        public override GPCState Eval()
        {
            GPCState state;
            for (int i = 0; i < ongoingChildren.Count; i++)
            {
                state = ongoingChildren[i].Eval();
                if (state == GPCState.SUCCESS)
                {
                    //It is important to decrement i if we don't want to skip the next element.
                    ongoingChildren.RemoveAt(i);
                    i--;
                }
                else if (state == GPCState.FAILURE)
                {
                    return GPCState.FAILURE;
                }
            }

            if (ongoingChildren.Count == 0)
            {
                return GPCState.SUCCESS;
            }
            return GPCState.RUNNING;
        }
    }
}
