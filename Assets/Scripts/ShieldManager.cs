using UnityEngine;
using JastSent.Keys;

namespace JastSent {

	public class ShieldManager : MonoBehaviour {
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

		public int lvl = 0;

		public static ShieldManager instance = null;

		public static Timer timer = null;
		public static bool active = false;

		void Awake(){
			if (instance != null)
				Debug.LogError ("FATAL ERROR, DOUBLE SHIELD MANAGER SCRIPT");
			else
				instance = this;
			

			lvl = PlayerPrefsEncrypt.GetInt (Prefs.SHIELD_LVL, 0);

			EventManager.AddListener (Keys.Messages.PickupShield, Pickup);

			timer = new Timer(durationPerLvl[lvl], false, true, EndShield);
		}

		public void Pickup(){
			if (active) {
				timer.Replay();
			} else {
				active = true;
				timer.SetTimes (0f, durationPerLvl[lvl]);
				timer.Play ();
				SoundManager.PlaySound (Sound.ShieldUp);
				EventManager.Trigger (Keys.Messages.ShieldActivate);
				EventManager.AddListener (Keys.Messages.StopGame, StopShield);
			}
		}
		public void EndShield(){
			active = false;
			timer.Stop();
			SoundManager.PlaySound (Sound.ShieldDown);
			EventManager.RemoveListener (Keys.Messages.StopGame, StopShield);
			EventManager.Trigger (Keys.Messages.ShieldDeactivate);
		}

		public void StopShield(){
			active = false;
			timer.Stop();
			EventManager.RemoveListener (Keys.Messages.StopGame, StopShield);
			EventManager.Trigger (Keys.Messages.ShieldDeactivate);
		}

		public bool Upgrade(){
			if (ScoreManager.Buy (price)) {
				lvl++;
				PlayerPrefsEncrypt.SetInt (Prefs.SHIELD_LVL, lvl);
				return true;
			} else
				return false;
		}
	}
}
