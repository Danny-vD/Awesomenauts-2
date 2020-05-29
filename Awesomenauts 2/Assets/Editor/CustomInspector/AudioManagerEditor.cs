using System;
using Audio;
using UnityEditor;

namespace CustomInspector
{
	[CustomEditor(typeof(AudioManager))]
	public class AudioManagerEditor : Editor
	{
		private bool eventPathsFoldout = false;

		private AudioManager audioManager;
		
		private void OnEnable()
		{
			audioManager = (target as AudioManager);
			audioManager.eventPaths.UpdateDictionaries();
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			if (IsFoldOut(ref eventPathsFoldout, "Event paths"))
			{
				
			}
			
			serializedObject.ApplyModifiedProperties();
		}

		private static bool IsFoldOut(ref bool foldout, string label = "")
		{
			return foldout = EditorGUILayout.Foldout(foldout, label);
		}
	}
}