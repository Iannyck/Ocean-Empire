using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GPComponents
{
	/// <summary>
	/// GamePlay component interface.
	/// </summary>
	public interface IGPComponent {

		/// <summary>
		/// Evaluates the status of this instance's objective
		/// </summary>
		GPCState Eval();

		/// <summary>
		/// Launch this instance.
		/// </summary>
		void Launch();

		/// <summary>
		/// Reset this instance.
		/// </summary>
		void Reset();

		/// <summary>
		/// Abort this instance.
		/// </summary>
		void Abort();

	}

	public enum GPCState {
		RUNNING,
		SUCCESS,
		FAILURE
	};
}
