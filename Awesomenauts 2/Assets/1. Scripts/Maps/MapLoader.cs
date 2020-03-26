using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class MapLoader : NetworkBehaviour
{
    [Server]
    public void LoadMap(int id)
    {
        Debug.Log("Loading Map: " + id);
        GameObject map = Instantiate(CardNetworkManager.Instance.AvailableMaps[id].Prefab);
        NetworkServer.Spawn(map);
    }
}
