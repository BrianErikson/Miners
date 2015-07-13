using UnityEngine;
using System.Collections;

public class FreemoveCamera : MonoBehaviour {
	public static int MOVE_SPEED = 10;
	private static Object blockPrefab = Resources.Load("BlockPrototype04x04x04");

	// Use this for initialization
	void Start () {
		transform.Translate(20f, 25f, 0f);
		transform.Rotate(new Vector3(45, -45));
	}
	
	// Update is called once per frame
	void Update () {
		UpdateMovement();
		UpdateActions();
	}

	void UpdateActions() {
		if (Input.GetKey(KeyCode.Space)) {
			Vector3 origin = transform.position;
			Vector3 dir = transform.forward;
			Ray ray = new Ray(origin, dir);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit)) {
				GameObject cube = (GameObject) Instantiate(blockPrefab);
				cube.transform.position = hit.point;
				cube.transform.localScale.Set(0.01f, 0.01f, 0.01f);
			}
		}
	}

	void UpdateMovement() {
		float deltaSpeed = MOVE_SPEED * Time.deltaTime;

		if (Input.GetKey(KeyCode.W)) {
			transform.Translate(0f, 0f, deltaSpeed);
		}
		else if (Input.GetKey(KeyCode.S)) {
			transform.Translate(0f, 0f, -deltaSpeed);
		}
		
		if (Input.GetKey(KeyCode.D)) {
			transform.Translate(deltaSpeed, 0f, 0f);
		}
		else if (Input.GetKey(KeyCode.A)) {
			transform.Translate(-deltaSpeed, 0f, 0f);
		}
		
		if (Input.GetKey(KeyCode.LeftShift)) {
			transform.Translate(0f, deltaSpeed, 0f);
		}
		else if (Input.GetKey(KeyCode.LeftControl)) {
			transform.Translate(0f, -deltaSpeed, 0f);
		}
		
		if (Input.GetKey(KeyCode.R)) {
			transform.Rotate(new Vector3(deltaSpeed, 0f));
		}
		else if (Input.GetKey(KeyCode.F)) {
			transform.Rotate(new Vector3(-deltaSpeed, 0f));
		}
		
		if (Input.GetKey(KeyCode.E)) {
			transform.Rotate(new Vector3(0f, deltaSpeed, 0f));
		}
		else if (Input.GetKey(KeyCode.Q)) {
			transform.Rotate(new Vector3(0f, -deltaSpeed, 0f));
		}
		
		if (Input.GetKey(KeyCode.C)) {
			transform.Rotate(new Vector3(0f, 0f, deltaSpeed));
		}
		else if (Input.GetKey(KeyCode.X)) {
			transform.Rotate(new Vector3(0f, 0f, -deltaSpeed));
		}
	}
}
