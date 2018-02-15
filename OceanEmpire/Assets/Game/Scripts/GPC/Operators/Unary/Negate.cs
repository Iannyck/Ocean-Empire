using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GPComponents
{
	public class Negate : Unary
	{
		public override GPCState Eval ()
		{
			GPCState state = child.Eval ();
			if (state == GPCState.FAILURE) {
				return GPCState.SUCCESS;
			} else if (state == GPCState.SUCCESS) {
				return GPCState.FAILURE;
			}
			return GPCState.RUNNING;
		}

		public override void Launch ()
		{
			child.Launch ();
		}

		public override void Reset ()
		{
			child.Reset ();
		}

		public override void Abort ()
		{
			child.Abort ();
		}
	}
}
