using UnityEngine;

namespace JastSent {

	public class ParticlePickup : MonoBehaviour, IPoolable {
		public ParticleSystem particle;
		public static Color32 nextColor;

		public void OnSpawn(){
			ParticleSystem.MainModule particleModule = particle.main; 
			ParticleSystem.MinMaxGradient particleColor = particleModule.startColor;
			particleColor.color = nextColor;
			particleModule.startColor = particleColor;
		}

		public void OnDespawn(){

		}
	}
}
