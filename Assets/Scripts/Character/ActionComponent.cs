using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class ActionComponent : NetworkBehaviour {
	private World world;

	// Use this for initialization
	void Start () {
		if (!isLocalPlayer)
			world = GameObject.FindGameObjectWithTag("World").GetComponent("World") as World;
	}
	
	// Update is called once per frame
	void Update () {
		if (isLocalPlayer) {
			HandleInput();
		}
	}

	void HandleInput() {
		if (Input.GetMouseButtonDown(0)) {
			CmdDig();
		}
	}

	[Command]
	public void CmdDig() {
		if (world == null) {
			world = GameObject.FindGameObjectWithTag("World").GetComponent("World") as World;
		}

		Vector3 origin = transform.position;
		Vector3 dir = transform.forward;
		Ray ray = new Ray(origin, dir);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit)) {
			GameObject obj = hit.transform.gameObject;
			if (obj.CompareTag("Chunk")) {
				int x = Mathf.FloorToInt(hit.point.x);
				int y = Mathf.FloorToInt(hit.point.y);
				int z = Mathf.FloorToInt(hit.point.z);

				world.RemoveBlock(x, y, z);
			}
		}
	}
}
