using UnityEngine;

namespace JastSent{
    
    public class RateUsButton : MonoBehaviour {
        
		public string packageName;
		public void RateUs(){ Application.OpenURL("market://details?id="+packageName);}
    }
    
}