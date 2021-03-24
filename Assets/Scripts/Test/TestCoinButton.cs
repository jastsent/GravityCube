using UnityEngine;

namespace JastSent{
    
    public class TestCoinButton : MonoBehaviour {
        
    	
		public void OnClick(){
			ScoreManager.ChangeCoins (1000);
		}
    }
    
}