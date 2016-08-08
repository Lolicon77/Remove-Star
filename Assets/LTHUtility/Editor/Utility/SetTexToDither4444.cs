using System.IO;
using UnityEditor;
using UnityEngine;
using System.Collections;


namespace LTHUtility {

	public class SetTexToDither4444 : MonoBehaviour {


//		[MenuItem("Assets/SetTexToDither4444")]//not good 
		private static void _SetTexToDither4444() {
			Texture2D tex = Selection.activeObject as Texture2D;
			if (!tex) {
				EditorUtility.DisplayDialog("error", "请选择一张图片", "ok");
				return;
			}
			string path = AssetDatabase.GetAssetPath(Selection.activeObject);
			TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
			TextureImporterFormat originFormat = importer.textureFormat;
			bool originIsReadAble = importer.isReadable;
			importer.isReadable = true;
			importer.textureFormat = TextureImporterFormat.ARGB32;
			AssetDatabase.ImportAsset(importer.assetPath);



			Texture2D newTex = ToDither4444(tex);

			var bytes = newTex.EncodeToPNG();
			path += "-dither.png";
			File.WriteAllBytes(path, bytes);

			AssetDatabase.Refresh();
			importer.isReadable = originIsReadAble;
			importer.textureFormat = originFormat;
			AssetDatabase.ImportAsset(importer.assetPath);


		}


		public static Texture2D ToDither4444(Texture2D texture) {
			var texw = texture.width;
			var texh = texture.height;

			Texture2D ret = new Texture2D(texw, texh);

			var pixels = texture.GetPixels();
			var offs = 0;

			var k1Per15 = 1.0f / 15.0f;
			var k1Per16 = 1.0f / 16.0f;
			var k3Per16 = 3.0f / 16.0f;
			var k5Per16 = 5.0f / 16.0f;
			var k7Per16 = 7.0f / 16.0f;

			for (var y = 0; y < texh; y++) {
				for (var x = 0; x < texw; x++) {
					float a = pixels[offs].a;
					float r = pixels[offs].r;
					float g = pixels[offs].g;
					float b = pixels[offs].b;

					var a2 = Mathf.Clamp01(Mathf.Floor(a * 16) * k1Per15);
					var r2 = Mathf.Clamp01(Mathf.Floor(r * 16) * k1Per15);
					var g2 = Mathf.Clamp01(Mathf.Floor(g * 16) * k1Per15);
					var b2 = Mathf.Clamp01(Mathf.Floor(b * 16) * k1Per15);

					var ae = a - a2;
					var re = r - r2;
					var ge = g - g2;
					var be = b - b2;

					pixels[offs].a = a2;
					pixels[offs].r = r2;
					pixels[offs].g = g2;
					pixels[offs].b = b2;

					var n1 = offs + 1;
					var n2 = offs + texw - 1;
					var n3 = offs + texw;
					var n4 = offs + texw + 1;

					if (x < texw - 1) {
						pixels[n1].a += ae * k7Per16;
						pixels[n1].r += re * k7Per16;
						pixels[n1].g += ge * k7Per16;
						pixels[n1].b += be * k7Per16;
					}

					if (y < texh - 1) {
						pixels[n3].a += ae * k5Per16;
						pixels[n3].r += re * k5Per16;
						pixels[n3].g += ge * k5Per16;
						pixels[n3].b += be * k5Per16;

						if (x > 0) {
							pixels[n2].a += ae * k3Per16;
							pixels[n2].r += re * k3Per16;
							pixels[n2].g += ge * k3Per16;
							pixels[n2].b += be * k3Per16;
						}

						if (x < texw - 1) {
							pixels[n4].a += ae * k1Per16;
							pixels[n4].r += re * k1Per16;
							pixels[n4].g += ge * k1Per16;
							pixels[n4].b += be * k1Per16;
						}
					}

					offs++;
				}
			}
			ret.SetPixels(pixels);
			EditorUtility.CompressTexture(ret, TextureFormat.RGBA4444, TextureCompressionQuality.Best);
			return ret;
		}

	}
}
