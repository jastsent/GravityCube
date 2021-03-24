using UnityEngine;
using UnityEngine.UI;

namespace JastSent {

	public class PowerupIndicators : MonoBehaviour {
		public Image magnetIndicator;
		public Image shieldIndicator;
		public Image multiIndicator;

		void OnEnable(){
			EventManager.AddListener (Keys.Messages.PickupMagnet, PickMagnet);
			EventManager.AddListener (Keys.Messages.PickupShield, PickShield);
			EventManager.AddListener (Keys.Messages.PickupMultiscore, PickMulti);
			//EventManager.AddListener ("StopGame", Stop);
		}

		void OnDisable(){
			EventManager.RemoveListener (Keys.Messages.PickupMagnet, PickMagnet);
			EventManager.RemoveListener (Keys.Messages.PickupShield, PickShield);
			EventManager.RemoveListener (Keys.Messages.PickupMultiscore, PickMulti);
			//EventManager.RemoveListener ("StopGame", Stop);

			if(magnetIndicator.gameObject.activeSelf)
				magnetIndicator.gameObject.SetActive (false);
			if(shieldIndicator.gameObject.activeSelf)
				shieldIndicator.gameObject.SetActive (false);
			if(multiIndicator.gameObject.activeSelf)
				multiIndicator.gameObject.SetActive (false);
		}

		void LateUpdate(){
			if (!Game.pause) {
				//magnet
				if (!MagnetManager.timer.stop)
					magnetIndicator.fillAmount = 1 - (MagnetManager.timer.time / (MagnetManager.timer.endTime / 100)) / 100;
				else if (MagnetManager.timer.stop && magnetIndicator.gameObject.activeSelf) 
					magnetIndicator.gameObject.SetActive (false);
				
				//shield
				if (!ShieldManager.timer.stop)
					shieldIndicator.fillAmount = 1 - (ShieldManager.timer.time / (ShieldManager.timer.endTime / 100)) / 100;
				else if (ShieldManager.timer.stop && shieldIndicator.gameObject.activeSelf)
					shieldIndicator.gameObject.SetActive (false);

				//multiscore
				if (!MultiscoreManager.timer.stop)
					multiIndicator.fillAmount = 1 - (MultiscoreManager.timer.time / (MultiscoreManager.timer.endTime / 100)) / 100;
				else if (MultiscoreManager.timer.stop && multiIndicator.gameObject.activeSelf)
					multiIndicator.gameObject.SetActive (false);
			}
		}

		void PickMagnet(){
			if (!magnetIndicator.gameObject.activeSelf) {
				magnetIndicator.gameObject.SetActive (true);
				magnetIndicator.rectTransform.SetAsFirstSibling ();
			}
			magnetIndicator.fillAmount = 1;
		}

		void PickShield(){
			if (!shieldIndicator.gameObject.activeSelf) {
				shieldIndicator.gameObject.SetActive (true);
				magnetIndicator.rectTransform.SetAsFirstSibling ();
			}
			shieldIndicator.fillAmount = 1;
		}

		void PickMulti(){
			if (!multiIndicator.gameObject.activeSelf) {
				multiIndicator.gameObject.SetActive (true);
				magnetIndicator.rectTransform.SetAsFirstSibling ();
			}
			multiIndicator.fillAmount = 1;
		}

		/*void Stop(){
			if(magnetIndicator.gameObject.activeSelf)
				magnetIndicator.gameObject.SetActive (false);
			if(shieldIndicator.gameObject.activeSelf)
				shieldIndicator.gameObject.SetActive (false);
			if(multiIndicator.gameObject.activeSelf)
				multiIndicator.gameObject.SetActive (false);
		}*/

	}
}
