using UnityEngine;

namespace JastSent {

	public class Borders : MonoBehaviour {	
		private BoxCollider2D leftCollider;
		private BoxCollider2D rightCollider;
		private BoxCollider2D floorCollider;
		void Awake(){
			EventManager.AddListener (Keys.Messages.AspectRationChanged, NewAspect);
		}
		void Start(){
			leftCollider = gameObject.AddComponent<BoxCollider2D> ();
			rightCollider = gameObject.AddComponent<BoxCollider2D> ();
			floorCollider = gameObject.AddComponent<BoxCollider2D> ();

			NewAspect ();
		}

		void NewAspect(){
			leftCollider.size = new Vector2 (2f, ScreenSize.height);
			rightCollider.size = new Vector2 (2f, ScreenSize.height);
			floorCollider.size = new Vector2 (ScreenSize.width, 2f);

			leftCollider.offset = new Vector2 (-ScreenSize.width/2-1f, 0);
			rightCollider.offset = new Vector2 (ScreenSize.width/2+1f, 0);
			floorCollider.offset = new Vector2 (0, -ScreenSize.height/2-1f);
		}
	}

}
