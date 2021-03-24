using UnityEngine;
using JastSent.Keys;

namespace JastSent {

	public class MagnetManager : MonoBehaviour {
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

		public float radius = 0f;
		public float coinSpeed = 0f;
		[HideInInspector] public int lvl = 0;
		public static MagnetManager instance;

		public static Timer timer = null;
		public static bool active = false;

		void Awake(){
			if (instance != null)			
				Debug.LogError ("FATAL ERROR, DOUBLE MAGNET MANAGER SCRIPT");
			else
				instance = this;
			
			lvl = PlayerPrefsEncrypt.GetInt (Prefs.MAGNET_LVL, 0);

			timer = new Timer(durationPerLvl [lvl], false, true);
		}

		public bool Upgrade(){
			if (ScoreManager.Buy (price)) {
				lvl++;
				PlayerPrefsEncrypt.SetInt (Prefs.MAGNET_LVL, lvl);
				return true;
			} else
				return false;
		}
	}
}
