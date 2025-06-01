using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab_4
{
    // Абстрактний обробник
    abstract class SupportHandler
    {
        protected SupportHandler next;

        public void SetNext(SupportHandler next)
        {
            this.next = next;
        }

        public abstract bool Handle(string issue);
    }

    // Конкретні обробники
    class BasicSupportHandler : SupportHandler
    {
        public override bool Handle(string issue)
        {
            if (issue == "1")
            {
                Console.WriteLine("Basic Support: Ми допоможемо з базовими питаннями.");
                return true;
            }
            else
            {
                return next?.Handle(issue) ?? false;
            }
        }
    }

    class BillingSupportHandler : SupportHandler
    {
        public override bool Handle(string issue)
        {
            if (issue == "2")
            {
                Console.WriteLine("Billing Support: З'єднання з бухгалтерією...");
                return true;
            }
            else
            {
                return next?.Handle(issue) ?? false;
            }
        }
    }

    class TechnicalSupportHandler : SupportHandler
    {
        public override bool Handle(string issue)
        {
            if (issue == "3")
            {
                Console.WriteLine("Technical Support: З'єднання з техпідтримкою...");
                return true;
            }
            else
            {
                return next?.Handle(issue) ?? false;
            }
        }
    }

    class HumanOperatorHandler : SupportHandler
    {
        public override bool Handle(string issue)
        {
            if (issue == "4")
            {
                Console.WriteLine("Оператор: Очікуйте з'єднання з оператором...");
                return true;
            }
            else
            {
                Console.WriteLine("Вибрано неправильний варіант. Спробуйте ще раз.\n");
                return false;
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.Unicode;
            Console.InputEncoding = Encoding.Unicode;
            // Налаштування ланцюжка обробників
            var basic = new BasicSupportHandler();
            var billing = new BillingSupportHandler();
            var technical = new TechnicalSupportHandler();
            var human = new HumanOperatorHandler();

            basic.SetNext(billing);
            billing.SetNext(technical);
            technical.SetNext(human);

            bool handled = false;

            while (!handled)
            {
                Console.WriteLine("\nВас вітає система підтримки:");
                Console.WriteLine("1 - Базова підтримка");
                Console.WriteLine("2 - Питання по оплаті");
                Console.WriteLine("3 - Технічна підтримка");
                Console.WriteLine("4 - Зв’язок з оператором");
                Console.Write("Ваш вибір: ");

                string choice = Console.ReadLine();
                handled = basic.Handle(choice);
            }

            Console.WriteLine("\nДякуємо за звернення!");
        }
    }
}
