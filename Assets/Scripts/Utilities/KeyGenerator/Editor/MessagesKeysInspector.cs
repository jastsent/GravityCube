using UnityEngine;
using UnityEditor;
using JastSent.Editor.Utilities;

[CustomEditor(typeof(MessagesKeysGeneration))]
public class MessagesKeysInspector : Editor {

	MessagesKeysGeneration script;

	string addKey = "";
	void OnEnable(){
		script = (MessagesKeysGeneration)target;
	}

	public override void OnInspectorGUI(){
		GUILayout.Label ("Total messages keys objects: " + script.keys.Length);
		////////

		script.FOLDER_LOCATION = EditorGUILayout.TextField ("Folder path:", script.FOLDER_LOCATION);
		script.filename = EditorGUILayout.TextField ("File name:", script.filename);
		script.Namespace = EditorGUILayout.TextField ("Namespace:", script.Namespace);
		if (GUILayout.Button ("Generate"))
			Generate ();

		GUILayout.BeginHorizontal ();

		if (GUILayout.Button ("Add", GUILayout.MaxWidth(40)))
			AddKey (addKey);

		addKey = EditorGUILayout.TextField (addKey);

		GUILayout.EndHorizontal ();
		///////
		for (int i = 0; i < script.keys.Length; i++) {
			GUILayout.BeginHorizontal ();

			GUILayout.Label (script.keys[i]);

			if (GUILayout.Button ("X", GUILayout.MaxWidth(40)))
				RemoveKey (i);

			GUILayout.EndHorizontal ();
		}
	}

	void AddKey(string key){
		ArrayUtility.Add (ref script.keys, key);
	}

	void RemoveKey(int id){
		ArrayUtility.RemoveAt (ref script.keys, id);
	}

	void Generate(){
		script.GeneratePrefsFile ();
	}
}
