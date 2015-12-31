using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdjacentCellsInMatrix
{
    class Program
    {
        public Program()
        {
            int totalCells, pos, sideLen;
            int? tLeft, t, tRight, left, right, bLeft, b, bRight;
            bool isOnTop, isFarLeft, isFarRight, isOnBottom, quit;
            quit = false;

            while (quit == false)
            {
                isOnTop = isFarLeft = isFarRight = isOnBottom = false;
                tLeft = t = tRight = left = right = bLeft = b = bRight = null;

                Console.WriteLine("Enter no of cells in Matrix");
                totalCells = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Enter cell position");
                pos = Convert.ToInt32(Console.ReadLine());
                sideLen = (int)Math.Sqrt(totalCells);

                // Check Top Line
                if (pos < sideLen)
                {
                    // Cell is on top line, there will be no top adjacent cells.
                    isOnTop = true;
                }
                // Check Bottom Line
                else if (pos >= (totalCells - sideLen))
                {
                    // Cell is on bottom line, there will be no bottom adjacent cells.
                    isOnBottom = true;
                }

                // Check far left
                if ((sideLen + pos) % sideLen == 0)
                {
                    // Cell is on far left, there will be no left adjacent cells.
                    isFarLeft = true;
                }
                // Check far right
                else if ((sideLen + pos) % sideLen == (sideLen - 1))
                {
                    // Cell is on far right, there will be no right adjacent cells.
                    isFarRight = true;
                }

                // Calculate coordinates
                // Top Layer
                if (!isOnTop && !isFarLeft)
                    tLeft = pos - sideLen - 1;

                if (!isOnTop)
                    t = pos - sideLen;

                if (!isOnTop && !isFarRight)
                    tRight = pos - sideLen + 1;

                // Middle Layer
                if (!isFarLeft)
                    left = pos - 1;

                if (!isFarRight)
                    right = pos + 1;

                // Bottom Layer
                if (!isOnBottom && !isFarLeft)
                    bLeft = pos + sideLen - 1;

                if (!isOnBottom)
                    b = pos + sideLen;

                if (!isOnBottom && !isFarRight)
                    bRight = pos + sideLen + 1;

                //Console.WriteLine("Is on Top : " + isOnTop);
                //Console.WriteLine("Is on Bottom : " + isOnBottom);
                //Console.WriteLine("Is Far Left : " + isFarLeft);
                //Console.WriteLine("Is Far Right : " + isFarRight);
                Console.WriteLine(tLeft + "\t" + t + "\t" + tRight);
                Console.WriteLine(left + "\t" + pos + "\t" + right);
                Console.WriteLine(bLeft + "\t" + b + "\t" + bRight);

                Console.WriteLine("\n Continue? y/n");
                string con = Console.ReadLine();
                quit = con == "n";
            }
        }

        static void Main(string[] args)
        {
            new Program();
        }
    }
}
