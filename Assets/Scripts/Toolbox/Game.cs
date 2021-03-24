using UnityEngine;

namespace JastSent {

	public class Game : MonoBehaviour {	
		public float delayGameOver = 0f;

		public static bool pause = true;
		public static int condition = 0; //0-menu, 1 - game, 2 - paused, 3 - gameover

		void Awake () {
			Application.targetFrameRate = 60;
			Input.multiTouchEnabled = false;
			
			EventManager.AddListener (Keys.Messages.GoSignal, Begin);
			EventManager.AddListener (Keys.Messages.StopSignal, Stop);
			EventManager.AddListener (Keys.Messages.PauseSignal, Pause);
			EventManager.AddListener (Keys.Messages.PlaySignal, Play);
			EventManager.AddListener (Keys.Messages.OverSignal, OverSignal);
		}
		
		void Update () {
			EventManager.Trigger (Keys.Messages.Update);
		}
		void LateUpdate () {
			EventManager.Trigger (Keys.Messages.LateUpdate);
		}

		public void Begin(){
			if (condition == 3) {
				EventManager.Trigger(Keys.Messages.StopGame);
				EventManager.Trigger(Keys.Messages.GoGame);
			} else {
				EventManager.Trigger(Keys.Messages.GoGame);
			}
			pause = false;
			condition = 1;
		}
		public void Stop(){
			EventManager.Trigger(Keys.Messages.StopGame);
			pause = true;
			condition = 0;
		}
		public void Play(){
			EventManager.Trigger(Keys.Messages.PlayGame);
			pause = false;
			condition = 1;
		}
		public void Pause(){
			EventManager.Trigger(Keys.Messages.PauseGame);
			pause = true;
			condition = 2;
		}

		public void OverSignal(){
			EventManager.Trigger(Keys.Messages.PauseGame);
			pause = true;
			condition = 3;
			new Timer (delayGameOver, true, false, GameOver);
		}

		public void GameOver(){
			EventManager.Trigger(Keys.Messages.OverGame);
		}

		public void Exit(){
			EventManager.Trigger (Keys.Messages.Exit);
			Application.Quit ();
		}
	}
}