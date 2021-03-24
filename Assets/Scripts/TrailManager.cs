using UnityEngine;

namespace JastSent{

	public class TrailManager : MonoBehaviour {

		public int[] prices; 
		[HideInInspector] public int[] poolId;
		[HideInInspector] public bool[] bought;

		public static int lastTrail = 0;
		public static int lastTrailId = 0;

		public static TrailManager instance;

		void Awake(){
			if (instance != null)
				Debug.LogError ("FATAL ERROR, DOUBLE TRAIL MANAGER SCRIPT");
			else
				instance = this;

			poolId = new int[] {
				Keys.Pools.Redtrail,
				Keys.Pools.Yellowtrail,
				Keys.Pools.Greentrail,
				Keys.Pools.Bluetrail,
				Keys.Pools.Graytrail,
				Keys.Pools.Bluewhitetrail,
				Keys.Pools.Orangetrail,
				Keys.Pools.Rainbowtrail
			};

			if(prices.Length != poolId.Length)
				Debug.LogError ("FATAL ERROR, SHOP ITEMS LENGTH SCRIPT");

			lastTrailId = PlayerPrefsEncrypt.GetInt (Keys.Prefs.LAST_TRAIL, 0);
			lastTrail = poolId [lastTrailId];

			bought = new bool[poolId.Length];

			bought [0] = true;//first red trail already bought



			//load saves about bought trails
			for (int i = 1; i < bought.Length; i++) {
				if (PlayerPrefsEncrypt.GetInt ("trailPurchased_" + i.ToString (), 0) == 0) {
					bought [i] = false;
				} else
					bought [i] = true;
			}
		}

		public bool Buy(int id){
			if (!bought [id]) {
				if(ScoreManager.Buy(prices[id])){
					PlayerPrefsEncrypt.SetInt ("trailPurchased_" + id.ToString (), 1);
					bought [id] = true;
					return true;
				}
			}
			return false;
		}

		public bool Select(int id){
			if (bought [id]) {
				lastTrail = poolId[id];
				lastTrailId = id;
				PlayerPrefsEncrypt.SetInt (Keys.Prefs.LAST_TRAIL, id);
				return true;
			}
			return false;
		}
	}

}
