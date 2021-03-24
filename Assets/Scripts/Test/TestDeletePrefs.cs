using UnityEngine;

namespace JastSent{
    
    public class TestDeletePrefs : MonoBehaviour {
        
    	
		public void OnClick(){
			PlayerPrefs.DeleteAll ();
		}
    }
    
}