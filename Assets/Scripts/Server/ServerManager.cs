using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class ServerManager : NetworkManager {
	public GameObject worldPrefab;

	// Use this for initialization
	void Start () {
		//GameObject worldPrefab = Resources.Load("World") as GameObject;
		GameObject world = GameObject.Instantiate(worldPrefab);
		NetworkServer.Spawn(world);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
