using NUnit.Framework;
using System;
using System.Linq;

namespace CircularBuffer.Tests
{
    [TestFixture]
    public class CircularBufferTests
    {
        [Test]
        public void CircularBuffer_GetEnumeratorConstructorCapacity_ReturnsEmptyCollection()
        {
            var buffer = new CircularBuffer<string>(5);
            CollectionAssert.IsEmpty(buffer.ToArray());
        }

        [Test]
        public void CircularBuffer_ConstructorSizeIndexAccess_CorrectContent()
        {
            var buffer = new CircularBuffer<int>(5, new[] { 0, 1, 2, 3 });

            Assert.That(buffer.Capacity, Is.EqualTo(5));
            Assert.That(buffer.Count, Is.EqualTo(4));
            for (int i = 0; i < 4; i++)
            {
                Assert.That(buffer[i], Is.EqualTo(i));
            }
        }

        [Test]
        public void CircularBuffer_Constructor_ExceptionWhenSourceIsLargerThanCapacity()
        {
            Assert.That(() => new CircularBuffer<int>(3, new[] { 0, 1, 2, 3 }),
                        Throws.Exception.TypeOf<ArgumentException>());
        }

        [Test]
        public void CircularBuffer_GetEnumeratorConstructorDefinedArray_CorrectContent()
        {
            var buffer = new CircularBuffer<int>(5, new[] { 0, 1, 2, 3 });

            int x = 0;
            foreach (var item in buffer)
            {
                Assert.That(item, Is.EqualTo(x));
                x++;
            }
        }

        [Test]
        public void CircularBuffer_PushBack_CorrectContent()
        {
            var buffer = new CircularBuffer<int>(5);

            for (int i = 0; i < 5; i++)
            {
                buffer.PushBack(i);
            }

            Assert.That(buffer.First(), Is.EqualTo(0));
            for (int i = 0; i < 5; i++)
            {
                Assert.That(buffer[i], Is.EqualTo(i));
            }
        }

        [Test]
        public void CircularBuffer_PushBackOverflowingBuffer_CorrectContent()
        {
            var buffer = new CircularBuffer<int>(5);

            for (int i = 0; i < 10; i++)
            {
                buffer.PushBack(i);
            }

            Assert.That(buffer.ToArray(), Is.EqualTo(new[] { 5, 6, 7, 8, 9 }));
        }

        [Test]
        public void CircularBuffer_GetEnumeratorOverflowedArray_CorrectContent()
        {
            var buffer = new CircularBuffer<int>(5);

            for (int i = 0; i < 10; i++)
            {
                buffer.PushBack(i);
            }

            // buffer should have [5,6,7,8,9]
            int x = 5;
            foreach (var item in buffer)
            {
                Assert.That(item, Is.EqualTo(x));
                x++;
            }
        }

        [Test]
        public void CircularBuffer_ToArrayConstructorDefinedArray_CorrectContent()
        {
            var buffer = new CircularBuffer<int>(5, new[] { 0, 1, 2, 3 });

            Assert.That(buffer.ToArray(), Is.EqualTo(new[] { 0, 1, 2, 3 }));
        }

        [Test]
        public void CircularBuffer_ToArrayOverflowedBuffer_CorrectContent()
        {
            var buffer = new CircularBuffer<int>(5);

            for (int i = 0; i < 10; i++)
            {
                buffer.PushBack(i);
            }

            Assert.That(buffer.ToArray(), Is.EqualTo(new[] { 5, 6, 7, 8, 9 }));
        }

        [Test]
        public void CircularBuffer_CopyToConstructorDefinedArray_Exceptions()
        {
            var buffer = new CircularBuffer<int>(5, new[] { 0, 1, 2, 3, 4 });

            Assert.Throws<ArgumentNullException>(() => buffer.CopyTo(null));

            var array = new int[5];

            Assert.Throws<ArgumentOutOfRangeException>(() => buffer.CopyTo(array, -1));
            Assert.Throws<ArgumentOutOfRangeException>(() => buffer.CopyTo(array, -1L));

            Assert.Throws<ArgumentException>(() => buffer.CopyTo(array, 1));
            Assert.Throws<ArgumentException>(() => buffer.CopyTo(array, 1L));

            array = new int[4];

            Assert.Throws<ArgumentException>(() => buffer.CopyTo(array));

#if (NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER)

            array = new int[5];
            Assert.Throws<ArgumentException>(() => buffer.CopyTo(array.AsMemory(1)));
            Assert.Throws<ArgumentException>(() => buffer.CopyTo(array.AsSpan(1)));

#endif
        }

