using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEditor;
using Object = UnityEngine.Object;

[InitializeOnLoad]
public class OpenFileWithSublime : EditorWindow {

	private const string sublimePathPref = "LTH_SublimePath";
	private const string sublimeEnablePref = "LTH_SublimeEnable";
	private const string FilesOpenWhenNoVS_Pref = "LTH_FilesOpenWhenNoVS";
	private const string FilesOpenWhenHaveVS_Pref = "LTH_FilesOpenWhenHaveVS";


	private static string sublimePath = "/";
	private static string FilesOpenWhenNoVS_String = "";
	private static string FilesOpenWhenHaveVS_String = "";

	private static bool enable;
	private static bool haveSublime;
	private static bool haveVS;
	private static string[] OpenWhenNoVS_Array;
	private static string[] OpenWhenHaveVS_Array;

	static OpenFileWithSublime() {
		Reload();
	}

	static void Reload() {
		sublimePath = EditorPrefs.GetString(sublimePathPref, "/");
		enable = EditorPrefs.GetBool(sublimeEnablePref, false);
		FilesOpenWhenHaveVS_String = EditorPrefs.GetString(FilesOpenWhenHaveVS_Pref, "shader,cginc,js");
		FilesOpenWhenNoVS_String = EditorPrefs.GetString(FilesOpenWhenNoVS_Pref, "shader,cginc,js,cs");
		haveSublime = File.Exists(sublimePath);
		try {
			var vsAssembly = Assembly.Load("SyntaxTree.VisualStudio.Unity.Bridge");
			haveVS = vsAssembly != null;
		} catch {
			haveVS = false;
		}
		OpenWhenHaveVS_Array = FilesOpenWhenHaveVS_String.Split(',');
		OpenWhenNoVS_Array = FilesOpenWhenNoVS_String.Split(',');

	}

	[MenuItem("Assets/用sublime打开")]
	public static void OpenSelectionWithSublime() {
		string path = AssetDatabase.GetAssetPath(Selection.activeObject);
		path = "\"" + path + "\"";
		ProcessStartInfo proc = new ProcessStartInfo();
		proc.FileName = EditorPrefs.GetString(sublimePathPref, "");
		proc.Arguments = path;
		Process.Start(proc);
	}

	[MenuItem("LTH/Utility/配置Sublime打开文件")]
	public static void OpenWindow() {
		OpenFileWithSublime win = EditorWindow.GetWindow<OpenFileWithSublime>();
		win.Show();
	}

	void OnGUI() {
		enable = EditorGUILayout.Toggle("开启", enable);
		GUILayout.BeginHorizontal();
		if (!haveSublime) {
			GUI.color = Color.red;
		} else {
			GUI.color = Color.green;
		}
		EditorGUILayout.TextField("Sublime Path:", sublimePath);
		GUI.color = Color.white;
		if (GUILayout.Button("find", GUILayout.MaxWidth(30f))) {
			string tempPath = EditorUtility.OpenFilePanel("sublime路径", "/", "exe");
			if (File.Exists(tempPath)) {
				sublimePath = tempPath;
			}
		}
		GUILayout.EndHorizontal();

		EditorGUILayout.Toggle("是否找到了UnityVS", haveVS);
		EditorGUILayout.LabelField("当找到UnityVS时，用sublime打开以下文件:");
		FilesOpenWhenHaveVS_String = EditorGUILayout.TextField(FilesOpenWhenHaveVS_String);
		EditorGUILayout.LabelField("当没有找到UnityVS时，用sublime打开以下文件:");
		FilesOpenWhenNoVS_String = EditorGUILayout.TextField(FilesOpenWhenNoVS_String);
		if (GUI.changed) {
			EditorPrefs.SetString(sublimePathPref, sublimePath);
			EditorPrefs.SetBool(sublimeEnablePref, enable);
			EditorPrefs.SetString(FilesOpenWhenNoVS_Pref, FilesOpenWhenNoVS_String);
			EditorPrefs.SetString(FilesOpenWhenHaveVS_Pref, FilesOpenWhenHaveVS_String);
			Reload();
		}
	}

	[OnOpenAsset]
	private static bool OpenShaderWithSubl(int instanceID, int line) {
		if (!enable){
			return false;
		}
		if (!haveSublime) {
			return false;
		}

		Object obj = EditorUtility.InstanceIDToObject(instanceID);
		string path = AssetDatabase.GetAssetPath(obj);

		var filesCanOpen = haveVS ? OpenWhenHaveVS_Array : OpenWhenNoVS_Array;
		foreach (var s in filesCanOpen) {
			if (string.IsNullOrEmpty(s)){
				continue;
			}
			if (path.EndsWith(s) || path.EndsWith(s.ToUpper()) || path.EndsWith(s.ToLower())) {
				path = "\"" + path + "\"";
				ProcessStartInfo proc = new ProcessStartInfo();
				proc.FileName = EditorPrefs.GetString(sublimePathPref, "");
				proc.Arguments = path;
				Process.Start(proc);
				return true;
			}
		}
		return false;
	}


}