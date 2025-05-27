using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab_1
{

    abstract class Animal
    {
        public string Name { get; set; }
        public string Species { get; set; }
        public int Age { get; set; }

        public Animal(string name, string species, int age)
        {
            Name = name;
            Species = species;
            Age = age;
        }

        public abstract void MakeSound();
    }

    class Mammal : Animal
    {
        public Mammal(string name, string species, int age)
            : base(name, species, age) { }

        public override void MakeSound()
        {
            Console.WriteLine($"{Name} (мамал) ричить.");
        }
    }

    class Bird : Animal
    {
        public Bird(string name, string species, int age)
            : base(name, species, age) { }

        public override void MakeSound()
        {
            Console.WriteLine($"{Name} (птах) щебече.");
        }
    }

    class Enclosure
    {
        public string Type { get; set; }
        public int SizeInSquareMeters { get; set; }
        public List<Animal> Animals { get; set; }

        public Enclosure(string type, int size)
        {
            Type = type;
            SizeInSquareMeters = size;
            Animals = new List<Animal>();
        }

        public void AddAnimal(Animal animal)
        {
            Animals.Add(animal);
        }
    }

    class Food
    {
        public string Name { get; set; }
        public string ForSpecies { get; set; }
        public int QuantityKg { get; set; }

        public Food(string name, string forSpecies, int quantityKg)
        {
            Name = name;
            ForSpecies = forSpecies;
            QuantityKg = quantityKg;
        }
    }

    class ZooWorker
    {
        public string FullName { get; set; }
        public string Position { get; set; }

        public ZooWorker(string name, string position)
        {
            FullName = name;
            Position = position;
        }
    }

    class ZooInventory
    {
        public List<Enclosure> Enclosures { get; set; }
        public List<Food> Foods { get; set; }
        public List<ZooWorker> Workers { get; set; }

        public ZooInventory(List<Enclosure> enclosures, List<Food> foods, List<ZooWorker> workers)
        {
            Enclosures = enclosures;
            Foods = foods;
            Workers = workers;
        }

        public void PrintInventory()
        {
            Console.WriteLine("=== Інвентаризація зоопарку ===");

            Console.WriteLine("\n Тварини:");
            foreach (Enclosure enclosure in Enclosures)
            {
                Console.WriteLine($"Вольєр: {enclosure.Type}, Розмір: {enclosure.SizeInSquareMeters}м²");
                foreach (Animal animal in enclosure.Animals)
                {
                    Console.WriteLine($" - {animal.Species} на ім’я {animal.Name}, вік {animal.Age}");
                }
            }

            Console.WriteLine("\n Корм:");
            foreach (Food food in Foods)
            {
                Console.WriteLine($"{food.Name} для {food.ForSpecies} — {food.QuantityKg} кг");
            }

            Console.WriteLine("\n Працівники:");
            foreach (ZooWorker worker in Workers)
            {
                Console.WriteLine($"{worker.FullName} — {worker.Position}");
            }

            Console.WriteLine("==============================");
        }
    }

    class Program
    {
        static void Main()
        {
            Console.OutputEncoding = Encoding.Unicode;
            Console.InputEncoding = Encoding.Unicode;
            Console.WriteLine("Лабораторна робота 1\nВиконала Черкавська Д.В., група ВТ-23-2");

            Animal lion = new Mammal("Шрам", "Лев", 5);
            Animal parrot = new Bird("Голубчик", "Папуга", 2);

            Enclosure mammalEnclosure = new Enclosure("Саванна", 200);
            mammalEnclosure.AddAnimal(lion);

            Enclosure birdEnclosure = new Enclosure("Вольєр для птахів", 50);
            birdEnclosure.AddAnimal(parrot);

            Food meat = new Food("М'ясо", "Лев", 30);
            Food seeds = new Food("Насіння", "Папуга", 10);

            ZooWorker keeper = new ZooWorker("Артем Оленчук", "Доглядач");
            ZooWorker vet = new ZooWorker("Наталія Степанчук", "Ветеринар");

            List<Enclosure> enclosures = new List<Enclosure> { mammalEnclosure, birdEnclosure };
            List<Food> foods = new List<Food> { meat, seeds };
            List<ZooWorker> workers = new List<ZooWorker> { keeper, vet };

            ZooInventory inventory = new ZooInventory(enclosures, foods, workers);
            inventory.PrintInventory();
        }
    }
}
