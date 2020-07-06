using System;
using AwsomenautsCardGame.UI.TooltipSystem;
using UnityEditor;

[CustomEditor(typeof(TooltipScript))]
public class ToolTipEditor : UnityEditor.Editor
{

	public override void OnInspectorGUI()
	{
		TooltipScript scr = (TooltipScript)target;
		for (int i = 0; i < scr.Tooltips.Count; i++)
		{
			TooltipScript.Tooltip t = scr.Tooltips[i];
			t.Name = Enum.GetName(typeof(TooltipType), t.Type);
			scr.Tooltips[i] = t;
		}


		base.OnInspectorGUI();
	}

}