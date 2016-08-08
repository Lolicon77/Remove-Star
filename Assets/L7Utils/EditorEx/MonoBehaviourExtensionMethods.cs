using System.Collections;

namespace UnityEngine {
	public static class MonoBehaviourExtensionMethods {

		public delegate void Task();

		//	public delegate IEnumerator Continue(Object t = null);

		public static void Invoke(this MonoBehaviour mono, Task task, float time) {
			mono.Invoke(task.Method.Name, time);
		}

		public static void InvokeRepeating(this MonoBehaviour mono, Task task, float time, float repeatRate) {
			mono.InvokeRepeating(task.Method.Name, time, repeatRate);
		}

		public static void CancelInvoke(this MonoBehaviour mono, Task task) {
			mono.CancelInvoke(task.Method.Name);
		}

		public static bool IsInvoking(this MonoBehaviour mono, Task task, float time, float repeatRate) {
			return mono.IsInvoking(task.Method.Name);
		}

		public static void StopOldCoroutineAndStartNew(this MonoBehaviour mono, ref Coroutine oldCoroutine, IEnumerator newCoroutine) {
			if (oldCoroutine != null) {
				mono.StopCoroutine(oldCoroutine);
			}
			oldCoroutine = mono.StartCoroutine(newCoroutine);
		}

		//	public static Coroutine StartCoroutine(this MonoBehaviour mono, Continue con, [DefaultValue("null")]Object value) {
		//		return mono.StartCoroutine(con.Method.Name, value);
		//	}
		//
		//	public static void StopCoroutine(this MonoBehaviour mono, Continue con) {
		//		mono.StopCoroutine(con.Method.Name);
		//	}


		//	public static void Log(this MonoBehaviour mono, string text) {
		//		Debug.Log(mono.gameObject.name +"   " + text);
		//	}


	}
}

