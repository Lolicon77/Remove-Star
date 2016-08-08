using System.Linq;
using UnityEditor;
using UnityEngine;
using System.Collections;

[InitializeOnLoad]
public class SceneEditorMode : EditorWindow {

	private static bool _enableSceneEditorMode = false;

	static Color RedTrans = new Color(1, 0, 0, 0.2f);
	static Color YellowTrans = new Color(1, 1, 0, 0.2f);
//	static Color GreenTrans = new Color(0, 1, 0, 0.2f);

	static SceneEditorMode() {
		_enableSceneEditorMode = EditorPrefs.GetBool("SceneEditorMode", false);

		EditorApplication.hierarchyWindowItemOnGUI += (id, rect) => {
			if (!_enableSceneEditorMode) {
				return;
			}

			var obj = EditorUtility.InstanceIDToObject(id);
			if (obj == null) {
				return;
			}
			var prefabType = PrefabUtility.GetPrefabType(obj);
			switch (prefabType) {
				case PrefabType.DisconnectedModelPrefabInstance:
				case PrefabType.DisconnectedPrefabInstance:
				case PrefabType.MissingPrefabInstance:
				case PrefabType.ModelPrefabInstance:
					SetAsError(obj, rect);
					break;

				case PrefabType.PrefabInstance:
					var modify = PrefabUtility.GetPropertyModifications(obj);
					bool findChange = false;
					if (modify != null) {
						foreach (var pm in modify) {
							if (pm != null && pm.target != null) {

								if (pm.target.GetType() != typeof(Transform) && pm.target.GetType() != typeof(MeshRenderer)) {
									findChange = true;
									break;
								}
							}
						}
					}
					if (findChange) {
						SetAsPrefabError(obj, rect);
					} else {
						SetAsGoolPrefab(obj, rect);
					}
					break;
			}
		};
	}

	[MenuItem("LTH/Utility/场景编辑模式")]
	static void Menu() {
		GetWindow<SceneEditorMode>();
	}


	private Vector2 _scroll;
	void OnGUI() {
		EditorGUI.BeginChangeCheck();
		_enableSceneEditorMode = GUILayout.Toggle(_enableSceneEditorMode, "开启");
		if (EditorGUI.EndChangeCheck()) {
			EditorPrefs.SetBool("SceneEditorMode", _enableSceneEditorMode);

		}


		_scroll= GUILayout.BeginScrollView(_scroll);
		var g = Selection.activeGameObject;
		var modify = PrefabUtility.GetPropertyModifications(g);
		if (modify != null) {
			foreach (var pm in modify) {
				GUILayout.Label(pm.target.GetType() + "/" + pm.target + "/" + pm.propertyPath);
			}
		}
		GUILayout.EndScrollView();
	}



	static void SetAsError(Object obj, Rect rect) {
		EditorGUI.DrawRect(rect, RedTrans);
	}

	static void SetAsPrefabError(Object obj, Rect rect) {
		EditorGUI.DrawRect(rect, YellowTrans);
	}

	private static void SetAsGoolPrefab(Object obj, Rect rect) {
		//		EditorGUI.DrawRect(rect, GreenTrans);
		GameObject gameObj = (GameObject)obj;
		if (gameObj != null) {
			var com = gameObj.GetComponents<Component>();
			foreach (var c in com) {
				if (c.GetType() != typeof(Transform) && c.GetType() != typeof(MeshRenderer)) {
					//c.hideFlags = HideFlags.NotEditable;
				}
			}
		}

	}



}
