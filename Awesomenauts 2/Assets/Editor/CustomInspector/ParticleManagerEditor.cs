using AwsomenautsCardGame.Enums.Particles;
using AwsomenautsCardGame.Particles;
using UnityEditor;
using UnityEngine;
using static Utility.EditorUtils;

namespace CustomInspector
{
	[CustomEditor(typeof(ParticleManager))]
	public class ParticleManagerEditor : Editor
	{
		private bool[] keyFoldOuts;
		
		////////////////////////////////////////////
		private SerializedProperty particlesPerParticleType;
		
		private void OnEnable()
		{
			ParticleManager manager = target as ParticleManager;
			manager.UpdateDictionaries();

			particlesPerParticleType = serializedObject.FindProperty("particlesPerParticleType");
			keyFoldOuts = new bool[particlesPerParticleType.arraySize];
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			DrawFoldoutKeyValueArray<ParticleType>(particlesPerParticleType, "key", "value", keyFoldOuts, DrawElement);
			
			serializedObject.ApplyModifiedProperties();
		}

		private static void DrawElement(int i, SerializedProperty key, SerializedProperty value)
		{
			float oldLabel = EditorGUIUtility.labelWidth;
			
			const string label = "Prefab";
			EditorGUIUtility.labelWidth = GetLabelWidth(label);
			EditorGUILayout.PropertyField(value, new GUIContent(label, GetTexture("Prefab Icon")));

			EditorGUIUtility.labelWidth = oldLabel;
		}
		
				
		private static float GetLabelWidth(string label)
		{
			return 15 * EditorGUI.indentLevel + 11 * label.Length;
		}
	}
}