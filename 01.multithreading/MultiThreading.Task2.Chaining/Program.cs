/*
 * 2.	Write a program, which creates a chain of four Tasks.
 * First Task – creates an array of 10 random integer.
 * Second Task – multiplies this array with another random integer.
 * Third Task – sorts this array by ascending.
 * Fourth Task – calculates the average value. All this tasks should print the values to console.
 */
using System;
using System.Collections;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace MultiThreading.Task2.Chaining
{
    class Program
    {
        const int ArraySize = 10;

        static void Main(string[] args)
        {
            Console.WriteLine(".Net Mentoring Program. MultiThreading V1 ");
            Console.WriteLine("2.	Write a program, which creates a chain of four Tasks.");
            Console.WriteLine("First Task – creates an array of 10 random integer.");
            Console.WriteLine("Second Task – multiplies this array with another random integer.");
            Console.WriteLine("Third Task – sorts this array by ascending.");
            Console.WriteLine("Fourth Task – calculates the average value. All this tasks should print the values to console");
            Console.WriteLine();

            Task.Run(
                () =>
                {
                    // creating an array of 10 random integer
                    Random random = new Random();
                    int[] intArray = new int[ArraySize];
                    for (int i = 0; i < ArraySize; i++)
                        intArray[i] = random.Next(1, 100);

                    Console.WriteLine("Created array of random integers:");
                    PrintArray(intArray);
                    return intArray;
                })
                .ContinueWith(
                    antecedent =>
                    {
                        // multiplying this array with another random integer
                        int[] intArray = antecedent.Result;

                        Random random = new Random();
                        int mulptiplier = random.Next(1, 10);

                        for (int i = 0; i < ArraySize; i++)
                            intArray[i] *= mulptiplier;

                        Console.WriteLine("Array mulptiplied with {0}:", mulptiplier);
                        PrintArray(intArray);
                        return intArray;
                    })
                .ContinueWith(
                    antecedent =>
                    {
                        // sorting this array by ascending
                        int[] intArray = antecedent.Result;

                        Array.Sort(intArray);

                        Console.WriteLine("Sorted array:");
                        PrintArray(intArray);
                        return intArray;
                    })
                .ContinueWith(
                    antecedent =>
                    {
                        // calculating the average value

                        int[] intArray = antecedent.Result;

                        Console.WriteLine("The average value = {0}", intArray.Average());
                    });

            Console.ReadLine();
        }

        static void PrintArray(int[] array)
        {
            Console.WriteLine("[{0}]\n", string.Join(", ", array));
        }
    }
}
