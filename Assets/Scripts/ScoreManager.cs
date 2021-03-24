using UnityEngine;

namespace JastSent {

	public class ScoreManager : MonoBehaviour {
		public bool clearOnStart = false;
		public int coinsOnStart = 0;
		public static int coins = 0;
		public static int scores = 0;
		public static int highscore = 0;

		void Awake () {
			#if UNITY_EDITOR
			if(clearOnStart) PlayerPrefsEncrypt.DeleteAll ();
			#endif
			/*if (PlayerPrefsEncrypt.HasKey ("coins")) {
				coins = PlayerPrefsEncrypt.GetInt ("coins", 0);
			} else {
				PlayerPrefsEncrypt.SetInt ("coins", 0);
				coins = 0;
			}

			if (PlayerPrefsEncrypt.HasKey ("highscore")) {
				highscore = PlayerPrefsEncrypt.GetInt ("highscore", 0);
			} else {
				PlayerPrefsEncrypt.SetInt ("highscore", 0);
				highscore = 0;
			}*/

			coins = PlayerPrefsEncrypt.GetInt (Keys.Prefs.COINS, 0);
			#if UNITY_EDITOR
			if(coinsOnStart != 0) coins += coinsOnStart;
			#endif

			highscore = PlayerPrefsEncrypt.GetInt (Keys.Prefs.HIGHSCORE, 0);

			EventManager.AddListener (Keys.Messages.GoGame, Begin);
			EventManager.AddListener (Keys.Messages.StopGame, Stop);
		}

		void Start(){
			EventManager.Trigger<int>(Keys.Messages.Coinschange, coins);
			EventManager.Trigger<int>(Keys.Messages.Highscorechange, highscore);
		}
		public void Begin(){
			scores = 0;
			EventManager.Trigger<int>(Keys.Messages.Scoreschange, scores);
		}
		public void Stop(){
			if (scores > highscore) {
				highscore = scores;
				PlayerPrefsEncrypt.SetInt (Keys.Prefs.HIGHSCORE, highscore);
				EventManager.Trigger<int>(Keys.Messages.Highscorechange, highscore);
			}
			//EventManager.Trigger<int>("final score", scores);
			//scores = 0;
			//EventManager.Trigger<int>("scores change", scores);
			PlayerPrefsEncrypt.SetInt (Keys.Prefs.COINS, coins);
			PlayerPrefsEncrypt.Save ();
		}

		public static void ChangeCoins(int value){
			coins += value;
			EventManager.Trigger<int> (Keys.Messages.Coinschange, coins);
		}
		public static void ChangeScores(int value){
			scores += value;
			EventManager.Trigger<int>(Keys.Messages.Scoreschange, scores);
		}
		public static void ChangeHighscore(int value){
			highscore += value;
			EventManager.Trigger<int>(Keys.Messages.Highscorechange, highscore);
		}

		public static bool Buy(int price){
				if (coins >= price && price >= 0) {
				ChangeCoins (-price);
				PlayerPrefsEncrypt.SetInt (Keys.Prefs.COINS, coins);
				return true;
			} else
				return false;
		}

		void OnApplicationQuit(){
			PlayerPrefsEncrypt.SetInt (Keys.Prefs.COINS, coins);
			PlayerPrefsEncrypt.SetInt (Keys.Prefs.HIGHSCORE, highscore);
		}
	}
}