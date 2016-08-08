using System;
using UnityEngine;


namespace L7{
	/// <summary>
	/// 简易系统计时器
	/// </summary>
	public class SystemTimer : MonoBehaviour {

		public static DateTime StartTime;

		void OnEnable()
		{
			ResetStartTime();
		}

		public static TimeSpan ReturnTimeSpan()
		{
			return DateTime.Now - StartTime;
		}

		public static void ResetStartTime()
		{
			StartTime = DateTime.Now;
		}

	}	
}

