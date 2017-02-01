using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpConcepts.ValueAndReferenceTypes
{
    class Foo
    {
        public int Id { get; set; }
        public Foo(int id)
        {
            Id = id;
        }
    }

    public class ValueAndReferenceTypes
    {
        public ValueAndReferenceTypes()
        {
            Foo foo = new Foo(1);
            Console.WriteLine(foo.Id);

            ChangeId1(foo);
            Console.WriteLine(foo.Id);

            ChangeId2(foo);
            Console.WriteLine(foo.Id);

            ChangeId3(ref foo);
            Console.WriteLine(foo.Id);
        }

        void ChangeId1(Foo foo)
        {
            foo.Id = 5;
        }

        void ChangeId2(Foo foo)
        {
            foo = new Foo(10);
        }

        void ChangeId3(ref Foo foo)
        {
            foo = new Foo(20);
        }
    }
}
