using NUnit.Framework;
using System;
using System.Collections;

namespace CircularBuffer
{
    [TestFixture()]
    public class CircularBufferTests
    {
        [Test()]
        public void CircularBuffer_ConstructurSizeIndexAccess_CorrectContent()
        {
            var buffer = new CircularBuffer<int>(5, new[] { 0, 1, 2, 3 });

            Assert.That(buffer.Capacity, Is.EqualTo(5));
            Assert.That(buffer.Size, Is.EqualTo(4));
            for (int i = 0; i < 4; i++)
            {
                Assert.That(buffer[i], Is.EqualTo(i));
            }
        }

        [Test()]
        public void CircularBuffer_Constructor_ExceptionWhenSourceIsLargerThanCapacity()
        {
            Assert.That(() => new CircularBuffer<int>(3, new[] { 0, 1, 2, 3 }),
                        Throws.Exception.TypeOf<ArgumentException>());
        }

        [Test()]
        public void CircularBuffer_GetEnumeratorConstructorDefinedArray_Correctcontent()
        {
            var buffer = new CircularBuffer<int>(5, new[] { 0, 1, 2, 3 });

            int x = 0;
            foreach (var item in buffer)
            {
                Assert.That(item, Is.EqualTo(x));
                x++;
            }
        }

        [Test()]
        public void CircularBuffer_PushBack_CorrectContent()
        {
            var buffer = new CircularBuffer<int>(5);

            for (int i = 0; i < 5; i++)
            {
                buffer.PushBack(i);
            }

            Assert.That(buffer.Front(), Is.EqualTo(0));
            for (int i = 0; i < 5; i++)
            {
                Assert.That(buffer[i], Is.EqualTo(i));
            }
        }

        [Test()]
        public void CircularBuffer_PushBackOverflowingBuffer_CorrectContent()
        {
            var buffer = new CircularBuffer<int>(5);

            for (int i = 0; i < 10; i++)
            {
                buffer.PushBack(i);
            }

            Assert.That(buffer.ToArray(), Is.EqualTo(new[] { 5, 6, 7, 8, 9 }));
        }

        [Test()]
        public void CircularBuffer_GetEnumeratorOverflowedArray_Correctcontent()
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

        [Test()]
        public void CircularBuffer_ToArrayConstructorDefinedArray_Correctcontent()
        {
            var buffer = new CircularBuffer<int>(5, new[] { 0, 1, 2, 3 });

            Assert.That(buffer.ToArray(), Is.EqualTo(new[] { 0, 1, 2, 3 }));
        }

        [Test()]
        public void CircularBuffer_ToArrayOverflowedBuffer_Correctcontent()
        {
            var buffer = new CircularBuffer<int>(5);

            for (int i = 0; i < 10; i++)
            {
                buffer.PushBack(i);
            }

            Assert.That(buffer.ToArray(), Is.EqualTo(new[] { 5, 6, 7, 8, 9 }));
        }

        [Test()]
        public void CircularBuffer_PushFront_CorrectContent()
        {
            var buffer = new CircularBuffer<int>(5);

            for (int i = 0; i < 5; i++)
            {
                buffer.PushFront(i);
            }

            Assert.That(buffer.ToArray(), Is.EqualTo(new[] { 4, 3, 2, 1, 0 }));
        }

        [Test()]
        public void CircularBuffer_PushFrontAndOverflow_CorrectContent()
        {
            var buffer = new CircularBuffer<int>(5);

            for (int i = 0; i < 10; i++)
            {
                buffer.PushFront(i);
            }

            Assert.That(buffer.ToArray(), Is.EqualTo(new[] { 9, 8, 7, 6, 5 }));
        }

        [Test()]
        public void CircularBuffer_Front_CorrectItem()
        {
            var buffer = new CircularBuffer<int>(5, new[] { 0, 1, 2, 3, 4 });

            Assert.That(buffer.Front(), Is.EqualTo(0));
        }

