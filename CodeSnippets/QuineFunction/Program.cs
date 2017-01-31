using System;
using System.Collections.Generic;
namespace QuineFunction
{
    class Program
    {
        void Quine()
        {
            char qoutes = (char)34;
            char comma = (char)44;
            string space = new string((char)32, 10);
            List<string> lines = new List<string>{                
                "class Program",
                "{",
                "  void Quine1()",
                "  {",
                "     char qoutes = (char)34;",
                "     char comma = (char)44;",
                "     string space = new string((char)32, 10);",
                "     List<string> lines = new List<string>{",
                "     };",
                "     for (int i = 0; i < 8; i++)",
                "     {",
                "        Console.WriteLine(lines[i]);",
                "     }",
                "     for (int i = 0; i < 23; i++)",
                "     {",
                "        Console.WriteLine(space + qoutes + lines[i] + qoutes + comma);",
                "     }",
                "     for (int i = 9; i < 23; i++)",
                "     {",
                "        Console.WriteLine(lines[i]);",
                "     }",
                "  }",
                "}",
            };

            for (int i = 0; i < 8; i++)
            {
                Console.WriteLine(lines[i]);
            }
            for (int i = 0; i < 23; i++)
            {
                Console.WriteLine(space + qoutes + lines[i] + qoutes + comma);
            }
            for (int i = 9; i < 23; i++)
            {
                Console.WriteLine(lines[i]);
            }
        }
        
        static void Main(string[] args)
        {
            new Program().Quine();
            Console.ReadLine();
        }
    }
}
