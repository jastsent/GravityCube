using UnityEngine;
using UnityEngine.UI;
namespace JastSent{
    
    public class GameOverScreen : MonoBehaviour {
        
		public GameObject newRecordTxt;
		public Text highscoreNum;
		public Text scoreNum;
		public Text planetsNum;
		public Text coinsNum;

		void Awake() {
			highscoreNum.text = "0";
			scoreNum.text = "0";
			planetsNum.text = "0";

			if (newRecordTxt.activeSelf)
				newRecordTxt.SetActive (false);

			EventManager.AddListener (Keys.Messages.OverGame, OverGame);
			EventManager.AddListener <int>(Keys.Messages.ReachPoint, PlanetReach);
			EventManager.AddListener (Keys.Messages.PickUpCoin, CoinReach);
    	}

		int planets = 0;
		void PlanetReach(int r){
			planets++;
		}

		int coins = 0;
		void CoinReach(){
			coins++;
		}

		void OverGame(){
			highscoreNum.text = ScoreManager.highscore.ToString();
			scoreNum.text = ScoreManager.scores.ToString();

			planetsNum.text = planets.ToString ();
			planets = 0;

			coinsNum.text = coins.ToString ();
			coins = 0;

			if (ScoreManager.highscore < ScoreManager.scores) {
				if (!newRecordTxt.activeSelf)
					newRecordTxt.SetActive (true);
			} else {
				if(newRecordTxt.activeSelf)
					newRecordTxt.SetActive (false);
			}
		}
    }
    
}