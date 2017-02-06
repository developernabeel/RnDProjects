using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SetTheory
{
    /// <summary>
    /// If the input is the set {1,2,3} then to generate all possible subsets we start by adding an empty set - {} to all possible subsets. 
    /// Now we add element 1 to this empty set to create set {1} and we add this set {1} to all possible subsets. All possible subsets at 
    /// this point has set {} and set {1} in it. Now we add element 2 to all sets in all possible subsets to create new sets {2}, {1,2}. 
    /// We add these newly created sets to all possible subsets. We continue this process till all elements from given set are considered 
    /// for generating all possible subsets. Refer to following link for explaination.
    /// https://www.quora.com/What-is-the-recursive-solution-for-finding-all-subsets-of-a-given-array
    /// </summary>
    public class FindSubsetByRecursion
    {
        public FindSubsetByRecursion()
        {
            Console.WriteLine("Enter set numbers");
            string[] ln = Console.ReadLine().Split(' ');
            var set = new List<int>();
            for (int i = 0; i < ln.Length; i++)
                set.Add(Convert.ToInt32(ln[i]));

            var subsets = GetSubsets(set);
            foreach (var subset in subsets)
            {
                Console.WriteLine("{" + string.Join(",", subset) + "}");
            }

            Console.ReadKey();
        }

        List<List<int>> GetSubsets(List<int> set)
        {
            if (set.Count == 0)
                return new List<List<int>>() { new List<int>() };

            int h = set[0];
            List<int> t = set.GetRange(1, set.Count - 1);

            List<List<int>> subsets1 = GetSubsets(t);
            List<List<int>> subsets2 = new List<List<int>>();

            foreach (var subset in subsets1)
            {
                var newSubset = new List<int>(subset);
                newSubset.Add(h);
                subsets2.Add(newSubset);
            }
            subsets1.AddRange(subsets2);
            return subsets1;
        }
    }
}
