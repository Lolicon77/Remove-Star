using UnityEngine;
using System.Collections;
using Random = System.Random;

namespace Test {
	public class TestSystemRandom : MonoBehaviour {
		private Random random;
		private int randomSeed = 0;

		void Start() {
			random = new Random(randomSeed);
		}

		void OnGUI() {

			if (GUILayout.Button("newRandom")) {
				random = new Random(randomSeed);
			}
			if (GUILayout.Button("int")) {
				Debug.Log(random.Next());
			}
			if (GUILayout.Button("float")) {
				Debug.Log(random.NextDouble());
			}


		}


	}
}