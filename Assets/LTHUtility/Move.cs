using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour {
	public Vector3 speed = new Vector3(0f, 0f, 10f);
	private Vector3 originalPosition;

	void Awake() {
		originalPosition = transform.localPosition;
	}

	void OnEnable() {
		transform.localPosition = originalPosition;
	}


	void Update() {
		transform.Translate(speed * Time.deltaTime);
	}

}