        [Test]
        public void CircularBuffer_CopyToConstructorDefinedArray_CorrectContent()
        {
            var buffer = new CircularBuffer<int>(5, new[] { 0, 1, 2, 3 });

            var array = new int[4];
            buffer.CopyTo(array);
            Assert.That(array, Is.EqualTo(new[] { 0, 1, 2, 3 }));

            array = new int[5];
            buffer.CopyTo(array, 1);
            Assert.That(array, Is.EqualTo(new[] { 0, 0, 1, 2, 3 }));

            array = new int[5];
            buffer.CopyTo(array, 1L);
            Assert.That(array, Is.EqualTo(new[] { 0, 0, 1, 2, 3 }));

#if (NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER)

            array = new int[5];
            buffer.CopyTo(array.AsMemory(1));
            Assert.That(array, Is.EqualTo(new[] { 0, 0, 1, 2, 3 }));

            array = new int[5];
            buffer.CopyTo(array.AsSpan(1));
            Assert.That(array, Is.EqualTo(new[] { 0, 0, 1, 2, 3 }));

#endif
        }

        [Test]
        public void CircularBuffer_CopyToOverflowedBuffer_CorrectContent()
        {
            var buffer = new CircularBuffer<int>(5);

            for (int i = 0; i < 10; i++)
            {
                buffer.PushBack(i);
            }

            var array = new int[5];
            buffer.CopyTo(array);
            Assert.That(array, Is.EqualTo(new[] { 5, 6, 7, 8, 9 }));

            array = new int[6];
            buffer.CopyTo(array, 1);
            Assert.That(array, Is.EqualTo(new[] { 0, 5, 6, 7, 8, 9 }));

            array = new int[6];
            buffer.CopyTo(array, 1L);
            Assert.That(array, Is.EqualTo(new[] { 0, 5, 6, 7, 8, 9 }));

#if (NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER)

            array = new int[6];
            buffer.CopyTo(array.AsMemory(1));
            Assert.That(array, Is.EqualTo(new[] { 0, 5, 6, 7, 8, 9 }));

            array = new int[6];
            buffer.CopyTo(array.AsSpan(1));
            Assert.That(array, Is.EqualTo(new[] { 0, 5, 6, 7, 8, 9 }));

#endif
        }

        [Test]
        public void CircularBuffer_ToArraySegmentsConstructorDefinedArray_CorrectContent()
        {
            var buffer = new CircularBuffer<int>(5, new[] { 0, 1, 2, 3 });

            var arraySegments = buffer.ToArraySegments();

            Assert.That(arraySegments.Count, Is.EqualTo(2)); // length of 2 is part of the contract.
            Assert.That(arraySegments.SelectMany(x => x), Is.EqualTo(new[] { 0, 1, 2, 3 }));
        }

        [Test]
        public void CircularBuffer_ToArraySegmentsOverflowedBuffer_CorrectContent()
        {
            var buffer = new CircularBuffer<int>(5);

            for (int i = 0; i < 10; i++)
            {
                buffer.PushBack(i);
            }

            var arraySegments = buffer.ToArraySegments();
            Assert.That(arraySegments.Count, Is.EqualTo(2)); // length of 2 is part of the contract.
            Assert.That(arraySegments.SelectMany(x => x), Is.EqualTo(new[] { 5, 6, 7, 8, 9 }));
        }

#if (NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER)

        [Test]
        public void CircularBuffer_ToMemoryConstructorDefinedArray_CorrectContent()
        {
            var buffer = new CircularBuffer<int>(5, new[] { 0, 1, 2, 3 });

            var (memoryA, memoryB) = buffer.ToMemory();

            var array = new int[4];
            memoryA.CopyTo(array.AsMemory());
            memoryB.CopyTo(array.AsMemory(memoryA.Length));

            Assert.That(array, Is.EqualTo(new[] { 0, 1, 2, 3 }));
        }

