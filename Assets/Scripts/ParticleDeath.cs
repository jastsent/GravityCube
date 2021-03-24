using UnityEngine;

namespace JastSent {

	public class ParticleDeath : MonoBehaviour, IPoolable {
		//public bool dieOnStop = true;
		private ParticleSystem[] particles;
		private Timer timer;
		private bool slowMotion = false;
		void Awake () {
			particles = GetComponentsInChildren<ParticleSystem> (true);
			timer = new Timer (particles[0].main.duration, false, true, Stop);
		}

		public void OnSpawn(){
			EventManager.AddListener (Keys.Messages.PauseGame, Pause);
			EventManager.AddListener (Keys.Messages.PlayGame, Play);
			EventManager.AddListener (Keys.Messages.StopGame, Stop);
			EventManager.AddListener (Keys.Messages.OverSignal, DecraseSpeed);
			EventManager.AddListener (Keys.Messages.OverGame, Pause);
			particles[0].Play ();
			timer.Replay ();
		}

		public void OnDespawn(){
			EventManager.RemoveListener (Keys.Messages.PauseGame, Pause);
			EventManager.RemoveListener (Keys.Messages.PlayGame, Play);
			EventManager.RemoveListener (Keys.Messages.StopGame, Stop);
			EventManager.RemoveListener (Keys.Messages.OverSignal, DecraseSpeed);
			EventManager.RemoveListener (Keys.Messages.OverGame, Pause);
			if (slowMotion)
				IncreaseSpeed ();
			particles[0].Stop ();
			timer.Stop();
			timer.Reset ();
		}

		void Play(){
			particles[0].Play ();
			timer.Play ();
		}

		void Pause(){
			particles [0].Pause ();
			timer.Stop ();
		}

		void Stop(){
			if (slowMotion)
				IncreaseSpeed ();
			Pool.Return (gameObject);
		}

		void DecraseSpeed(){
			if (particles [0].isPaused)
				Play ();
			if (!slowMotion) {
				ParticleSystem.MainModule particleModule;
				foreach (ParticleSystem particle in  particles) {
					particleModule = particle.main;
					particleModule.simulationSpeed = 0.05f;
				}
				slowMotion = true;
			}

		}

		void IncreaseSpeed(){
			if (slowMotion) {
				ParticleSystem.MainModule particleModule; 
				foreach (ParticleSystem particle in  particles) {
					particleModule = particle.main;
					particleModule.simulationSpeed = 1;
				}
				slowMotion = false;
			}
		}

	}
}
