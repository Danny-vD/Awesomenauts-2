//using System;
//using UnityEditor;
//using UnityEngine;
//
//namespace CustomInspector.AudioManager
//{
//	[CustomEditor(typeof(Audio.AudioManager))]
//	public class AudioManagerEditor : Editor
//	{
//		// Link NautAudioClip ScriptableObject to Awesomenaut enum	
//		// link scriptableObject to enum
//
//		//private SerializedProperty;
//		
//		private void OnEnable()
//		{
//			
//		}
//
//		public override void OnInspectorGUI()
//		{
//			serializedObject.Update();
//
//			
//			
//			serializedObject.ApplyModifiedProperties();
//		}
//
//		private void ShowListEnumScriptableObject<TEnum, TSObject>()
//			where TEnum : struct, Enum
//			where TSObject : ScriptableObject
//		{
//			
//		}
//	}
//}