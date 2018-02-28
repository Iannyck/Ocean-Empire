using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GPComponents
{
    public class ParallelOR : Nary
    {
        public ParallelOR(IGPComponent child) : base(child) { }
        public ParallelOR(params IGPComponent[] children) : base(children) { }
        public ParallelOR(IEnumerable<IGPComponent> children) : base(children) { }

        private List<IGPComponent> ongoingChildren = new List<IGPComponent>();

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

        public override void Launch()
        {
            foreach (IGPComponent gComponent in children)
            {
                gComponent.Launch();
            }
            RebuildOngoingChildrenList();
        }

        public override void Reset()
        {
            foreach (IGPComponent gComponent in children)
            {
                gComponent.Reset();
            }
            RebuildOngoingChildrenList();
        }

        public override void Abort()
        {
            foreach (IGPComponent gComponent in children)
            {
                gComponent.Abort();
            }
        }

        private void RebuildOngoingChildrenList()
        {
            ongoingChildren.Clear();
            if (ongoingChildren.Capacity != children.Count)
                ongoingChildren.Capacity = children.Count;
            for (int i = 0; i < children.Count; i++)
            {
                ongoingChildren.Add(children[i]);
            }
        }
    }
}
