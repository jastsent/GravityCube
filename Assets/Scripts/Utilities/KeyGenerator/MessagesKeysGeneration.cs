#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;
using System.Text.RegularExpressions;
using System.IO;
using System.Linq;

namespace JastSent.Editor.Utilities{

	public class MessagesKeysGeneration : MonoBehaviour {
		public string FOLDER_LOCATION = "scripts/keys/";
		public string filename = "Messages.cs";
		public string Namespace = "JastSent.Keys";
		public string[] keys = new string[0];

		private const string DIGIT_PREFIX = "k";
		private enum ConstantNamingStyle
		{
			UppercaseWithUnderscores,
			CamelCase
		}

		private ConstantNamingStyle CONSTANT_NAMING_STYLE = ConstantNamingStyle.CamelCase;

		public void GeneratePrefsFile(){
			var folderPath = Application.dataPath + "/" + FOLDER_LOCATION;
			if( !Directory.Exists( folderPath ) )
				Directory.CreateDirectory( folderPath );

			File.WriteAllText(folderPath + filename, getClassContent(filename.Replace( ".cs", string.Empty ), keys));
			AssetDatabase.ImportAsset( "Assets/" + FOLDER_LOCATION + filename, ImportAssetOptions.ForceUpdate );
		}

		private string getClassContent( string className, string[] labelsArray )
		{
			var output = "";
			output += "//This class is auto-generated do not modify\n";
			if (Namespace != "") {
				output += "namespace " + Namespace + "\n";
				output += "{\n";
			}
			output += "\tpublic static class " + className + "\n";
			output += "\t{\n";

			for (int i = 0; i < labelsArray.Length; i++)
				output += "\t\tpublic const int " + formatConstVariableName (labelsArray[i])+" = "+ i.ToString()+";\n";
//				if(i == labelsArray.Length-1)
//					output += "\t\t" + formatConstVariableName (labelsArray[i])+ "\n";
//				else
//					output += "\t\t" + formatConstVariableName (labelsArray[i])+ ",\n";

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
}
#endif