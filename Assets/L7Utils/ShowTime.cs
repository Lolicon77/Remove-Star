using System;
using UnityEngine;

namespace Game {
	public class ShowTime : MonoBehaviour {


		public enum TimerDirection {
			leftUp,
			rightUp,
			leftDown,
			rightDown
		}

		public int fontSize = 16;
		public TimerDirection timerDirection;
		public Color color;
		private float rate;

		private string time;
		private GUIStyle style;

		private Rect rect;

		void Start() {
			rate = Screen.height / 640f;
		}

		void OnGUI() {
			style = new GUIStyle {fontSize = (int) (fontSize*rate), normal = {textColor = color}};
			time = DateTime.Now.TimeOfDay.ToString().Split('.')[0];

			switch (timerDirection) {
				case TimerDirection.leftUp:
					rect = new Rect(100f * rate, 100f, 100f * rate, 20f * rate);
					break;
				case TimerDirection.leftDown:
					rect = new Rect(100f * rate, Screen.height - 100f, 100f * rate, 20f * rate);
					break;
				case TimerDirection.rightUp:
					rect = new Rect((Screen.width - 100f) * rate, 100f, 100f * rate, 20f * rate);
					break;
				case TimerDirection.rightDown:
					rect = new Rect((Screen.width - 100f) * rate, Screen.height - 100f, 100f * rate, 20f * rate);
					break;
			}

			GUI.Label(rect, time,style);
		}


	}
}

