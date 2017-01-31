using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpConcepts.OrderOfExecution
{
    class A
    {
        static int i = 1;
        private int j = 2;

        static A()
        {

        }

        public A()
        {

        }
    }

    class B : A
    {
        static int i = 1;
        private int j = 2;

        static B()
        {

        }

        public B()
        {

        }
    }

    public class OrderOfExecution
    {
        public OrderOfExecution()
        {
            new B();
        }
    }
}
