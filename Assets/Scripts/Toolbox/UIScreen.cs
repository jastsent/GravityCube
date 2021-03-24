using System.Collections.Generic;
using UnityEngine;

namespace JastSent {

	public class UIScreen : MonoBehaviour {
		public UIScreen backButtonSwitchScreen;
		public int backButtonMessage = -1;
		public int activateMessage = -1;
		public bool popup = false;
		private static List<UIScreen> activeScreens = new List<UIScreen>();

		void OnAwake(){

		}
		void OnEnable(){
			activeScreens.Add(this);
			EventManager.AddListener(Keys.Messages.Backbutton, BackButton);		
		}
		void OnDisable(){
			EventManager.RemoveListener(Keys.Messages.Backbutton, BackButton);
		}

		public void Switch(){
			if (popup) {//this screen popup
				/*if (activeScreens [activeScreens.Count - 1].popup) {//last screen popup
					if (gameObject.activeSelf) {
						int index = activeScreens.IndexOf (this);
						for (int i = activeScreens.Count - 1; i > index; i--) {
							activeScreens [i].gameObject.SetActive (false);
							activeScreens.RemoveAt (i);
						}
						EventManager.AddListener ("back button", BackButton);
					} else {//this screen popup wasn't active
						EventManager.RemoveListener("back button", activeScreens[activeScreens.Count-1].BackButton);
						gameObject.SetActive (true);
						activeScreens.Add(this);
						EventManager.AddListener("back button", BackButton);
					}
				} else {//last screen not popup
					EventManager.RemoveListener("back button", activeScreens[activeScreens.Count-1].BackButton);
					gameObject.SetActive (true);
					activeScreens.Add(this);
					EventManager.AddListener("back button", BackButton);
				}*/
			} else {//this screen not popup
				if (gameObject.activeSelf) {
					if (activeScreens.Count > 1){
						for (int i = 1; i < activeScreens.Count; i++) {
							activeScreens [i].gameObject.SetActive (false);
							activeScreens.RemoveAt (i);
						}
						EventManager.AddListener (Keys.Messages.Backbutton, BackButton);
					}
				} else {
					for (int i = 0; i < activeScreens.Count; i++) {
						activeScreens [i].gameObject.SetActive (false);
					}
					activeScreens.Clear ();
					gameObject.SetActive (true);
				}
			}
		}

		public void Switch(int message){
			if (popup) {//this screen popup
				/*if (activeScreens [activeScreens.Count - 1].popup) {//last screen popup
					if (gameObject.activeSelf) {//this screen popup was active 
						int index = activeScreens.IndexOf (this);
						for (int i = activeScreens.Count - 1; i > index; i--) {
							activeScreens [i].gameObject.SetActive (false);
							activeScreens.RemoveAt (i);
						}
						EventManager.AddListener ("back button", BackButton);
						if (message != "") {
							//EventManager.Trigger (globalMessage);
							EventManager.Trigger (message);
						}
					} else {//this screen popup wasn't active
						//globalMessage = message;
						EventManager.RemoveListener("back button", activeScreens[activeScreens.Count-1].BackButton);
						gameObject.SetActive (true);
						activeScreens.Add(this);
						EventManager.AddListener("back button", BackButton);
						EventManager.Trigger (message);
					}
				} else {//last screen not popup
					//globalMessage = message;
					EventManager.RemoveListener("back button", activeScreens[activeScreens.Count-1].BackButton);
					gameObject.SetActive (true);
					activeScreens.Add(this);
					EventManager.AddListener("back button", BackButton);
					EventManager.Trigger (message);
				}*/
			} else {//this screnn not popup
				if (gameObject.activeSelf) {
					if (activeScreens.Count > 1) {
						for (int i = 1; i < activeScreens.Count; i++) {
							activeScreens [i].gameObject.SetActive (false);
							activeScreens.RemoveAt (i);
						}
						EventManager.AddListener (Keys.Messages.Backbutton, BackButton);
						if(message >= 0)
							EventManager.Trigger (message);
					}
				} else {
					for (int i = 0; i < activeScreens.Count; i++) {
						activeScreens [i].gameObject.SetActive (false);
					}
					activeScreens.Clear();
					gameObject.SetActive (true);
					EventManager.Trigger (message);
				}
			}
		}

		public void Disable(){
			if (popup) {//this screen popup
				/*if (activeScreens [activeScreens.Count - 1].popup) {//last screen popup
					if (gameObject.activeSelf) {
						int index = activeScreens.IndexOf (this);
						for (int i = activeScreens.Count - 1; i > index; i--) {
							activeScreens [i].gameObject.SetActive (false);
							activeScreens.RemoveAt (i);
						}
						EventManager.AddListener ("back button", BackButton);
					} else {//this screen popup wasn't active
						EventManager.RemoveListener("back button", activeScreens[activeScreens.Count-1].BackButton);
						gameObject.SetActive (true);
						activeScreens.Add(this);
						EventManager.AddListener("back button", BackButton);
					}
				} else {//last screen not popup
					EventManager.RemoveListener("back button", activeScreens[activeScreens.Count-1].BackButton);
					gameObject.SetActive (true);
					activeScreens.Add(this);
					EventManager.AddListener("back button", BackButton);
				}*/
			} else {//this screen not popup
				if (gameObject.activeSelf) {
					for (int i = 0; i < activeScreens.Count; i++) {
						activeScreens [i].gameObject.SetActive (false);
						activeScreens.RemoveAt (i);
					}
				}
			}
		}

		public void BackButton(){
			if (backButtonSwitchScreen != null) {
				if (backButtonMessage >= 0) 
					backButtonSwitchScreen.Switch (backButtonMessage);
				else 
					backButtonSwitchScreen.Switch ();
			}
		}
	}
}
