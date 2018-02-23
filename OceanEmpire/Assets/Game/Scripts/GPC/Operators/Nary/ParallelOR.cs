using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GPComponents
{
    public class ParallelOR : Nary
    {
        public override GPCState Eval()
        {
            GPCState state;
            for (int i = 0; i < children.Count; i++)
            {
                state = children[i].Eval();
                if (state == GPCState.FAILURE)
                {
                    //It is important to decrement i if we don't want to skip the next element.
                    children.RemoveAt(i);
                    i--;
                }
                else if (state == GPCState.SUCCESS)
                {
                    return GPCState.SUCCESS;
                }
            }

            if (children.Count == 0)
            {
                return GPCState.FAILURE;
            }
            return GPCState.RUNNING;
        }

        public override void Launch()
        {
            foreach (IGPComponent gComponent in children)
            {
                gComponent.Launch();
            }
        }

        public override void Reset()
        {
            foreach (IGPComponent gComponent in children)
            {
                gComponent.Reset();
            }
        }

        public override void Abort()
        {
            foreach (IGPComponent gComponent in children)
            {
                gComponent.Abort();
            }
        }

    }
}
