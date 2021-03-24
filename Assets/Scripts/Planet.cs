using UnityEngine;

namespace JastSent {

	public class Planet : MonoBehaviour, IPoolable {
		public Transform planet;
		public Transform gravity;
		public SpriteRenderer earthRend;
		public SpriteRenderer decalRend;
		public SpriteRenderer additionRend;
		public Collider2D _collider;

		public void OnSpawn(){
			EventManager.AddListener (Keys.Messages.ShieldActivate, SetColliderTrigger);
			EventManager.AddListener (Keys.Messages.ShieldDeactivate, SetColliderNoTrigger);

			PlanetManager.instance.SetSpritePlanet(earthRend);
			//decal
			PlanetManager.instance.SetSpriteDecal(decalRend);
			//additional
			PlanetManager.instance.SetSpriteAddition(additionRend);
			//size
			float newSize = Random.Range (PlanetManager.instance.minPlanetSize, PlanetManager.instance.maxPlanetSize);
			planet.localScale = new Vector3 (newSize, newSize, planet.localScale.z);
			newSize = Random.Range (PlanetManager.instance.minGravitySize, PlanetManager.instance.maxGravitySize);
			gravity.localScale = new Vector3 (newSize, newSize, gravity.localScale.z);

			if(!transform.parent.gameObject.activeSelf)
				transform.parent.gameObject.SetActive (true);

			_collider.isTrigger = ShieldManager.active;
		}

		public void OnDespawn(){
			EventManager.RemoveListener (Keys.Messages.ShieldActivate, SetColliderTrigger);
			EventManager.RemoveListener (Keys.Messages.ShieldDeactivate, SetColliderNoTrigger);

			decalRend.transform.rotation = Quaternion.identity;
			additionRend.transform.rotation = Quaternion.identity;
			earthRend.transform.rotation = Quaternion.identity;

			_collider.isTrigger = false;
		}

		void SetColliderTrigger(){
			_collider.isTrigger = true;
		}

		void SetColliderNoTrigger(){
			_collider.isTrigger = false;
		}

		void OnTriggerEnter2D(Collider2D coll){
			if (ShieldManager.active) {
				if (coll.CompareTag (Keys.Tags.HERO))
					Exterminatus ();
			}
		}

		public void Exterminatus(){
			transform.parent.parent.gameObject.SetActive (false);
			Transform explousion = Pool.Get (Keys.Pools.PlanetDestroy).transform;
			explousion.position = new Vector3
				(planet.position.x+explousion.position.x,
					planet.position.y+explousion.position.y,
					planet.position.z+explousion.position.z);
			SoundManager.PlaySound (Keys.Sound.Boom);
		}
	}
}
