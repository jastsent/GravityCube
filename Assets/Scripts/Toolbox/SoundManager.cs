using UnityEngine;

namespace JastSent{
    
    public class SoundManager : MonoBehaviour {
        
		public AudioSource[] channels = new AudioSource[0];
		public bool[] interruptable = new bool[0];

		[System.Serializable]
		public class SoundData {
			public float minPitch = 1;
			public float maxPitch = 1;
			public float volume  = 1;
			public int channel  = 0;
			public AudioClip clip;
		}

		[SerializeField]
		public SoundData[] clips = new SoundData[0];

		private static SoundManager instance;
		public static bool mute = false;

    	void Awake () {
			if (instance != null)
				Debug.LogError ("FATAL ERROR, DOUBLE SOUND MANAGER SCRIPT");
			else
				instance = this;			
    	}

		public static void PlaySound(Keys.Sound key){
			if (!mute) {			
				
				int _key = (int)key;
				int id = instance.clips [_key].channel;


				if (instance.channels [id].isPlaying && !instance.interruptable [id]) {
				} else {				
					instance.channels [id].pitch = Random.Range (instance.clips [_key].minPitch, instance.clips [_key].maxPitch);
					instance.channels [id].clip = instance.clips [_key].clip;
					instance.channels [id].volume = instance.clips [_key].volume;
					instance.channels [id].Play ();
				}
			}
		}

		public static void Pause(){
			//if (instance.source.isPlaying)
			//	instance.source.Pause ();
		}
		public static void Continue(){
			//instance.source.UnPause ();
		}


		public static void Stop(){
			//instance.source.Stop ();
		}

    }
    
}