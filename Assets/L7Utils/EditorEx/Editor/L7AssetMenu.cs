using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;


namespace L7 {
	[InitializeOnLoad]
	public class L7AssetMenu : MonoBehaviour {

		//		private static List<DirectoryInfo> directoryList;
		//		private static List<FileInfo> FileList = new List<FileInfo>();


		[MenuItem("GameObject/模型轴心归中(雨松MOMO)")]
		static void ChangePivotPoint() {
			Transform parent = Selection.activeGameObject.transform;
			Vector3 postion = parent.position;
			Quaternion rotation = parent.rotation;
			Vector3 scale = parent.localScale;
			parent.position = Vector3.zero;
			parent.rotation = Quaternion.Euler(Vector3.zero);
			parent.localScale = Vector3.one;


			Vector3 center = Vector3.zero;
			Renderer[] renders = parent.GetComponentsInChildren<Renderer>();
			foreach (Renderer child in renders) {
				center += child.bounds.center;
			}
			center /= parent.GetComponentsInChildren<Transform>().Length;
			Bounds bounds = new Bounds(center, Vector3.zero);
			foreach (Renderer child in renders) {
				bounds.Encapsulate(child.bounds);
			}

			parent.position = postion;
			parent.rotation = rotation;
			parent.localScale = scale;

			foreach (Transform t in parent) {
				t.position = t.position - bounds.center;
			}
			parent.transform.position = bounds.center + parent.position;

		}


		//		[MenuItem("Assets/快速创建美术资源归类文件夹")]
		//		static void FastCreateArtFolder() {
		//			directoryList = new List<DirectoryInfo>();
		//			string path = AssetDatabase.GetAssetPath(Selection.activeObject);
		//			directoryList.Add(CreateChildDirectory(path, "Materials"));
		//			directoryList.Add(CreateChildDirectory(path, "Prefabs"));
		//			directoryList.Add(CreateChildDirectory(path, "Textures"));
		//			directoryList.Add(CreateChildDirectory(path, "Shaders"));
		//			directoryList.Add(CreateChildDirectory(path, "Scripts"));
		//			directoryList.Add(CreateChildDirectory(path, "Meshes"));
		//
		//			//			SortOut(path);
		//			directoryList = null;
		//			GC.Collect();
		//		}
		//
		//		static DirectoryInfo CreateChildDirectory(string path, string name) {
		//			string sPath = path + "/" + name;
		//			if (!Directory.Exists(sPath)) {
		//				return Directory.CreateDirectory(sPath);
		//			} else {
		//				return new DirectoryInfo(sPath);
		//			}
		//		}
		//
		//		static void SortOut(string path) {
		//			DirectoryInfo TheFolder = new DirectoryInfo(path);
		//			List<FileInfo> list = GetAll(TheFolder);
		//			foreach (var file in list) {
		//				switch (file.Extension) {
		//					case ".mat":
		//						MoveTo(file, directoryList[0]);
		//						break;
		//					case ".prefab":
		//						MoveTo(file, directoryList[1]);
		//						break;
		//					case ".png":
		//						MoveTo(file, directoryList[2]);
		//						break;
		//					case ".tga":
		//						MoveTo(file, directoryList[2]);
		//						break;
		//					case ".psd":
		//						MoveTo(file, directoryList[2]);
		//						break;
		//					case ".jpg":
		//						MoveTo(file, directoryList[2]);
		//						break;
		//					case ".FBX":
		//						MoveTo(file, directoryList[5]);
		//						break;
		//					case ".cs":
		//						MoveTo(file, directoryList[4]);
		//						break;
		//					case ".js":
		//						MoveTo(file, directoryList[4]);
		//						break;
		//					case ".shader":
		//						MoveTo(file, directoryList[3]);
		//						break;
		//					default:
		//						//						Debug.Log(file.Extension);
		//						//						Debug.Log(directoryList[3].FullName);
		//						break;
		//				}
		//			}
		//		}
		//
		//
		//		static void MoveTo(FileInfo file, DirectoryInfo dir) {
		//			bool exist = false;
		//			foreach (var child in dir.GetFiles()) {
		//				if (file == child) {
		//					exist = true;
		//				}
		//			}
		//			if (exist) {
		//				return;
		//			}
		//			file.MoveTo(dir.FullName + @"\" + file.Name);
		//		}
		//
		//		static List<FileInfo> GetAll(DirectoryInfo dir)//搜索文件夹中的文件
		//		{
		//
		//			FileInfo[] allFile = dir.GetFiles();
		//			foreach (FileInfo fi in allFile) {
		//				FileList.Add(fi);
		//			}
		//
		//			DirectoryInfo[] allDir = dir.GetDirectories();
		//			foreach (DirectoryInfo d in allDir) {
		//				GetAll(d);
		//			}
		//			return FileList;
		//		}


		[MenuItem("GameObject/MutliSetChildrenSiblingIndex", false, 10)]
		static void MutliSetChildrenSiblingIndex() {
			SetChildrenSiblingRecursion(Selection.activeTransform);
		}

		private static void SetChildrenSiblingRecursion(Transform trans) {
			if (trans) {
				AutoSetChildrenSiblingIndex(trans);
				foreach (Transform child in trans) {
					SetChildrenSiblingRecursion(child);
				}
			}
		}

		[MenuItem("GameObject/AutoSetChildrenSiblingIndex", false, 10)]
		static void AutoSetChildrenSiblingIndex() {
			AutoSetChildrenSiblingIndex(Selection.activeTransform);
		}

		[MenuItem("GameObject/AutoSetChildrenSiblingIndex", true)]
		static bool ValidateLogSelectedTransformName() {
			return Selection.activeTransform != null && Selection.activeTransform.childCount > 1;
		}

		static void AutoSetChildrenSiblingIndex(Transform trans) {
			if (trans.childCount > 1) {
				List<Transform> children = trans.Cast<Transform>().ToList();
				children.Sort((x, y) => string.Compare(x.name, y.name, StringComparison.Ordinal));

				for (int i = 0; i < children.Count; i++) {
					children[i].SetSiblingIndex(i);
				}
			}
		}

	}

}

