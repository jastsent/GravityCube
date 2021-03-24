using System.Collections.Generic;
using UnityEngine;

namespace JastSent {

	public class Magnet : MonoBehaviour, IPoolable {
		public CircleCollider2D triggerCollider;
		public SpriteRenderer _renderer;

		private float basicRadius = 0f;
		private Transform _transform;
		private static Transform heroTransform;
		private bool activated = false;

		private readonly List<Transform> coins = new List<Transform> ();

		private void Awake(){
			if (triggerCollider == null) {
				triggerCollider = gameObject.GetComponent<CircleCollider2D> ();
			}
			if (_renderer == null) {
				_renderer = gameObject.GetComponent<SpriteRenderer> ();
			}
			_transform = transform;
			basicRadius = triggerCollider.radius;
		}

		public void OnSpawn(){

		}

		public void OnDespawn(){
			if (!activated) return;
			if (triggerCollider.radius != basicRadius)
				triggerCollider.radius = basicRadius;
			if (!_renderer.enabled)
				_renderer.enabled = true;
			activated = false;
			coins.Clear ();
			MagnetManager.timer.SetTime(0f);
			MagnetManager.timer.Stop ();
			MagnetManager.active = false;
		}

		private void LateUpdate ()
		{
			if (Game.pause) return;
			if (!activated) return;
			_transform.position = heroTransform.position;
			if (coins.Count > 0) {
				float step = MagnetManager.instance.coinSpeed*Time.deltaTime;
				for (int i = coins.Count-1; i >= 0; i--) {
					if (coins[i].gameObject.activeSelf)
						coins[i].position = Vector3.MoveTowards (coins[i].position, heroTransform.position, step);
					else
						coins.RemoveAt (i);
				}
			} else if(coins.Count == 0 && MagnetManager.timer.end){
				gameObject.SetActive (false);
			}
		}

		private void OnTriggerEnter2D(Collider2D coll){
			if (coll.CompareTag (Keys.Tags.HERO) && !activated) {	
				if (MagnetManager.active) {
					MagnetManager.timer.Replay();
					gameObject.SetActive (false);
				} else {				
					heroTransform = coll.transform;
					triggerCollider.radius = MagnetManager.instance.radius;
					activated = true;
					MagnetManager.active = true;
					_renderer.enabled = false;
					MagnetManager.timer.SetTimes (0f,  MagnetManager.instance.durationPerLvl[MagnetManager.instance.lvl]);
					MagnetManager.timer.Play ();
				}
				EventManager.Trigger(Keys.Messages.PickupMagnet);
			} else if (coll.CompareTag (Keys.Tags.COIN) 
				&& activated 
				&& !MagnetManager.timer.end) {
				if (!coins.Contains (coll.transform)) {
					coins.Add (coll.transform);
				}
			}
		}
	}
}
