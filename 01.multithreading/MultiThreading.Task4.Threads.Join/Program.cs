/*
 * 4.	Write a program which recursively creates 10 threads.
 * Each thread should be with the same body and receive a state with integer number, decrement it,
 * print and pass as a state into the newly created thread.
 * Use Thread class for this task and Join for waiting threads.
 * 
 * Implement all of the following options:
 * - a) Use Thread class for this task and Join for waiting threads.
 * - b) ThreadPool class for this task and Semaphore for waiting threads.
 */

using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Task4.Threads.Join
{
    class Program
    {
        public static int count = 10;
        private static SemaphoreSlim semaphore;
        static void Main(string[] args)
        {
            Console.WriteLine("4.	Write a program which recursively creates 10 threads.");
            Console.WriteLine("Each thread should be with the same body and receive a state with integer number, decrement it, print and pass as a state into the newly created thread.");
            Console.WriteLine("Implement all of the following options:");
            Console.WriteLine();
            Console.WriteLine("- a) Use Thread class for this task and Join for waiting threads.");
            Console.WriteLine("- b) ThreadPool class for this task and Semaphore for waiting threads.");

            Console.WriteLine();

            // a) Use Thread class for this task and Join for waiting threads.
            Console.WriteLine("Option A:");
            Thread t = new Thread(() => ThreadProc(20));
            t.Start();
            t.Join();

            // b) ThreadPool class for this task and Semaphore for waiting threads.
            
            count = 10;

            Console.WriteLine("\nOption B: {0}", count);
            semaphore = new SemaphoreSlim(3, 3);
            ThreadPool.QueueUserWorkItem(ThreadProcWithSemaphore, 30);

            Console.ReadLine();
        }

        private static void ThreadProc(int state)
        {
            if (count <= 0) return;

            Interlocked.Decrement(ref count);
          
            Thread t = new Thread(() => ThreadProc(state - 1));
            //Thread.Sleep(500);
            t.Start();
            t.Join();
            Console.WriteLine("ThreadId : {0}, state : {1}", Thread.CurrentThread.ManagedThreadId, state);
        }

        private static void ThreadProcWithSemaphore(Object state)
        {
            if (count <= 0) return;

            semaphore.Wait();

            try
            {
                Interlocked.Decrement(ref count);              
                Console.WriteLine("ThreadId : {0}, state : {1}", Thread.CurrentThread.ManagedThreadId, (int)state);
                ThreadPool.QueueUserWorkItem(ThreadProcWithSemaphore, (int)state - 1);
                Thread.Sleep(1000);
            }
            finally
            {
                semaphore.Release();
            }
        }


    }

}
