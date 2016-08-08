using UnityEngine;

namespace L7
{
	public class StaticBatchThis : MonoBehaviour {

		void Start() {
			StopWatchUtil.Start("StaticBatch测试");
			StaticBatchingUtility.Combine(gameObject);
			StopWatchUtil.StopAndPrint("StaticBatch测试", name);
		}

	}
}
