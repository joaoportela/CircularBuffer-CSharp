using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

namespace CircularBuffer
{
	/// <summary>
	/// Circular buffer.
	/// 
	/// When writting to a full buffer:
	/// PushBack -> removes this[0] / Front()
	/// PushFront -> removes this[Size-1] / Back()
	/// 
	/// this implementation is inspired by
	/// http://www.boost.org/doc/libs/1_53_0/libs/circular_buffer/doc/circular_buffer.html
	/// because I liked their interface.
	/// </summary>
	public class CircularBuffer<T> : IEnumerable<T>
	{
		private int _capacity;
		private T[] _buffer;
		private int _start;
		private int _end;
		private int _size;

		public CircularBuffer (int capacity) : this (capacity, null)
		{
		}

		public CircularBuffer (int capacity, IEnumerable<T> source)
		{
			if (capacity < 1) {
				throw new ArgumentException ("Circular buffer cannot have negative or zero capacity.");
			}

			_capacity = capacity;
			_buffer = new T[_capacity];

			if (source != null) {
				T[] sourceArray = source.ToArray ();
				int sourceStartIndex = Math.Max (sourceArray.Length - _capacity, 0);
				int numberOfElementsToCopy = sourceArray.Length - sourceStartIndex;
				Array.Copy (sourceArray, sourceStartIndex, _buffer, 0, numberOfElementsToCopy);
				_size = numberOfElementsToCopy;
			} else {
				_size = 0;
			}

			_start = 0;
			_end = _size - 1;
		}

		public int Capacity { get { return _capacity; } }

		public bool IsFull {
			get {
				return Size == Capacity;
			}
		}

		public bool IsEmpty {
			get {
				return Size == 0;
			}
		}

		public int Size { get { return _size; } }
				
		public T Front ()
		{
			ThrowIfEmpty ();
			return _buffer[_start];
		}
		
		public T Back ()
		{
			ThrowIfEmpty ();
			return _buffer[_end];
		}

		public T this [int index] {
			get {
				ThrowIfEmpty (string.Format ("Cannot access index {0}. Buffer is empty", index));
				throw new NotImplementedException ();
			}
			set {
				ThrowIfEmpty (string.Format ("Cannot access index {0}. Buffer is empty", index));
				throw new NotImplementedException ();
			}
		}

		public void PushBack (T item)
		{
			throw new NotImplementedException ();
		}

		public void PushFront (T item)
		{
			throw new NotImplementedException ();
		}

		public void PopBack ()
		{
			ThrowIfEmpty ("Cannot take elements from an empty buffer.");
			throw new NotImplementedException ();
		}

		public void PopFront ()
		{
			ThrowIfEmpty ("Cannot take elements from an empty buffer.");
			throw new NotImplementedException ();
		}

		#region IEnumerable<T> implementation
		public IEnumerator<T> GetEnumerator ()
		{
			throw new NotImplementedException ();
		}
		#endregion
		#region IEnumerable implementation
		IEnumerator IEnumerable.GetEnumerator ()
		{
			return (IEnumerator)GetEnumerator ();
		}
		#endregion

		private void ThrowIfEmpty (string message = "Cannot access an empty buffer.")
		{
			if (IsEmpty) {
				throw new InvalidOperationException (message);
			}
		}

		private void Increment (ref int index)
		{
			if (++index == _capacity) {
				index = 0;
			}
		}

		private void Decrement (ref int index)
		{
			if (index == 0) {
				index = _capacity;
			}
			index--;
		}

		// private
		// doing ArrayOne and ArrayTwo methods returning ArraySegment<T> as seen here: 
		// http://www.boost.org/doc/libs/1_37_0/libs/circular_buffer/doc/circular_buffer.html#classboost_1_1circular__buffer_1957cccdcb0c4ef7d80a34a990065818d
		// http://www.boost.org/doc/libs/1_37_0/libs/circular_buffer/doc/circular_buffer.html#classboost_1_1circular__buffer_1f5081a54afbc2dfc1a7fb20329df7d5b
		// should help a lot with the code.
	}
}
