using System;
using System.Linq;
using Interfaces.Audio;
using ScriptableObjects;
using UnityEditor;
using UnityEngine;
using VDFramework.Extensions;

namespace CustomInspector.AudioSets
{
	public abstract class AAudioSetEditor<TEnum, TStruct> : Editor
		where TEnum : struct, Enum
		where TStruct : IAudioClipDataPerSoundType<TEnum>, new()
	{
		private SerializedProperty clipsPerSoundType;

		private static TEnum[] enumValues;

		private void OnEnable()
		{
			(target as AAudioSet<TEnum, TStruct>)?.PopulateDictionary();

			TEnum @enum = default;
			enumValues = @enum.GetValues().ToArray();

			clipsPerSoundType = serializedObject.FindProperty("clipDataPerSound");
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			ShowInfoBox("Put the audio clips under the appropriate type");
			ShowAudioClipDictionary();

			serializedObject.ApplyModifiedProperties();
		}

		private static void ShowInfoBox(string message)
		{
			EditorGUILayout.HelpBox(
				message,
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
				SerializedProperty audioClipData = pair.FindPropertyRelative("ClipData");

				string enumString = ConvertIntToEnumString(soundType.enumValueIndex);

				ShowVariables(enumString, audioClipData);

				GUILayout.Space(20.0f);
			}
		}

		private static void ShowVariables(string enumString, SerializedProperty audioClipData)
		{
			EditorGUILayout.LabelField(new GUIContent($"{enumString}"), EditorStyles.boldLabel,
				GUILayout.MaxWidth(100.0f));


			EditorGUILayout.PropertyField(audioClipData, new GUIContent("Audio Data"), true);
		}

		private static string ConvertIntToEnumString(int integer)
		{
			return enumValues[integer].ToString();
		}
	}
}