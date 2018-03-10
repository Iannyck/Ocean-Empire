using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GPComponents
{
	public abstract class Nary : IGPComponent
	{
		protected List<IGPComponent> children;

        public Nary(IGPComponent child)
        {
            children = new List<IGPComponent>() { child };
        }
        public Nary(params IGPComponent[] children)
        {
            this.children = new List<IGPComponent>(children);
        }
        public Nary(IEnumerable<IGPComponent> children)
        {
            this.children = new List<IGPComponent>(children);
        }

        public List<IGPComponent> GetChildren()
        {
            return children;
        }


        public abstract GPCState Eval ();

		public abstract void Launch ();

		public abstract void Reset ();

		public abstract void Abort ();
	}
}
