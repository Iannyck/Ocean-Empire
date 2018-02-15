using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GPComponents
{
	public class ParallelOR : Nary
	{

		private List<IGPComponent> childrenCopy;

		public override GPCState Eval ()
		{

			GPCState state;
			foreach (IGPComponent gComponent in children) {
				state = gComponent.Eval ();
				if (state == GPCState.FAILURE) {
					childrenCopy.Remove (gComponent);
				} else if (state == GPCState.SUCCESS) {
					return GPCState.SUCCESS;
				}
			}
			if (childrenCopy.Count == 0) {
				return GPCState.FAILURE;
			}
			return GPCState.RUNNING;
		}

		public override void Launch ()
		{
			foreach (IGPComponent gComponent in children) {
				gComponent.Launch ();
			}
			childrenCopy = children;
		}

		public override void Reset ()
		{
			foreach (IGPComponent gComponent in children) {
				gComponent.Reset ();
			}
			childrenCopy = children;
		}

		public override void Abort ()
		{
			foreach (IGPComponent gComponent in children) {
				gComponent.Abort ();
			}
		}

	}
}
