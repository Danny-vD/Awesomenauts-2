using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTransformInfo : MonoBehaviour
{
    [Serializable]
    public struct PlayerTransformInfo
    {
        public Transform DeckPosition;
        public Transform HandPosition;
        public Transform CameraPosition;
        public Transform GravePosition;
    }

    public PlayerTransformInfo[] PlayerTransformInfos;
    public SocketManager SocketManager;

    public static MapTransformInfo Instance;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
