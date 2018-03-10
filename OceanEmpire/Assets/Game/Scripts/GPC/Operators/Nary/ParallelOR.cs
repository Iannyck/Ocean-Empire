using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GPComponents
{
    public class ParallelOR : ParallelBASE
    {
        public ParallelOR(IGPComponent child) : base(child) { }
        public ParallelOR(params IGPComponent[] children) : base(children) { }
        public ParallelOR(IEnumerable<IGPComponent> children) : base(children) { }

        public override GPCState Eval()
        {
            GPCState state;
            for (int i = 0; i < ongoingChildren.Count; i++)
            {
                state = ongoingChildren[i].Eval();
                if (state == GPCState.FAILURE)
                {
                    //It is important to decrement i if we don't want to skip the next element.
                    ongoingChildren.RemoveAt(i);
                    i--;
                }
                else if (state == GPCState.SUCCESS)
                {
                    return GPCState.SUCCESS;
                }
            }

            if (ongoingChildren.Count == 0)
            {
                return GPCState.FAILURE;
            }
            return GPCState.RUNNING;
        }
    }
}
