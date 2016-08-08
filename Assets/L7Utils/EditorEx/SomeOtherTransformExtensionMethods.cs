namespace UnityEngine{
	
	
	public static class SomeOtherTransformExtensionMethods {

		public static void SetParentAndResetArgs(this Transform transform, Transform parent) {
			transform.SetParent(parent);
			transform.localPosition = Vector3.zero;
			transform.localRotation = Quaternion.identity;
			transform.localScale = Vector3.one;
		}
		
		public static void SmoothSetLocalEulerAngleX(this Transform transform, float angle, float interpolation) {
			transform.localEulerAngles = new Vector3( Mathf.LerpAngle(transform.localEulerAngles.x, angle, interpolation), transform.localEulerAngles.y,transform.localEulerAngles.z);
		}
		
		public static void SmoothSetLocalEulerAngleY(this Transform transform, float angle, float interpolation) {
			transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, Mathf.LerpAngle(transform.localEulerAngles.y, angle, interpolation), transform.localEulerAngles.z);
		}
		
		public static void SmoothSetLocalEulerAngleZ(this Transform transform, float angle, float interpolation) {
			transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, Mathf.LerpAngle(transform.localEulerAngles.z, angle, interpolation));
		}

		public static void SmoothSetLocalEulerAngle(this Transform transform, Vector3 eularAngle, float interpolation) {
			transform.SmoothSetLocalEulerAngleX(eularAngle.x, interpolation);
			transform.SmoothSetLocalEulerAngleY(eularAngle.y, interpolation);
			transform.SmoothSetLocalEulerAngleZ(eularAngle.z, interpolation);	
		}

	}	
}

