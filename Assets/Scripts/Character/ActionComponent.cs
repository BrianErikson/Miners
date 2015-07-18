using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class ActionComponent : NetworkBehaviour {
	private World world;

	// Use this for initialization
	void Start () {
		world = GameObject.FindGameObjectWithTag("World").GetComponent("World") as World;
	}
	
	// Update is called once per frame
	void Update () {
		HandleInput();
	}

	[Client]
	void HandleInput() {
		if (Input.GetMouseButtonDown(0)) {
			Dig();
		}
	}

	[Client]
	public void Dig() {
		Transform mainTransform = Camera.main.transform;
		Vector3 origin = mainTransform.position;
		Vector3 dir = mainTransform.forward;
		Ray ray = new Ray(origin, dir);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit)) {
			GameObject obj = hit.transform.gameObject;
			if (obj.CompareTag("Chunk")) {
				int x = Mathf.RoundToInt(hit.point.x);
				int y = Mathf.RoundToInt(hit.point.y);
				int z = Mathf.RoundToInt(hit.point.z);
				world.CmdRemoveBlock(x, y, z);
			}
		}
	}
}
