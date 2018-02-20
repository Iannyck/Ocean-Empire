using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GPComponents
{
	public abstract class SceneManager : MonoBehaviour
	{

		// Use this for initialization
		void Start ()
		{
		
		}
	
		// Update is called once per frame
		void Update ()
		{
		
		}
			
		public abstract void Create (string tag, ArrayList parameters);

		public abstract void Read (string tag);

		public abstract void Update (string tag, ArrayList parameters);

		public abstract void Delete (string tag, ArrayList parameters);

	}
}

