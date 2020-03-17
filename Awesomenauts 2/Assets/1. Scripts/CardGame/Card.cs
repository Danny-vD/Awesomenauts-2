using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour, ICard
{
	public CardInfo CardInfo;
	public CardInfo PlayerStatistics => CardInfo;
	public Transform CardTransform => transform;
	public int CardID => GetInstanceID();

	public IEffect[] Effects;
	public IEffect[] CardEffects => Effects;

	public MeshRenderer iconMeshR;

	// Start is called before the first frame update
	void Start()
	{
		MeshFilter cardMeshF = GetComponent<MeshFilter>();
		MeshRenderer cardMeshR = GetComponent<MeshRenderer>();
		cardMeshF.mesh = CardInfo.CardDesign.CardMesh;
		cardMeshR.materials[0] = Instantiate(CardInfo.CardDesign.CardMaterial);
		iconMeshR.materials[0].mainTexture = CardInfo.CardIcon;
	}

	// Update is called once per frame
	void Update() { }
}