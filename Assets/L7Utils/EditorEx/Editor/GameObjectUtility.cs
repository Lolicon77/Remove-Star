using System;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace L7 {
	public class GameObjectUtility : MonoBehaviour {
		[MenuItem("GameObject/CustomCopy &d", false, 10)]
		static void MyCopy() {
			string name = Selection.activeGameObject.name;
			var prefabParent = PrefabUtility.GetPrefabParent(Selection.activeGameObject);
			Object instance;
			if (prefabParent) {
				instance = PrefabUtility.InstantiatePrefab(prefabParent);
			} else {
				instance = Instantiate(Selection.activeGameObject);
			}
			instance.name = name;
		}

		[MenuItem("GameObject/BatchModifyFileName", false, 10)]
		static void BatchModifyFileName() {
			foreach (var gameObject in Selection.gameObjects) {
				string name = gameObject.name;
				if (name.EndsWith(")")) {
					name = name.Remove(name.LastIndexOf("(", StringComparison.Ordinal) - 1);
					gameObject.name = name;
				}
			}
		}
	}
}