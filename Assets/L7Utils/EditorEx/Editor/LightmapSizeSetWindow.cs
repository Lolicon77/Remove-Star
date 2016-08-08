#if UNITY_4

using UnityEditor;
using UnityEngine;

namespace L7 {
	public class LightmapSizeSetWindow : EditorWindow {

		public enum LightmapSize {
			size256,
			size512,
			size1024,
			size2048,
			size4096
		}

		private LightmapSize lightmapSize;
		private int size;

		[MenuItem("Lightmap(4.6)/LightmapSizeSet")]
		static void Init() {
			LightmapSizeSetWindow win = EditorWindow.GetWindow<LightmapSizeSetWindow>();
			win.Show();
		}

		void OnEnable() {
			switch (LightmapEditorSettings.maxAtlasWidth)
			{
				case 256:
					lightmapSize = LightmapSize.size256;
					break;
				case 512:
					lightmapSize = LightmapSize.size512;
					break;
				case 1024:
					lightmapSize = LightmapSize.size1024;
					break;
				case 2048:
					lightmapSize = LightmapSize.size2048;
					break;
				case 4096:
					lightmapSize = LightmapSize.size4096;
					break;
			}
		}


		private void OnGUI()
		{

			lightmapSize = (LightmapSize) EditorGUILayout.EnumPopup("LightMapSize(4.6)", lightmapSize);

			switch (lightmapSize)
			{
				case LightmapSize.size256:
					size = 256;
					break;
				case LightmapSize.size512:
					size = 512;
					break;
				case LightmapSize.size1024:
					size = 1024;
					break;
				case LightmapSize.size2048:
					size = 2048;
					break;
				case LightmapSize.size4096:
					size = 4096;
					break;
			}

			if (GUILayout.Button("Save"))
			{
				LightmapEditorSettings.maxAtlasHeight = size;
				LightmapEditorSettings.maxAtlasWidth = size;
			}
		}
	}
}
#endif