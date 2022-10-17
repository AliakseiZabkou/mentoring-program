/*
 * 5. Write a program which creates two threads and a shared collection:
 * the first one should add 10 elements into the collection and the second should print all elements
 * in the collection after each adding.
 * Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Task5.Threads.SharedCollection
{
    class Program
    {
        public static int count = 10;
        public static List<int> collection = new List<int>();

        static AutoResetEvent autoResetEventRead = new AutoResetEvent(true);
        static AutoResetEvent autoResetEventWrite = new AutoResetEvent(false);


        static void Main(string[] args)
        {
            Console.WriteLine("5. Write a program which creates two threads and a shared collection:");
            Console.WriteLine("the first one should add 10 elements into the collection and the second should print all elements in the collection after each adding.");
            Console.WriteLine("Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.");
            Console.WriteLine();

            // feel free to add your code

            Task.Run(() => WriteToCollection());
            Task.Run(() => ReadFromCollection());


            Console.ReadLine();
        }

        public static void WriteToCollection()
        {
            for (int i = 0; i < count; i++)
            {
                autoResetEventRead.WaitOne();
                int square = i * i;
                collection.Add(square);
                Console.WriteLine("New element added");
                autoResetEventWrite.Set();             
            }
        }

        public static void ReadFromCollection()
        {
            for (int i = 0; i < count; i++)
            {
                autoResetEventWrite.WaitOne();
                int value = collection.Last();
                Console.WriteLine("New element = {0}", value);
                autoResetEventRead.Set();            
            }
        }
    }
}
