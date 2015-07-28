using UnityEngine;
using UnityEngine.Networking;
using UnityStandardAssets.CrossPlatformInput;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;

public class FreemoveCamera : NetworkBehaviour {
	public float XSensitivity = 2f;
	public float YSensitivity = 2f;
	public static int MOVE_SPEED = 10;


	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		UpdateInput();
	}

	[Client]
	void UpdateInput() {
		UpdateMovement();
		UpdateMouselook();
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

		if (Input.GetKey(KeyCode.Space)) {
			transform.Translate(0f, deltaSpeed, 0f);
		}
	}

	void UpdateMouselook() {
		float yRot = CrossPlatformInputManager.GetAxis("Mouse X") * XSensitivity;
		float xRot = CrossPlatformInputManager.GetAxis("Mouse Y") * YSensitivity;

		transform.localRotation *= Quaternion.Euler (0f, yRot, 0f);
		transform.localRotation *= Quaternion.Euler (-xRot, 0f, 0f);
		transform.rotation = Quaternion.Euler(new Vector3(transform.localRotation.eulerAngles.x, transform.localRotation.eulerAngles.y, 0f));
	}
}
