using System;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace L7 {
	public class ModifyAnimationClipsBatch : MonoBehaviour {
		public enum EventType {
			OnAttack,
			OnUseSkill,
			OnDeathAnimationEnd,
			None
		}

		private static void ModifyAnimation(String filePath, EventType eventType) {
			StringBuilder stringBuilder = new StringBuilder();
			FileStream fileStream = new FileStream(filePath, FileMode.Open);
			StreamReader streamReader = new StreamReader(fileStream);
			try {
				fileStream.Seek(0, SeekOrigin.Begin);
				string content = streamReader.ReadLine();
				while (content != null) {
					if (content.Contains(@"loopTime:") && eventType != EventType.OnDeathAnimationEnd) {
						content = "      loopTime: 1";
						stringBuilder.AppendLine(content);
					} else if (content.Contains(@"events: []") && eventType != EventType.None && eventType != EventType.OnDeathAnimationEnd) {
						content = "      events:";
						stringBuilder.AppendLine(content);
						stringBuilder.Append(GetEventString(eventType));
					} else {
						stringBuilder.AppendLine(content);
					}
					content = streamReader.ReadLine();
				}
			} catch {
				throw;
			} finally {
				fileStream.Close();
				streamReader.Close();
			}

			fileStream = File.Open(filePath, FileMode.OpenOrCreate, FileAccess.Write);
			StreamWriter streamWriter = new StreamWriter(fileStream);
			try {
				fileStream.Seek(0, SeekOrigin.Begin);
				fileStream.SetLength(0);
				streamWriter.Write(stringBuilder);
				streamWriter.Flush();
				Debug.Log("成功插入对应事件");
			} catch (Exception) {
				throw;
			} finally {
				fileStream.Close();
				streamWriter.Close();
			}

		}

		private static void ModifyClip(String filePath) {
			ModelImporter modelImporter =
				(ModelImporter)AssetImporter.GetAtPath(filePath.Remove(filePath.LastIndexOf(@".meta", StringComparison.Ordinal)));

			string originalClipName = modelImporter.defaultClipAnimations[0].name;
			float originalFirstFrame = modelImporter.defaultClipAnimations[0].firstFrame;
			float originalLastFrame = modelImporter.defaultClipAnimations[0].lastFrame;

			StringBuilder stringBuilder = new StringBuilder();
			FileStream fileStream = new FileStream(filePath, FileMode.Open);
			StreamReader streamReader = new StreamReader(fileStream);
			try {
				fileStream.Seek(0, SeekOrigin.Begin);
				string content = streamReader.ReadLine();
				while (content != null) {
					if (content.Contains(@"    clipAnimations: []")) {
						content = "    clipAnimations:";
						stringBuilder.AppendLine(content);
						stringBuilder.Append(@"    - serializedVersion: 16
      name: " + originalClipName + @"
      takeName: Take 001
      firstFrame: " + originalFirstFrame + @"
      lastFrame: " + originalLastFrame + @"
      wrapMode: 0
      orientationOffsetY: 0
      level: 0
      cycleOffset: 0
      loop: 0
      loopTime: 0
      loopBlend: 0
      loopBlendOrientation: 0
      loopBlendPositionY: 0
      loopBlendPositionXZ: 0
      keepOriginalOrientation: 0
      keepOriginalPositionY: 1
      keepOriginalPositionXZ: 0
      heightFromFeet: 0
      mirror: 0
      bodyMask: 01000000010000000100000001000000010000000100000001000000010000000100000001000000010000000100000001000000
      curves: []
      events: []
      transformMask: []
      maskType: 0
      maskSource: {instanceID: 0}
");
					} else {
						stringBuilder.AppendLine(content);
					}
					content = streamReader.ReadLine();
				}
			} catch {
				throw;
			} finally {
				fileStream.Close();
				streamReader.Close();
			}

			fileStream = File.Open(filePath, FileMode.OpenOrCreate, FileAccess.Write);
			StreamWriter streamWriter = new StreamWriter(fileStream);
			try {
				fileStream.Seek(0, SeekOrigin.Begin);
				fileStream.SetLength(0);
				streamWriter.Write(stringBuilder);
				streamWriter.Flush();
				Debug.Log("成功插入对应clip");
			} catch (Exception) {
				throw;
			} finally {
				fileStream.Close();
				streamWriter.Close();
			}
		}


		private static StringBuilder GetEventString(EventType eventType) {
			StringBuilder sb = new StringBuilder();
			if (eventType == EventType.OnDeathAnimationEnd) {
				sb.AppendLine("      - time: " + ".9");
			} else {
				sb.AppendLine("      - time: " + ".5");
			}
			sb.AppendLine("        functionName: " + eventType);
			sb.AppendLine("        data: ");
			sb.AppendLine("        objectReferenceParameter: {instanceID: 0}");
			sb.AppendLine("        floatParameter: 0");
			sb.AppendLine("        intParameter: 0");
			sb.AppendLine("        messageOptions: 0");
			return sb;
		}


//		[MenuItem("L7/批量更改动画文件")]
//		static void ModifyAnimationBatch() {
//			string path = AssetDatabase.GetAssetPath(Selection.activeGameObject);
//		}

		public static void ModifyAnimationBatch(string existPath) {
			var path = Path.GetDirectoryName(existPath);
			if (path != null) {
				string[] filePaths = Directory.GetFiles(path, "*.meta", SearchOption.TopDirectoryOnly);

				foreach (var filePath in filePaths) {
					if (filePath.Contains(@"@")) {
						ModifyClip(filePath);
					}
					if (filePath.Contains(@"@attack")) {
						ModifyAnimation(filePath, EventType.OnAttack);
					} else if (filePath.Contains(@"@skill")) {
						ModifyAnimation(filePath, EventType.OnUseSkill);
					} else if (filePath.Contains(@"@death")) {
						ModifyAnimation(filePath, EventType.OnDeathAnimationEnd);
					} else if (filePath.Contains(@"@idle") || filePath.Contains(@"@run") || filePath.Contains(@"@stand")) {
						ModifyAnimation(filePath, EventType.None);
					}
					if (filePath.Contains(@"@")) {
						string animationFilePath = filePath.Remove(filePath.LastIndexOf(@".meta", StringComparison.Ordinal));
						ModelImporter modelImporter = (ModelImporter)AssetImporter.GetAtPath(animationFilePath);
						modelImporter.SaveAndReimport();
					}
				}
			}
		}
	}
}