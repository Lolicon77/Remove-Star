using System;
using System.Collections.Generic;
using UnityEngine;

namespace LTHUtility {

	public class Pool : MonoBehaviour {
		private static Dictionary<string, Pool> _allPool = new Dictionary<string, Pool>();

		[SerializeField]
		[Tooltip("对象池名称，将在代码中获取对象池时使用")]
		private string _name;
		[SerializeField]
		[Tooltip("生成时是否作为生成物体的父物体")]
		private bool _asParentWhenSpawn = true;
		[SerializeField]
		[Tooltip("销毁后是否作为生成物体的父物体")]
		private bool _asParentAfterDespawn = true;

		private Dictionary<GameObject, PoolObjList> _prefabToList;
		private Dictionary<GameObject, PoolObjList> _insToList;

		void Awake() {
			_allPool.Add(_name, this);
			_prefabToList = new Dictionary<GameObject, PoolObjList>();
			_insToList = new Dictionary<GameObject, PoolObjList>();
		}

		void OnDestroy() {
			_allPool.Remove(_name);
		}

		/// <summary>
		/// 获取一个对象池
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public static Pool GetPool(string name) {
			return _allPool[name];
		}

		/// <summary>
		/// 清理对象池
		/// </summary>
		/// <param name="destroy">是否销毁对象池中的物体</param>
		public void Clean(bool destroy) {
			foreach (var list in _prefabToList.Values) {
				list.Clean(destroy);
			}
			_prefabToList.Clear();
			_insToList.Clear();
		}

		/// <summary>
		/// 产生物体
		/// 试图从对象池中获取一个物体，如果没有则实例化一个物体
		/// </summary>
		/// <param name="prefab"></param>
		/// <returns></returns>
		public GameObject Spawn(GameObject prefab) {
			return Spawn(prefab, Vector3.zero);
		}

		/// <summary>
		/// 产生物体
		/// 试图从对象池中获取一个物体，如果没有则实例化一个物体
		/// </summary>
		/// <param name="prefab"></param>
		/// <param name="pos"></param>
		/// <returns></returns>
		public GameObject Spawn(GameObject prefab, Vector3 pos) {
			return Spawn(prefab, pos, Quaternion.identity);
		}

		/// <summary>
		/// 产生物体
		/// 试图从对象池中获取一个物体，如果没有则实例化一个物体
		/// </summary>
		/// <param name="prefab"></param>
		/// <param name="pos"></param>
		/// <param name="rot"></param>
		/// <returns></returns>
		public GameObject Spawn(GameObject prefab, Vector3 pos, Quaternion rot) {
			PoolObjList list;
			if (!_prefabToList.TryGetValue(prefab, out list)) {
				list = new PoolObjList();
				_prefabToList.Add(prefab, list);
			}
			var ins = list.Spawn(prefab, pos, rot);
			_insToList.Add(ins, list);
			if (_asParentWhenSpawn) {
				ins.transform.SetParent(transform);
			}
			ins.SetActive(true);
			return ins;
		}

		/// <summary>
		/// 回收物体
		/// </summary>
		/// <param name="ins">回收物体的实例</param>
		public void Despawn(GameObject ins) {
			if (ins == null) {
				throw new ArgumentNullException("ins" + "回收实例不能为空");
			}
			PoolObjList list;
			if (!_insToList.TryGetValue(ins, out list)) {
				ins.LogError("没有找到回收实例对应的列表,将会直接销毁该物体");
				Destroy(ins);
				return;
			}
			list.Despawn(ins);
			_insToList.Remove(ins);
			ins.SetActive(false);
			if (_asParentAfterDespawn) {
				ins.transform.SetParent(transform);
			}
		}

		/// <summary>
		/// 池中是否已存在已激活的该预制的实例
		/// </summary>
		/// <param name="ins"></param>
		/// <returns></returns>
		public bool ContainActiveIns(GameObject prefab) {
			return _prefabToList.ContainsKey(prefab) && _prefabToList[prefab].ContainActiveIns();
		}
	}
}
