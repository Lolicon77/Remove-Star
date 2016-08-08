using System.Collections.Generic;

namespace L7 {
	public static class IListExtensions {
		public static T Random<T>(this IList<T> collection) {
			if ((collection == null) || (collection.Count == 0)) {
				return default(T);
			}
			int num = UnityEngine.Random.Range(0, collection.Count);
			return collection[num];
		}

		public static void Shuffle<T>(this IList<T> list) {
			list.ShuffleRange<T>(0, list.Count - 1);
		}

		public static void ShuffleRange<T>(this IList<T> list, int startIndex, int endIndex) {
			for (int i = endIndex; i > startIndex; i--) {
				int num2 = UnityEngine.Random.Range(startIndex, i + 1);
				T local = list[num2];
				list[num2] = list[i];
				list[i] = local;
			}
		}

		public static void Swap<T>(this IList<T> collection, int firstIndex, int secondIndex) {
			T local = collection[firstIndex];
			collection[firstIndex] = collection[secondIndex];
			collection[secondIndex] = local;
		}

		public static bool IsNullOrEmpty<T>(this IList<T> collection) {
			if (collection == null || collection.Count == 0) {
				return true;
			}
			return false;
		}

	}
}

