using UnityEngine;
using UnityEngine.UI;
namespace JastSent{
    
    public class MuteButton : MonoBehaviour {
		public bool mutedOnStart = false;
		public Color onColor;
		public Color offColor;
		public Image buttonImg;
		void Start(){
			if (mutedOnStart) {
				buttonImg.color = offColor;
				SoundManager.mute = true;
			}
		}

		public void OnClick(){
			if (SoundManager.mute) {
				buttonImg.color = onColor;
				SoundManager.mute = false;
			} else {
				buttonImg.color = offColor;
				SoundManager.mute = true;
			}
		}
    }
    
}