using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GPComponents
{
	/// <summary>
	/// Ignore the end state of your child by always returning true when the child reaches it.
	/// </summary>
	public class Repeat : Unary
	{

		private int iterations;
		private int count;

		public Repeat (IGPComponent child, int iterations) : base(child)
		{
			this.iterations = iterations;
			count = 0;
		}

        public override GPCState Eval ()
		{
			
			GPCState state = child.Eval ();
			if (state != GPCState.RUNNING) {
				count++;
				if (count >= iterations) {
					return GPCState.SUCCESS;
				} else {
					child.Reset ();
				}
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
