using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GPComponents
{
	public class SequentialAND : Nary
	{

		private int childrenIndex;

		public override GPCState Eval ()
		{
			GPCState state = children [childrenIndex].Eval ();
			if (state == GPCState.SUCCESS) {
				childrenIndex++;
			} else if (state == GPCState.FAILURE) {
				return GPCState.FAILURE;
			}
			if (childrenIndex >= children.Count) {
				return GPCState.SUCCESS;
			}
			return GPCState.RUNNING;
		}

		public override void Launch ()
		{
			childrenIndex = 0;
			if (children != null && children.Count > 0)
				children [childrenIndex].Launch ();
		}

		public override void Reset ()
		{
			for (int tempChildrenIndex = 0; tempChildrenIndex < childrenIndex; tempChildrenIndex++) {
				children [childrenIndex].Reset ();
			}
			childrenIndex = 0;
		}

		public override void Abort ()
		{
			for (int tempChildrenIndex = 0; tempChildrenIndex < childrenIndex; tempChildrenIndex++) {
				children [childrenIndex].Abort ();
			}
		}

	}
}
