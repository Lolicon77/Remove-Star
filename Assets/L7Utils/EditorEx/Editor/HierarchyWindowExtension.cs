using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace L7 {
	public class HierarchyWindowExtension {

		private static List<Transform> tempTransforms;
		private static Transform currentSelected;
		private static Transform lastSelected;

		static int GetCurrentSelectedIndex() {
			if (tempTransforms == null) {
				tempTransforms = new List<Transform>();
			}

			currentSelected = Selection.activeTransform;

			if (!currentSelected) {
				return -1;
			}

			if (!currentSelected.parent) {
				//todo 
				Debug.Log("暂不支持无父节点的物体搜索，寻找此类物体没想到效率较高的方法");
				return -1;
			}

			if (lastSelected == null || currentSelected.parent != lastSelected.parent) {
				tempTransforms.Clear();
				tempTransforms.AddRange(currentSelected.parent.Cast<Transform>());
				tempTransforms.Sort((left, right) => left.GetSiblingIndex().CompareTo(right.GetSiblingIndex()));
			}

			int index = tempTransforms.IndexOf(currentSelected);
			lastSelected = currentSelected;
			return index;
		}


		[MenuItem("L7/HotKey/SelectNextGameObject #DOWN")]
		static public void SelectNextGameObject() {
			var index = GetCurrentSelectedIndex();
			if (index == -1) {
				return;
			}
			if (++index < tempTransforms.Count) {
				Selection.activeGameObject = tempTransforms[index].gameObject;
			}
			SceneView sv = (SceneView)(SceneView.sceneViews[0]);
			sv.LookAt(Selection.activeTransform.position);
		}

		[MenuItem("L7/HotKey/SelectNextPriorObject #UP")]
		static public void SelectNextPriorObject() {
			int index = GetCurrentSelectedIndex();
			if (index == -1) {
				return;
			}
			if (--index >= 0) {
				Selection.activeGameObject = tempTransforms[index].gameObject;
			}
			SceneView sv = (SceneView)(SceneView.sceneViews[0]);
			sv.LookAt(Selection.activeTransform.position);
		}
	}
}