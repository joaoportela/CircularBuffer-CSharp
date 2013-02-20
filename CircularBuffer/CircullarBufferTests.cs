using NUnit.Framework;
using System;

namespace CircularBuffer
{
	[TestFixture()]
	public class CircullarBufferTests
	{
		[Test()]
		public void CircularBuffer_IndexAccessAndConstructor_CorrectContent ()
		{
			var buffer = new CircularBuffer<int> (5, new int[] { 0, 1, 2, 3 });

			Assert.That (buffer.Capacity, Is.EqualTo (5));
			Assert.That (buffer.Size, Is.EqualTo (4));
			for (int i = 0; i < 4; i++) {
				Assert.That (buffer [i], Is.EqualTo (i));
			}
		}
				
		[Test()]
		public void CircularBuffer_Constructor_SkipsElementsWhenSourceIsLargerThanCapacity ()
		{
			var buffer = new CircularBuffer<int> (3, new int[] { 0 , 1, 2, 3 });

			Assert.That (buffer.Capacity, Is.EqualTo (3));
			Assert.That (buffer.Size, Is.EqualTo (3));

			Assert.That (buffer.Front(), Is.EqualTo (1));
			Assert.That (buffer.Back (), Is.EqualTo (3));
		}

		[Test()]
		public void CircularBuffer_PushBack_CorrectContent ()
		{
			var buffer = new CircularBuffer<int> (5);

			for (int i = 0; i < 5; i++) {
				buffer.PushBack (i);
			}

			for (int i = 0; i < 5; i++) {
				Assert.That (buffer [i], Is.EqualTo (i));
			}
		}

		[Test()]
		public void CircularBuffer_PushFront_CorrectContent ()
		{
			var buffer = new CircularBuffer<int> (5);
			
			for (int i = 0; i < 5; i++) {
				buffer.PushFront (i);
			}
			
			for (int i = 0; i < 5; i++) {
				Assert.That (buffer [i], Is.EqualTo (4 - i));
			}
		}
	}
}
