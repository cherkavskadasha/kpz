using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bridge
{
    // 1. Інтерфейс для рендерингу (реалізація)
    interface IRenderer
    {
        void Render(string shapeName);
    }

    // 2. Конкретні рендерери
    class VectorRenderer : IRenderer
    {
        public void Render(string shapeName)
        {
            Console.WriteLine($"Drawing {shapeName} as vectors");
        }
    }

    class RasterRenderer : IRenderer
    {
        public void Render(string shapeName)
        {
            Console.WriteLine($"Drawing {shapeName} as pixels");
        }
    }

    // 3. Базовий клас фігури (абстракція)
    abstract class Shape
    {
        protected IRenderer renderer;

        public Shape(IRenderer renderer)
        {
            this.renderer = renderer;
        }

        public abstract void Draw();
    }

    // 4. Конкретні фігури
    class Circle : Shape
    {
        public Circle(IRenderer renderer) : base(renderer) { }

        public override void Draw()
        {
            renderer.Render("Circle");
        }
    }

    class Square : Shape
    {
        public Square(IRenderer renderer) : base(renderer) { }

        public override void Draw()
        {
            renderer.Render("Square");
        }
    }

    class Triangle : Shape
    {
        public Triangle(IRenderer renderer) : base(renderer) { }

        public override void Draw()
        {
            renderer.Render("Triangle");
        }
    }

    // 5. Демонстрація
    class Program
    {
        static void Main()
        {
            IRenderer vectorRenderer = new VectorRenderer();
            IRenderer rasterRenderer = new RasterRenderer();

            Shape circleVector = new Circle(vectorRenderer);
            Shape squareRaster = new Square(rasterRenderer);
            Shape triangleVector = new Triangle(vectorRenderer);

            circleVector.Draw();   
            squareRaster.Draw();  
            triangleVector.Draw(); 
        }
    }
}
