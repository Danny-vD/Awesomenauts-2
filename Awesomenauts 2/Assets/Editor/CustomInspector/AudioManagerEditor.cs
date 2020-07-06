using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using AwsomenautsCardGame.Audio;
using AwsomenautsCardGame.Enums.Audio;
using AwsomenautsCardGame.Structs.Audio;
using AwsomenautsCardGame.Utility;
using EventType = AwsomenautsCardGame.Enums.Audio.EventType;
using static Utility.EditorUtils;

namespace CustomInspector
{
	[CustomEditor(typeof(AudioManager))]
	public class AudioManagerEditor : Editor
	{
		// AudioManager
		private AudioManager audioManager;
		private bool showBusVolume;
		private bool[] busVolumeFoldout;

		// EventPaths
		private bool showEventPaths;
		private bool[] eventPathsFoldout;

		private bool showBuses;
		private bool[] busesFoldout;

		private bool showEmitterEvents;
		private bool[] emitterEventsFoldout;

		// Fmod
		private static Type eventBrowser;

		// Icons
		private static Texture[] eventIcon;
		private static Texture[] busIcon;

		//////////////////////////////////////////////////

		// AudioManager
		private SerializedProperty initialVolumes;

		// EventPaths
		private SerializedProperty events;
		private SerializedProperty buses;
		private SerializedProperty emitterEvents;

		private void OnEnable()
		{
			// AudioManager

			audioManager = target as AudioManager;
			audioManager.EventPaths.UpdateDictionaries();
			FakeDictionaryUtil.PopulateEnumDictionary<InitialValuePerBus, BusType, float>(audioManager.initialVolumes);

			initialVolumes = serializedObject.FindProperty("initialVolumes");
			busVolumeFoldout = new bool[initialVolumes.arraySize];

			// EventPaths
			events        = serializedObject.FindProperty("EventPaths.events");
			buses         = serializedObject.FindProperty("EventPaths.buses");
			emitterEvents = serializedObject.FindProperty("EventPaths.emitterEvents");

			eventPathsFoldout    = new bool[events.arraySize];
			busesFoldout         = new bool[buses.arraySize];
			emitterEventsFoldout = new bool[emitterEvents.arraySize];

			// Fmod
			Assembly assembly = Assembly.Load("Assembly-CSharp-Editor-firstpass");
			eventBrowser = assembly.GetType("FMODUnity.EventBrowser", true);

			// Icons
			eventIcon = new[]
			{
				GetTexture("Fmod/EventIcon.png"),
			};

			busIcon = new[]
			{
				GetTexture("AudioManager/BusIcon.png")
			};
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			DrawEventPaths();
			DrawBusPaths();

			DrawSeperatorLine(); //-------------------

			DrawEmitterEvents();

			DrawSeperatorLine(); //-------------------

			DrawInitialVolumes();

			DrawSeperatorLine(); //-------------------

			DrawEventBrowserButton();

			serializedObject.ApplyModifiedProperties();
		}

		private void DrawEventPaths()
		{
			if (IsFoldOut(ref showEventPaths, "Event Paths"))
			{
				DrawFoldoutKeyValueArray<EventType>(events, eventPathsFoldout, eventIcon, new GUIContent("Path"));
			}
		}

		private void DrawBusPaths()
		{
			if (IsFoldOut(ref showBuses, "Bus Paths"))
			{
				DrawFoldoutKeyValueArray<BusType>(buses, "key", "value", busesFoldout, busIcon, DrawElement);
			}

			void DrawElement(int index, SerializedProperty key, SerializedProperty value)
			{
				string path = value.stringValue;

				if (index == 0)
				{
					EditorGUILayout.LabelField("Path", path);
					return;
				}

				if (!path.StartsWith("Bus:/"))
				{
					value.stringValue = path.Insert(0, "Bus:/");
				}

				EditorGUILayout.PropertyField(value, new GUIContent("Path"));
			}
		}

		private void DrawEmitterEvents()
		{
			if (IsFoldOut(ref showEmitterEvents, "Emitters"))
			{
				DrawFoldoutKeyValueArray<EmitterType>(emitterEvents, emitterEventsFoldout,
					new GUIContent("Event to play", eventIcon[0]));
			}
		}

		private void DrawInitialVolumes()
		{
			if (IsFoldOut(ref showBusVolume, "Volume"))
			{
				DrawFoldoutKeyValueArray<BusType>(initialVolumes, "key", busVolumeFoldout, busIcon, DrawStruct);
			}

			void DrawStruct(int i, SerializedProperty @struct)
			{
				SerializedProperty value = @struct.FindPropertyRelative("value");
				SerializedProperty isMuted = @struct.FindPropertyRelative("isMuted");

				float volume = value.floatValue;

				EditorGUILayout.PropertyField(isMuted, new GUIContent("Mute"));
				value.floatValue = EditorGUILayout.Slider("Volume", volume, 0.0f, 1.0f);
			}
		}

		private static void DrawEventBrowserButton()
		{
			if (GUILayout.Button("Event Browser"))
			{
				EditorWindow window = EditorWindow.GetWindow(eventBrowser, false, "FMOD Events");
				window.minSize = new Vector2(280, 600);
				window.Show();
			}
		}
	}
}