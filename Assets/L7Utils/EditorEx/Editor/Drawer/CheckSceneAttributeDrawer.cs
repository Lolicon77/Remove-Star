using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace L7 {
	[CustomPropertyDrawer(typeof(CheckSceneAttribute))]
	public class CheckSceneAttributeDrawer : PropertyDrawer {

		private string path;
		private Object scene;

		public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label) {

			switch (Event.current.type) {
				case EventType.DragUpdated:
				case EventType.DragPerform:
					if (!position.Contains(Event.current.mousePosition)) {
						return;
					}
					DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
					if (Event.current.type == EventType.DragPerform) {
						DragAndDrop.AcceptDrag();

						if (DragAndDrop.objectReferences.Length == 1) {
							scene = DragAndDrop.objectReferences[0];
							path = AssetDatabase.GetAssetPath(DragAndDrop.objectReferences[0]);
							if (path.EndsWith(".unity")) {
								prop.stringValue = DragAndDrop.objectReferences[0].name;
							}
						}
					}
					break;
			}

			Color originColor = GUI.color;
			bool notInBuildSetting = false;
			if (string.IsNullOrEmpty(prop.stringValue)) {
				GUI.color = Color.yellow;
			} else {
				//				g = Resources.Load<GameObject>(checkResourceAttr.path + prop.stringValue);
				List<string> currentEnableBuildScenePaths = new List<string>();
				foreach (var s in EditorBuildSettings.scenes) {
					if (s.enabled) {
						currentEnableBuildScenePaths.Add(s.path);
					}
				}
				if (!currentEnableBuildScenePaths.Contains(path)) {
					GUI.color = Color.red;
					notInBuildSetting = true;
				} else {
					GUI.color = Color.green;
				}
			}
			position.width -= 20f;
			EditorGUI.PropertyField(position, prop, label);
			if (!notInBuildSetting) {
				position.x += position.width + 2;
				position.width = 20f;
				if (GUI.Button(position, "●")) {
					EditorGUIUtility.PingObject(scene);
				}
			}
			if (notInBuildSetting) {
				position.x += position.width + 2;
				position.width = 20f;
				if (GUI.Button(position, "+")) {
					if (path != null && path.EndsWith(".unity")) {
						List<EditorBuildSettingsScene> scenes = EditorBuildSettings.scenes.ToList();
						scenes.Add(new EditorBuildSettingsScene(path, true));
						EditorBuildSettings.scenes = scenes.ToArray();
					}
				}
			}
			GUI.color = originColor;
		}
	}
}