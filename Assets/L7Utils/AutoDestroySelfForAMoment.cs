using UnityEngine;

namespace L7{

	/// <summary>
	/// 销毁自己的工具类
	/// </summary>
	public class AutoDestroySelfForAMoment : MonoBehaviour
	{
		/// <summary>
		/// 若不定义对象存活时间，默认为5秒
		/// </summary>
		public float GameObjectDuration = 5;

		/// <summary>
		/// 子类继承该类如果有Start方法需添加base.start
		/// </summary>
		protected virtual void Start()
		{
			this.Invoke(DestroyThisGameObject,GameObjectDuration);
		}

		void DestroyThisGameObject()
		{
			Destroy(gameObject);
		}
	}	
}

