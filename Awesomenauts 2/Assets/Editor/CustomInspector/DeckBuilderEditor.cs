using AwsomenautsCardGame.DeckBuilder;
using AwsomenautsCardGame.Enums.Cards;
using UnityEditor;
using UnityEngine;
using static Utility.EditorUtils;

namespace CustomInspector
{
	[CustomEditor(typeof(DeckBuilder))]
	public class DeckBuilderEditor : Editor
	{
		private DeckBuilder deckBuilder;

		private bool deckRequirementsFoldout;
		private bool minMaxCardFoldOut;

		private bool[] cardTypeFoldouts;
		private bool[] uiElementFoldouts;

		private Texture[] cardTypeIcons;
		private Texture uiIcon;

		//////////////////////////////////////////////////

		private SerializedProperty currentDeckParent;
		private SerializedProperty availableCardsParent;

		// deck requirements
		private SerializedProperty maxSameCard;
		private SerializedProperty maxTotalCards;

		private SerializedProperty minMaxPerCardType;

		// deck requirements UI
		private SerializedProperty sameCardCountUI;
		private SerializedProperty totalCardCountUI;
		private SerializedProperty uiElements;

		private void OnEnable()
		{
			deckBuilder = target as DeckBuilder;
			deckBuilder.UpdateDictionaries();

			currentDeckParent    = serializedObject.FindProperty("currentDeckParent");
			availableCardsParent = serializedObject.FindProperty("availableCardsParent");

			// deck requirements
			maxSameCard       = serializedObject.FindProperty("deckRequirements.maxSameCard");
			maxTotalCards     = serializedObject.FindProperty("deckRequirements.maxTotalCards");
			minMaxPerCardType = serializedObject.FindProperty("deckRequirements.minMaxPerCardTypes");

			// deck requirements UI
			sameCardCountUI  = serializedObject.FindProperty("deckRequirements.UI.sameCardCountUI");
			totalCardCountUI = serializedObject.FindProperty("deckRequirements.UI.totalCardCountUI");
			uiElements       = serializedObject.FindProperty("deckRequirements.UI.uiElements");

			cardTypeFoldouts  = new bool[minMaxPerCardType.arraySize];
			uiElementFoldouts = new bool[minMaxPerCardType.arraySize];

			cardTypeIcons = new[]
			{
				GetTexture("Deckbuilder/action_card_icon.png"),
				GetTexture("Deckbuilder/melee_card_icon.png"),
				GetTexture("Deckbuilder/ranged_card_icon.png"),
				GetTexture("Deckbuilder/tank_card_icon.png"),
			};

			uiIcon = GetTexture("DeckBuilder/UIIcon.png");
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			DrawCardParents();

			DrawSeperatorLine();

			if (IsFoldOut(ref deckRequirementsFoldout, GetTexture("DeckBuilder/RequirementsIcon.png"), "Requirements"))
			{
				++EditorGUI.indentLevel;
				DrawDeckRequirements();
				--EditorGUI.indentLevel;
			}

			serializedObject.ApplyModifiedProperties();
		}

		private void DrawCardParents()
		{
			EditorGUILayout.LabelField("The parents of the card collections");

			EditorGUILayout.PropertyField(currentDeckParent, new GUIContent("current deck"));
			EditorGUILayout.PropertyField(availableCardsParent, new GUIContent("available cards"));
		}

		private void DrawDeckRequirements()
		{
			float oldLabelWidth = EditorGUIUtility.labelWidth;
			float fittingLabelWidth = 15 * EditorGUI.indentLevel + 15;
			
			EditorGUILayout.BeginHorizontal();
			{
				EditorGUILayout.PropertyField(maxSameCard, new GUIContent("max same card"));

				EditorGUIUtility.labelWidth = fittingLabelWidth;
				EditorGUILayout.PropertyField(sameCardCountUI, new GUIContent(uiIcon));
			}
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			{
				EditorGUIUtility.labelWidth = oldLabelWidth;
				EditorGUILayout.PropertyField(maxTotalCards, new GUIContent("max total card"));

				EditorGUIUtility.labelWidth = fittingLabelWidth;
				EditorGUILayout.PropertyField(totalCardCountUI, new GUIContent(uiIcon));
			}
			EditorGUILayout.EndHorizontal();

			EditorGUIUtility.labelWidth = oldLabelWidth;
			

			if (IsFoldOut(ref minMaxCardFoldOut, "Min-Max per card type"))
			{
				++EditorGUI.indentLevel;
				DrawFoldoutKeyValueArray<CardType>(minMaxPerCardType, "cardType", "minMax", cardTypeFoldouts,
					cardTypeIcons, DrawValueElement);
				--EditorGUI.indentLevel;
			}

			void DrawValueElement(int index, SerializedProperty key, SerializedProperty value)
			{
				value.vector2IntValue = DrawVector2Int(value.vector2IntValue.x, "Min", value.vector2IntValue.y, "Max");
				++EditorGUI.indentLevel;

				DrawArray(uiElements.GetArrayElementAtIndex(index).FindPropertyRelative("value"), "UI elements",
					"Amount", DrawElement, ref uiElementFoldouts[index]);

				--EditorGUI.indentLevel;

				void DrawElement(int i, SerializedProperty element)
				{
					EditorGUILayout.PropertyField(element, new GUIContent(uiIcon));
				}
			}
		}
	}
}