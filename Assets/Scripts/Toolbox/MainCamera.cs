using UnityEngine;

namespace JastSent{
    
    public class MainCamera : MonoBehaviour {
        
		public static Camera _camera;

	    private void Awake(){
			_camera = Camera.main;
		}
    }
    
}