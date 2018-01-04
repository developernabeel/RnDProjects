using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpConcepts.Overloading
{
    /*
        In C# the signature consists of the methods:
        - name
        - number of type parameters
        - number of formal parameters
        - type of each formal parameter
        - out/ref/value-ness of each formal parameter
      
        With the following additional notes:
        - generic type parameter constraints are not part of the signature
        - return type is not part of the signature
        - type parameter and formal parameter names are not part of the signature
        - two methods may not differ only in out/ref
     
        https://stackoverflow.com/questions/8808703/method-signature-in-c-sharp
     */

    public class MethodOverloading
    {
        public void Add(int a, int b) { }
        //public int Add(int a, int b) { return 1; }

        public void Add(int a, int b, int c) { }

        public void Add<T>(int a, int b) { }
        public void Add<T1, T2>(int a, int b) { }
        //public void Add<T>(int a, int b) where T : class { } 

        public void Add(ref int a, int b) { }
        
        // Two methods may not differ only in out/ref
        //public void Add(out int a, int b) { a = 1; }
    }
}
