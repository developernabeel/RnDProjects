using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SetTheory
{
    public class FindSubsetByBitmask
    {
        public FindSubsetByBitmask()
        {
            Console.WriteLine("Enter set numbers");
            string[] ln = Console.ReadLine().Split(' ');
            int[] set = new int[ln.Length];
            int index = 0;
            for (int i = 0; i < ln.Length; i++)
                set[index++] = Convert.ToInt32(ln[i]);

            var bitmasks = GetBitmasks(ln.Length);
            foreach (var bitmask in bitmasks)
            {
                var subsets = new List<string>();
                for (int i = 0; i < bitmask.Length; i++)
                {
                    if (bitmask[i] == 1)
                        subsets.Add(set[i].ToString());
                }
                Console.WriteLine("{" + string.Join(",", subsets) + "}");
            }
            Console.ReadKey();
        }

        List<int[]> GetBitmasks(int n)
        {
            List<int[]> bitmasks = new List<int[]>();
            int bit = 0;
            long power = (long)Math.Pow(2, n);
            for (int i = 0; i < power; i++)
            {
                bitmasks.Add(DecimalToBinary(bit, n));
                bit++;
            }
            return bitmasks;
        }

        int[] DecimalToBinary(int num, int size)
        {
            int[] binary = new int[size];
            int index = 0;
            while (num != 0)
            {
                binary[index++] = num % 2;
                num /= 2;
            }
            return binary.Reverse().ToArray();
        }
    }
}
