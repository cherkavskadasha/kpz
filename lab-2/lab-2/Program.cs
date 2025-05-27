using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab_2
{
    public interface ISubscription
    {
        decimal MonthlyFee { get; }
        int MinPeriodMonths { get; }
        List<string> Channels { get; }
        void ShowDetails();
    }

    // Конкретні реалізації підписок
    public class DomesticSubscription : ISubscription
    {
        public decimal MonthlyFee => 9.99m;
        public int MinPeriodMonths => 1;
        public List<string> Channels => new List<string> { "News", "Movies", "Sports" };

        public void ShowDetails()
        {
            Console.WriteLine("Domestic Subscription:");
            Console.WriteLine($"Monthly Fee: {MonthlyFee}, Min Period: {MinPeriodMonths} months");
            Console.WriteLine("Channels: " + string.Join(", ", Channels));
        }
    }

    public class EducationalSubscription : ISubscription
    {
        public decimal MonthlyFee => 6.99m;
        public int MinPeriodMonths => 3;
        public List<string> Channels => new List<string> { "Science", "History", "Documentary" };

        public void ShowDetails()
        {
            Console.WriteLine("Educational Subscription:");
            Console.WriteLine($"Monthly Fee: {MonthlyFee}, Min Period: {MinPeriodMonths} months");
            Console.WriteLine("Channels: " + string.Join(", ", Channels));
        }
    }

    public class PremiumSubscription : ISubscription
    {
        public decimal MonthlyFee => 19.99m;
        public int MinPeriodMonths => 6;
        public List<string> Channels => new List<string> { "All Channels", "4K Content", "Exclusive Shows" };

        public void ShowDetails()
        {
            Console.WriteLine("Premium Subscription:");
            Console.WriteLine($"Monthly Fee: {MonthlyFee}, Min Period: {MinPeriodMonths} months");
            Console.WriteLine("Channels: " + string.Join(", ", Channels));
        }
    }

    // Абстрактний клас - фабрика
    public abstract class SubscriptionCreator
    {
        public abstract ISubscription CreateSubscription(string type);
    }

    // Конкретні фабрики
    public class WebSite : SubscriptionCreator
    {
        public override ISubscription CreateSubscription(string type)
        {
            Console.WriteLine("[WebSite] Creating subscription...");
            if (type == "Domestic")
                return new DomesticSubscription();
            else if (type == "Educational")
                return new EducationalSubscription();
            else if (type == "Premium")
                return new PremiumSubscription();
            else
                throw new ArgumentException("Invalid subscription type for WebSite");
        }
    }

    public class MobileApp : SubscriptionCreator
    {
        public override ISubscription CreateSubscription(string type)
        {
            Console.WriteLine("[MobileApp] Creating subscription...");
            if (type == "Domestic")
                return new DomesticSubscription();
            else if (type == "Educational")
                return new EducationalSubscription();
            else if (type == "Premium")
                return new PremiumSubscription();
            else
                throw new ArgumentException("Invalid subscription type for MobileApp");
        }
    }

    public class ManagerCall : SubscriptionCreator
    {
        public override ISubscription CreateSubscription(string type)
        {
            Console.WriteLine("[ManagerCall] Creating subscription with personal assistance...");
            if (type == "Domestic")
                return new DomesticSubscription();
            else if (type == "Educational")
                return new EducationalSubscription();
            else if (type == "Premium")
                return new PremiumSubscription();
            else
                throw new ArgumentException("Invalid subscription type for ManagerCall");
        }
    }

    // Головна програма
    public class Program
    {
        public static void Main()
        {
            Console.WriteLine("Лабораторна робота 2, Завдання 1\nВиконала Черкавська Д.В., група ВТ-23-2\n");
            SubscriptionCreator creator;

            creator = new WebSite();
            ISubscription sub1 = creator.CreateSubscription("Domestic");
            sub1.ShowDetails();

            Console.WriteLine();

            creator = new MobileApp();
            ISubscription sub2 = creator.CreateSubscription("Premium");
            sub2.ShowDetails();

            Console.WriteLine();

            creator = new ManagerCall();
            ISubscription sub3 = creator.CreateSubscription("Educational");
            sub3.ShowDetails();
        }
    }
}
