using UnityEngine;

/// <summary>
/// Be aware this will not prevent a non singleton constructor
///   such as `T myT = new T();`
/// To prevent that, add `protected T () {}` to your singleton class.
/// 
/// As a note, this is made as MonoBehaviour because we need Coroutines.
/// </summary>
public class Singleton<T> : MonoBehaviour where T : Singleton<T> {
	protected static T _instance;

	public static T Ins {
		get {
			return _instance;
		}
	}

	protected virtual void Awake(){
		_instance = (T)this;
	}

	protected virtual void OnDestroy() {
		_instance = null;
	}
}