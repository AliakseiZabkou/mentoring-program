/*
*  Create a Task and attach continuations to it according to the following criteria:
   a.    Continuation task should be executed regardless of the result of the parent task.
   b.    Continuation task should be executed when the parent task finished without success.
   c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation
   d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled
   Demonstrate the work of the each case with console utility.
*/
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Task6.Continuation
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Create a Task and attach continuations to it according to the following criteria:");
            Console.WriteLine("a.    Continuation task should be executed regardless of the result of the parent task.");
            Console.WriteLine("b.    Continuation task should be executed when the parent task finished without success.");
            Console.WriteLine("c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation.");
            Console.WriteLine("d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled.");
            Console.WriteLine("Demonstrate the work of the each case with console utility.");
            Console.WriteLine();

            // a.    Continuation task should be executed regardless of the result of the parent task.

            Task.Run(
                () =>
                {
                    return true;
                    
                })
                .ContinueWith(
                    antecedent =>
                    {
                        Console.WriteLine("I am continuation task A");
                        Console.WriteLine();
                    });

            Thread.Sleep(100);

            // b.    Continuation task should be executed when the parent task finished without success.

            Task.Run(
                () =>
                {
                    throw new Exception("some parent exception");
                })
                .ContinueWith(
                    antecedent =>
                    {
                        Console.WriteLine("I am continuation task B");
                        Console.WriteLine();
                    }, TaskContinuationOptions.OnlyOnFaulted);

            Thread.Sleep(100);

            // c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation.

            Task.Run(
                () =>
                {
                    Console.WriteLine("I am parent task C running on thread: {0}", Thread.CurrentThread.ManagedThreadId);               
                    throw new Exception("some parent exception");
                })
                .ContinueWith(
                    antecedent =>
                    {
                        Console.WriteLine("I am continuation task C running on thread: {0}", Thread.CurrentThread.ManagedThreadId);
                        Console.WriteLine();
                    }, TaskContinuationOptions.ExecuteSynchronously);

            Thread.Sleep(100);

            // d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled.
            CancellationTokenSource cts = new CancellationTokenSource();
            CancellationToken token = cts.Token;

            Task parentTask = new Task(() =>
            {
                
                Console.WriteLine("I am parent task D running on thread: id = {0}, IsThreadPoolThread = {1}", Thread.CurrentThread.ManagedThreadId, Thread.CurrentThread.IsThreadPoolThread);
                while (!token.IsCancellationRequested)
                {
                    Thread.Sleep(100);
                }
            });
            parentTask.ContinueWith(
                    antecedent =>
                    {
                        Console.WriteLine("I am continuation task D running on thread: id = {0}, IsThreadPoolThread = {1}", Thread.CurrentThread.ManagedThreadId, Thread.CurrentThread.IsThreadPoolThread);
                        Console.WriteLine();
                    }, TaskContinuationOptions.LongRunning);

           
            try
            {
                parentTask.Start();
                Thread.Sleep(1000);
                cts.Cancel();
                parentTask.Wait();
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine($"\n{nameof(OperationCanceledException)} thrown\n");
            }
            finally
            {
                cts.Dispose();
            }

            Console.ReadLine();
        }
    }
}
