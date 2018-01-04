using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpConcepts.OutAndRef
{
    public class OutAndRef
    {
        public OutAndRef()
        {
            int i = 1;
            Console.WriteLine("i = " + i);
            Foo1(i);
            Console.WriteLine("i = " + i + "\n");

            int j = 1;
            Console.WriteLine("j = " + j);
            Foo2(ref j);
            Console.WriteLine("j = " + j + "\n");

            int k;
            Console.WriteLine("k = uninitialized");
            //k = 1;
            //Console.WriteLine("k = " + k);
            Foo3(out k);
            Console.WriteLine("k = " + k + "\n");
        }

        void Foo1(int i)
        {
            i = 2;
        }

        void Foo2(ref int j)
        {
            j = 2;
        }

        void Foo3(out int k)
        {
            k = 2;
        }
    }
}
