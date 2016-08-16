using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using DG.Tweening;
using L7;
using UnityEditor;
using UnityEngine.EventSystems;

namespace Game {
	public enum ElementType {
		One = 1,
		Two,
		Three,
		Four,
		Five,
		Six
	}

	public enum Orientation {
		Up = 1,
		Down,
		Left,
		Right
	}

	public struct PositionIndex {
		public int x;
		public int y;

		PositionIndex(int x, int y) {
			this.x = x;
			this.y = y;
		}

		public static PositionIndex operator +(PositionIndex lhs, PositionIndex rhs) {
			return new PositionIndex(lhs.x + rhs.x, lhs.y + rhs.y);
		}

		public static PositionIndex operator -(PositionIndex lhs, PositionIndex rhs) {
			return new PositionIndex(lhs.x - rhs.x, lhs.y - rhs.y);
		}
	}

	public class Element : MonoBehaviour, IDragHandler, IPointerUpHandler {

		private Element targetElement;
		private ElementView elementView;

		public ElementType elementType;
		public PositionIndex lastPositionIndex;
		public PositionIndex currentPositionIndex;

		public bool isMoving;
		public bool isRemoving;
		public bool canDrag = true;
		private bool isRevert = false;


		public Array orientationValues;


		void Awake() {
			orientationValues = Enum.GetValues(typeof(Orientation));
			elementView = this.GetOrAddComponent<ElementView>();
		}

		bool CanMove() {
			return !isMoving && !isRemoving;
		}

		bool CanRemove() {
			return !isMoving && !isRemoving;
		}

		void MoveByOrientation(Orientation orientation) {
		}

		void MoveTo(PositionIndex targetPositionIndex) {
			isMoving = true;
			elementView.MoveTo(currentPositionIndex, targetPositionIndex);
			ElementManager.Ins.elements[targetPositionIndex] = this;
			lastPositionIndex = currentPositionIndex;
			currentPositionIndex = targetPositionIndex;
			StartCoroutine(OnMoveEnd(ElementManager.MOVE_TIME_ONCE));
		}

		IEnumerator OnMoveEnd(float seconds) {
			yield return new WaitForSeconds(seconds);
			isMoving = false;
			if (!isRevert) {
				if (!ElementManager.Ins.TryToRemove(this, targetElement)) {
					isRevert = true;
					RealExchangeWith(targetElement);
				}
			} else {
				isRevert = false;
			}
		}

		void TryExchangeWith(Orientation orientation) {
			if (!CanMove()) {
				return;
			}
			var target = ElementManager.Ins.GetNextElementByOrientation(this, orientation);
			if (target && target.CanMove()) {
				RealExchangeWith(target);
			}
		}

		void RealExchangeWith(Element target) {
			MoveTo(target.currentPositionIndex);
			targetElement = target;
			target.MoveTo(currentPositionIndex);
			target.targetElement = this;
		}

		public bool TryRemoveSelf() {
			List<Element> rowList = new List<Element>();
			List<Element> lineList = new List<Element>();

			foreach (Orientation orientation in orientationValues) {
				var nextElement = ElementManager.Ins.GetNextElementByOrientation(this, orientation);
				switch (orientation) {
					case Orientation.Up:
					case Orientation.Down:
						TryToGetElementsByOrientationUtilFindAnotherType(nextElement, orientation, ref rowList);
						break;
					case Orientation.Left:
					case Orientation.Right:
						TryToGetElementsByOrientationUtilFindAnotherType(nextElement, orientation, ref lineList);
						break;
				}
			}
			if (rowList.Count >= 2 || lineList.Count >= 2) {
				List<Element> removingList = new List<Element>();
				removingList.Add(this);
				if (rowList.Count >= 2) {
					removingList.AddRange(rowList);
				}
				if (lineList.Count >= 2) {
					removingList.AddRange(lineList);
				}
				for (int i = 0; i < removingList.Count; i++) {
					removingList[i].RealRemove();
				}
				ElementManager.Ins.CheckDrop(removingList);
				return true;
			}
			return false;
		}

		void RealRemove() {
			ElementManager.Ins.elements[currentPositionIndex] = null;
			elementView.Disappear();
		}

		void TryToGetElementsByOrientationUtilFindAnotherType(Element element, Orientation orientation, ref List<Element> elements) {
			if (elements.IsNullOrEmpty()) {
				return;
			}
			var nextElement = ElementManager.Ins.GetNextElementByOrientation(element, orientation);
			if (nextElement.elementType == elementType && nextElement.CanRemove()) {
				elements.Add(nextElement);
				TryToGetElementsByOrientationUtilFindAnotherType(nextElement, orientation, ref elements);
			}
		}


		public void OnDrag(PointerEventData eventData) {
			if (!canDrag) {
				return;
			}

			var deltaX = eventData.position.x - eventData.pressPosition.x;
			var deltaY = eventData.position.y - eventData.pressPosition.y;

			if (Mathf.Abs(deltaX) > 30 || Mathf.Abs(deltaY) > 30) {
				canDrag = false;
				if (Mathf.Abs(deltaX) > Mathf.Abs(deltaY)) {
					TryExchangeWith(deltaX > 0 ? Orientation.Right : Orientation.Left);
				} else {
					TryExchangeWith(deltaY > 0 ? Orientation.Up : Orientation.Down);
				}
			}

		}

		public void OnPointerUp(PointerEventData eventData) {
			canDrag = true;
		}
	}



}