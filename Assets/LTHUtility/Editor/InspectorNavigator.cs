using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace LTHUtility {

	public class InspectorNavigator : EditorWindow, IHasCustomMenu {

		private List<Object> objects;

		[MenuItem("LTH/Inspector Navigator")]
		public static void OpenWindow() {
			var v = GetWindow<InspectorNavigator>();
			v.Show();
		}

		void OnEnable() {
			objects = new List<Object>();
		}

		void OnGUI() {
			switch (Event.current.type) {
				case EventType.DragUpdated:
				case EventType.DragPerform:
					DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
					if (Event.current.type == EventType.DragPerform) {
						DragAndDrop.AcceptDrag();
						foreach (var v in DragAndDrop.objectReferences) {
							objects.Add(v);
						}
					}
					break;
			}
			Rect r = new Rect();
			r.height = EditorGUIUtility.singleLineHeight;
			foreach (var o in objects) {
				r.width = 10f * o.name.Length;
				float w = r.x + r.width;
				if (w > position.width) {
					r.y += EditorGUIUtility.singleLineHeight;
					r.x = 0;
				}
				if (GUI.Button(r, o.name)) {
					EditorGUIUtility.PingObject(o);
				}
				r.x += r.width;
			}

		}

		public void AddItemsToMenu(GenericMenu menu) {
			menu.AddItem(new GUIContent("Clear breadcrumbs"), false, new GenericMenu.MenuFunction(() => { objects.Clear(); }));
		}

	}

}