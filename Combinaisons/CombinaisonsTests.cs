#if TEST

using System;
using System.Collections.Generic;

using NUnit.Framework;

namespace CH.Combinations {
	/// <summary>
	/// Tests the <see cref="Combinations"/> class.
	/// </summary>
	[TestFixture]
	public class CombinationsTests {
		private void CheckIt<T>(T[][] expected, Combinations<T> comb) {
			IEnumerator<T[]> enumerator = comb.GetEnumerator();
			enumerator.Reset();
			for (int i = 0; i < expected.Length; i++) {
				Assert.IsTrue(enumerator.MoveNext());
				Assert.AreEqual(expected[i], enumerator.Current);
			}
			Assert.IsFalse(enumerator.MoveNext());
		}

		/// <summary>
		/// Tests generating 4C3 integers, all elements distinct.
		/// </summary>
		[Test]
		public void SimpleCombinations() {
			int[] data = new int[] { 0, 1, 2, 3 };
			int[][] expected = new int[][] { new int[] { 0, 1, 2 }, new int[] { 0, 1, 3 }, new int[] { 0, 2, 3 }, new int[] { 1, 2, 3 } };
			Combinations<int> comb = new Combinations<int>(data, 3);
			CheckIt(expected, comb);
		}

		/// <summary>
		/// Tests generating 4C3 integers with a pair of elements equal.
		/// </summary>
		[Test]
		public void Duplicates() {
			int[] data = new int[] { 0, 1, 1, 2 };
			int[][] expected = new int[][] { new int[] { 0, 1, 1 }, new int[] { 0, 1, 2 }, new int[] { 1, 1, 2 } };
			Combinations<int> comb = new Combinations<int>(data, 3);
			CheckIt(expected, comb);
		}

		/// <summary>
		/// Generates 6C3 integers with two pairs of duplicates.
		/// </summary>
		[Test]
		public void TwoPairsOfDuplicates() {
			int[] data = new int[] { 0, 1, 1, 2, 2, 3 };
			int[][] expected = new int[][] { new int[] { 0, 1, 1 }, new int[] { 0, 1, 2 }, new int[] { 0, 1, 3 }, new int[] { 0, 2, 2 }, new int[] { 0, 2, 3 }, new int[] { 1, 1, 2 }, new int[] { 1, 1, 3 }, new int[] { 1, 2, 2 }, new int[] { 1, 2, 3 }, new int[] { 2, 2, 3 } };
			Combinations<int> comb = new Combinations<int>(data, 3);
			CheckIt(expected, comb);
		}

		/// <summary>
		/// Tests generating 5C3 integers with three elements equal.
		/// </summary>
		[Test]
		public void Triplicates() {
			int[] data = new int[] { 0, 1, 1, 1, 2 };
			int[][] expected = new int[][] { new int[] { 0, 1, 1 }, new int[] { 0, 1, 2 }, new int[] { 1, 1, 1 }, new int[] { 1, 1, 2 } };
			Combinations<int> comb = new Combinations<int>(data, 3);
			CheckIt(expected, comb);
		}

		/// <summary>
		/// Generates 0C0 integers.
		/// </summary>
		[Test]
		public void EmptySet() {
			int[] data = new int[] { };
			int[][] expected = new int[][] { new int[] { } };
			Combinations<int> comb = new Combinations<int>(data, 0);
			CheckIt(expected, comb);
		}

		/// <summary>
		/// Generates 3C0 integers.
		/// </summary>
		[Test]
		public void EmptyChoose() {
			int[] data = new int[] { 0, 1, 2 };
			int[][] expected = new int[][] { new int[] { } };
			Combinations<int> comb = new Combinations<int>(data, 0);
			CheckIt(expected, comb);
		}

		/// <summary>
		/// Generates 3C3 integers.
		/// </summary>
		[Test]
		public void ChooseAll() {
			int[] data = new int[] { 0, 1, 2 };
			int[][] expected = new int[][] { new int[] { 0, 1, 2 } };
			Combinations<int> comb = new Combinations<int>(data, 3);
			CheckIt(expected, comb);
		}

		/// <summary>
		/// Generates 3C4 integers.
		/// </summary>
		[Test]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void ChooseTooMany() {
			int[] data = new int[] { 0, 1, 2 };
			Combinations<int> comb = new Combinations<int>(data, 4);
		}

		/// <summary>
		/// Generates 4C2 characters.
		/// </summary>
		[Test]
		public void Chars() {
			char[] data = new char[] { 'a', 'A', 'b', 'c' };
			char[][] expected = new char[][] { new char[] { 'A', 'a' }, new char[] { 'A', 'b' }, new char[] { 'A', 'c' }, new char[] { 'a', 'b' }, new char[] { 'a', 'c' }, new char[] { 'b', 'c' } };
			Combinations<char> comb = new Combinations<char>(data, 2);
			CheckIt(expected, comb);
		}

		/// <summary>
		/// Generates 4C2 strings.
		/// </summary>
		[Test]
		public void Strings() {
			string[] data = new string[] { "A", "a", "b", "c" };
			string[][] expected = new string[][] { new string[] { "a", "A" }, new string[] { "a", "b" }, new string[] { "a", "c" }, new string[] { "A", "b" }, new string[] { "A", "c" }, new string[] { "b", "c" } };
			Combinations<string> comb = new Combinations<string>(data, 2);
			CheckIt(expected, comb);
		}

		/// <summary>
		/// Generates 4C2 strings using a case-insensitive comparer.
		/// </summary>
		[Test]
		public void StringsInsensitive() {
			string[] data = new string[] { "a", "A", "b", "c" };
			string[][] expected = new string[][] { new string[] { "a", "a" }, new string[] { "a", "b" }, new string[] { "a", "c" }, new string[] { "b", "c" } };
			Combinations<string> comb = new Combinations<string>(data, StringComparer.CurrentCultureIgnoreCase, 2);
			CheckIt(expected, comb);
		}
	}
}

#endif
