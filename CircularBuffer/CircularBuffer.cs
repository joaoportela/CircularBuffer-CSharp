using System;
using System.Collections.Generic;
using System.Collections;

namespace CircularBuffer
{
	/// <summary>
	/// Circular buffer.
	/// 
	/// this implementation is inspired by
	/// http://www.boost.org/doc/libs/1_37_0/libs/circular_buffer/doc/circular_buffer.html
	/// because I liked their interface.
	/// </summary>
	public class CircularBuffer<T> : IEnumerable<T>
	{
		private int _capacity;
		private T[] _buffer;

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
				// TODO - Add elements from source.
			}
		}

		public int Capacity { get { return _capacity; } }

		public bool IsFull {
			get {
				return Size == Capacity;
			}
		}

		public int Size { get { throw new NotImplementedException (); } }
				
		public T Front ()
		{
			throw new NotImplementedException ();
		}
		
		public T Back ()
		{
			throw new NotImplementedException ();
		}

		public T this [int index] {
			get { throw new NotImplementedException ();}
			set { throw new NotImplementedException ();}
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
			throw new NotImplementedException ();
		}

		public void PopFront ()
		{
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

		// private
		// doing ArrayOne and ArrayTwo methods as seen here: 
		// http://www.boost.org/doc/libs/1_37_0/libs/circular_buffer/doc/circular_buffer.html#classboost_1_1circular__buffer_1957cccdcb0c4ef7d80a34a990065818d
		// http://www.boost.org/doc/libs/1_37_0/libs/circular_buffer/doc/circular_buffer.html#classboost_1_1circular__buffer_1f5081a54afbc2dfc1a7fb20329df7d5b
		// should help a lot with the code.
	}
}
