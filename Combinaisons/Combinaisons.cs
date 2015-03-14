using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace CH.Combinations
{
  /// <summary>
  /// Stores a full specification of combinations to generate, and allows the construction of enumerators to generate the combinations.
  /// </summary>
  /// <typeparam name="T">the type to generate combinations of</typeparam>
  public class Combinations<T> : IEnumerable<T[]>
  {
    #region Combinations Members
    private readonly ElementSet<T> _elements;
    private readonly int _choose;

    /// <summary>
    /// Constructs a new <c>Combinations</c> given a collection of elements and how many elements should be present in each combination. Note that if the same collection of elements is used with multiple <paramref name="choose"/> values, it is faster to construct an <see cref="ElementSet{T}"/> once and use <see cref="Combinations{T}(ElementSet{T}, int)"/>.
    /// </summary>
    /// <param name="elements">the collection of elements</param>
    /// <param name="choose">the length of a combination</param>
    public Combinations(IEnumerable<T> elements, int choose)
      : this(new ElementSet<T>(elements, null), choose)
    {
    }

    /// <summary>
    /// Constructs a new <c>Combinations</c> given a collection of elements and how many elements should be present in each combination. Note that if the same collection of elements is used with multiple <paramref name="choose"/> values, it is faster to construct an <see cref="ElementSet{T}"/> once and use <see cref="Combinations{T}(ElementSet{T}, int)"/>.
    /// </summary>
    /// <param name="elements">the collection of elements</param>
    /// <param name="choose">the length of a combination</param>
    /// <param name="comparer">the comparer used to order the elements</param>
    public Combinations(IEnumerable<T> elements, IComparer<T> comparer, int choose)
      : this(new ElementSet<T>(elements, comparer), choose)
    {
    }

    /// <summary>
    /// Constructs a new <c>Combinations</c> given a collection of elements and how many elements should be present in each combination. If the same set of elements will be used with different values of <paramref name="choose"/>, this constructor is the best choice as the elements are processed only once, during the construction of the <see cref="ElementSet{T}"/>.
    /// </summary>
    /// <param name="elements">the collection of elements</param>
    /// <param name="choose">the length of a combination</param>
    public Combinations(ElementSet<T> elements, int choose)
    {
      if (elements == null)
      {
        throw new ArgumentNullException("elements");
      }
      if (choose < 0 || choose > elements.Count)
      {
        throw new ArgumentOutOfRangeException("choose");
      }
      _elements = elements;
      _choose = choose;
    }
    #endregion

    #region IEnumerable<T[]> Members
    /// <summary>
    /// Creates and returns an enumerator over the combinations. Each combination's elements will be returned in ascending order as defined by the compararer. The combinations themselves will also be returned in ascending order.
    /// </summary>
    /// <returns>an enumerator</returns>
    /// <seealso cref="IEnumerable{T}.GetEnumerator"/>
    public IEnumerator<T[]> GetEnumerator()
    {
      if (_choose == 0)
      {
        return new ChooseZeroEnumerator();
      }

      return new Enumerator(this);
    }

    #endregion

    #region IEnumerable Members
    IEnumerator IEnumerable.GetEnumerator()
    {
      return new Enumerator(this);
    }
    #endregion

    #region Enumerator Class
    private class Enumerator : IEnumerator<T[]>
    {
      #region Enumerator Members
      private readonly Combinations<T> _combinations;
      private readonly int[] _indices;
      private readonly int[] _indicesAt;
      private bool _pastEnd;

      internal Enumerator(Combinations<T> combinations)
      {
        _combinations = combinations;
        _indices = new int[_combinations._choose];
        _indicesAt = new int[_combinations._elements.Elements.Count];
        Reset();
      }

      private void MoveFirst()
      {
        // Move all the indices as far left as possible.
        // Lower-numbered indices go further left.
        int currentIndex = 0;
        int usedCurrent = 0;
        for (int i = 0; i < _indices.Length; i++)
        {
          Debug.Assert(currentIndex < _combinations._elements.Elements.Count);
          _indices[i] = currentIndex;
          _indicesAt[currentIndex]++;
          usedCurrent++;
          if (usedCurrent == _combinations._elements.Elements.Values[currentIndex])
          {
            // We've used all of the current element. Go to the next element.
            currentIndex++;
            usedCurrent = 0;
          }
        }
      }

      private void MoveIndex(int index, int offset)
      {
        if (_indices[index] >= 0 && _indices[index] < _indicesAt.Length)
        {
          _indicesAt[_indices[index]]--;
        }
        _indices[index] += offset;
        if (_indices[index] >= 0 && _indices[index] < _indicesAt.Length)
        {
          _indicesAt[_indices[index]]++;
        }
      }

      private bool IsFull(int position)
      {
        // True if (position) has as many indices as it can hold.
        return _indicesAt[position] == _combinations._elements.Elements.Values[position];
      }

      private bool CanFitRemainingIndices(int index)
      {
        int space = _combinations._elements.ElementsAtOrAfter[_indices[index]];
        return space >= _indices.Length - index;
      }

      private bool AdvanceIndex(int index, int doNotReach)
      {
        // First move the index one position to the right.
        MoveIndex(index, 1);
        // If we've found an existing position with no other index, and there's room at-and-to-the-right-of us for all the indices greater than us, we're good.
        if (_indices[index] < doNotReach)
        {
          if (_indicesAt[_indices[index]] == 1)
          {
            if (CanFitRemainingIndices(index))
            {
              return true;
            }
          }
        }
        // We've either fallen off the right hand edge, or hit a position with another index. If we're index 0, we're past the end of the enumeration. If not, we need to advance the next lower index, then push ourself left as far as possible until we hit another occupied position.
        if (index == 0)
        {
          _pastEnd = true;
          return false;
        }
        if (!AdvanceIndex(index - 1, _indices[index]))
        {
          return false;
        }
        // We must move at least one space to the left. If we can't, the enumeration is over.
        // If the position immediately to the left is already full, we can't move there.
        if (IsFull(_indices[index] - 1))
        {
          _pastEnd = true;
          return false;
        }
        // Move left until the next leftmost element is full, or the current element has an index.
        do
        {
          MoveIndex(index, -1);
        } while (_indicesAt[_indices[index]] == 1 && !IsFull(_indices[index] - 1));
        return true;
      }
      #endregion

      #region IEnumerator<T[]> Members
      public T[] Current
      {
        get
        {
          if (_indices[0] == -1 || _pastEnd)
          {
            // Before first or after last.
            throw new InvalidOperationException();
          }

          T[] current = new T[_indices.Length];
          for (int i = 0; i < _indices.Length; i++)
          {
            current[i] = _combinations._elements.Elements.Keys[_indices[i]];
          }
          return current;
        }
      }
      #endregion

      #region IDisposable Members
      public void Dispose()
      {
        // Do nothing.
      }
      #endregion

      #region IEnumerator Members
      object IEnumerator.Current
      {
        get
        {
          return Current;
        }
      }

      public bool MoveNext()
      {
        if (_pastEnd)
        {
          return false;
        }
        if (_indices[0] == -1)
        {
          MoveFirst();
          return true;
        }

        bool ret = AdvanceIndex(_indices.Length - 1, _indicesAt.Length);
        return ret;
      }

      public void Reset()
      {
        for (int i = 0; i < _indices.Length; i++)
        {
          _indices[i] = -1;
        }
        Array.Clear(_indicesAt, 0, _indicesAt.Length);
        _pastEnd = false;
      }
      #endregion
    }
    #endregion

    #region ChooseZeroEnumerator Class
    private class ChooseZeroEnumerator : IEnumerator<T[]>
    {
      #region ChooseZeroEnumerator Members
      private enum State
      {
        BeforeBeginning,
        OnElement,
        PastEnd,
      }

      private State _state;

      internal ChooseZeroEnumerator()
      {
        Reset();
      }
      #endregion

      #region IEnumerator<T[]> Members
      public T[] Current
      {
        get
        {
          if (_state == State.OnElement)
          {
            return new T[0];
          }

          throw new InvalidOperationException();
        }
      }
      #endregion

      #region IDisposable Members
      public void Dispose()
      {
        // Do nothing.
      }
      #endregion

      #region IEnumerator Members
      object IEnumerator.Current
      {
        get
        {
          return Current;
        }
      }

      public bool MoveNext()
      {
        switch (_state)
        {
          case State.BeforeBeginning:
            _state = State.OnElement;
            return true;

          case State.OnElement:
          case State.PastEnd:
            _state = State.PastEnd;
            return false;

          default:
            throw new InvalidOperationException();
        }
      }

      public void Reset()
      {
        _state = State.BeforeBeginning;
      }
      #endregion
    }
    #endregion
  }
}