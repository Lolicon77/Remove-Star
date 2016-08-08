using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
#if UNITY_5_3_OR_NEWER
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
#endif

namespace L7 {
	/// <summary>
	/// 该类用于找到所有场景和预制体中挂有某一个脚本的文件 （暂时必须activeInHierarchy）
	/// </summary>
	public class FindScriptReferenceWindow : EditorWindow {

		struct GameObjectDataInScene {
			public string scenePath;
			public int[] locationInScece;
		}


		private int frameCount;
		private int tempFrame;
		private bool goToFindGameObject;
		private string gameObjectKey;

		private string curScene;

		private List<string> findResult;
		private Dictionary<string, GameObject> findResultPrefab;
		private Dictionary<string, GameObjectDataInScene> findResultInstance;

		private MonoScript script;

		private Vector2 scrollPosition;

		[SerializeField]
		private bool onlyFindInAsset;
		[SerializeField]
		private bool onlyFindRefInScene;

		private bool OnlyFindInAsset
		{
			get
			{
				return onlyFindInAsset;
			}
			set
			{
				if (value) {
					onlyFindRefInScene = false;
				}
				onlyFindInAsset = value;
			}
		}

		public bool OnlyFindRefInScene
		{
			get
			{
				return onlyFindRefInScene;
			}
			set
			{
				if (value) {
					onlyFindInAsset = false;
				}
				onlyFindRefInScene = value;
			}
		}

		[MenuItem("Assets/获取所有脚本关联GameObject")]
		public static void ShowWindow() {
			//Show existing window instance. If one doesn’t exist, make one.
			GetWindow(typeof(FindScriptReferenceWindow));
		}

		void OnEnable() {
			if (Selection.activeObject is MonoScript) {
				script = (MonoScript)(Selection.activeObject);
			}
			//save current scene 
			curScene = GetCurrentSceneName();
			findResult = new List<string>();
			findResultPrefab = new Dictionary<string, GameObject>();
			findResultInstance = new Dictionary<string, GameObjectDataInScene>();
			EditorApplication.update = Update;
		}

		void Update() {
			frameCount++;
			if (goToFindGameObject && (frameCount > tempFrame + 20)) {
				Selection.activeGameObject = FindGameObjectByDataInScene(findResultInstance[gameObjectKey]);// GameObject.(findResultInstanceName[gameObjectKey]);
				goToFindGameObject = false;
			}
		}

		GameObject FindGameObjectByDataInScene(GameObjectDataInScene data) {
			var allObjects = FindObjectsOfType<Transform>();
			foreach (var obj in allObjects) {
				if (obj.parent == null && obj.GetSiblingIndex() == data.locationInScece[0]) {
					Transform tran = obj;
					for (int i = 1; i < data.locationInScece.Length; i++) {
						tran = GetChildBySiblingIndex(tran, data.locationInScece[i]);
					}
					return tran.gameObject;
				}
			}
			return null;
		}

		Transform GetChildBySiblingIndex(Transform tran, int index) {
			//			foreach (Transform child in tran) {
			//				if (child.GetSiblingIndex() == index) {
			//					return child;
			//				}
			//			}
			//			return null;
			return tran.GetChild(index);
		}

		GameObjectDataInScene SaveGameObjectDataInScene(Transform tran, string scenePath) {
			GameObjectDataInScene data = new GameObjectDataInScene();
			data.scenePath = scenePath;
			List<int> siblingList = new List<int>();
			while (tran != null) {
				siblingList.Add(tran.GetSiblingIndex());
				tran = tran.parent;
			}
			siblingList.Reverse();
			data.locationInScece = siblingList.ToArray();
			return data;
		}

