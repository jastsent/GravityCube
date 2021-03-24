using UnityEngine;

namespace JastSent {

	public class HeroControl : MonoBehaviour, IPoolable {
		
		public float power = 0f;
		public float speed = 0f;
		public float chargeSpeed = 0f;
		public float chargeTime = 0f;

		//cash
		private Rigidbody2D _rigidbody;
		private Transform _transform;

		//sprite animation
		public Animator spriteAnim;

		//charge options
		private Vector2 bufferVelocity;
		public bool charge = false;
		private float chargeTimer = 0f;

		//engine particle
		public ParticleSystem fire;
		public int birstCount = 0;
		public float birstSpeed = 0;
		private float bufferBirstSpeed = 0;
		private ParticleSystem.MainModule particleModule; 

		//particles
		public GameObject shieldParticle;

		//trail

		private GameObject trail;

		void Awake(){
			_transform = transform;
			_rigidbody = GetComponent<Rigidbody2D> ();
			particleModule = fire.main;
			bufferBirstSpeed = particleModule.startSpeed.constant;
		}

		public void OnSpawn(){
			EventManager.AddListener (Keys.Messages.Tap, Tap);
			EventManager.AddListener (Keys.Messages.PauseGame, Pause);
			EventManager.AddListener (Keys.Messages.PlayGame, Play);

			EventManager.AddListener (Keys.Messages.ShieldActivate, ShieldActive);
			EventManager.AddListener (Keys.Messages.ShieldDeactivate, ShieldDeactive);

			_rigidbody.velocity = _transform.up * speed;

			trail = Pool.Get (TrailManager.lastTrail, _transform);
			//trail.transform.SetParent (_transform);
		}

		public void OnDespawn(){
			EventManager.RemoveListener (Keys.Messages.Tap, Tap);
			EventManager.RemoveListener (Keys.Messages.PauseGame, Pause);
			EventManager.RemoveListener (Keys.Messages.PlayGame, Play);

			EventManager.RemoveListener (Keys.Messages.ShieldActivate, ShieldActive);
			EventManager.RemoveListener (Keys.Messages.ShieldDeactivate, ShieldDeactive);

			_rigidbody.constraints = RigidbodyConstraints2D.None;
			_rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;

			spriteAnim.speed = 1;

			particleModule.startSpeed = bufferBirstSpeed;

			ShieldDeactive ();

			Pool.Return (trail);
		}

		void Update () {
			if (!Game.pause) {	
				//rotation
				if (_rigidbody.velocity.magnitude != 0) {
					Vector2 dir = _rigidbody.velocity;
					float angle = Mathf.Atan2 (dir.y, dir.x) * Mathf.Rad2Deg + 270f;
					_transform.rotation = Quaternion.AngleAxis (angle, Vector3.forward);
				}

				if (charge) {
					chargeTimer += Time.deltaTime;
					//_rigidbody.AddRelativeForce (Vector2.up * 18 - _rigidbody.velocity);
					if (chargeTimer >= chargeTime) {
						charge = false;
						chargeTimer = 0;
						spriteAnim.speed = 1;
						//engine particle
						particleModule.startSpeed = bufferBirstSpeed;
						_rigidbody.velocity = _rigidbody.velocity.normalized * speed;
					} else
						_rigidbody.velocity = _rigidbody.velocity.normalized * chargeSpeed;
				} else
					//_rigidbody.AddRelativeForce (Vector2.up * 10 - _rigidbody.velocity);
					_rigidbody.velocity = _rigidbody.velocity.normalized * speed;
			}
		}

		void Tap(){
			if (Game.condition == 1) {
				_rigidbody.velocity = _transform.up * chargeSpeed;
				charge = true;
				chargeTimer = 0;
				spriteAnim.speed = 1.5f;
				//fire.Emit (birstCount);
				particleModule.startSpeed = birstSpeed;
			}
		}

