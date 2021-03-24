using UnityEngine;
using UnityEngine.UI;

namespace JastSent{
    
    public class TrailsScrollView : MonoBehaviour {
        
    	
		public GameObject leftArrow;
		public GameObject rightArrow;

		public void ValueChanged (Scrollbar slider) {
			if (slider.value == 0f) {
				if (leftArrow.activeSelf)
					leftArrow.SetActive (false);
				if (!rightArrow.activeSelf)
					rightArrow.SetActive (true);
			} else if (slider.value == 1f) {
				if (!leftArrow.activeSelf)
					leftArrow.SetActive (true);
				if (rightArrow.activeSelf)
					rightArrow.SetActive (false);
			} else {
				if (!leftArrow.activeSelf)
					leftArrow.SetActive (true);
				if (!rightArrow.activeSelf)
					rightArrow.SetActive (true);
			}
    	}
    }
    
}