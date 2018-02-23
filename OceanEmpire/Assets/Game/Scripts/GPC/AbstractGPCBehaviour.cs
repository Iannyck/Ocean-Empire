using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GPComponents
{
	/// <summary>
	/// Abstract GPC behaviour. This class must be extended to create atomic gameplay components. 
	/// These components must represent the player's atomic objectives (for more details read the articles on gameplay components).
	/// </summary>
	public abstract class AbstractGPCBehaviour : IGPComponent
    {
        protected SceneManager sceneManager;

        public AbstractGPCBehaviour(SceneManager sceneManager)
        {
            this.sceneManager = sceneManager;
        }

        public abstract GPCState Eval ();

		public abstract void Launch ();

		public abstract void Reset ();

		public abstract void Abort ();
	}
}
