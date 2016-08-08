using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

public class SplitAtlas : MonoBehaviour {
//	[MenuItem("Assets/切割图集")]
	static void DoIt() {
		//var atlas = Selection.activeGameObject.GetComponent<UIAtlas>();
		//if (atlas == null) {
		//	Debug.LogError("Error: atlas is null");
		//	return;
		//}
		//string savePath = EditorUtility.SaveFolderPanel("save", Application.dataPath, "");

		//if (string.IsNullOrEmpty(savePath)) {
		//	return;
		//}
		//List<UIAtlasMaker.SpriteEntry> ses = new List<UIAtlasMaker.SpriteEntry>();
		//UIAtlasMaker.ExtractSprites(atlas, ses);

		//foreach (var se in ses) {
		//	if (se != null) {
		//		byte[] bytes = se.tex.EncodeToPNG();
		//		File.WriteAllBytes(savePath + "/" + se.name + ".png", bytes);
		//		if (se.temporaryTexture) DestroyImmediate(se.tex);
		//	}
		//}

		//EditorUtility.ClearProgressBar();
	}
}