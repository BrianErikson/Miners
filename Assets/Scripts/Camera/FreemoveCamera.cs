using UnityEngine;
using System.Collections;

public class FreemoveCamera : MonoBehaviour {
	public static int MOVE_SPEED = 1;

	// Use this for initialization
	void Start () {
		transform.Translate(20f, 25f, 0f);
		transform.Rotate(new Vector3(45, -45));
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.W)) {
			transform.Translate(0f, 0f, MOVE_SPEED);
		}
		else if (Input.GetKey(KeyCode.S)) {
			transform.Translate(0f, 0f, -MOVE_SPEED);
		}

		if (Input.GetKey(KeyCode.D)) {
			transform.Translate(MOVE_SPEED, 0f, 0f);
		}
		else if (Input.GetKey(KeyCode.A)) {
			transform.Translate(-MOVE_SPEED, 0f, 0f);
		}

		if (Input.GetKey(KeyCode.LeftShift)) {
			transform.Translate(0f, MOVE_SPEED, 0f);
		}
		else if (Input.GetKey(KeyCode.LeftControl)) {
			transform.Translate(0f, -MOVE_SPEED, 0f);
		}

		if (Input.GetKey(KeyCode.R)) {
			transform.Rotate(new Vector3(MOVE_SPEED, 0f));
		}
		else if (Input.GetKey(KeyCode.F)) {
			transform.Rotate(new Vector3(-MOVE_SPEED, 0f));
		}

		if (Input.GetKey(KeyCode.E)) {
			transform.Rotate(new Vector3(0f, MOVE_SPEED, 0f));
		}
		else if (Input.GetKey(KeyCode.Q)) {
			transform.Rotate(new Vector3(0f, -MOVE_SPEED, 0f));
		}

		if (Input.GetKey(KeyCode.C)) {
			transform.Rotate(new Vector3(0f, 0f, MOVE_SPEED));
		}
		else if (Input.GetKey(KeyCode.X)) {
			transform.Rotate(new Vector3(0f, 0f, -MOVE_SPEED));
		}
	}
}
