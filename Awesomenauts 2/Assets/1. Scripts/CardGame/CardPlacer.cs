using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class CardPlacer : MonoBehaviour
{
	public Camera camera;
	public LayerMask Socket;
	public LayerMask CardLayer;
	public LayerMask CardDragLayer;
	public GameObject Prefab;
	[Range(0, 1f)]
	public float DragWhenMoving = 0.7f;

	[Range(0, 1f)]
	public float DragWhenSnapping = 0.1f;

	private bool dragging;
	private float Drag => snapping ? DragWhenSnapping : DragWhenMoving;
	private bool snapping;

	private Transform draggedObject;

	// Start is called before the first frame update
	private void Start() { }

	// Update is called once per frame
	private void Update()
	{
		if (!dragging)
		{
			if (HasClickedOnCard(out RaycastHit cardHit))
			{
				dragging = true;
				draggedObject = cardHit.transform;
				Debug.Log("Clicked On Card");
			}
		}
		else
		{
			if (!Input.GetMouseButton(0))
			{
				dragging = false;
				Debug.Log("Released Card");
			}
			else
			{

				Vector3 dir = GetCardPosition() - draggedObject.position;
				dir *= Drag;
				draggedObject.position += dir;
			}

		}

	}

	private Vector3 GetCardPosition()
	{
		if (HasPlacedCard(out RaycastHit socketPlace))
		{
			snapping = true;
			return socketPlace.transform.position + Vector3.up;
		}

		snapping = false;
		return GetMousePositionOnDragLayer();
	}


	private Vector3 GetMousePositionOnDragLayer()
	{
		Ray r = camera.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(r, out RaycastHit info, float.MaxValue, CardDragLayer))
		{
			return info.point;
		}
		return Vector3.zero;
	}

	private bool HasClickedOnCard(out RaycastHit info)
	{
		Ray r = camera.ScreenPointToRay(Input.mousePosition);
		info = new RaycastHit();
		return Input.GetMouseButtonDown(0) && Physics.Raycast(r, out info, float.MaxValue, CardLayer);
	}

	private bool HasPlacedCard(out RaycastHit info)
	{
		Ray r = camera.ScreenPointToRay(Input.mousePosition);
		info = new RaycastHit();
		return Physics.Raycast(r, out info, float.MaxValue, Socket);
	}
}