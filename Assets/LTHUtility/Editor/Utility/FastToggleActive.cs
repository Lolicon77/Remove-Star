using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class FastToggleActive {
	private static bool enable = false;

	[MenuItem("LTH/ToggleFastActive")]
	static void ToggleFastActive() {
		enable = !enable;
		EditorPrefs.SetBool("fastToggleActive", enable);
		EditorApplication.RepaintHierarchyWindow();
	}

	static FastToggleActive() {
		EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyGUI;
		enable = EditorPrefs.GetBool("fastToggleActive", false);
	}
	static void OnHierarchyGUI(int instanceID, Rect selectionRect) {
		if (!enable) {
			return;
		}
		GameObject g = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
		Rect rec = selectionRect;
		rec.x += rec.width - 15f;
		rec.width = 14f;
		if (g) {
			g.SetActive(GUI.Toggle(rec, g.activeSelf, "D"));
		}


		//		// Whether this object was right clicked
		//		if (Event.current != null && selectionRect.Contains(Event.current.mousePosition)
		//		&& Event.current.button == 1 && Event.current.type <= EventType.mouseUp) {
		//			// Find what object this is
		//			GameObject clickedObject = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
		//			if (clickedObject) {
		//				//Debug.Log("Clicked " + clickedObject.name);
		//				_ClickedOBject = clickedObject;
		//				_MenuPosition = Event.current.mousePosition;
		//				_MenuOpened = true;
		//
		//				// Consume the event to remove Unity's default context menu
		//				Event.current.Use();
		//			}
		//		}
		//
		//		if (_MenuOpened) {
		//			if (GUI.Button(new Rect(_MenuPosition.x, _MenuPosition.y, 150, 20f), "Delete")) {
		//				_MenuOpened = false;
		//				GameObject.Destroy(_ClickedOBject);
		//			}
		//		}
	}
}