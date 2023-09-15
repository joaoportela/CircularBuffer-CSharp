using System;
using System.Collections.Generic;

namespace CircularBuffer
{
    /// <summary>
    /// Circular buffer.
    ///
    /// When writing to a full buffer:
    /// PushBack -> removes this[0] / Front()
    /// PushFront -> removes this[Size-1] / Back()
    ///
    /// this implementation is inspired by
    /// http://www.boost.org/doc/libs/1_53_0/libs/circular_buffer/doc/circular_buffer.html
    /// because I liked their interface.
    /// </summary>
    public interface ICircularBuffer<T> : IReadOnlyCollection<T>
    {
        /// <summary>
        /// Index access to elements in buffer.
        /// Index does not loop around like when adding elements,
        /// valid interval is [0;Size[
        /// </summary>
        /// <param name="index">Index of element to access.</param>
        /// <exception cref="IndexOutOfRangeException">Thrown when index is outside of [; Size[ interval.</exception>
        T this[int index] { get; set; }

        /// <summary>
        /// Maximum capacity of the buffer. Elements pushed into the buffer after
        /// maximum capacity is reached (IsFull = true), will remove an element.
        /// </summary>
        int Capacity { get; }

        /// <summary>
        /// Boolean indicating if Circular is at full capacity.
        /// Adding more elements when the buffer is full will
        /// cause elements to be removed from the other end
        /// of the buffer.
        /// </summary>
        bool IsFull { get; }

        /// <summary>
        /// True if has no elements.
        /// </summary>
        bool IsEmpty { get; }

        /// <summary>
        /// Current buffer size (the number of elements that the buffer has).
        /// </summary>
        [Obsolete("Use Count property instead")]
        int Size { get; }

        /// <summary>
        /// Element at the back of the buffer - this[Size - 1].
        /// </summary>
        /// <returns>The value of the element of type T at the back of the buffer.</returns>
        [Obsolete("Use Last() method instead")]
        T Back();

        /// <summary>
        /// Element at the front of the buffer - this[0].
        /// </summary>
        /// <returns>The value of the element of type T at the front of the buffer.</returns>
        [Obsolete("Use First() method instead")]
        T Front();

        /// <summary>
        /// Element at the back of the buffer - this[Size - 1].
        /// </summary>
        /// <returns>The value of the element of type T at the back of the buffer.</returns>
        T First();

        /// <summary>
        /// Element at the front of the buffer - this[0].
        /// </summary>
        /// <returns>The value of the element of type T at the front of the buffer.</returns>
        T Last();

        /// <summary>
        /// Clears the contents of the array. Size = 0, Capacity is unchanged.
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        void Clear();

        /// <summary>
        /// Removes the element at the back of the buffer. Decreasing the 
        /// Buffer size by 1.
        /// </summary>
        void PopBack();

        /// <summary>
        /// Removes the element at the front of the buffer. Decreasing the 
        /// Buffer size by 1.
        /// </summary>
        void PopFront();

        /// <summary>
        /// Pushes a new element to the back of the buffer. Back()/this[Size-1]
        /// will now return this element.
        ///
        /// When the buffer is full, the element at Front()/this[0] will be
        /// popped to allow for this new element to fit.
        /// </summary>
        /// <param name="item">Item to push to the back of the buffer</param>
        void PushBack(T item);

        /// <summary>
        /// Pushes a new element to the front of the buffer. Front()/this[0]
        /// will now return this element.
        ///
        /// When the buffer is full, the element at Back()/this[Size-1] will be
        /// popped to allow for this new element to fit.
        /// </summary>
        /// <param name="item">Item to push to the front of the buffer</param>
        void PushFront(T item);

        /// <summary>
        /// Copies the buffer contents to an array, according to the logical
        /// contents of the buffer (i.e. independent of the internal
        /// order/contents)
        /// </summary>
        /// <returns>A new array with a copy of the buffer contents.</returns>
        T[] ToArray();

        /// <summary>
        /// Copies the buffer contents to the array, according to the logical
        /// contents of the buffer (i.e. independent of the internal
        /// order/contents)
        /// </summary>
        /// <param name="array">The array that is the destination of the elements copied from the current buffer.</param>
        void CopyTo(T[] array);

        /// <summary>
        /// Copies the buffer contents to the array, according to the logical
        /// contents of the buffer (i.e. independent of the internal
        /// order/contents)
        /// </summary>
        /// <param name="array">The array that is the destination of the elements copied from the current buffer.</param>
        /// <param name="index">A 32-bit integer that represents the index in array at which copying begins.</param>
        void CopyTo(T[] array, int index);

        /// <summary>
        /// Copies the buffer contents to the array, according to the logical
        /// contents of the buffer (i.e. independent of the internal
        /// order/contents)
        /// </summary>
        /// <param name="array">The array that is the destination of the elements copied from the current buffer.</param>
        /// <param name="index">A 64-bit integer that represents the index in array at which copying begins.</param>
        void CopyTo(T[] array, long index);

#if (NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER)

        /// <summary>
        /// Copies the buffer contents to the <see cref="Memory&lt;T&gt;"/>, according to the logical
        /// contents of the buffer (i.e. independent of the internal
        /// order/contents)
        /// </summary>
        /// <param name="memory">The memory that is the destination of the elements copied from the current buffer.</param>
        void CopyTo(Memory<T> memory);

        /// <summary>
        /// Copies the buffer contents to the <see cref="Span&lt;T&gt;"/>, according to the logical
        /// contents of the buffer (i.e. independent of the internal
        /// order/contents)
        /// </summary>
        /// <param name="span">The span that is the destination of the elements copied from the current buffer.</param>
        void CopyTo(Span<T> span);

#endif

        /// <summary>
        /// Get the contents of the buffer as 2 ArraySegments.
        /// Respects the logical contents of the buffer, where
        /// each segment and items in each segment are ordered
        /// according to insertion.
        ///
        /// Fast: does not copy the array elements.
        /// Useful for methods like <c>Send(IList&lt;ArraySegment&lt;Byte&gt;&gt;)</c>.
        ///
        /// <remarks>Segments may be empty.</remarks>
        /// </summary>
        /// <returns>An IList with 2 segments corresponding to the buffer content.</returns>
        IList<ArraySegment<T>> ToArraySegments();

#if (NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER)

        /// <summary>
        /// Get the contents of the buffer as ref struct with 2 read only spans (<see cref="SpanTuple&lt;T&gt;"/>).
        /// Respects the logical contents of the buffer, where
        /// each segment and items in each segment are ordered
        /// according to insertion.
        ///
        /// <remarks>Segments may be empty.</remarks>
        /// </summary>
        /// <returns>A <see cref="SpanTuple&lt;T&gt;"/> with 2 read only spans corresponding to the buffer content.</returns>
        SpanTuple<T> ToSpan();

        /// <summary>
        /// Get the contents of the buffer as tuple with 2 <see cref="ReadOnlyMemory&lt;T&gt;"/>.
        /// Respects the logical contents of the buffer, where
        /// each segment and items in each segment are ordered
        /// according to insertion.
        ///
        /// <remarks>Segments may be empty.</remarks>
        /// </summary>
        /// <returns>A tuple with 2 read only spans corresponding to the buffer content.</returns>
        (ReadOnlyMemory<T> A, ReadOnlyMemory<T> B) ToMemory();

#endif
    }
}