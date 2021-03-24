using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JastSent {

	public class InteractiveObject : MonoBehaviour {

		public bool OnCollision = false;
		public List<string> collisionTags;
		public List<UnityEvent> collisionEvents;

		public bool OnTrigger = false;
		public List<string> triggerTags;
		public List<UnityEvent> triggerEvents;

		void OnCollisionEnter2D(Collision2D coll){
			if (OnCollision) {
				for (int i = 0; i < collisionTags.Count; i++) {
					if(coll.gameObject.CompareTag(collisionTags[i])){
						collisionEvents [i].Invoke ();
					}
				}
			}		
		}

		void OnTriggerEnter2D(Collider2D coll){
			if (OnTrigger) {
				for (int i = 0; i < triggerTags.Count; i++) {
					if(coll.gameObject.CompareTag(triggerTags[i])){
						triggerEvents [i].Invoke ();
					}
				}
			}	
		}

		public void TriggerMessage(int message){
			EventManager.Trigger (message);
		}
	}

}
