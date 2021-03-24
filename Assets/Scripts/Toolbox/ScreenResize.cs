using UnityEngine;

namespace JastSent {

	public class ScreenResize : MonoBehaviour {
		public Camera[] addCameras;
		public Vector2 referenceSize;
		private float bufferAspect = 0f;

		void Awake(){
			bufferAspect = Camera.main.aspect;
			Resize ();
		}

		void Update () {
			if (bufferAspect != MainCamera._camera.aspect) {
				Resize ();
				EventManager.Trigger (Keys.Messages.AspectRationChanged);
				bufferAspect = MainCamera._camera.aspect;
			}
		}

		void Resize(){
			float targetW = referenceSize.x / MainCamera._camera.aspect;
			float percentDifference = 0;
			if (referenceSize.y  >= targetW) {
				//percentDifference = ((referenceSize.y - Camera.main.orthographicSize) /(Camera.main.orthographicSize / 100))/100;
				percentDifference = (referenceSize.y / (MainCamera._camera.orthographicSize / 100))/100;
				MainCamera._camera.orthographicSize = referenceSize.y;
			} else {
				percentDifference = (targetW / (MainCamera._camera.orthographicSize / 100))/100;
				//percentDifference = ((targetW - Camera.main.orthographicSize) /(Camera.main.orthographicSize / 100))/100;
				MainCamera._camera.orthographicSize = targetW;
			}
			for (int i = 0; i < addCameras.Length; i++) {
				if (addCameras [i].orthographic) {
					addCameras [i].orthographicSize *= percentDifference; 
				} else {
					/*if (Camera.main.aspect >= 1)// wide
						addCameras [i].fieldOfView = 78;
					
					else if (Camera.main.aspect >= 0.75)// 3:4
						addCameras [i].fieldOfView = 78;
					
					else if (Camera.main.aspect > 0.66)// 2:3
						addCameras [i].fieldOfView = 78;
					
					else if (Camera.main.aspect >= 0.625)// 10:16
						addCameras [i].fieldOfView = 78;
					
					else if (Camera.main.aspect >= 0.6)// 3:5
						addCameras [i].fieldOfView = 81.3f;
					
					else if (Camera.main.aspect >= 0.5625)// 9:16
						addCameras [i].fieldOfView = 85;
					
					else if (Camera.main.aspect > 0.53)// 100:186 (1.86)
						addCameras [i].fieldOfView = 87.6f;

					else if (Camera.main.aspect >= 0.5)// 1:2
						addCameras [i].fieldOfView = 91.8f;

					else if (Camera.main.aspect > 0.47)// 10:21
						addCameras [i].fieldOfView = 94.7f;*/
					
					
					//addCameras [i].fieldOfView += Mathf.Round(addCameras [i].fieldOfView*percentDifference*0.7f); 
					//print (Camera.main.aspect);
				}
			}
		}
	}
}
