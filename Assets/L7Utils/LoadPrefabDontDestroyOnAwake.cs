using System.Collections.Generic;
using UnityEngine;

namespace L7 {
	public class LoadPrefabDontDestroyOnAwake : MonoBehaviour {
		[SerializeField]
		private bool _asParent;

		private static List<GameObject> alreadyInstantiatePrefabList;

		public GameObject[] Prefabs;

		void Awake() {
			if (alreadyInstantiatePrefabList == null) {
				alreadyInstantiatePrefabList = new List<GameObject>();
			}
			if (Prefabs != null) {
				foreach (var prefab in Prefabs) {
					if (alreadyInstantiatePrefabList.Contains(prefab)) {
						return;
					}
					GameObject g = Instantiate<GameObject>(prefab);
					if (!g.GetComponent<DontDestroyThis>()) {
						g.AddComponent<DontDestroyThis>();
					}
					alreadyInstantiatePrefabList.Add(prefab);
					if (_asParent) {
						g.transform.SetParent(transform);
					}
				}
			}
		}
	}
}