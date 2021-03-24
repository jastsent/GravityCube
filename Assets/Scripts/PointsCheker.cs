using System.Collections.Generic;
using UnityEngine;

namespace JastSent {

	public class PointsCheker : MonoBehaviour, IPoolable {
		public int pointsReward = 0;

		private List<float> chekpoints = new List<float> ();
		private Transform _transform;

		void Awake(){
			_transform = transform;
		}

		public void OnSpawn(){
			EventManager.AddListener<Transform> (Keys.Messages.PlanetCreated, PlanetCreated);
		}

		public void OnDespawn(){
			EventManager.RemoveListener<Transform> (Keys.Messages.PlanetCreated, PlanetCreated);
			chekpoints.Clear ();
		}

		void PlanetCreated(Transform planetObj){
			chekpoints.Add (planetObj.position.y);
		}

		void LateUpdate () {
			if (!Game.pause) {			
				if (chekpoints.Count>0 && _transform.position.y > chekpoints [0]) {
					EventManager.Trigger <int>(Keys.Messages.ReachPoint, pointsReward);
					ScoreManager.ChangeScores(pointsReward);
					chekpoints.RemoveAt (0);
				}
			}
		}
	}
}
