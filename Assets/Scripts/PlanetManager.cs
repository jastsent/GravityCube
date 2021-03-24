using UnityEngine;

namespace JastSent {

public class PlanetManager : MonoBehaviour {
	public float minPlanetSize = 0f;
	public float maxPlanetSize = 0f;
	public float minGravitySize = 0f;
	public float maxGravitySize = 0f;
	[System.Serializable]
	public struct SpriteData {
		public Sprite sprite;
		public bool randomRotation;
		public bool randomColor;
		public Color32 color;
		public byte alpha;
	}
	[SerializeField]
	public SpriteData[] planets;
	public bool emptyPlanet;

	[SerializeField]
	public SpriteData[] decals;
	public bool emptyDecal;

	[SerializeField]
	public SpriteData[] additions;
	public bool emptyAdd;

	public static PlanetManager instance = null;

	void Awake () {
		if (instance != null)
			Debug.LogError ("FATAL ERROR, DOUBLE PLANET MANAGER SCRIPT");
		else
			instance = this;
	}

	void Start(){
	}

	public void SetSpritePlanet(SpriteRenderer renderer){
		SetSprite (renderer, planets, emptyPlanet);
	}

	public void SetSpriteDecal(SpriteRenderer renderer){
		SetSprite (renderer, decals, emptyDecal);
	}

	public void SetSpriteAddition(SpriteRenderer renderer){
		SetSprite (renderer, additions, emptyAdd);
	}

	public void SetSprite(SpriteRenderer renderer, SpriteData[] spritesData, bool empty){
		int index;
		if (empty) {
			index = Random.Range (-1, spritesData.Length);
			if (index < 0) {
				renderer.sprite = null;
				return;
			}
		} else
			index = Random.Range (0, spritesData.Length);
		
		renderer.sprite = spritesData [index].sprite;
		//rotate
		if (spritesData [index].randomRotation)
			renderer.transform.rotation = Quaternion.Euler(new Vector3
				(renderer.transform.rotation.x,
				renderer.transform.rotation.y,
				Random.Range(0f, 360f)));
		//color
		Color32 newColor;
		
		if (spritesData [index].randomColor)
			newColor = new Color32 
				((byte)Random.Range (0, 256),
				(byte)Random.Range (0, 256),
				(byte)Random.Range (0, 256),
				spritesData [index].alpha);
		else {
			newColor = spritesData [index].color;
			newColor.a = spritesData [index].alpha;
		}

		renderer.color = newColor;
	}
}
}
