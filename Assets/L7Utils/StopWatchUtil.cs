using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;


namespace L7{
	/// <summary>
	/// 简易系统计时器
	/// </summary>
	public class StopWatchUtil
	{
		public static Stopwatch quickStopWatch;
		public static Dictionary<string,Stopwatch> stopwatchs;

		public static void QuickStart(bool printInfo = true) {
			if (quickStopWatch == null) {
				quickStopWatch = new Stopwatch();
			}
			quickStopWatch.Reset();
			quickStopWatch.Start();
			if (printInfo) {
				UnityEngine.Debug.Log("QuickStopWatch时间已归零");
			}
		}

		public static void QuickStopAndPrint(string introduction = null) {
			if (quickStopWatch == null) {
				UnityEngine.Debug.Log("未检测到quickStopWatch");
			} else {
				quickStopWatch.Stop();
				UnityEngine.Debug.Log(introduction + "  quickStopWatch 此次即使结果为：" + quickStopWatch.ElapsedMilliseconds + "ms");
			}
		}


		public static void Start(string name,bool printInfo =true) {
			if (stopwatchs == null) {
				stopwatchs = new Dictionary<string, Stopwatch>();
			}
			Stopwatch sw;
			stopwatchs.TryGetValue(name,out sw);
			if (!stopwatchs.TryGetValue(name, out sw)) {
				sw = new Stopwatch();
				if (printInfo) {
					UnityEngine.Debug.Log( "实例化一个新的stopWatch  " + name);
				}
				stopwatchs.Add(name,sw);
				sw.Start();
			} else {
				stopwatchs[name].Reset();
				stopwatchs[name].Start();
				if (printInfo)
				{
					UnityEngine.Debug.Log("stopWatch " + name+ " 的时间已重置");
				}
			}
		}

		public static void StopAndPrint(string name,string introduction =null)
		{
			Stopwatch sw;
			if (!stopwatchs.TryGetValue(name, out sw))
			{
				UnityEngine.Debug.Log("未找到该stopWatch");
			}
			else
			{
				sw =stopwatchs[name];
				sw.Stop();
				UnityEngine.Debug.Log(introduction +"   stopWatch " + name +  " 此次计时结果为：" + sw.ElapsedMilliseconds + "ms");
			}
		}
	}	
}

