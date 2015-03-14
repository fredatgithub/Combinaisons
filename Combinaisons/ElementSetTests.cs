#if TEST

using System.Collections.Generic;

using NUnit.Framework;

namespace CH.Combinations {
	/// <summary>
	/// Tests the <see cref="ElementSet{T}"/> class.
	/// </summary>
	[TestFixture]
	public class ElementSetTests {
		private void CheckSortedList<T, U>(SortedList<T, U> list, T[] expectedKeys, U[] expectedValues) {
			Assert.IsNotNull(list);
			Assert.AreEqual(expectedKeys.Length, list.Count);
			Assert.AreEqual(expectedKeys, new List<T>(list.Keys).ToArray());
			Assert.AreEqual(expectedValues, new List<U>(list.Values).ToArray());
		}

		/// <summary>
		/// Creates an <see cref="ElementSet{T}"/> from an array of integers and verifies its contents.
		/// </summary>
		[Test]
		public void TestSimpleList() {
			int[] data = new int[] { 3, 1, 5, 2, 4 }; // 1, 2, 3, 4, 5
			int[] expectedKeys = new int[] { 1, 2, 3, 4, 5 };
			int[] expectedValues = new int[] { 1, 1, 1, 1, 1 };
			int[] expectedAtOrAfter = new int[] { 5, 4, 3, 2, 1 };
			ElementSet<int> es = new ElementSet<int>(data);
			Assert.AreEqual(5, es.Count);
			Assert.AreEqual(expectedAtOrAfter, es.ElementsAtOrAfter);
			CheckSortedList(es.Elements, expectedKeys, expectedValues);
		}

		/// <summary>
		/// Verifies that duplicates are detected in an array of integers.
		/// </summary>
		[Test]
		public void TestDuplicates() {
			int[] data = new int[] { 1, 2, 5, 2 };
			int[] expectedKeys = new int[] { 1, 2, 5 };
			int[] expectedValues = new int[] { 1, 2, 1 };
			int[] expectedAtOrAfter = new int[] { 4, 3, 1 };
			ElementSet<int> es = new ElementSet<int>(data);
			Assert.AreEqual(4, es.Count);
			Assert.AreEqual(expectedAtOrAfter, es.ElementsAtOrAfter);
			CheckSortedList(es.Elements, expectedKeys, expectedValues);
		}
	}
}
#endif