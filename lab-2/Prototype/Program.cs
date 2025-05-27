using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prototype
{
    public class Virus
    {
        public string Name { get; set; }
        public string Species { get; set; }
        public int Age { get; set; }
        public double Weight { get; set; }
        public List<Virus> Children { get; set; } = new List<Virus>();

        public Virus(string name, string species, int age, double weight)
        {
            Name = name;
            Species = species;
            Age = age;
            Weight = weight;
        }

        // Метод глибокого клонування
        public Virus Clone()
        {
            Virus clone = new Virus(Name, Species, Age, Weight);
            foreach (var child in Children)
            {
                clone.Children.Add(child.Clone());
            }
            return clone;
        }

        public void Print(string indent = "")
        {
            Console.WriteLine($"{indent}Virus: {Name}, Species: {Species}, Age: {Age}, Weight: {Weight}");
            foreach (var child in Children)
            {
                child.Print(indent + "  ");
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.Unicode;
            Console.InputEncoding = Encoding.Unicode;
            Console.WriteLine("Лабораторна робота 2, Завдання 4\nВиконала Черкавська Д.В., група ВТ-23-2\n");

            // Створення "сімейства" вірусів (мінімум 3 покоління)
            Virus grandchild1 = new Virus("Delta", "Type-C", 0, 0.1);

            Virus child1 = new Virus("Beta", "Type-B", 1, 0.3);
            child1.Children.Add(grandchild1);

            Virus child2 = new Virus("Gamma", "Type-B", 1, 0.25);

            Virus parent = new Virus("Alpha", "Type-A", 5, 1.5);
            parent.Children.Add(child1);
            parent.Children.Add(child2);

            Console.WriteLine("=== Оригінальний вірус ===");
            parent.Print();

            // Клонування
            Virus cloned = parent.Clone();

            Console.WriteLine("\n=== Клонований вірус ===");
            cloned.Print();

            // Перевірка, що це різні об'єкти
            Console.WriteLine($"\nparent == cloned: {object.ReferenceEquals(parent, cloned)}");
            Console.WriteLine($"parent.Children[0] == cloned.Children[0]: {object.ReferenceEquals(parent.Children[0], cloned.Children[0])}");

            Console.WriteLine("\nНатисніть будь-яку клавішу для завершення...");
            Console.ReadKey();
        }
    }
}
