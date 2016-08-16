using UnityEngine;
using System.Collections;
using DG.Tweening;

namespace Game {
	public class ElementView : MonoBehaviour {
		private RectTransform rectTransform;

		void Awake() {
			rectTransform = GetComponent<RectTransform>();
		}

		public void MoveTo(PositionIndex from,PositionIndex to) {
			PositionIndex deltaPos = to - from;
			rectTransform.DOAnchorPos(rectTransform.anchoredPosition +
									  new Vector2(deltaPos.x * ElementManager.CELL_WIDTH, deltaPos.y + ElementManager.CELL_HEIGHT), ElementManager.MOVE_TIME_ONCE);
		}

		public void Disappear() {
			Destroy(gameObject,ElementManager.DISAPPEAR_TIME);
//			gameObject.SetActive(false);
		}

	}
}