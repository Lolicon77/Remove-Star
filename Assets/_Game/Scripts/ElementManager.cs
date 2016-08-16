using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using L7;

namespace Game {
	public class ElementManager : Singleton<ElementManager> {

		public const int CELL_ROW_COUNT = 8;
		public const int CELL_LINE_COUNT = 8;

		public const int CELL_WIDTH = 30;
		public const int CELL_HEIGHT = 30;

		public const float MOVE_TIME_ONCE = 0.2f;
		public const float DISAPPEAR_TIME = 0.1f;
		public const float REBORN_TIME_AFTER_DISAPPEAR = 0.1f;

		public Element elementPrefab;

		public Dictionary<PositionIndex, Element> elements = new Dictionary<PositionIndex, Element>();

		private List<PositionIndex> nullPositions;

		protected override void Awake() {
			base.Awake();
			nullPositions = new List<PositionIndex>();
		}

		void CreateElements() {
			for (int i = 0; i < CELL_ROW_COUNT; i++) {
				for (int j = 0; j < CELL_LINE_COUNT; j++) {
					var elementIns = Instantiate(elementPrefab);
					elementIns.currentPositionIndex = new PositionIndex();
				}
			}
		}

		public Element GetNextElementByOrientation(Element source, Orientation orientation) {
			PositionIndex targetPositionIndex = source.currentPositionIndex;
			Element target;
			switch (orientation) {
				case Orientation.Up:
					targetPositionIndex.y += 1;
					break;
				case Orientation.Down:
					targetPositionIndex.y -= 1;
					break;
				case Orientation.Left:
					targetPositionIndex.x -= 1;
					break;
				case Orientation.Right:
					targetPositionIndex.x += 1;
					break;
			}
			Ins.elements.TryGetValue(source.currentPositionIndex, out target);
			return target;
		}


		public Orientation GetOpposite(Orientation orientation) {
			Orientation result;
			switch (orientation) {
				case Orientation.Up:
					result = Orientation.Down;
					break;
				case Orientation.Down:
					result = Orientation.Up;
					break;
				case Orientation.Left:
					result = Orientation.Right;
					break;
				case Orientation.Right:
					result = Orientation.Left;
					break;
				default:
					throw new Exception();
			}
			return result;
		}


		public bool TryToRemove(Element element, Element targetElement) {
			if (element.TryRemoveSelf() || targetElement.TryRemoveSelf()) {
				return true;
			}
			return false;
		}

		public void CheckDrop(List<Element> removingList = null) {
			if (!removingList.IsNullOrEmpty()) {
				for (int i = 0; i < removingList.Count; i++) {
					if (!nullPositions.Contains(removingList[i].currentPositionIndex)) {
						nullPositions.Add(removingList[i].currentPositionIndex);
					}
				}
				StartCoroutine(RealDrop(REBORN_TIME_AFTER_DISAPPEAR));
			}
		}

		private IEnumerator RealDrop(float seconds) {
			yield return new WaitForSeconds(seconds);
			throw new NotImplementedException();
		}
	}
}