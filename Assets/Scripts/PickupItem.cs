using UnityEngine;

namespace JastSent {

	public class PickupItem : MonoBehaviour {
		public Color32 color;

		void OnTriggerEnter2D(Collider2D coll){
			if (coll.transform.CompareTag (Keys.Tags.HERO)) {
				ParticlePickup.nextColor = color;			
				Pool.Get (Keys.Pools.PickupHit).transform.position = transform.position;	
				SoundManager.PlaySound (Keys.Sound.Pickup);
			}
		}
	}
}
