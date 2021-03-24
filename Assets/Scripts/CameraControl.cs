using UnityEngine;

namespace JastSent {

	public class CameraControl : MonoBehaviour, IPoolable {
		public Vector3 resetPosition;
		public float offsetY = 0;
		private Transform _transform;
		private Transform _cameraTransform;
		private float highPosition = 0;

		void Awake () {
			_transform = transform;
			_cameraTransform = Camera.main.transform;
			highPosition = offsetY;
		}	

		public void OnSpawn(){}

		public void OnDespawn(){
			if (_cameraTransform != null)
				_cameraTransform.position = resetPosition;
			
			highPosition = offsetY;
		}

		void Update () {
			if (_transform.position.y > highPosition) {
				highPosition = _transform.position.y;
				_cameraTransform.position = new Vector3 (
					_cameraTransform.position.x,
					_transform.position.y - offsetY,
					_cameraTransform.position.z);
				EventManager.Trigger (Keys.Messages.CameraMoved);		
			}
		}
	}

}
