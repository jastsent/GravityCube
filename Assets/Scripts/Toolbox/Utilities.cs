using UnityEngine;

namespace JastSent {

	public static class Utilities {
		//random + or -
		public static float RandomSymbol(float num){
			if (Random.value >= 0.5) {
				return num;
			} else
				return -num;
		}
		//random + or -
		public static int RandomSymbol(int num){
			if (Random.value >= 0.5) {
				return num;
			} else
				return -num;
		}
		//random + or -
		public static int RandomSymbol(){
			if (Random.value >= 0.5) {
				return 1;
			} else
				return -1;
		}
		//random boolean
		public static bool RandomBool(){
			if (Random.value >= 0.5) {
				return true;
			} else
				return false;
		}
		//int to boolean
		public static bool NumberToBool(int num){
			if (num > 0) {
				return true;
			} else
				return false;
		}
		//float to boolean
		public static bool NumberToBool(float num){
			if (num > 0) {
				return true;
			} else
				return false;
		}

		//contains for built-in arrays like int[] 
		public static bool ArrayContains(System.Array array, object value){
			int index = System.Array.IndexOf (array, value);
			if (index >= 0)
				return true;
			else
				return false;
		}
	}
	
}
