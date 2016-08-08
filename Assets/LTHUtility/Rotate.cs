using UnityEngine;
using System.Collections;

public class Rotate : MonoBehaviour {
		public Vector3 eulerAngle;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
				transform.Rotate (eulerAngle*Time.deltaTime);
	}
}
