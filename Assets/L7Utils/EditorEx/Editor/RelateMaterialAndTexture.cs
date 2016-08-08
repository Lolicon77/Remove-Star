using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace L7 {
	public class RelateMaterialAndTexture : MonoBehaviour {

		[MenuItem("Assets/修改文件夹的材质并关联贴图 %&#F12")]
		static void Handle() {
			string path = AssetDatabase.GetAssetPath(Selection.activeObject);
			if (!AssetDatabase.IsValidFolder(path)) {
				path = path.Remove(path.LastIndexOf("/", StringComparison.Ordinal));
			}
			string[] matFiles = Directory.GetFiles(path, "*.mat", SearchOption.AllDirectories);
			string[] texFiles = Directory.GetFiles(path, "*.psd", SearchOption.AllDirectories);

			foreach (var matFile in matFiles) {
				Material mat = AssetDatabase.LoadAssetAtPath(matFile, typeof(Material)) as Material;
				mat.shader = Shader.Find("LTH/Unlit/Rim");
				//				mat.shader = Shader.Find("Mobile/Diffuse");
				foreach (var texFile in texFiles) {
					string matFileName = matFile.Remove(matFile.LastIndexOf(".", StringComparison.Ordinal));
					matFileName = matFileName.Remove(0, matFileName.LastIndexOf(@"\", StringComparison.Ordinal) + 1);
					string texfileName = texFile.Remove(texFile.LastIndexOf(".", StringComparison.Ordinal));
					texfileName = texfileName.Remove(0, texfileName.LastIndexOf(@"\", StringComparison.Ordinal) + 1);
					if (texfileName == matFileName) {
						Texture2D tex = AssetDatabase.LoadAssetAtPath(texFile, typeof(Texture2D)) as Texture2D;
						mat.SetTexture("_MainTex", tex);
						mat.SetColor("_RimColor", Color.gray / 2);
						mat.SetFloat("_RimPower", 10);
						AssetDatabase.Refresh();
					}

				}
			}
		}

	}
}