		void Play(){
			_rigidbody.constraints = RigidbodyConstraints2D.None;
			_rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
			_rigidbody.velocity = bufferVelocity;

			if(charge)
				spriteAnim.speed = 1.5f;
			else
				spriteAnim.speed = 1;
		}

		void Pause(){
			bufferVelocity = _rigidbody.velocity;
			_rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;

			spriteAnim.speed = 0;
		}

		public void ShieldActive(){
			shieldParticle.SetActive (true);
		}
		public void ShieldDeactive(){
			shieldParticle.SetActive (false);
		}

		void OnCollisionEnter2D(Collision2D coll){
			if (ShieldManager.active) {
				if (coll.transform.CompareTag (Keys.Tags.WALL)) {
					//down wall
					if (coll.transform.position.y + coll.contacts [0].collider.offset.y < _transform.position.y) {

						CreateCollisionParticle (Keys.Pools.DeathHit, coll);
						GameOver ();

					} else {

						CreateCollisionParticle (Keys.Pools.CollisionHit, coll);

						SoundManager.PlaySound (Keys.Sound.WallStack);

						//left wall
						if (coll.transform.position.x + coll.contacts [0].collider.offset.x < _transform.position.x) {
							if (Game.condition == 1) {
								Vector2 v2Force = new Vector2 (0.1f, 0.9f);
								_rigidbody.velocity = v2Force * chargeSpeed;
								charge = true;
								chargeTimer = 0;
							}
						// righ wall
						} else if (coll.transform.position.x + coll.contacts [0].collider.offset.x > _transform.position.x) {
							if (Game.condition == 1) {
								Vector2 v2Force = new Vector2 (-0.1f, 0.9f) * power;
								_rigidbody.velocity = new Vector3 (0, 0, 0);
								_rigidbody.AddForce (v2Force, ForceMode2D.Impulse);
								charge = true;
								chargeTimer = 0;
							}
						}
					}
				}
			} else if(coll.transform.CompareTag (Keys.Tags.PLANET) || coll.transform.CompareTag (Keys.Tags.WALL)){

				CreateCollisionParticle (Keys.Pools.DeathHit, coll);
				GameOver ();

			}
		}

	//	void OnTriggerEnter2D(Collider2D coll){
	//		if (ShieldManager.active) {
	//			if (coll.CompareTag ("planet")) {
	//				coll.gameObject.GetComponent<Planet> ().Exterminatus ();
	//				/*if(charge)
	//					_rigidbody.velocity = _transform.up * chargeSpeed;
	//				else
	//					_rigidbody.velocity = _transform.up * speed;*/
	//
	//			} 
	//		} else if(coll.CompareTag ("planet")){
	//			/*Vector2 normalizedAngle = coll.contacts [0].normal;
	//			normalizedAngle.Normalize ();
	//			float anglee = Mathf.Atan2 (normalizedAngle.x, normalizedAngle.y) * Mathf.Rad2Deg;
	//			print(Quaternion.AngleAxis(anglee, Vector3.forward).eulerAngles);*/
	//
	//			EventManager.Trigger ("OverSignal");
	//		}
	//	}
		void CreateCollisionParticle(int particleId, Collision2D coll){
			Vector2 normalizedAngle = coll.contacts [0].normal;
			normalizedAngle.Normalize ();
			float anglee = Mathf.Atan2 (normalizedAngle.x, normalizedAngle.y) * Mathf.Rad2Deg;
			Transform deathParticle = Pool.Get (particleId).transform;
			deathParticle.position = new Vector3 (coll.contacts [0].point.x, coll.contacts [0].point.y, -0.1f);
			deathParticle.rotation = Quaternion.AngleAxis (anglee, Vector3.back);
		}

		void GameOver(){
			SoundManager.PlaySound (Keys.Sound.Crash);
			EventManager.Trigger (Keys.Messages.OverSignal);
		}
	}
}
