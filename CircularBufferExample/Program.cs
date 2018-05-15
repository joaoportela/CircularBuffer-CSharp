using System;
using CircularBuffer;

namespace CircularBufferExample
{
    static class MainClass
    {
        public static void Main(string[] args)
        {
            var buffer = new CircularBuffer<int>(5);

            for (int i = 0; i < 5; i++)
            {
                buffer.PushBack(i);
            }

            for (int i = 0; i < buffer.Size; i++)
            {
                Console.WriteLine(buffer[i]);
            }

            Console.WriteLine("---");

            for (int i = 0; i < 3; i++)
            {
                buffer.PushBack(10 + i);
            }
            
            for (int i = 0; i < buffer.Size; i++)
            {
                Console.WriteLine(buffer[i]);
            }
        }
    }
}
