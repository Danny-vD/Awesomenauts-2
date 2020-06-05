using System;
using System.Linq;
using Audio;
using Enums.Audio;
using UnityEditor;
using UnityEngine;
using VDFramework.Extensions;
using EventType = Enums.Audio.EventType;

namespace CustomInspector
{
	[CustomEditor(typeof(AudioManager))]
	public class AudioManagerEditor : Editor
	{
		private AudioManager audioManager;

		private bool showEventPaths;
		private bool[] eventPathsFoldout;

		private bool showEmitterEvents;
		private bool[] emitterEventsFoldout;

		//////////////////////////////////////////////////

		private SerializedProperty events;
		private SerializedProperty emitterEvents;

		private void OnEnable()
		{
			audioManager = (target as AudioManager);
			audioManager.eventPaths.UpdateDictionaries();

			events        = serializedObject.FindProperty("eventPaths.events");
			emitterEvents = serializedObject.FindProperty("eventPaths.emitterEvents");

			eventPathsFoldout    = new bool[events.arraySize];
			emitterEventsFoldout = new bool[emitterEvents.arraySize];
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			DrawEventPaths();

			EditorGUILayout.Space();
			DrawUILine(new Color(0.4f, 0.4f, 0.4f), 1);
			
			DrawEmitterEvents();

			serializedObject.ApplyModifiedProperties();
		}

		private void DrawEventPaths()
		{
			if (IsFoldOut(ref showEventPaths, "Event Paths"))
			{
				DrawFoldoutKeyValueArray<EventType>(events, eventPathsFoldout, "Path");
			}
		}

		private void DrawEmitterEvents()
		{
			if (IsFoldOut(ref showEmitterEvents, "Emitters"))
			{
				DrawFoldoutKeyValueArray<EmitterType>(emitterEvents, emitterEventsFoldout, "Event to play");
			}
		}

		private static bool IsFoldOut(ref bool foldout, string label = "")
		{
			return foldout = EditorGUILayout.Foldout(foldout, label);
		}

		private static void DrawFoldoutKeyValueArray<TEnum>(SerializedProperty array, bool[] foldouts, string valueLabel)
			where TEnum : struct, Enum
		{
			DrawKeyValueArray(array, DrawFoldout);

			void DrawFoldout(int i, SerializedProperty key, SerializedProperty value)
			{
				string enumString = ConvertIntToEnumString<TEnum>(key.enumValueIndex);

				if (IsFoldOut(ref foldouts[i], enumString))
				{
					++EditorGUI.indentLevel;
					EditorGUILayout.PropertyField(value, new GUIContent(valueLabel));
					--EditorGUI.indentLevel;
				}
			}
		}

		private static void DrawKeyValueArray(SerializedProperty       array,
			Action<int, SerializedProperty, SerializedProperty> elementAction)
		{
			++EditorGUI.indentLevel;

			for (int i = 0; i < array.arraySize; i++)
			{
				SerializedProperty @struct = array.GetArrayElementAtIndex(i);
				SerializedProperty key = @struct.FindPropertyRelative("key");
				SerializedProperty value = @struct.FindPropertyRelative("value");

				elementAction(i, key, value);
			}

			--EditorGUI.indentLevel;
		}

		private static string ConvertIntToEnumString<TEnum>(int integer)
			where TEnum : struct, Enum
		{
			return default(TEnum).GetValues().ElementAt(integer).ToString().ReplaceUnderscoreWithSpace();
		}

		private static void EnumPopup<TEnum>(ref TEnum enumValue, string label)
			where TEnum : struct, Enum
		{
			Enum.TryParse(EditorGUILayout.EnumPopup(label, enumValue).ToString(), out enumValue);
		}

		private static void DrawUILine(Color color, int thickness = 2, int padding = 10)
		{
			Rect r = EditorGUILayout.GetControlRect(GUILayout.Height(padding + thickness));
			r.height =  thickness;
			r.y      += padding / 2;
			r.x      -= 2;
			r.width  += 6;
			EditorGUI.DrawRect(r, color);
		}
	}
}