		void OnGUI() {
			script = (MonoScript)EditorGUILayout.ObjectField("Select a script", script, typeof(MonoScript), true);
			if (!script) {
				return;
			}
			OnlyFindInAsset = GUILayout.Toggle(OnlyFindInAsset, "只在预制件中搜索");
			OnlyFindRefInScene = GUILayout.Toggle(OnlyFindRefInScene, "只在场景文件中搜索");
			var name = script.name;
			//			Debug.Log(name);
			//			System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
			//			var dict = System.IO.Path.GetDirectoryName(assembly.Location);
			//			assembly = System.Reflection.Assembly.LoadFile(System.IO.Path.Combine(dict, "Assembly-CSharp.dll"));
			var selectType = script.GetClass();
			if (!selectType.IsSubclassOf(typeof(MonoBehaviour))) {
				GUILayout.Label("被搜索的脚本必须继承MonoBehaviour");
				return;
			}
			//			if (string.IsNullOrEmpty(name) || selectType == null)
			//			{
			//				GUILayout.Label("select a noxss file from Project Window.");
			//				return;
			//			} 
			GUILayout.BeginVertical();
			GUILayout.BeginHorizontal(); //列出脚本名称和"Find"按钮 
			GUILayout.Label(name);
			bool click = GUILayout.Button("Find");
			GUILayout.EndHorizontal();
			GUILayout.Space(10); //列出搜索结果 
			if (findResult != null && findResult.Count > 0) {
				scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUIStyle.none);
				foreach (string path in findResult) {
					GUILayout.BeginHorizontal();
					if (findResultPrefab.ContainsKey(path)) {
						GUILayout.Label(path);
						if (GUILayout.Button("●", GUILayout.Width(20))) {
							EditorGUIUtility.PingObject(findResultPrefab[path]);
						}
					} else if (findResultInstance.ContainsKey(path)) {
						GUILayout.Label(Path.GetFileNameWithoutExtension(findResultInstance[path].scenePath) + " : " + Path.GetFileNameWithoutExtension(path.Remove(path.LastIndexOf("_", StringComparison.Ordinal))));
						if (GUILayout.Button("●", GUILayout.Width(20))) {
							OpenScene(findResultInstance[path].scenePath);
							tempFrame = frameCount;
							gameObjectKey = path;
							goToFindGameObject = true;
						}
					}
					GUILayout.EndHorizontal();
				}
				GUILayout.EndScrollView();
				if (GUILayout.Button("Return original scene")) {
					OpenScene(curScene);
				}
			}
			if (click) {
				Find(selectType);
			}
			if (OnlyFindInAsset) {
				if (GUILayout.Button("SelectAll")) {
					Selection.objects = findResultPrefab.Values.ToArray();
				}
			}
			GUILayout.EndVertical();
		}

		void Find(Type type) {
			//step 1:find ref in assets 
			if (!onlyFindRefInScene) {
				FindInAsset(type);
			}

			//step 2: find ref in scenes
			if (!onlyFindInAsset) {
				FindInScenes(type);
			}

			//reopen current scene 
			if (!string.IsNullOrEmpty(curScene)) {
				OpenScene(curScene);
			}
			EditorUtility.ClearProgressBar();
		}

		void FindInAsset(Type type) {
			//filter all GameObject from assets（so-called 'Prefab'）
			var guids = AssetDatabase.FindAssets("t:GameObject");
			findResult.Clear();
			findResultPrefab.Clear();
			findResultInstance.Clear();
			var tp = typeof(GameObject);
			for (int i = 0; i < guids.Length; i++) {
				var guid = guids[i];
				var path = AssetDatabase.GUIDToAssetPath(guid);
				//load Prefab 
				var obj = AssetDatabase.LoadAssetAtPath(path, tp) as GameObject;
				//check whether prefab contains noxss with type 'type'xsstag 
				if (obj != null) {
					EditorUtility.DisplayProgressBar("搜索预置中", obj.name, (float)i / guids.Length);
					var component = obj.GetComponent(type);
					if (component == null) {
						component = obj.GetComponentInChildren(type);
					}
					if (component != null) {
						findResult.Add(path);
						findResultPrefab.Add(path, obj);
					}
				}
			}
		}

		void FindInScenes(Type type) {
			SaveCurrentScene();
			//find all scenes from dataPath 
			string[] scenes = Directory.GetFiles(Application.dataPath, "*.unity", SearchOption.AllDirectories);
			//iterates all scenes 
			foreach (var scene in scenes) {
				OpenScene(scene);
				int no = 0;
				//iterates all gameObjects 
				var objList = FindObjectsOfType<GameObject>();
				for (int i = 0; i < objList.Length; i++) {
					GameObject obj = objList[i];
					EditorUtility.DisplayProgressBar("搜索 " + Path.GetFileNameWithoutExtension(scene) + " 场景中", obj.name, (float)i / objList.Length);
					var component = obj.GetComponent(type);
					if (component != null) {
						no++;
						string path = scene.Substring(Application.dataPath.Length) + "Assets:" + component.gameObject.name + "_" + no;
						findResult.Add(path);
						if (!findResultInstance.ContainsKey(path)) {
							findResultInstance.Add(path, SaveGameObjectDataInScene(component.transform, scene));
						}
					}
				}
			}
		}

		void OpenScene(string sceneName) {
#if UNITY_5_3_OR_NEWER
			EditorSceneManager.OpenScene(sceneName);
#else
			EditorApplication.OpenScene(sceneName);
#endif
		}

		string GetCurrentSceneName() {
#if UNITY_5_3_OR_NEWER
			return SceneManager.GetActiveScene().path;
#else
			return EditorApplication.currentScene;
#endif
		}

		void SaveCurrentScene() {
#if UNITY_5_3_OR_NEWER
			EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
#else
			EditorApplication.SaveScene();
#endif
		}


	}
}

