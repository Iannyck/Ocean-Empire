using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GPComponents
{
	public abstract class Nary : IGPComponent
	{
		protected List<IGPComponent> children = new List<IGPComponent>();

        public void AddChild(IGPComponent child)
        {
            children.Add(child);
        }
        public void AddChildren(IEnumerable<IGPComponent> children)
        {
            this.children.AddRange(children);
        }


        public abstract GPCState Eval ();

		public abstract void Launch ();

		public abstract void Reset ();

		public abstract void Abort ();
	}
}