        [Test]
        public void CircularBuffer_ToMemoryOverflowedBuffer_CorrectContent()
        {
            var buffer = new CircularBuffer<int>(5);

            for (int i = 0; i < 10; i++)
            {
                buffer.PushBack(i);
            }

            var (memoryA, memoryB) = buffer.ToMemory();

            var array = new int[5];
            memoryA.CopyTo(array.AsMemory());
            memoryB.CopyTo(array.AsMemory(memoryA.Length));

            Assert.That(array, Is.EqualTo(new[] { 5, 6, 7, 8, 9 }));
        }

        [Test]
        public void CircularBuffer_ToSpanConstructorDefinedArray_CorrectContent()
        {
            var buffer = new CircularBuffer<int>(5, new[] { 0, 1, 2, 3 });

            var (spanA, spanB) = buffer.ToSpan();

            var array = new int[4];
            spanA.CopyTo(array.AsSpan());
            spanB.CopyTo(array.AsSpan(spanA.Length));

            Assert.That(array, Is.EqualTo(new[] { 0, 1, 2, 3 }));
        }

        [Test]
        public void CircularBuffer_ToSpanOverflowedBuffer_CorrectContent()
        {
            var buffer = new CircularBuffer<int>(5);

            for (int i = 0; i < 10; i++)
            {
                buffer.PushBack(i);
            }

            var (spanA, spanB) = buffer.ToSpan();

            var array = new int[5];
            spanA.CopyTo(array.AsSpan());
            spanB.CopyTo(array.AsSpan(spanA.Length));

            Assert.That(array, Is.EqualTo(new[] { 5, 6, 7, 8, 9 }));
        }

#endif

        [Test]
        public void CircularBuffer_PushFront_CorrectContent()
        {
            var buffer = new CircularBuffer<int>(5);

            for (int i = 0; i < 5; i++)
            {
                buffer.PushFront(i);
            }

            Assert.That(buffer.ToArray(), Is.EqualTo(new[] { 4, 3, 2, 1, 0 }));
        }

        [Test]
        public void CircularBuffer_PushFrontAndOverflow_CorrectContent()
        {
            var buffer = new CircularBuffer<int>(5);

            for (int i = 0; i < 10; i++)
            {
                buffer.PushFront(i);
            }

            Assert.That(buffer.ToArray(), Is.EqualTo(new[] { 9, 8, 7, 6, 5 }));
        }

        [Test]
        public void CircularBuffer_Front_CorrectItem()
        {
            var buffer = new CircularBuffer<int>(5, new[] { 0, 1, 2, 3, 4 });

            Assert.That(buffer.First(), Is.EqualTo(0));
        }

        [Test]
        public void CircularBuffer_Back_CorrectItem()
        {
            var buffer = new CircularBuffer<int>(5, new[] { 0, 1, 2, 3, 4 });
            Assert.That(buffer.Last(), Is.EqualTo(4));
        }

        [Test]
        public void CircularBuffer_BackOfBufferOverflowByOne_CorrectItem()
        {
            var buffer = new CircularBuffer<int>(5, new[] { 0, 1, 2, 3, 4 });
            buffer.PushBack(42);
            Assert.That(buffer.ToArray(), Is.EqualTo(new[] { 1, 2, 3, 4, 42 }));
            Assert.That(buffer.Last(), Is.EqualTo(42));
        }

        [Test]
        public void CircularBuffer_Front_EmptyBufferThrowsException()
        {
            var buffer = new CircularBuffer<int>(5);

            Assert.That(() => buffer.First(),
                         Throws.Exception.TypeOf<InvalidOperationException>().
                         With.Property("Message").Contains("empty buffer"));
        }

        [Test]
        public void CircularBuffer_Back_EmptyBufferThrowsException()
        {
            var buffer = new CircularBuffer<int>(5);
            Assert.That(() => buffer.Last(),
                         Throws.Exception.TypeOf<InvalidOperationException>().
                         With.Property("Message").Contains("empty buffer"));
        }

