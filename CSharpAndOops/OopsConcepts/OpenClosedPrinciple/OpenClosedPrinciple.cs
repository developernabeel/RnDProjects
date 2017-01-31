using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OopsConcepts.OpenClosedPrinciple
{
    /// <summary>
    /// Shape class is closed for modifications but open for extension as the Area method is implemented by the child classes below. Similarly we can add as many classes as extension
    /// as we want and the CalculateArea method will run smoothly. Which Area method to call will be decided at runtime.
    /// </summary>
    class Shape
    {
        public virtual double Area()
        {
            throw new NotImplementedException();
        }

        public void Foo() { }
    }

    class Rectangle : Shape
    {
        public double Width { get; set; }
        public double Height { get; set; }

        /// <summary>
        /// Here the Area method can be extended to suit the needs of a Rectangle, it is open for extensions but in base class it is closed for modifications.
        /// </summary>
        /// <returns></returns>
        public override double Area()
        {
            return Width * Height;
        }
    }

    class Circle : Shape
    {
        public double Radius { get; set; }

        /// <summary>
        /// Here the Area method can be extended to suit the needs of a Circle, it is open for extensions but in base class it is closed for modifications.
        /// </summary>
        /// <returns></returns>
        public override double Area()
        {
            return 2 * Radius * Math.PI;
        }
    }

    public class OpenClosedPrinciple
    {
        public void CalculateArea(Shape[] shapes)
        {
            foreach (var shape in shapes)
            {
                Console.WriteLine(shape.Area());
                shape.Foo();
            }
        }

        public OpenClosedPrinciple()
        {
            var rectangle = new Rectangle { Width = 10, Height = 10 };
            var circle = new Circle { Radius = 5 };
            Shape[] shapes = new Shape[2];
            shapes[0] = rectangle;
            shapes[1] = circle;
            CalculateArea(shapes);
        }
    }
}
