using UnityEngine;
using System.Collections;

namespace L7 {
	public class BaseLayerManager<T> : Singleton<T> where T : BaseLayerManager<T> {

		public bool TryParseMaskToLayer(LayerMask layerMask, out int layer) {
			layer = 0;
			if (layerMask.value != 1 && !Mathf.IsPowerOfTwo(layerMask.value)) {
				return false;
			} else {
				layer = (int)Mathf.Log(layerMask.value, 2);
				return true;
			}
		}

		public void SetLayer(GameObject go, int layer) {
			go.layer = layer;
		}


		public void SetLayer(GameObject go, LayerMask layerMask) {
			int layer;
			if (TryParseMaskToLayer(layerMask, out layer)) {
				go.layer = layer;
			}
		}

		public void SetLayerRecursion(GameObject go, int layer) {
			go.layer = layer;
			foreach (Transform child in go.transform) {
				SetLayerRecursion(child.gameObject, layer);
			}
		}

		public void SetLayerRecursion(GameObject go, LayerMask layerMask) {
			int layer;
			if (TryParseMaskToLayer(layerMask, out layer)) {
				SetLayerRecursion(go, layer);
			}
		}

	}
}