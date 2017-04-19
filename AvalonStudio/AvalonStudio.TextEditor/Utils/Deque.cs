using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace AvalonStudio.TextEditor.Utils
{
    /// <summary>
    ///     Double-ended queue.
    /// </summary>
    [SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
    public sealed class Deque<T> : ICollection<T>
    {
        private T[] arr = Empty<T>.Array;
        private int head;
        private int tail;

        /// <summary>
        ///     Gets/Sets an element inside the deque.
        /// </summary>
        public T this[int index]
        {
            get
            {
                ThrowUtil.CheckInRangeInclusive(index, "index", 0, Count - 1);
                return arr[(head + index) % arr.Length];
            }
            set
            {
                ThrowUtil.CheckInRangeInclusive(index, "index", 0, Count - 1);
                arr[(head + index) % arr.Length] = value;
            }
        }

        /// <inheritdoc />
        public int Count { get; private set; }

        /// <inheritdoc />
        public void Clear()
        {
            arr = Empty<T>.Array;
            Count = 0;
            head = 0;
            tail = 0;
        }

        /// <inheritdoc />
        public IEnumerator<T> GetEnumerator()
        {
            if (head < tail)
            {
                for (var i = head; i < tail; i++)
                    yield return arr[i];
            }
            else
            {
                for (var i = head; i < arr.Length; i++)
                    yield return arr[i];
                for (var i = 0; i < tail; i++)
                    yield return arr[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        bool ICollection<T>.IsReadOnly
        {
            get { return false; }
        }

        void ICollection<T>.Add(T item)
        {
            PushBack(item);
        }

        /// <inheritdoc />
        public bool Contains(T item)
        {
            var comparer = EqualityComparer<T>.Default;
            foreach (var element in this)
                if (comparer.Equals(item, element))
                    return true;
            return false;
        }

        /// <inheritdoc />
        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array == null)
                throw new ArgumentNullException("array");
            if (head < tail)
            {
                Array.Copy(arr, head, array, arrayIndex, tail - head);
            }
            else
            {
                var num1 = arr.Length - head;
                Array.Copy(arr, head, array, arrayIndex, num1);
                Array.Copy(arr, 0, array, arrayIndex + num1, tail);
            }
        }

        bool ICollection<T>.Remove(T item)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        ///     Adds an element to the end of the deque.
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "PushBack")]
        public void PushBack(T item)
        {
            if (Count == arr.Length)
                SetCapacity(Math.Max(4, arr.Length * 2));
            arr[tail++] = item;
            if (tail == arr.Length) tail = 0;
            Count++;
        }

        /// <summary>
        ///     Pops an element from the end of the deque.
        /// </summary>
        public T PopBack()
        {
            if (Count == 0)
                throw new InvalidOperationException();
            if (tail == 0)
                tail = arr.Length - 1;
            else
                tail--;
            var val = arr[tail];
            arr[tail] = default(T); // allow GC to collect the element
            Count--;
            return val;
        }

        /// <summary>
        ///     Adds an element to the front of the deque.
        /// </summary>
        public void PushFront(T item)
        {
            if (Count == arr.Length)
                SetCapacity(Math.Max(4, arr.Length * 2));
            if (head == 0)
                head = arr.Length - 1;
            else
                head--;
            arr[head] = item;
            Count++;
        }

        /// <summary>
        ///     Pops an element from the end of the deque.
        /// </summary>
        public T PopFront()
        {
            if (Count == 0)
                throw new InvalidOperationException();
            var val = arr[head];
            arr[head] = default(T); // allow GC to collect the element
            head++;
            if (head == arr.Length) head = 0;
            Count--;
            return val;
        }

        private void SetCapacity(int capacity)
        {
            var newArr = new T[capacity];
            CopyTo(newArr, 0);
            head = 0;
            tail = Count == capacity ? 0 : Count;
            arr = newArr;
        }
    }
}