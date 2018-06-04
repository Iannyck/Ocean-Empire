using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GPComponents
{
    public class SequentialOR : SequentialBASE
    {
        public override GPCState Eval()
        {
            GPCState state = children[childrenIndex].Eval();
            if (state == GPCState.FAILURE)
            {
                childrenIndex++;
                children[childrenIndex].Launch();
            }
            else if (state == GPCState.SUCCESS)
            {
                return GPCState.SUCCESS;
            }
            if (childrenIndex >= children.Count)
            {
                return GPCState.FAILURE;
            }
            return GPCState.RUNNING;
        }
    }
}
