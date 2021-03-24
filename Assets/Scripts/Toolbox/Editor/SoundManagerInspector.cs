using UnityEngine;
using UnityEditor;
using System;
using System.Text.RegularExpressions;
using System.IO;
using System.Linq;
using JastSent;

[CustomEditor(typeof(SoundManager))]
public class SoundManagerInspector : Editor {
	SoundManager manager;

	public string FOLDER_LOCATION = "scripts/keys/";
	public string filename = "Sound.cs";
	public string Namespace = "JastSent.Keys";
	private const string DIGIT_PREFIX = "k";
	private ConstantNamingStyle CONSTANT_NAMING_STYLE = ConstantNamingStyle.CamelCase;

	void OnEnable(){
		manager = (SoundManager)target;
	}

	public override void OnInspectorGUI(){
		GUILayout.Label ("Total sounds: " + manager.clips.Length);
		//channels

		if (GUILayout.Button ("Add channel"))
			AddChannel ();
		
		for (int i = 0; i < manager.channels.Length; i++) {
			GUILayout.BeginHorizontal ();

			GUILayout.Label ("Channel " +i.ToString());
			manager.channels [i] = (AudioSource) EditorGUILayout.ObjectField(manager.channels [i], typeof (AudioSource), true);
			manager.interruptable [i] = EditorGUILayout.Toggle ("Interruptable", manager.interruptable [i]);

			if (GUILayout.Button ("X"))
				RemoveChannel (i);
			
			GUILayout.EndHorizontal ();
		}

		FOLDER_LOCATION = EditorGUILayout.TextField ("Folder path:", FOLDER_LOCATION);
		filename = EditorGUILayout.TextField ("File name:", filename);
		Namespace = EditorGUILayout.TextField ("Namespace:", Namespace);

		if (GUILayout.Button ("Generate keys"))
			GenerateKeys();
		////////

		if (GUILayout.Button ("Add slot"))
			AddObject ();

		///////
		for (int i = manager.clips.Length-1; i >= 0; i--) {
			
			GUILayout.BeginHorizontal ();

			manager.clips [i].clip = (AudioClip) EditorGUILayout.ObjectField(manager.clips [i].clip, typeof (AudioClip), false);

			if (GUILayout.Button ("X"))
				RemoveObject (i);
			
			GUILayout.EndHorizontal ();

			GUILayout.BeginHorizontal ();

			GUILayout.Label ("Pitch");
			manager.clips [i].minPitch = EditorGUILayout.FloatField (manager.clips [i].minPitch, GUILayout.MaxWidth(32));
			manager.clips [i].maxPitch = EditorGUILayout.FloatField (manager.clips [i].maxPitch, GUILayout.MaxWidth(32));
			GUILayout.Label ("Vol");
			manager.clips [i].volume = EditorGUILayout.FloatField (manager.clips [i].volume, GUILayout.MaxWidth(32));
			GUILayout.Label ("Channel");
			manager.clips [i].channel = EditorGUILayout.IntField (manager.clips [i].channel, GUILayout.MaxWidth(32));

			GUILayout.EndHorizontal ();
		}
	}

	void AddChannel(){
		AudioSource[] oldData = manager.channels;

		manager.channels = new AudioSource[oldData.Length+1];
		oldData.CopyTo (manager.channels, 0);

		//intterupt array
		bool[] oldInterrupt = manager.interruptable;

		manager.interruptable = new bool[oldInterrupt.Length+1];
		oldInterrupt.CopyTo (manager.interruptable, 0);
	}

	void RemoveChannel(int id){		
		ArrayUtility.RemoveAt(ref manager.channels, id);
		ArrayUtility.RemoveAt(ref manager.interruptable, id);
	}

	void RemoveObject(int id){		
		ArrayUtility.RemoveAt(ref manager.clips, id);
	}

	void AddObject(){	
		ArrayUtility.Add(ref manager.clips, new SoundManager.SoundData());
		/*SoundManager.SoundData[] oldData = manager.clips;

		manager.clips = new SoundManager.SoundData[oldData.Length+1];
		oldData.CopyTo (manager.clips, 0);*/
	}


	//GENERATOR///
	/// //////////////// ///
	/// ////////////////////////////////////////////////////////////////////////////////////////////////////////////
	///
	private enum ConstantNamingStyle
	{
		UppercaseWithUnderscores,
		CamelCase
	}

	private void GenerateKeys(){
		var folderPath = Application.dataPath + "/" + FOLDER_LOCATION;
		if( !Directory.Exists( folderPath ) )
			Directory.CreateDirectory( folderPath );

		File.WriteAllText(folderPath + filename, getClassContent(filename.Replace( ".cs", string.Empty )));
		AssetDatabase.ImportAsset( "Assets/" + FOLDER_LOCATION + filename, ImportAssetOptions.ForceUpdate );
	}

	private string getClassContent( string className)
	{
		var output = "";
		output += "//This class is auto-generated do not modify\n";
		if (Namespace != "") {
			output += "namespace " + Namespace + "\n";
			output += "{\n";
		}
		output += "\tpublic enum " + className + "\n";
		output += "\t{\n";

		for (int i = 0; i < manager.clips.Length; i++)
			if(i == manager.clips.Length-1)
				output += "\t\t" + formatConstVariableName (manager.clips[i].clip.name)+ "\n";
		    else
				output += "\t\t" + formatConstVariableName (manager.clips[i].clip.name)+ ",\n";

		output += "\t}\n";

		if(Namespace != "")
			output += "}";

		return output;
	}

	private string formatConstVariableName( string input )
	{
		switch( CONSTANT_NAMING_STYLE )
		{
		case ConstantNamingStyle.UppercaseWithUnderscores:
			return toUpperCaseWithUnderscores( input );
		case ConstantNamingStyle.CamelCase:
			return toCamelCase( input );
		default:
			return toUpperCaseWithUnderscores( input );
		}
	}

	private string toCamelCase( string input )
	{
		input = input.Replace( " ", "" );

		if( char.IsLower( input[0] ) )
			input = char.ToUpper( input[0] ) + input.Substring( 1 );

		// uppercase letters before dash or underline
		Func<char,int,string> func = ( x, i ) =>
		{
			if( x == '-' || x == '_' )
				return "";

			if( i > 0 && ( input[i - 1] == '-' || input[i - 1] == '_' ) )
				return x.ToString().ToUpper();

			return x.ToString();
		};
		input = string.Concat( input.Select( func ).ToArray() );

		// digits are a no-no so stick prefix in front
		if( char.IsDigit( input[0] ) )
			return DIGIT_PREFIX + input;
		return input;
	}

	private string toUpperCaseWithUnderscores( string input )
	{
		input = input.Replace( "-", "_" );
		input = Regex.Replace( input, @"\s+", "_" );

		// make camel-case have an underscore between letters
		Func<char,int,string> func = ( x, i ) =>
		{
			if( i > 0 && char.IsUpper( x ) && char.IsLower( input[i - 1] ) )
				return "_" + x.ToString();
			return x.ToString();
		};
		input = string.Concat( input.Select( func ).ToArray() );

		// digits are a no-no so stick prefix in front
		if( char.IsDigit( input[0] ) )
			return DIGIT_PREFIX + input.ToUpper();
		return input.ToUpper();
	}
}