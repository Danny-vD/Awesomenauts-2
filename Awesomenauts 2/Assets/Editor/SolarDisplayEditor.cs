using AwsomenautsCardGame.UI.DebugPanel;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SolarDisplay))]
public class SolarDisplayEditor : UnityEditor.Editor
{

	public override void OnInspectorGUI()
	{
		SolarDisplay scr = (SolarDisplay)target;
		base.OnInspectorGUI();

		if (scr.Mode == SolarDisplay.AnimationMode.AbsModSin || scr.Mode == SolarDisplay.AnimationMode.ModSin)
			scr.Modulus = EditorGUILayout.FloatField("Modulus: ", scr.Modulus, GUIStyle.none);
	}

}