using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpConcepts.Dynamic
{
    public class Dynamic
    {
        public Dynamic()
        {
            Console.WriteLine("Dynamic assignment :");
            dynamic dynamicAssignment = 1;
            dynamicAssignment = "Type Changed";
            dynamicAssignment = DateTime.Now;
            DynamicParameter(dynamicAssignment);

            dynamic foobar = new Foobar();
            Console.WriteLine(foobar.foo);
            foobar.DoFoo();
            // Compile error
            //foobar.DoBar();

            Console.WriteLine("\nDynamic Parameter :");
            DynamicParameter(2);
            DynamicParameter("Hello");

            Console.WriteLine("\nDynamic Return :");
            Console.WriteLine(DynamicReturn(1));
            Console.WriteLine(DynamicReturn(2));
            Console.WriteLine(DynamicReturn(3));

            ExpandoObject();
        }

        void DynamicParameter(dynamic a)
        {
            Console.WriteLine(a);
        }

        dynamic DynamicReturn(int type)
        {
            if (type == 1)
                return 1;
            if (type == 2)
                return 'a';
            return 1.2;
        }

        void ExpandoObject()
        {
            dynamic person = new ExpandoObject();
            person.name = "foo";
            person.age = 30;
            person.walk = new Func<string>(() => "");
            person.run = new Action<string>((s) => { });
            person.drive = new Predicate<string>((s) => true);
        }

        void Do() { }
    }

    public class Foobar
    {
        public int foo = 1;

        public void DoFoo()
        {

        }
    }
}
