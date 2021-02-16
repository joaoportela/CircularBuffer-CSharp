using CircularBuffer;
using System;

namespace CircularBufferExample
{
    class Program
    {
        static void Main(string[] args)
        {
            CircularBuffer<int> buffer = new CircularBuffer<int>(5, new[] {0, 1, 2});
            Console.WriteLine("Initial buffer {0,1,2}:");
            PrintBuffer(buffer);


            for (int i = 3; i < 7; i++)
            {
                buffer.PushBack(i);
            }
            Console.WriteLine("\nAfter adding a 7 elements to a 5 elements capacity buffer:");
            PrintBuffer(buffer);


            buffer.PopFront();
            Console.WriteLine("\nbuffer.PopFront():");
            PrintBuffer(buffer);


            buffer.PopBack();
            Console.WriteLine("\nbuffer.PopBack():");
            PrintBuffer(buffer);

            for (int i = 2; i >= 0; i--)
            {
                buffer.PushFront(i);
            }
            Console.WriteLine("\nbuffer.PushFront() {2,1,0} respectively:");
            PrintBuffer(buffer);
        }

        private static void PrintBuffer(CircularBuffer<int> buffer)
        {
            for (int i = 0; i < buffer.Size; i++)
            {
                Console.WriteLine($"buffer[{i}] = {buffer[i]}");
            }
        }
    }
}
