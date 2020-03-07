using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Create Card Design")]
public class CardDesign : ScriptableObject
{
	public string DesignName;
	public Mesh CardMesh;
	public Material CardMaterial;
}