        [Test()]
        public void CircularBuffer_Back_CorrectItem()
        {
            var buffer = new CircularBuffer<int>(5, new[] { 0, 1, 2, 3, 4 });
            Assert.That(buffer.Back(), Is.EqualTo(4));
        }

        [Test()]
        public void CircularBuffer_BackOfBufferOverflowByOne_CorrectItem()
        {
            var buffer = new CircularBuffer<int>(5, new[] { 0, 1, 2, 3, 4 });
            buffer.PushBack(42);
            Assert.That(buffer.ToArray(), Is.EqualTo(new[] { 1, 2, 3, 4, 42 }));
            Assert.That(buffer.Back(), Is.EqualTo(42));
        }

        [Test()]
        public void CircularBuffer_Front_EmptyBufferThrowsException()
        {
            var buffer = new CircularBuffer<int>(5);

            Assert.That(() => buffer.Front(),
                         Throws.Exception.TypeOf<InvalidOperationException>().
                         With.Property("Message").ContainsSubstring("empty buffer"));
        }

        [Test()]
        public void CircularBuffer_Back_EmptyBufferThrowsException()
        {
            var buffer = new CircularBuffer<int>(5);
            Assert.That(() => buffer.Back(),
                         Throws.Exception.TypeOf<InvalidOperationException>().
                         With.Property("Message").ContainsSubstring("empty buffer"));
        }

        [Test()]
        public void CircularBuffer_PopBack_RemovesBackElement()
        {
            var buffer = new CircularBuffer<int>(5, new[] { 0, 1, 2, 3, 4 });

            Assert.That(buffer.Size, Is.EqualTo(5));

            buffer.PopBack();

            Assert.That(buffer.Size, Is.EqualTo(4));
            Assert.That(buffer.ToArray(), Is.EqualTo(new[] { 0, 1, 2, 3 }));
        }

        [Test()]
        public void CircularBuffer_PopBackInOverflowBuffer_RemovesBackElement()
        {
            var buffer = new CircularBuffer<int>(5, new[] { 0, 1, 2, 3, 4 });
            buffer.PushBack(5);

            Assert.That(buffer.Size, Is.EqualTo(5));
            Assert.That(buffer.ToArray(), Is.EqualTo(new[] { 1, 2, 3, 4, 5 }));

            buffer.PopBack();

            Assert.That(buffer.Size, Is.EqualTo(4));
            Assert.That(buffer.ToArray(), Is.EqualTo(new[] { 1, 2, 3, 4 }));
        }

        [Test()]
        public void CircularBuffer_PopFront_RemovesBackElement()
        {
            var buffer = new CircularBuffer<int>(5, new[] { 0, 1, 2, 3, 4 });

            Assert.That(buffer.Size, Is.EqualTo(5));

            buffer.PopFront();

            Assert.That(buffer.Size, Is.EqualTo(4));
            Assert.That(buffer.ToArray(), Is.EqualTo(new[] { 1, 2, 3, 4 }));
        }

        [Test()]
        public void CircularBuffer_PopFrontInOverflowBuffer_RemovesBackElement()
        {
            var buffer = new CircularBuffer<int>(5, new[] { 0, 1, 2, 3, 4 });
            buffer.PushFront(5);

            Assert.That(buffer.Size, Is.EqualTo(5));
            Assert.That(buffer.ToArray(), Is.EqualTo(new[] { 5, 0, 1, 2, 3 }));

            buffer.PopFront();

            Assert.That(buffer.Size, Is.EqualTo(4));
            Assert.That(buffer.ToArray(), Is.EqualTo(new[] { 0, 1, 2, 3 }));
        }

        [Test()]
        public void CircularBuffer_SetIndex_ReplacesElement()
        {
            var buffer = new CircularBuffer<int>(5, new[] { 0, 1, 2, 3, 4 });

            buffer[1] = 10;
            buffer[3] = 30;

            Assert.That(buffer.ToArray(), Is.EqualTo(new[] { 0, 10, 2, 30, 4 }));
        }
    }
}
