using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class CardPlayer : MonoBehaviour, IPlayer
{
	//IPlayer Implementation
	public IHand Hand { get; set; }
	public IDeck Deck { get; set; }
	public Camera PlayerCamera;
	public Transform CameraTransform => PlayerCamera.transform;
	public Transform HandAnchorPoint => transform;

	public void Initialize(GameSettingsObject settings, IHand hand, IDeck deck)
	{
		//TODO Empty for now.
		Hand = hand;
		Deck = deck;
		Debug.Log("Player Socket ID: " + Socket.value);
		Debug.Log("Player Socket: " + LayerMask.LayerToName(CardHand.UnityTrashWorkaround(Socket)));
		Debug.Log("Player CardDragLayer ID: " + CardDragLayer.value);
		Debug.Log("Player v: " + LayerMask.LayerToName(CardHand.UnityTrashWorkaround(CardDragLayer)));
		Debug.Log("Player Card Hand Layer ID: " + PlayerCardLayer.value);
		Debug.Log("Player Card Hand Layer: " + LayerMask.LayerToName(CardHand.UnityTrashWorkaround(PlayerCardLayer)));
		Hand.SetLayer(PlayerCardLayer);
		Hand.SetAnchor(HandAnchorPoint);
		Fill();

	}


	//Card Placing Code
	public LayerMask Socket;
	public LayerMask PlayerCardLayer;
	public LayerMask CardDragLayer;
	[Range(0, 1f)]
	public float DragWhenMoving = 0.7f;

	[Range(0, 1f)]
	public float DragWhenSnapping = 0.1f;

	public float DragIntertiaMultiplier = 1;
	public float MaxIntertia = 25;

	private bool dragging;
	private float Drag => snapping ? DragWhenSnapping : DragWhenMoving;
	private bool snapping;

	private Transform draggedObject;


	// Update is called once per frame
	private void Update()
	{
		if (!dragging)
		{
			if (HasClickedOnCard(out RaycastHit cardHit))
			{
				dragging = true;
				Hand.SetSelectedCard(cardHit.transform.GetComponent<Card>());
				draggedObject = cardHit.transform;
				Debug.Log("Clicked On Card");
			}
		}
		else
		{
			if (!Input.GetMouseButton(0))
			{
				dragging = false;
				if (!snapping)
				{
					Hand.SetSelectedCard(null);
				}
				else
				{
					Hand.RemoveCard(draggedObject.GetComponent<Card>());
					Hand.SetSelectedCard(null);
				}
				Debug.Log("Released Card");
			}
			else
			{
				Vector3 dir = GetCardPosition() - draggedObject.position;
				float m = Mathf.Clamp(dir.magnitude * DragIntertiaMultiplier, 0, MaxIntertia);

				dir *= Drag;
				draggedObject.position += dir;
				Vector3 axis = Vector3.Cross(Vector3.up, dir);
				Quaternion q = Quaternion.AngleAxis(m, axis);


				draggedObject.rotation = q;
			}
		}

		Hand.UpdateCardPositions();
	}

	public void Fill()
	{
		Debug.Log("Filling Deck: "+PlayerCamera.gameObject.name);
		while (Hand.CanAddCard())
			Hand.AddCard(Deck.DrawCard());
	}

	private Vector3 GetCardPosition()
	{
		if (HasPlacedCard(out RaycastHit socketPlace))
		{
			snapping = true;
			Vector3 socketPos = socketPlace.transform.position + Vector3.up;
			return socketPos;
		}

		snapping = false;
		return GetMousePositionOnDragLayer();
	}


	private Vector3 GetMousePositionOnDragLayer()
	{
		Ray r = PlayerCamera.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(r, out RaycastHit info, float.MaxValue, CardDragLayer))
		{
			return info.point;
		}

		return Vector3.zero;
	}

	private bool HasClickedOnCard(out RaycastHit info)
	{
		Ray r = PlayerCamera.ScreenPointToRay(Input.mousePosition);
		info = new RaycastHit();
		bool ret = Physics.Raycast(r, out info, float.MaxValue, PlayerCardLayer);

		return Input.GetMouseButtonDown(0) && ret;
	}

	private bool HasPlacedCard(out RaycastHit info)
	{
		Ray r = PlayerCamera.ScreenPointToRay(Input.mousePosition);
		info = new RaycastHit();
		return Physics.Raycast(r, out info, float.MaxValue, Socket);
	}
}
