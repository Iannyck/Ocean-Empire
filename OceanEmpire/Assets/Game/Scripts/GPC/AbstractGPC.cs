using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GPComponents
{
	/// <summary>
	/// Abstract Gameplay Component. This class must be extended to create atomic gameplay components. 
	/// These components must represent the player's atomic objectives (for more details read the articles on gameplay components).
	/// </summary>
	public abstract class AbstractGPC : MonoBehaviour
	{

		public enum State
		{
			RUNNING,
			SUCCESS,
			FAILURE}

		;

		private State internalState;

		private bool isReady;
		private bool hasRewardBeenGiven;

		/// <summary>
		/// Gets the state.
		/// </summary>
		/// <value>The state.</value>
		public State InternalState {
			get {
				return this.internalState;
			}
		}

		// Use this for initialization
		void Start ()
		{
			Init ();
			InitComponent ();
		}
	
		// Update is called once per frame
		void Update ()
		{
			if (isReady) {
				if (internalState == State.RUNNING)
					internalState = ObjectiveState ();
				else if (!hasRewardBeenGiven) {
					hasRewardBeenGiven = Rewards ();
				}
			} else {
				isReady = InitScene ();
			}
		}

		/// <summary>
		/// Init this instance.
		/// </summary>
		private void Init ()
		{
			internalState = State.RUNNING;
			isReady = false;
			hasRewardBeenGiven = false;
		}

		/// <summary>
		/// Resets the component.
		/// </summary>
		public void ResetComponent ()
		{
			Init ();
			InitComponent ();
		}

		/// <summary>
		/// Inits the component.
		/// </summary>
		protected abstract void InitComponent ();

		/// <summary>
		/// Inits the scene.
		/// </summary>
		/// <returns><c>true</c>, if scene was inited, <c>false</c> otherwise.</returns>
		protected abstract bool InitScene ();

		/// <summary>
		/// State of the objective after evaluation.
		/// </summary>
		/// <returns>The state.</returns>
		protected abstract State ObjectiveState ();

		/// <summary>
		/// Awarding the reward or punishment according to the end state of the objective.
		/// </summary>
		/// <returns><c>true</c>, if reward/penalty has been given, <c>false</c> otherwise.</returns>
		protected abstract bool Rewards ();

		/// <summary>
		/// Abort this instance. Must eventually clean the scene of entities that are related only to this instance.
		/// </summary>
		protected abstract void Abort();


	}

}