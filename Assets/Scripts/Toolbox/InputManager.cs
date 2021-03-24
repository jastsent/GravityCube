using UnityEngine;

namespace JastSent {
	public class InputManager : MonoBehaviour {
			
		void Update () {
			/*#if UNITY_EDITOR
			if(Input.GetMouseButtonDown(0)){
				EventManager.Trigger("tap");
			}
			#elif (UNITY_ANDROID || UNITY_IOS)
			if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) {
				EventManager.Trigger("tap");
			}
			#endif*/

			if (Input.GetKeyDown(KeyCode.Escape))
			{
				EventManager.Trigger (Keys.Messages.Backbutton);
			}
		}
	}
}
