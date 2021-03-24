using UnityEngine;
using JastSent.Keys;

namespace JastSent {

	public class MultiscoreManager : MonoBehaviour {
		public float[] durationPerLvl;
		public float duration {
			get{
				return durationPerLvl [lvl];
			}
		}

		public int[] lvlPrices;// на 1 меньше чем durationPerLvl
		public int price {
			get{
				if (lvlPrices.Length > lvl)
					return lvlPrices [lvl];
				else
					return -1;
			}
		}

		public int factor = 0;
		[HideInInspector] public int lvl = 0;

		public static bool active = false;
		public static Timer timer = null;


		public static MultiscoreManager instance;

		void Awake(){
			if (instance != null)
				Debug.LogError ("FATAL ERROR, DOUBLE MULTISCORE MANAGER SCRIPT");
			else
				instance = this;

			lvl = PlayerPrefsEncrypt.GetInt (Prefs.MULTISCORE_LVL, 0);

			EventManager.AddListener (Keys.Messages.PickupMultiscore, Pickup);

			timer = new Timer(durationPerLvl [lvl], false, true, End);
		}

		public void Pickup(){
			if (active) {
				timer.Replay();
			} else {
				active = true;
				timer.SetTimes (0f, durationPerLvl [lvl]);
				timer.Play ();
				EventManager.AddListener (Keys.Messages.StopGame, End);
				EventManager.AddListener<int> (Keys.Messages.ReachPoint, Points);
			}
		}
		public void End(){
			active = false;
			timer.Stop();
			EventManager.RemoveListener (Keys.Messages.StopGame, End);
			EventManager.RemoveListener<int> (Keys.Messages.ReachPoint, Points);
		}

		void Points(int pointsNum){
			ScoreManager.ChangeScores(pointsNum*MultiscoreManager.instance.factor-pointsNum);
		}

		public bool Upgrade(){
			if (ScoreManager.Buy (price)) {
				lvl++;
				PlayerPrefsEncrypt.SetInt (Prefs.MULTISCORE_LVL, lvl);
				return true;
			} else
				return false;
		}

	}
}
