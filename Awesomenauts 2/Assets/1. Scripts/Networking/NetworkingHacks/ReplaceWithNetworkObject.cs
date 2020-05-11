using Maps;
using Mirror;
using Networking;
using UnityEngine;


public class ReplaceWithNetworkObject : MonoBehaviour
{
	public SocketSide SocketSide;
	public GameObject Prefab;
	// Start is called before the first frame update
	void Start()
	{
		if (CardNetworkManager.Instance.IsServer)
		{
			GameObject obj = Instantiate(Prefab, transform.position, transform.rotation);
			obj.GetComponent<CardSocket>().SocketSide = SocketSide;
			NetworkServer.Spawn(obj);
		}
		Destroy(gameObject);
	}

	// Update is called once per frame
	void Update()
	{

	}
}
