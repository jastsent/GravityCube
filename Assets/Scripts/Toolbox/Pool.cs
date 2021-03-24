using System.Collections.Generic;
using UnityEngine;

namespace JastSent {

	public class Pool : MonoBehaviour {
		
		public Transform parent;

		[System.Serializable]
		public struct PoolPart {
			public GameObject prefab;
			public int count;
			[HideInInspector] public List<Pooled> objects;
		}

		[SerializeField]
		public PoolPart[] pools = new PoolPart[0];

		private static Pool instance = null;

		void Awake(){
			if (instance != null)
				Debug.LogError ("FATAL ERROR, DOUBLE POOL SCRIPT");
			else
				instance = this;

			if (parent == null) {
				parent = new GameObject ().transform;
				parent.name = "[POOL]";
			}
		}

		void Start () {	    	
			Init ();
		}

		private void Init(){
			GameObject obj = null;
			for (int i = 0; i < pools.Length; i++) {
				//create first
				obj = GameObject.Instantiate(pools [i].prefab, parent);
				obj.name = pools [i].prefab.name;
				Pooled _component = obj.AddComponent<Pooled> ();
				_component.id = i;
				pools [i].objects.Add (_component);

				// clone first to many
				for (int j = 1; j < pools [i].count ; j++) {
					obj = GameObject.Instantiate(obj, parent);
					obj.name = pools [i].prefab.name;
					pools [i].objects.Add (obj.GetComponent<Pooled>());
				}
				//deactivate all
				for (int j = 0; j < pools [i].objects.Count; j++) {
					pools [i].objects[j].gameObject.SetActive(false);
				}
			}
		}

		public static GameObject Get(int id)
		{
			Pooled result = null;
			if (id >= 0) {
				if (instance.pools [id].prefab != null) {
					if (instance.pools [id].objects.Count != 0) {
						result = instance.pools [id].objects [instance.pools [id].objects.Count - 1];
						result.gameObject.SetActive (true);
						instance.pools [id].objects.RemoveAt (instance.pools [id].objects.Count - 1);
					} else {
						//if nothing in stack
						result = GameObject.Instantiate (instance.pools [id].prefab, instance.parent).AddComponent<Pooled> ();
						result.gameObject.name = instance.pools [id].prefab.name;
						result.id = id;
					}
					//on spawn
					//IPoolable[] poolables = result.GetComponentsInChildren<IPoolable> ();
					for (int i = 0; i < result.poolables.Length; i++)	result.poolables [i].OnSpawn ();
				} else {
					Debug.LogError ("Pool: absent prefab " + id.ToString ());
				} 
			}
			return result.gameObject;
		}

		public static GameObject Get(int id, Transform parent)
		{
			Pooled result = null;
			if (id >= 0) {
				if (instance.pools [id].prefab != null) {
					if (instance.pools [id].objects.Count != 0) {
						result = instance.pools [id].objects [instance.pools [id].objects.Count - 1];
						result.transform.SetParent (parent);
						result.gameObject.SetActive (true);
						instance.pools [id].objects.RemoveAt (instance.pools [id].objects.Count - 1);
					} else {
						result = GameObject.Instantiate (instance.pools [id].prefab, parent).AddComponent<Pooled> ();
						result.gameObject.name = instance.pools [id].prefab.name;
						result.id = id;
					}
					//on spawn
					//IPoolable[] poolables = result.GetComponentsInChildren<IPoolable> ();
					for (int i = 0; i < result.poolables.Length; i++)	result.poolables [i].OnSpawn ();
				} else {
					Debug.LogError ("Pool: absent prefab " + id.ToString ());
				} 
			}
			return result.gameObject;
		}

		public static GameObject Get(int id, Vector3 position, Quaternion rotation)
		{
			Pooled result = null;
			if (id >= 0) {
				if (instance.pools [id].prefab != null) {
					if (instance.pools [id].objects.Count != 0) {
						result = instance.pools [id].objects [instance.pools [id].objects.Count - 1];
						result.transform.position = position;
						result.transform.rotation = rotation;
						result.gameObject.SetActive (true);
						instance.pools [id].objects.RemoveAt (instance.pools [id].objects.Count - 1);
					} else {
						result = GameObject.Instantiate (instance.pools [id].prefab, position, rotation, instance.parent).AddComponent<Pooled> ();
						result.gameObject.name = instance.pools [id].prefab.name;
						result.id = id;
					}
					//on spawn
					//IPoolable[] poolables = result.GetComponentsInChildren<IPoolable> ();
					for (int i = 0; i < result.poolables.Length; i++)	result.poolables [i].OnSpawn ();
				} else {
					Debug.LogError ("Pool: absent prefab " + id.ToString ());
				}
			}
			return result.gameObject;
		}

		public static void Return(GameObject obj){
			Pooled componentPool = obj.GetComponent<Pooled> ();
			if (componentPool != null) {			
				int id = componentPool.id;

				if (!instance.pools [id].objects.Contains (componentPool)) {
					//on despawn
					//IPoolable[] poolables = obj.GetComponentsInChildren<IPoolable> ();
					for (int i = 0; i < componentPool.poolables.Length; i++)
						componentPool.poolables [i].OnDespawn ();

					///
					if (instance.pools [id].objects.Count >= instance.pools [id].count) {
						Destroy (obj);

					} else {
						obj.SetActive (false);
						obj.transform.SetParent (instance.parent);
						obj.transform.position = instance.pools [id].prefab.transform.position;
						obj.transform.rotation = instance.pools [id].prefab.transform.rotation;
						obj.transform.localScale = instance.pools [id].prefab.transform.localScale;
						instance.pools [id].objects.Add (componentPool);
					}
				}
			}
		}

		public static void Return(Transform obj){
			Pooled componentPool = obj.GetComponent<Pooled> ();
			if (componentPool != null) {			
				int id = componentPool.id;

				if (!instance.pools [id].objects.Contains (componentPool)) {					
					//on despawn
					//IPoolable[] poolables = obj.gameObject.GetComponentsInChildren<IPoolable> ();
					for (int i = 0; i < componentPool.poolables.Length; i++)
						componentPool.poolables [i].OnDespawn ();

					///
					if (instance.pools [id].objects.Count >= instance.pools [id].count) {
						Destroy (obj.gameObject);

					} else {
						obj.gameObject.SetActive (false);
						obj.SetParent (instance.parent);
						obj.position = instance.pools [id].prefab.transform.position;
						obj.rotation = instance.pools [id].prefab.transform.rotation;
						obj.localScale = instance.pools [id].prefab.transform.localScale;
						instance.pools [id].objects.Add (componentPool);
					}
				}
			}
		}
	}
}