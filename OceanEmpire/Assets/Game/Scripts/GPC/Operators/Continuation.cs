using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GPComponents
{
	public class Continuation : IGPComponent
	{
		private ContinuationFunction continuationFunction;

		public GPCState Eval ()
		{
			GPCState state = continuationFunction.Eval ();
			return state;
		}

		public void Launch ()
		{
			continuationFunction.Launch ();
		}

		public void Reset ()
		{
			continuationFunction.Reset ();
		}

		public void Abort ()
		{
			continuationFunction.Abort ();
		}
	}
}
