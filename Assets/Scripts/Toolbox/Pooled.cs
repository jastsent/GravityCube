using UnityEngine;

namespace JastSent {

	public class Pooled : MonoBehaviour {
		[HideInInspector]
		public int id = 0;

		[HideInInspector]
		public IPoolable[] poolables;

		void Awake(){
			poolables = GetComponentsInChildren<IPoolable> ();
		}
	}
}
