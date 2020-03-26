using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class CardPlayer : NetworkBehaviour
{

    public static CardPlayer LocalPlayer;
    public static List<CardPlayer> ServerPlayers = new List<CardPlayer>();

    public static bool AllPlayersReady
    {
        get
        {
            bool allready = true;
            ServerPlayers.ForEach(x => allready &= x.IsReady);
            return allready;
        }
    }

    public bool EnableInteractions = false;
    public bool IsReady { get; private set; }

    public CardHand Hand;
    public CardDeck Deck;

    public LayerMask Socket;
    private LayerMask PlayerHandLayer => Hand.PlayerHandLayer;
    public LayerMask CardDragLayer;
    [Range(0, 1f)]
    public float DragWhenMoving = 0.7f;
    [Range(0, 1f)]
    public float DragWhenSnapping = 0.1f;
    private float Drag => snapping ? DragWhenSnapping : DragWhenMoving;
    [Range(0, 2)]
    public float DragIntertiaMultiplier = 1;
    [Range(0, 50)]
    public float MaxIntertia = 25;

    private Camera Camera;
    private bool snapping;
    private CardSocket SnappedSocket;
    private bool dragging;
    private Transform draggedObject;

    public int ClientID { get; private set; }

    // Start is called before the first frame update
    private void Start()
    {
        Camera = Camera.main;
        if (isLocalPlayer)
        {
            LocalPlayer = this;
            //Got replaced by a non dynamic function BoardLogic.ClientRequestEndTurn()
            //DebugPanelInfo dpi = FindObjectOfType<DebugPanelInfo>();
            //if (dpi != null)
            //{
            //    dpi.RegisterEndTurn(EndTurn);
            //}
            CmdRequestClientID();
        }
        
        ServerPlayers.Add(this);
        Debug.Log($"isLocalPlayer: {isLocalPlayer}\nIsClient: {isClient}\nIsServer: {isServer}");
        
    }

    [Command]
    private void CmdRequestClientID()
    {
        ClientID = GetComponent<NetworkIdentity>().connectionToClient.connectionId;
        Debug.Log("ServerSide Client ID Set to: " + ClientID);
        TargetSetClientID(GetComponent<NetworkIdentity>().connectionToClient, ClientID);
    }

    [Command]
    private void CmdSetReady()
    {
        Debug.Log("Client is ready.");
        IsReady = true;
    }

    [TargetRpc]
    private void TargetSetClientID(NetworkConnection conn, int clientID)
    {
        ClientID = clientID;
        Debug.Log("Client ID Set to: " + ClientID);

        CmdSetReady();
        IsReady = true;
    }

    // Update is called once per frame
    private void Update()
    {
        if (EnableInteractions)
        {
            DragCard();
        }
    }

    [Server]
    public void DrawCard(int amount)
    {
        int cardsToDraw = Mathf.Min(Hand.CardSlotsFree, amount);
        if (cardsToDraw != amount)
        {
            Debug.Log("Adding To many cards to the hand, the maximum is: "+ Hand.MaxCardCount);
        }

        for (int i = 0; i < cardsToDraw; i++)
        {
            Card c = Deck.DrawCard();
            //Hand.AddCard(c);//Add the Card to the server

            Hand.AddToHand(c.GetComponent<NetworkIdentity>());
            Hand.TargetAddToHand(netIdentity.connectionToClient, c.GetComponent<NetworkIdentity>());//Add Card to the client
        }
    }

    [Client]
    public void EndTurn()
    {
        Debug.Log("Trying to end Turn as Client: " + ClientID);
        CmdEndTurn(ClientID);
    }

    [Command]
    private void CmdEndTurn(int clientID)
    {
        if (BoardLogic.Logic.CurrentTurnClient == clientID) BoardLogic.Logic.ServerEndTurn();
    }

    [TargetRpc]
    public void TargetSetCameraPosition(NetworkConnection target, Vector3 pos, Quaternion rot, bool InvertCardValues)
    {
        Camera.transform.SetPositionAndRotation(pos, rot);
        if (InvertCardValues) Hand.InvertValues();
    }

    private void DragCard()
    {
        if (!dragging)
        {
            if (HasClickedOnCard(out RaycastHit cardHit))
            {
                Card c = cardHit.transform.GetComponent<Card>();
                if (Hand.IsCardFromHand(c))
                    Hand.SetSelectedCard(c);
                else return;
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
                if (!snapping)
                {
                    Hand.SetSelectedCard(null);
                }
                else
                {
                    //Place card on board
                    Hand.RemoveCard(draggedObject.GetComponent<Card>());
                    SnappedSocket.DockTransform(draggedObject);
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
    }

    private Vector3 GetCardPosition()
    {
        if (HasPlacedCard(out RaycastHit socketPlace))
        {
            snapping = true;
            SnappedSocket = socketPlace.transform.GetComponent<CardSocket>();
            Vector3 socketPos = socketPlace.transform.position + Vector3.up;
            return socketPos;
        }

        snapping = false;
        SnappedSocket = null;
        return GetMousePositionOnDragLayer();
    }


    private Vector3 GetMousePositionOnDragLayer()
    {
        Ray r = Camera.ScreenPointToRay(Input.mousePosition);
        Debug.Log("DragLayer: " + LayerMask.LayerToName(UnityTrashWorkaround(CardDragLayer)));
        if (Physics.Raycast(r, out RaycastHit info, float.MaxValue, CardDragLayer))
        {

            return info.point;
        }

        return Vector3.zero;
    }

    public static int UnityTrashWorkaround(LayerMask lm)
    {
        int m = lm.value;
        int i = 1;
        if (lm == 0) return 0;
        while ((m = m >> 1) != 1)
        {
            i++;
        }

        return i;
    }

    private bool HasClickedOnCard(out RaycastHit info)
    {
        Ray r = Camera.ScreenPointToRay(Input.mousePosition);
        info = new RaycastHit();
        bool ret = Physics.Raycast(r, out info, float.MaxValue, PlayerHandLayer);
        return Input.GetMouseButtonDown(0) && ret;
    }

    private bool HasPlacedCard(out RaycastHit info)
    {
        Ray r = Camera.ScreenPointToRay(Input.mousePosition);
        info = new RaycastHit();
        return Physics.Raycast(r, out info, float.MaxValue, Socket) && MapTransformInfo.Instance.SocketManager.IsFromTeam(ClientID, info.transform);
    }
}
