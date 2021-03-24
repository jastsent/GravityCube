using UnityEngine;
using UnityEngine.UI;

namespace JastSent {

	public class ShopManager : MonoBehaviour {
		public Image[] magnetStars;
		public Image[] shieldStars;
		public Image[] multiStars;

		public Text magnetPrice;
		public Text shieldPrice;
		public Text multiPrice;

		public Button magnetButton;
		public Button shieldButton;
		public Button multiButton;
		/// ///////////
		public GameObject[] locks;
		public GameObject[] promos;
		public Image selectedTrailImg;
		public Image appliedTrailImg;
		public Button trailButton;
		public int appliedNum = 0;
		private int lastSelected = 0;
		///
		public Text trailButtonTxt;
		public Image trailButtonApply;
		public Image trailButtonDollar;
		public Color32 noApplyColor;
		public Color32 appliedColor;
		/// ///
		public float startPromoOffset = 0;
		public float spaceBetweenPromo = 0;

		private bool changed = false;

		void Start(){
			MagnetInit ();
			ShieldInit ();
			MultiscoreInit ();

			//trails
			for (int i = 0; i < locks.Length; i++) {
				if(TrailManager.instance.bought[i+1])
					locks [i].gameObject.SetActive (false);
			}

			//last selected init
			appliedNum = TrailManager.lastTrailId;
			lastSelected = appliedNum;

			selectedTrailImg.rectTransform.anchoredPosition = new Vector2 (startPromoOffset + spaceBetweenPromo * lastSelected, 	
				selectedTrailImg.rectTransform.anchoredPosition.y);

			appliedTrailImg.rectTransform.anchoredPosition = new Vector2 (startPromoOffset + spaceBetweenPromo * lastSelected, 	
				selectedTrailImg.rectTransform.anchoredPosition.y);

			promos [lastSelected].SetActive (true);

			TrailButtonChange (false, false, appliedColor);
		}

		//MAGNET
		private void MagnetInit(){
			if (MagnetManager.instance.price >= 0)
				magnetPrice.text = MagnetManager.instance.price.ToString ();
			else {
				magnetPrice.text = "Full";
				magnetButton.interactable = false;
			}
			
			for (int i = 0; i < MagnetManager.instance.lvl; i++) {
				magnetStars [i].color = Color.white;
			}
		}
		//SHIELD
		private void ShieldInit(){
			if (ShieldManager.instance.price >= 0)
				shieldPrice.text = ShieldManager.instance.price.ToString ();
			else {
				shieldPrice.text = "Full";
				shieldButton.interactable = false;
			}
			
			for (int i = 0; i < ShieldManager.instance.lvl; i++) {
				shieldStars [i].color = Color.white;
			}
		}
		//MULTISCORE
		private void MultiscoreInit(){
			if (MultiscoreManager.instance.price >= 0)
				multiPrice.text = MultiscoreManager.instance.price.ToString ();
			else {
				multiPrice.text = "Full";
				multiButton.interactable = false;
			}
			
			for (int i = 0; i < MultiscoreManager.instance.lvl; i++) {
				multiStars [i].color = Color.white;
			}
		}
		//buttons
		//MAGNET
		public void MagnetButton(){
			if (MagnetManager.instance.Upgrade ()) {
				MagnetInit ();
				changed = true;
			}
		}
		//SHIELD
		public void ShieldButton(){
			if (ShieldManager.instance.Upgrade ()) {
				ShieldInit ();
				changed = true;
			}
		}
		//MULTISCORE
		public void MultiscoreButton(){
			if (MultiscoreManager.instance.Upgrade ()) {
				MultiscoreInit ();
				changed = true;
			}
		}



		/// 
		/// ///////////////////////
		/// TRAIL ZONE
		///
		//buttons in scroll view
		public void Select(int id){
			if (lastSelected != id) {
				
				//not bought
				if (!TrailManager.instance.bought [id]) {
					TrailButtonChange (true, true);
					trailButtonTxt.text = TrailManager.instance.prices [id].ToString ();
					
				} else if (id == TrailManager.lastTrailId)//already applied
					TrailButtonChange (false, false, appliedColor);
				
				else //bought but not selected					
					TrailButtonChange (false, true, noApplyColor);

				promos [lastSelected].SetActive (false);
				promos [id].SetActive (true);

				lastSelected = id;

				selectedTrailImg.rectTransform.anchoredPosition = new Vector2 (startPromoOffset + spaceBetweenPromo * id, 	
					selectedTrailImg.rectTransform.anchoredPosition.y);
			}
		}
		// buy button
		public void TrailButton(){
			if (!TrailManager.instance.bought [lastSelected]) {
				if (TrailManager.instance.Buy (lastSelected)) {

					TrailButtonChange (false, true, noApplyColor);

					locks [lastSelected-1].SetActive (false);

					changed = true;
				}
			} else {
				if (TrailManager.instance.Select (lastSelected)) {

					TrailButtonChange (false, false, appliedColor);

					appliedTrailImg.rectTransform.anchoredPosition = new Vector2 (startPromoOffset + spaceBetweenPromo * lastSelected, 	
						selectedTrailImg.rectTransform.anchoredPosition.y);

					changed = true;
				}
			}
		}

		private void TrailButtonChange(bool enableTxt, bool enableButton, Color32 newColor = default(Color32)){
			
			//activate text
			if (trailButtonTxt.gameObject.activeSelf != enableTxt)
				trailButtonTxt.gameObject.SetActive (enableTxt);
			//activate apply icon
			if (trailButtonApply.gameObject.activeSelf != !enableTxt)
				trailButtonApply.gameObject.SetActive (!enableTxt);
			//set apply icon color
			if (trailButtonApply.color != newColor)
				trailButtonApply.color = newColor;
			//activate dollar
			if (trailButtonDollar.gameObject.activeSelf != enableTxt)
				trailButtonDollar.gameObject.SetActive (enableTxt);
			//set button interactable
			if (trailButton.interactable != enableButton)
				trailButton.interactable = enableButton;
		}

		//shop saves
		public void OnShopOpen(){
			EventManager.AddListener (Keys.Messages.Backbutton, OnShopClose);
		}
		public void OnShopClose(){
			if (changed) {
				PlayerPrefsEncrypt.Save ();
				changed = false;
			}
			EventManager.RemoveListener (Keys.Messages.Backbutton, OnShopClose);
		}
	}
}