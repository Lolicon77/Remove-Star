using System.Collections;
using System.Diagnostics;
using L7;
using UnityEngine;

namespace L7
{
	public class StaticBatchByStep : MonoBehaviour {
		public GameObject[] ToBatchs;

		IEnumerator Start() {
			foreach (var batch in ToBatchs) {
//				StopWatchUtil.Start("StaticBatching测试",false);
				StaticBatchingUtility.Combine(batch);
//				StopWatchUtil.StopAndPrint("StaticBatching测试",batch?batch.name:"unknown");
				for (int i = 0; i < 5; i++)
				{
					yield return null;
				}
			}
		}

	}
}

