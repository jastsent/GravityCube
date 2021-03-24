using UnityEngine;

namespace JastSent {

public class RotateAround : MonoBehaviour {
	public float speed = 0;
	public bool rightDirection = false;
	public bool randomDirection = false;
	private int side = 1;
	private Transform _transform;
	void Awake(){
		_transform = transform;

		if (randomDirection) {
			side = Random.Range (-1, 1);
		} else if (rightDirection) {
			side = -1;
		} else {
			side = 1;
		}
	}

	void Update () {
		if (!Game.pause) {
			_transform.Rotate (0, 0, speed * Time.deltaTime * side);
		}
	}
}
}
