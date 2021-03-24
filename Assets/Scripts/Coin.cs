using UnityEngine;

namespace JastSent {
	public class Coin : MonoBehaviour {
		public int rewardCoins = 0;

		void OnTriggerEnter2D(Collider2D coll){
			if (coll.transform.CompareTag (Keys.Tags.HERO)) {	
				EventManager.Trigger (Keys.Messages.PickUpCoin);
				ScoreManager.ChangeCoins (rewardCoins);
				gameObject.SetActive (false);
			}
		}
	}
}
