using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SetTheory
{
    public class FindSubsetByRecursion2
    {
        public FindSubsetByRecursion2()
        {
            Console.WriteLine("Enter set numbers");
            string[] ln = Console.ReadLine().Split(' ');
            Console.WriteLine("Enter N");
            int n = Convert.ToInt32(Console.ReadLine());

            var set = new List<int>();
            for (int i = 0; i < ln.Length; i++)
                set.Add(Convert.ToInt32(ln[i]));

            List<bool> flags = new List<bool>();
            foreach (var s in set)
            {
                flags.Add(false);
            }

            GetSubsets(set, flags, 0, 0, n);

            Console.ReadKey();
        }

        void GetSubsets(List<int> set, List<bool> flags, int fIndex, int len, int n)
        {
            if (len == n)
            {
                for (int i = 0; i < set.Count; i++)
                {
                    if (flags[i])
                    {
                        Console.Write(set[i]);
                    }
                }
                Console.WriteLine();
                return;
            }

            if (fIndex >= set.Count)
                return;

            flags[fIndex] = true;
            GetSubsets(set, flags, fIndex + 1, len + 1, n);

            flags[fIndex] = false;
            GetSubsets(set, flags, fIndex + 1, len, n);
        }
    }
}
