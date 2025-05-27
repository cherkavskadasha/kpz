using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractFactory
{
    // Інтерфейси пристроїв
    interface ILaptop
    {
        void GetDetails();
    }

    interface INetbook
    {
        void GetDetails();
    }

    interface IEBook
    {
        void GetDetails();
    }

    interface ISmartphone
    {
        void GetDetails();
    }

    // Абстрактна фабрика
    interface ITechFactory
    {
        ILaptop CreateLaptop();
        INetbook CreateNetbook();
        IEBook CreateEBook();
        ISmartphone CreateSmartphone();
    }

    // Реалізації девайсів для IProne

    class IProneLaptop : ILaptop
    {
        public void GetDetails() => Console.WriteLine("IProne Laptop");
    }

    class IProneNetbook : INetbook
    {
        public void GetDetails() => Console.WriteLine("IProne Netbook");
    }

    class IProneEBook : IEBook
    {
        public void GetDetails() => Console.WriteLine("IProne EBook");
    }

    class IProneSmartphone : ISmartphone
    {
        public void GetDetails() => Console.WriteLine("IProne Smartphone");
    }

    // Реалізації девайсів для Kiaomi

    class KiaomiLaptop : ILaptop
    {
        public void GetDetails() => Console.WriteLine("Kiaomi Laptop");
    }

    class KiaomiNetbook : INetbook
    {
        public void GetDetails() => Console.WriteLine("Kiaomi Netbook");
    }

    class KiaomiEBook : IEBook
    {
        public void GetDetails() => Console.WriteLine("Kiaomi EBook");
    }

    class KiaomiSmartphone : ISmartphone
    {
        public void GetDetails() => Console.WriteLine("Kiaomi Smartphone");
    }

    // Реалізації девайсів для Balaxy

    class BalaxyLaptop : ILaptop
    {
        public void GetDetails() => Console.WriteLine("Balaxy Laptop");
    }

    class BalaxyNetbook : INetbook
    {
        public void GetDetails() => Console.WriteLine("Balaxy Netbook");
    }

    class BalaxyEBook : IEBook
    {
        public void GetDetails() => Console.WriteLine("Balaxy EBook");
    }

    class BalaxySmartphone : ISmartphone
    {
        public void GetDetails() => Console.WriteLine("Balaxy Smartphone");
    }

    // Конкретні фабрики брендів

    class IProneFactory : ITechFactory
    {
        public ILaptop CreateLaptop() => new IProneLaptop();
        public INetbook CreateNetbook() => new IProneNetbook();
        public IEBook CreateEBook() => new IProneEBook();
        public ISmartphone CreateSmartphone() => new IProneSmartphone();
    }

    class KiaomiFactory : ITechFactory
    {
        public ILaptop CreateLaptop() => new KiaomiLaptop();
        public INetbook CreateNetbook() => new KiaomiNetbook();
        public IEBook CreateEBook() => new KiaomiEBook();
        public ISmartphone CreateSmartphone() => new KiaomiSmartphone();
    }

    class BalaxyFactory : ITechFactory
    {
        public ILaptop CreateLaptop() => new BalaxyLaptop();
        public INetbook CreateNetbook() => new BalaxyNetbook();
        public IEBook CreateEBook() => new BalaxyEBook();
        public ISmartphone CreateSmartphone() => new BalaxySmartphone();
    }

    // Демонстрація роботи

    class Program
    {
        static void Main()
        {
            Console.WriteLine("Лабораторна робота 2, Завдання 2\nВиконала Черкавська Д.В., група ВТ-23-2\n");
            // Створюємо фабрику бренду IProne
            ITechFactory factory = new IProneFactory();

            ILaptop laptop = factory.CreateLaptop();
            INetbook netbook = factory.CreateNetbook();
            IEBook ebook = factory.CreateEBook();
            ISmartphone phone = factory.CreateSmartphone();

            laptop.GetDetails();
            netbook.GetDetails();
            ebook.GetDetails();
            phone.GetDetails();

            Console.WriteLine();

            // Тепер фабрика Kiaomi
            factory = new KiaomiFactory();

            laptop = factory.CreateLaptop();
            netbook = factory.CreateNetbook();
            ebook = factory.CreateEBook();
            phone = factory.CreateSmartphone();

            laptop.GetDetails();
            netbook.GetDetails();
            ebook.GetDetails();
            phone.GetDetails();

            Console.WriteLine();

            // Фабрика Balaxy
            factory = new BalaxyFactory();

            laptop = factory.CreateLaptop();
            netbook = factory.CreateNetbook();
            ebook = factory.CreateEBook();
            phone = factory.CreateSmartphone();

            laptop.GetDetails();
            netbook.GetDetails();
            ebook.GetDetails();
            phone.GetDetails();
        }
    }
}
