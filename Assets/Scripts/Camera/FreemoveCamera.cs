using UnityEngine;
using System.Collections;

public class FreemoveCamera : MonoBehaviour {
	public static int MOVE_SPEED = 10;
	private static Object blockPrefab = Resources.Load("BlockPrototype04x04x04");

	Vector3 lastMousePos = new Vector3(0,0);
	bool mouseDown = false;

	// Use this for initialization
	void Start () {
		transform.Translate(20f, 25f, 0f);
		transform.Rotate(new Vector3(45, -45));
	}
	
	// Update is called once per frame
	void Update () {
		UpdateMovement();
		UpdateMouselook();
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
	}

	void UpdateMouselook() {
		if (Input.GetMouseButtonDown(0)) mouseDown = true;
		else if (Input.GetMouseButtonUp(0)) mouseDown = false;

		if (mouseDown) {
			Vector3 deltaPos = Input.mousePosition - lastMousePos;
			lastMousePos = Input.mousePosition;
			
			transform.Rotate(new Vector3(-deltaPos.y, deltaPos.x));
			transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0f);
		}
	}
}
