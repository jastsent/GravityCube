using UnityEngine;
using UnityEditor;
using System;
using System.Text.RegularExpressions;
using System.IO;
using System.Linq;
using JastSent;

[CustomEditor(typeof(Pool))]
public class PoolInspector : Editor {
	Pool pool;

	public string FOLDER_LOCATION = "scripts/keys/";
	public string filename = "Pools.cs";
	public string Namespace = "JastSent.Keys";
	private const string DIGIT_PREFIX = "k";
	private ConstantNamingStyle CONSTANT_NAMING_STYLE = ConstantNamingStyle.CamelCase;

	void OnEnable(){
		pool = (Pool)target;
	}

	public override void OnInspectorGUI(){
		GUILayout.Label ("Total pooled objects: " + pool.pools.Length);
		pool.parent = (Transform) EditorGUILayout.ObjectField(pool.parent, typeof (Transform), true);

		FOLDER_LOCATION = EditorGUILayout.TextField ("Folder path:", FOLDER_LOCATION);
		filename = EditorGUILayout.TextField ("File name:", filename);
		Namespace = EditorGUILayout.TextField ("Namespace:", Namespace);

		if (GUILayout.Button ("Generate keys"))
			GenerateKeys();
		////////

		if (GUILayout.Button ("Add slot"))
			AddObject ();
		
		///////
		for (int i = pool.pools.Length-1; i >= 0; i--) {
			GUILayout.BeginHorizontal ();

			//GUILayout.Label ("id " + i, GUILayout.MaxWidth(40));
			pool.pools [i].count = EditorGUILayout.IntField (pool.pools [i].count, GUILayout.MaxWidth(64));
			pool.pools [i].prefab = (GameObject) EditorGUILayout.ObjectField(pool.pools [i].prefab, typeof (GameObject), false);
			if (GUILayout.Button ("X"))
				RemoveObject (i);

			GUILayout.EndHorizontal ();
		}
	}

	void RemoveObject(int id){		
		ArrayUtility.RemoveAt(ref pool.pools, id);
	}

	void AddObject(){	
		Pool.PoolPart[] oldPools = pool.pools;

		pool.pools = new Pool.PoolPart[oldPools.Length+1];
		oldPools.CopyTo (pool.pools, 0);
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
		output += "\tpublic static class " + className + "\n";
		output += "\t{\n";

		for (int i = 0; i < pool.pools.Length; i++)
			output += "\t\tpublic const int " + formatConstVariableName (pool.pools[i].prefab.name) + " = " + i.ToString() + ";\n";

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