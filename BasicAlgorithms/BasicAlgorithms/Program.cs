using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicAlgorithms
{
    class Program
    {
        static void Main(string[] args)
        {
            RunSortingAlgorithms();
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        static void RunSortingAlgorithms()
        {
            Console.WriteLine("Enter array size");
            int n = Convert.ToInt32(Console.ReadLine());
            int[] array = new int[n];
            Console.WriteLine("Enter " + n + " space separated integers");
            string[] integers = Console.ReadLine().Split(' ');
            for (int i = 0; i < n; i++)
            {
                array[i] = Convert.ToInt32(integers[i]);
            }
            new SortingAlgorithms().InsertionSort(array);
            Console.WriteLine("Sorted array\n" + string.Join(" ", array));
        }
    }
}
