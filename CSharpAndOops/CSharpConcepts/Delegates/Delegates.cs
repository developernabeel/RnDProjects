using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpConcepts.Delegates
{

    public class Delegates
    {
        delegate int ComputeDelegate(int a, int b);

        public Delegates()
        {
            SimpleDelegate();
            MulticastDelegate();
        }

        void SimpleDelegate()
        {
            ComputeDelegate compute = new ComputeDelegate(Compute);
            Console.WriteLine("\nPassing method to delegate : " + compute(1, 2));

            compute = new ComputeDelegate(delegate(int a, int b) { return a + b; });
            Console.WriteLine("\nPassing anonymous method to delegate : " + compute(1, 2));

            compute = new ComputeDelegate((a, b) => a + b);
            Console.WriteLine("\nPassing Lambda expression to delegate : " + compute(1, 2));

            Console.WriteLine("\nPassing method as an argument to a delegate parameter : " + PassingDelegateAsParameter(Compute));

            Console.WriteLine("\nPassing anonymous method as an argument to a delegate parameter : " + PassingDelegateAsParameter(delegate(int a, int b) { return a + b; }));

            Console.WriteLine("\nPassing Lambda expression as an argument to a delegate parameter : " + PassingDelegateAsParameter((a, b) => a + b));
        }

        int Compute(int a, int b) { return a + b; }

        int PassingDelegateAsParameter(ComputeDelegate compute)
        {
            int a = 2, b = 4;
            return compute(a, b);
        }

        void MulticastDelegate()
        {
            ComputeDelegate compute = new ComputeDelegate(MulticastCompute);
            compute += MulticastComputeMultiply;
            compute += MulticastComputeDivide;
            Console.WriteLine("\n\nMulticast delegate:\n" + compute(4, 4));
        }

        int MulticastCompute(int a, int b) { return a + b; }
        int MulticastComputeMultiply(int x, int y) { return x * y; }
        int MulticastComputeDivide(int x, int y) { return x / y; }
    }
}
