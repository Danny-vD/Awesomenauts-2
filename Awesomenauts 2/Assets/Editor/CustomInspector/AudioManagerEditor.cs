using System;
using System.Reflection;
using Audio;
using Enums.Audio;
using UnityEditor;
using UnityEngine;
using EventType = Enums.Audio.EventType;
using static Utility.EditorUtils;

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

		private static Type eventBrowser;
		private static Texture[] eventIcons;

		//////////////////////////////////////////////////

		private SerializedProperty events;
		private SerializedProperty emitterEvents;

		private void OnEnable()
		{
			audioManager = target as AudioManager;
			audioManager.eventPaths.UpdateDictionaries();

			events        = serializedObject.FindProperty("eventPaths.events");
			emitterEvents = serializedObject.FindProperty("eventPaths.emitterEvents");

			eventPathsFoldout    = new bool[events.arraySize];
			emitterEventsFoldout = new bool[emitterEvents.arraySize];

			Assembly assembly = Assembly.Load("Assembly-CSharp-Editor-firstpass");
			eventBrowser = assembly.GetType("FMODUnity.EventBrowser", true);

			eventIcons = new[]
			{
				GetTexture("Fmod/EventIcon.png"),
			};
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			DrawEventPaths();

			EditorGUILayout.Space();
			DrawSeperatorLine();

			DrawEmitterEvents();

			DrawSeperatorLine();

			DrawPreviewEvents();
			
			serializedObject.ApplyModifiedProperties();
		}

		private void DrawEventPaths()
		{
			if (IsFoldOut(ref showEventPaths, "Event Paths"))
			{
				DrawFoldoutKeyValueArray<EventType>(events, eventPathsFoldout, eventIcons, new GUIContent("Path"));
			}
		}

		private void DrawEmitterEvents()
		{
			if (IsFoldOut(ref showEmitterEvents, "Emitters"))
			{
				DrawFoldoutKeyValueArray<EmitterType>(emitterEvents, emitterEventsFoldout, new GUIContent("Event to play", eventIcons[0]));
			}
		}

		private static void DrawPreviewEvents()
		{
			if (GUILayout.Button("Preview events"))
			{
				EditorWindow.GetWindow(eventBrowser);
			}
		}
	}
}