        [Test]
        public void CircularBuffer_PopBack_RemovesBackElement()
        {
            var buffer = new CircularBuffer<int>(5, new[] { 0, 1, 2, 3, 4 });

            Assert.That(buffer.Count, Is.EqualTo(5));

            buffer.PopBack();

            Assert.That(buffer.Count, Is.EqualTo(4));
            Assert.That(buffer.ToArray(), Is.EqualTo(new[] { 0, 1, 2, 3 }));
        }

        [Test]
        public void CircularBuffer_PopBackInOverflowBuffer_RemovesBackElement()
        {
            var buffer = new CircularBuffer<int>(5, new[] { 0, 1, 2, 3, 4 });
            buffer.PushBack(5);

            Assert.That(buffer.Count, Is.EqualTo(5));
            Assert.That(buffer.ToArray(), Is.EqualTo(new[] { 1, 2, 3, 4, 5 }));

            buffer.PopBack();

            Assert.That(buffer.Count, Is.EqualTo(4));
            Assert.That(buffer.ToArray(), Is.EqualTo(new[] { 1, 2, 3, 4 }));
        }

        [Test]
        public void CircularBuffer_PopFront_RemovesBackElement()
        {
            var buffer = new CircularBuffer<int>(5, new[] { 0, 1, 2, 3, 4 });

            Assert.That(buffer.Count, Is.EqualTo(5));

            buffer.PopFront();

            Assert.That(buffer.Count, Is.EqualTo(4));
            Assert.That(buffer.ToArray(), Is.EqualTo(new[] { 1, 2, 3, 4 }));
        }

        [Test]
        public void CircularBuffer_PopFrontInOverflowBuffer_RemovesBackElement()
        {
            var buffer = new CircularBuffer<int>(5, new[] { 0, 1, 2, 3, 4 });
            buffer.PushFront(5);

            Assert.That(buffer.Count, Is.EqualTo(5));
            Assert.That(buffer.ToArray(), Is.EqualTo(new[] { 5, 0, 1, 2, 3 }));

            buffer.PopFront();

            Assert.That(buffer.Count, Is.EqualTo(4));
            Assert.That(buffer.ToArray(), Is.EqualTo(new[] { 0, 1, 2, 3 }));
        }

        [Test]
        public void CircularBuffer_SetIndex_ReplacesElement()
        {
            var buffer = new CircularBuffer<int>(5, new[] { 0, 1, 2, 3, 4 });

            buffer[1] = 10;
            buffer[3] = 30;

            Assert.That(buffer.ToArray(), Is.EqualTo(new[] { 0, 10, 2, 30, 4 }));
        }

        [Test]
        public void CircularBuffer_WithDifferentSizeAndCapacity_BackReturnsLastArrayPosition()
        {
            // test to confirm this issue does not happen anymore:
            // https://github.com/joaoportela/CircularBuffer-CSharp/issues/2

            var buffer = new CircularBuffer<int>(5, new[] { 0, 1, 2, 3, 4 });

            buffer.PopFront(); // (make size and capacity different)

            Assert.That(buffer.Last(), Is.EqualTo(4));
        }

        [Test]
        public void CircularBuffer_Clear_ClearsContent()
        {
            var buffer = new CircularBuffer<int>(5, new[] { 4, 3, 2, 1, 0 });

            buffer.Clear();

            Assert.That(buffer.Count, Is.EqualTo(0));
            Assert.That(buffer.Capacity, Is.EqualTo(5));
            Assert.That(buffer.ToArray(), Is.EqualTo(new int[0]));
        }

        [Test]
        public void CircularBuffer_Clear_WorksNormallyAfterClear()
        {
            var buffer = new CircularBuffer<int>(5, new[] { 4, 3, 2, 1, 0 });

            buffer.Clear();
            for (int i = 0; i < 5; i++)
            {
                buffer.PushBack(i);
            }

            Assert.That(buffer.First(), Is.EqualTo(0));
            for (int i = 0; i < 5; i++)
            {
                Assert.That(buffer[i], Is.EqualTo(i));
            }
        }
    }
}
