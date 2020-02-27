using System;
using System.Linq;
using Enums.Announcer;
using Enums.Character;
using UnityEditor;
using UnityEngine;
using VDFramework.Extensions;

namespace CustomInspector.AudioManager
{
	using AudioManager = Audio.AudioManager;
	
	[CustomEditor(typeof(AudioManager))]
	public class AudioManagerEditor : Editor
	{
		private SerializedProperty nautClips;
		private SerializedProperty announcerClips;

		private void OnEnable()
		{
			(target as AudioManager)?.UpdateDictioneries();

			nautClips = serializedObject.FindProperty("nautClips");
			announcerClips = serializedObject.FindProperty("announcerClips");
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			ShowListEnumScriptableObject<Awesomenaut>(nautClips);
			ShowListEnumScriptableObject<Announcer>(announcerClips);

			serializedObject.ApplyModifiedProperties();
		}

		private static void ShowListEnumScriptableObject<TEnum>(SerializedProperty property)
			where TEnum : struct, Enum
		{
			int size = property.arraySize;

			for (int i = 0; i < size; ++i)
			{
				SerializedProperty pair = property.GetArrayElementAtIndex(i);
				SerializedProperty enumType = pair.FindPropertyRelative("key");
				SerializedProperty audioSet = pair.FindPropertyRelative("value");

				string enumString = ConvertIntToEnumString<TEnum>(enumType.enumValueIndex);

				ShowVariables(enumString, audioSet);

				GUILayout.Space(20.0f);
			}
		}

		private static void ShowVariables(string enumString, SerializedProperty audioSet)
		{
			EditorGUILayout.LabelField(new GUIContent($"{enumString.InsertSpaceBeforeCapitals()}"),
				EditorStyles.boldLabel,
				GUILayout.MaxWidth(100.0f));

			GUILayout.Space(8.0f);

			EditorGUILayout.PropertyField(audioSet, new GUIContent("Audio Set"), true);
		}

		private static string ConvertIntToEnumString<TEnum>(int integer)
			where TEnum : struct, Enum
		{
			TEnum @enum = default;

			return @enum.GetValues().ElementAt(integer).ToString();
		}
	}
}