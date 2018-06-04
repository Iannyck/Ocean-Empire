using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GPComponents
{
    public class SequentialAND : SequentialBASE
    {
        public override GPCState Eval()
        {
            GPCState state = children[childrenIndex].Eval();
            if (state == GPCState.SUCCESS)
            {
                childrenIndex++;
                children[childrenIndex].Launch();
            }
            else if (state == GPCState.FAILURE)
            {
                return GPCState.FAILURE;
            }
            if (childrenIndex >= children.Count)
            {
                return GPCState.SUCCESS;
            }
            return GPCState.RUNNING;
        }
    }
}
