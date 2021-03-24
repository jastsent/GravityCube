using UnityEngine;
using UnityEngine.UI;

namespace JastSent {
	public class UIManager : MonoBehaviour {
		public Text score_txt;
		public Text highscore_txt;
		public Text[] coins_txt;
		public UIScreen pauseScreen;
		public UIScreen gameScreen;
		public UIScreen gameOverScreen;
		public UIScreen errorScreen;


		public Text logText;
		void Awake () {
			Application.logMessageReceived += ErrorLog;
			EventManager.AddListener (Keys.Messages.OverGame, GameOver);
			EventManager.AddListener (Keys.Messages.OverSignal, GameOverSignal);
			EventManager.AddListener<int> (Keys.Messages.Scoreschange, SetScores);
			EventManager.AddListener<int> (Keys.Messages.Highscorechange, SetHighscore);
			EventManager.AddListener<int> (Keys.Messages.Coinschange, SetCoins);
		}

		void SetHighscore(int highscore){
			if (highscore < 10) {
				highscore_txt.text = "00000" + highscore.ToString ();
			} else if (highscore > 9 && highscore < 100) {
				highscore_txt.text = "0000" + highscore.ToString ();
			} else if (highscore > 99 && highscore < 1000) {
				highscore_txt.text = "000" + highscore.ToString ();
			} else if (highscore > 999 && highscore < 10000) {
				highscore_txt.text = "00" + highscore.ToString ();
			} else if (highscore > 9999 && highscore < 100000) {
				highscore_txt.text = "0" + highscore.ToString ();
			} else {
				highscore_txt.text = highscore.ToString ();
			}
		}

		void SetScores(int scores){
			score_txt.text = scores.ToString ();
		}

		void SetCoins(int coins){
			for (int i = 0; i < coins_txt.Length; i++) {
				coins_txt [i].text = coins.ToString ();
			}
		}

		void GameOverSignal(){
			gameScreen.Disable ();
		}

		void GameOver(){
			gameOverScreen.Switch ();
		}

		void ErrorLog(string logString, string stackTrace, LogType type){
			if (type == LogType.Error || type == LogType.Assert || type == LogType.Exception) {
				if (Game.condition == 1)
					EventManager.Trigger(Keys.Messages.PauseSignal);
				errorScreen.Switch ();
				logText.text = logString;
				EventManager.Trigger(Keys.Messages.Error);
			}
		}

		void OnApplicationPause(){
			if (Game.condition == 1) {
				pauseScreen.Switch (Keys.Messages.PauseSignal);
			}
		}

		public void Tap(){
			EventManager.Trigger (Keys.Messages.Tap);
		}
	}
}
