using UnityEngine;

namespace L7 {
	/// <summary>
	/// 用于重置回收池中对象的Transform
	/// </summary>
	public class ResetTransform : MonoBehaviour {

		private Vector3 originalPosition;
		private Quaternion originalRotation;
		private Vector3 originalScale;

		void Awake() {
			originalPosition = transform.localPosition;
			originalRotation = transform.localRotation;
			originalScale = transform.localScale;
		}


		void OnDisable() {
			Reset();
		}

		public void Reset() {
			transform.localPosition = originalPosition;
			transform.localRotation = originalRotation;
			transform.localScale = originalScale;
		}

	}
}