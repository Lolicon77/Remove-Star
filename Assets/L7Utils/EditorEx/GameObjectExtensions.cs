using System;
using System.Runtime.InteropServices;

namespace UnityEngine {
	public static class GameObjectExtensions {
		// Methods
		public static GameObject AddChild(this GameObject gObj, string name, [Optional, DefaultParameterValue(null)] Vector3? localPosition, [Optional, DefaultParameterValue(null)] Quaternion? localRotation, [Optional, DefaultParameterValue(null)] Vector3? localScale) {
			GameObject obj2 = new GameObject(name);
			Transform transform = obj2.transform;
			if (!localPosition.HasValue) {
				localPosition = new Vector3?(Vector3.zero);
			}
			if (!localRotation.HasValue) {
				localRotation = new Quaternion?(Quaternion.identity);
			}
			if (!localScale.HasValue) {
				localScale = new Vector3?(Vector3.one);
			}
			transform.SetParent(gObj.transform);
			transform.localPosition = localPosition.Value;
			transform.localRotation = localRotation.Value;
			transform.localScale = localScale.Value;
			return obj2;
		}

		/// <summary>
		/// 设置层级
		/// </summary>
		/// <param name="go">GameObject</param>
		/// <param name="layer">int</param>
		[Obsolete("Use LayerManager.SetLayerRecursion instead.")]
		public static void SetAllLayer(this GameObject go, int layer) {
			go.layer = layer;
			foreach (Transform child in go.transform) {
				SetAllLayer(child.gameObject, layer);
			}
		}
	}

	[AttributeUsage(AttributeTargets.Parameter)]
	public sealed class DefaultParameterValueAttribute : Attribute {
		// Fields
		private object value;

		// Methods
		public DefaultParameterValueAttribute(object value) {
			this.value = value;
		}

		// Properties
		public object Value {
			get {
				return this.value;
			}
		}
	}




}

