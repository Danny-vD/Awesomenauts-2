using Enums;
using ScriptableObjects;
using UnityEditor;
using UnityEngine;

namespace CustomInspector
{
	[CustomEditor(typeof(NautAudioClips)), CanEditMultipleObjects]
	public class NautAudioClipsEditor : Editor
	{
		private SerializedProperty clipsPerSoundType;

		private void OnEnable()
		{
			clipsPerSoundType = serializedObject.FindProperty("clipsPerSoundType");

			(target as NautAudioClips).PopulateDictionary();
		}

		public override void OnInspectorGUI()
		{
			OnEnable();

			serializedObject.Update();

			ShowAudioClipDictionary();

			serializedObject.ApplyModifiedProperties();
		}

		private void ShowAudioClipDictionary()
		{
			EditorGUILayout.HelpBox(
				"Put the audio clips under the appropriate type",
				MessageType.Info);

			int size = clipsPerSoundType.arraySize;

			for (int i = 0; i < size; ++i)
			{
				SerializedProperty pair = clipsPerSoundType.GetArrayElementAtIndex(i);
				SerializedProperty type = pair.FindPropertyRelative("SoundType");
				SerializedProperty audioClips = pair.FindPropertyRelative("Clips");
				SerializedProperty isLooping = pair.FindPropertyRelative("IsLooping");

				NautSoundType soundType = (NautSoundType) type.enumValueIndex;

				EditorGUILayout.BeginHorizontal();
				{
					EditorGUILayout.LabelField(new GUIContent($"{soundType}"), EditorStyles.boldLabel,
						GUILayout.MaxWidth(100.0f));
					GUILayout.Space(8.0f);

					EditorGUILayout.PropertyField(isLooping, new GUIContent("Loop"));
					GUILayout.Space(8.0f);
				}
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.PropertyField(audioClips, new GUIContent("Clips"), true);

				GUILayout.Space(20.0f);
			}
		}
	}
}