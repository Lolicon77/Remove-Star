using UnityEngine;

namespace L7 {
	public class AlwaysFaceCamera : MonoBehaviour {

		public new Camera camera;

		void Update() {
			if (!camera) {
				camera = Camera.main;
			}
			if (camera) {
				transform.LookAt(camera.transform);
			}
		}

	}
}