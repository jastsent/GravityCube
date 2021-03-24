using UnityEngine;

namespace JastSent {

	public class TrailHero : MonoBehaviour, IPoolable {

		public TrailRenderer[] trail;
		private float bufferTime = 0;
		private Timer timer;
		void Awake(){
			bufferTime = trail[0].time;
			timer = new Timer (bufferTime, false, true, SetTrailTime); 

		}
		public void OnSpawn(){
			EventManager.AddListener (Keys.Messages.PauseGame, Pause);
			EventManager.AddListener (Keys.Messages.PlayGame, Play);
		}

		public void OnDespawn(){
			EventManager.RemoveListener (Keys.Messages.PauseGame, Pause);
			EventManager.RemoveListener (Keys.Messages.PlayGame, Play);
			for (int i = 0; i < trail.Length; i++) {
				trail [i].time = bufferTime;
				trail [i].Clear ();
			}

			timer.Stop ();
			timer.SetTime (0);
		}
		void Play(){
			timer.Play ();
		}

		void Pause(){
			for(int i = 0; i < trail.Length; i++)
				trail[i].time = Mathf.Infinity;
			timer.Stop ();
			timer.SetTime (0);
		}

		public void SetTrailTime(){
			for(int i = 0; i < trail.Length; i++)
				trail[i].time = bufferTime;
		}
	}
}
