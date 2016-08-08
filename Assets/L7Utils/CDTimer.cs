using UnityEngine;
using System.Collections;

namespace L7 {
	public class CDTimer
	{

		private float startTime = -10000;
		private readonly float cdTime;

		public CDTimer(float cdTime,bool isCoolingDownAtTheBegining = false)
		{
			this.cdTime = cdTime;
			if (isCoolingDownAtTheBegining)
			{
				EnterCoolingDown();
			}
		}

		public void EnterCoolingDown() {
			startTime = Time.timeSinceLevelLoad;
		}

		public bool IsCoolingDown() {
			return Time.timeSinceLevelLoad - startTime < cdTime;
		}

		public float TimeSinceEnterCoolingDown()
		{
			return Time.timeSinceLevelLoad - startTime;
		}

		public float RemainingCoolingDownTime()
		{
			return cdTime - TimeSinceEnterCoolingDown();
		}

		public void Reset()
		{
			startTime = -10000;
		}

	}
}