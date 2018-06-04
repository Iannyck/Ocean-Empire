using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GPComponents
{
    public abstract class ParallelBASE : Nary
    {
        public ParallelBASE(IGPComponent child) : base(child) { }
        public ParallelBASE(params IGPComponent[] children) : base(children) { }
        public ParallelBASE(IEnumerable<IGPComponent> children) : base(children) { }

        protected List<IGPComponent> ongoingChildren = new List<IGPComponent>();

        public List<IGPComponent> GetOngoingChildren()
        {
            return ongoingChildren;
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

        protected void RebuildOngoingChildrenList()
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
