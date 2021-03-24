using System.Collections.Generic;
using UnityEngine;
using JastSent.Keys;
namespace JastSent {

	public class LevelGenerator : MonoBehaviour {
		
		private struct ObjectData {
			public bool free;
			public bool bordered;
			public Transform objTransform;
		}
		private List<ObjectData> levelObjects = new List<ObjectData>();

		private int[] idPowerups;

		public float minDistance = 0f;
		public float maxDistance = 0f;
		public float heroPositionOffset = 0f;

		public float offsetMoneyObjects = 0f; 
		public float powerupChance = 0f;

		public static LevelGenerator instance;

		float globalLastY = 0f;

		void Awake(){
			instance = this;
			EventManager.AddListener (Keys.Messages.GoGame, Go);
			EventManager.AddListener (Keys.Messages.StopGame, Stop);
			EventManager.AddListener (Keys.Messages.PauseGame, Pause);
			EventManager.AddListener (Keys.Messages.PlayGame, Play);

			//powerups
			idPowerups = new int[]{Pools.Magnet, Pools.Shield, Pools.Banana, Pools.Multiscore};
		}

		void FrameUpdate () {
			float deleteBorder = ScreenSize.down - 5f;
			float createBorder = ScreenSize.up;

			while (globalLastY <= createBorder) {				
				float randomDistance = Random.Range (minDistance, maxDistance);
				float planetY = globalLastY + randomDistance;
				float coinsY = globalLastY + randomDistance/2;
				//coins & poweups
				Transform coinObj;
				for (int i = 3; i > 0; i--) {
					
					float offsetObject = 0f;
					if (i == 3)
						offsetObject = -offsetMoneyObjects;
					else if(i == 1)
						offsetObject = offsetMoneyObjects;

					//poweup
					if (Random.Range (0f, 100f) < powerupChance)
						coinObj = CreateObject (true, true, idPowerups[Random.Range (0, idPowerups.Length)] );
					//coin
					else 
						coinObj = CreateObject (true, true, Pools.Coin);

					coinObj.position = new Vector3 
						(coinObj.transform.position.x + offsetObject, 
						coinsY, 
						coinObj.transform.position.z);
				}
				//planet
				CreatePlanet(planetY);
				//
				globalLastY += randomDistance;
			}
			for(int i = levelObjects.Count-1; i >= 0; i--){
				if (levelObjects[i].bordered && levelObjects[i].objTransform.position.y <= deleteBorder) {
					Pool.Return (levelObjects[i].objTransform);
					levelObjects.RemoveAt(i);
				}
			}
		}

		void Go(){
			//hero
			Transform hero = CreateObject(true, false, Pools.Hero);	
			hero.position = new Vector3 (hero.transform.position.x+Utilities.RandomSymbol(heroPositionOffset),
				ScreenSize.down + hero.transform.localScale.y/2 + 0.05f,
				hero.transform.position.z);
			
			//first planet
			globalLastY = ScreenSize.down + 1 + Random.Range (minDistance, maxDistance);
			CreatePlanet(globalLastY);


			while (globalLastY < ScreenSize.up) {	
				float randomDistance = Random.Range (minDistance, maxDistance);
				//coins & poweups
				Transform coinObj;
				for (int i = 3; i > 0; i--) {
					
					float offsetObject = 0f;
					if (i == 3)
						offsetObject = -offsetMoneyObjects;
					else if(i == 1)
						offsetObject = offsetMoneyObjects;

					//poweup
					if (Random.Range (0f, 100f) < powerupChance)
						coinObj = CreateObject (true, true, idPowerups[Random.Range (0, idPowerups.Length)] );
					//coin
					else 
						coinObj = CreateObject (true, true, Pools.Coin);

					coinObj.transform.position = new Vector3 
						(coinObj.transform.position.x + offsetObject, 
							globalLastY + randomDistance/2, 
						coinObj.transform.position.z);
				}

				//planet
				CreatePlanet(globalLastY + randomDistance);

				globalLastY += randomDistance;
			}

			EventManager.AddListener (Keys.Messages.Update, FrameUpdate);
		}

		void Stop(){
			foreach (ObjectData lvlData in levelObjects) {
				Pool.Return (lvlData.objTransform);
			}
			levelObjects.Clear ();
			EventManager.RemoveListener (Keys.Messages.Update, FrameUpdate);
		}

		void Play(){
			EventManager.AddListener (Keys.Messages.Update, FrameUpdate);
		}
		void Pause(){
			EventManager.RemoveListener (Keys.Messages.Update, FrameUpdate);
		}

		Transform CreateObject(bool free, bool bordered, int idFromPool){
			GameObject newObject = Pool.Get (idFromPool);

			ObjectData newData = new ObjectData ();
			newData.bordered = bordered;
			newData.free = free;
			newData.objTransform = newObject.transform;

			levelObjects.Add (newData);

			return newData.objTransform;
		}

		void CreatePlanet(float YY){
			Transform planet = CreateObject (false, true, Pools.Planet);
			planet.position = new Vector3 
				(planet.transform.position.x, 
				YY, 
				planet.transform.position.z);
			EventManager.Trigger<Transform> (Keys.Messages.PlanetCreated, planet);
		}
	}
}
