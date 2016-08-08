using UnityEngine;
using System.Collections;


namespace LTHUtility {


	public class LoadPrefabOnAwake : MonoBehaviour {
		[SerializeField]
		private bool _asParent;

		public GameObject[] Prefabs;
		public string[] PrefabsInRes;

		void Awake() {
			if (Prefabs != null) {
				foreach (var prefab in Prefabs) {
					GameObject g = Instantiate<GameObject>(prefab);
					if (_asParent) {
						g.transform.SetParent(transform);
					}
				}
			}
			if (PrefabsInRes != null) {
				foreach (var s in PrefabsInRes) {
					GameObject g = Instantiate<GameObject>(Resources.Load<GameObject>(s));
					if (_asParent) {
						g.transform.SetParent(transform);
					}
				}
			}
		}


	}
}
