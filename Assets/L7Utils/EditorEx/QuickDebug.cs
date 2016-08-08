//该类暂时不用


//using System.Collections;
//using Game;
//using LuaInterface;
//using UnityEngine;
//
//namespace L7 {
//	public class QuickDebug : MonoBehaviour {
//		//		public MonoBehaviour mono;
//		//
//		//		public string fieldName;
//		//
//		//		void Start()
//		//		{
//		//			var field = GetComponent(mono.GetType()).GetPrivateField<object>(fieldName);
//		//			Debug.Log(field);
//		//		}
//
//		public TextAsset lua;
//		public LuaState l;
//
//		void Awake() {
//			l = new LuaState();
//		}
//
//		void OnGUI() {
//			//
//			//			GUILayout.Label(UIInterface.AlreadyArrive(type).ToString());
//			//			MainMenuNPC npc = (MainMenuNPC) (MainMenuGameCamera.Ins.TryGetSelectedByType(type));
//			//			GUILayout.Label(Vector3.Distance(MainMenuGameCamera.Ins.target.transform.position, npc.transform.position).ToString("f1"));
//			GUILayout.Space(100);
//			if (GUILayout.Button("QuickDebug")) {
//				l.DoString(lua.text);
//			}
//		}
//
//	}
//}