using System;
using CircularBuffer;

namespace CircularBufferExample
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            var buffer = new CircularBuffer<int>(5);

            for (int i = 0; i < 5; i++)
            {
                buffer.PushBack(i);
            }

            Console.WriteLine(buffer.Front()); // 0
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine(buffer[i]);
            }
        }
    }
}
