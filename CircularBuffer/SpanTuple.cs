using System;

namespace CircularBuffer
{

#if (NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER)

    /// <summary>
    /// Reference struct that contains two read only spans of type T
    /// </summary>
    public ref struct SpanTuple<T>
    {
        /// <summary>
        /// First span
        /// </summary>
        public ReadOnlySpan<T> A { get; }

        /// <summary>
        /// Second span
        /// </summary>
        public ReadOnlySpan<T> B { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="a">First span</param>
        /// <param name="b">Second span</param>
        public SpanTuple(ReadOnlySpan<T> a, ReadOnlySpan<T> b)
        {
            A = a;
            B = b;
        }

        /// <summary>
        /// Deconstructs the current <see cref="SpanTuple&lt;T&gt;"/>
        /// </summary>
        /// <param name="a">First span target</param>
        /// <param name="b">Second span target</param>
        public void Deconstruct(out ReadOnlySpan<T> a, out ReadOnlySpan<T> b)
        {
            a = A;
            b = B;
        }
    }

#endif

}