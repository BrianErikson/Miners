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
		else if (Input.GetMouseButtonDown(1)) {
			Place();
		}
	}

	[Client]
	public void Dig() {
		Transform mainTransform = Camera.main.transform;
		Vector3 origin = mainTransform.position;
		Vector3 dir = mainTransform.forward;
		Ray ray = new Ray(origin, dir);
		RaycastHit hit;

		if (Physics.Raycast(ray, out hit, 100f, LayerMask.WORLD)) {
			GameObject obj = hit.transform.gameObject;
			if (obj.CompareTag("Chunk")) {
				Vector3 blockCoord = Chunk.GetHitBlock(hit);

				Debug.Log("digging block: " + blockCoord.x + " " + blockCoord.y + " " + blockCoord.z);

				world.CmdRemoveBlock((int)blockCoord.x, (int)blockCoord.y, (int)blockCoord.z);
			}
		}
	}

	[Client]
	public void Place() {
		Transform mainTransform = Camera.main.transform;
		Vector3 origin = mainTransform.position;
		Vector3 dir = mainTransform.forward;
		Ray ray = new Ray(origin, dir);
		RaycastHit hit;
		
		if (Physics.Raycast(ray, out hit, 100f, LayerMask.WORLD)) {
			GameObject obj = hit.transform.gameObject;
			if (obj.CompareTag("Chunk")) {
				Vector3 blockCoord = hit.point;
				blockCoord += hit.normal * 0.5f;
				blockCoord.x = Mathf.Round(blockCoord.x);
				blockCoord.y = Mathf.Round(blockCoord.y);
				blockCoord.z = Mathf.Round(blockCoord.z);
				
				Debug.Log("Placing block: " + blockCoord.x + " " + blockCoord.y + " " + blockCoord.z);
				
				world.CmdAddBlock(World.Block.GRASS, (int)blockCoord.x, (int)blockCoord.y, (int)blockCoord.z);
			}
		}
	}
}
