using System.Collections.Generic;
using UnityEngine;
using JastSent.Keys;

namespace JastSent {
	
	public class BackgroundSpace : MonoBehaviour {
		public GameObject menuParticle;
		public float maxSpeedOffset = 0;

		public float minSize = 0;
		public float maxSize = 0;
		public float step = 0;
		public int startNum = 0;

		private Transform cameraTransform;
		private Vector3 lastPosition = new Vector3();
		private float countYpos = 0;

		private Transform _transform;

		private struct StarData {
			public Transform transform;
			public float speedOffset;
		}
		private List<StarData> stars;

		void Awake(){
			_transform = transform;
			cameraTransform = Camera.main.transform;
			stars = new List<StarData>();

			EventManager.AddListener (Keys.Messages.GoGame, Go);
			EventManager.AddListener (Keys.Messages.StopGame, Stop);
		}

		void OnCameraMove () {
			Vector3 deltaPosition = new Vector3(0,0,0);
			deltaPosition = cameraTransform.position - lastPosition;
			lastPosition = cameraTransform.position;

			for (int i = stars.Count-1; i >= 0; i--) {
				if (stars [i].transform.position.y < ScreenSize.down || stars [i].transform.position.y > ScreenSize.up) {
					Pool.Return (stars [i].transform);
					stars.RemoveAt (i);
				} else {
					stars [i].transform.Translate (new Vector3 (0, -deltaPosition.y*stars [i].speedOffset, 0));
				}
			}

			countYpos += deltaPosition.y;
			if (countYpos > 0) {	
				while ((countYpos - step) > 0) {
					CreateStar (new Vector2
						(Random.Range(ScreenSize.left, ScreenSize.right), 
						ScreenSize.up-countYpos+step));
					countYpos -= step;
				}
			} else if (countYpos < 0) {
				while ((countYpos + step) < 0) {
					CreateStar (new Vector2
						(Random.Range(ScreenSize.left, ScreenSize.right), 
						ScreenSize.up+countYpos-step));
					countYpos += step;
				}
			}
		}

		void Go(){
			EventManager.AddListener (Keys.Messages.CameraMoved, OnCameraMove);
			EventManager.AddListener (Keys.Messages.AspectRationChanged, Resize);

			lastPosition = cameraTransform.position;
			countYpos = 0;

			menuParticle.SetActive (false);
			for (int i = 0; i < startNum; i++) {
				CreateStar (new Vector2
					(Random.Range(ScreenSize.left, ScreenSize.right), 
					Random.Range(ScreenSize.down, ScreenSize.up)));
			}
		}

		void Stop(){
			EventManager.RemoveListener (Keys.Messages.CameraMoved, OnCameraMove);
			EventManager.RemoveListener (Keys.Messages.AspectRationChanged, Resize);
			for (int i = stars.Count-1; i >= 0; i--) {
				Pool.Return (stars [i].transform);
				stars.RemoveAt (i);
			}
			menuParticle.SetActive (true);
		}

		void Resize(){
			for (int i = stars.Count-1; i >= 0; i--) {
				stars [i].transform.position = new Vector2 
					(Random.Range (ScreenSize.left, ScreenSize.right), 
					Random.Range (ScreenSize.down, ScreenSize.up));
			}
		}

		void CreateStar(Vector2 position){
			Transform star = Pool.Get (Pools.Flayer).transform;
			star.SetParent (_transform);
			star.position = position;
			//star.localRotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(0f, 360f)) );

			float scale = Random.Range (minSize, maxSize);
			star.localScale = new Vector2 (scale, scale);

			StarData starData = new StarData ();
			starData.transform = star;
			starData.speedOffset = Random.Range (maxSpeedOffset, 1);
			stars.Add(starData);
		}
	}

}
