using System;
using System.Linq;
using Interfaces.Audio;
using ScriptableObjects;
using UnityEditor;
using UnityEngine;
using VDFramework.Extensions;

namespace CustomInspector.AudioClips
{
	public abstract class AAudioClipsEditor<TEnum, TStruct> : Editor
		where TEnum : struct, Enum
		where TStruct : IAudioClipsPerEnum<TEnum>, new()
	{
		private SerializedProperty clipsPerSoundType;

		private static TEnum[] enumValues;

		private void OnEnable()
		{
			(target as AAudioClips<TEnum, TStruct>)?.PopulateDictionary();

			TEnum @enum = default;
			enumValues = @enum.GetValues().ToArray();

			clipsPerSoundType = serializedObject.FindProperty("clipsPerEnum");
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			ShowInfoBox();
			ShowAudioClipDictionary();

			serializedObject.ApplyModifiedProperties();
		}

		private static void ShowInfoBox()
		{
			EditorGUILayout.HelpBox(
				"Put the audio clips under the appropriate type",
				MessageType.Info);
		}

		private void ShowAudioClipDictionary()
		{
			int size = clipsPerSoundType.arraySize;

			ShowDictionary(size);
		}

		private void ShowDictionary(int size)
		{
			for (int i = 0; i < size; ++i)
			{
				SerializedProperty pair = clipsPerSoundType.GetArrayElementAtIndex(i);
				SerializedProperty soundType = pair.FindPropertyRelative("SoundType");
				SerializedProperty audioClips = pair.FindPropertyRelative("Clips");
				SerializedProperty isLooping = pair.FindPropertyRelative("IsLooping");

				string enumString = ConvertIntToEnumString(soundType.enumValueIndex);

				ShowVariables(enumString, isLooping, audioClips);

				GUILayout.Space(20.0f);
			}
		}

		private static void ShowVariables(string enumString, SerializedProperty isLooping,
			SerializedProperty audioClips)
		{
			EditorGUILayout.BeginHorizontal();
			{
				EditorGUILayout.LabelField(new GUIContent($"{enumString}"), EditorStyles.boldLabel,
					GUILayout.MaxWidth(100.0f));
				GUILayout.Space(8.0f);

				EditorGUILayout.PropertyField(isLooping, new GUIContent("Loop"));
				GUILayout.Space(8.0f);
			}
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.PropertyField(audioClips, new GUIContent("Clips"), true);
		}

		private static string ConvertIntToEnumString(int integer)
		{
			return enumValues[integer].ToString();
		}
	}
}