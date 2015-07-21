﻿using UnityEngine;
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

		if (Physics.Raycast(ray, out hit, 100f, LayerMask.WORLD)) {
			GameObject obj = hit.transform.gameObject;
			Debug.Log(obj.name);
			if (obj.CompareTag("Chunk")) {
				int x = Mathf.FloorToInt(hit.point.x);
				int y = Mathf.FloorToInt(hit.point.y);
				int z = Mathf.FloorToInt(hit.point.z);

				// FB
				if (Mathf.Approximately(hit.point.z, z)) {
					y += 1;

					// Back
					if (world.GetBlock(x, y, z) == World.Block.AIR) {
						z -= 1;
					}
				}

				// NS
				else if (Mathf.Approximately(hit.point.y, y)) {
					// North
					if (world.GetBlock(x, y, z) == World.Block.AIR) {
						y -= 1;
					}
				}

				// EW
				else if (Mathf.Approximately(hit.point.x, x)) {
					y += 1;

					// East
					if (world.GetBlock(x, y, z) == World.Block.AIR) {
						x -= 1;
					}
				}

				if (x < 0) x = 0;
				if (y < 0) y = 0;
				if (z < 0) z = 0;

				Debug.Log("digging block: " + x + " " + y + " " + z);

				world.CmdRemoveBlock(x, y, z);
			}
		}
	}
}
