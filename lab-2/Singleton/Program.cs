using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Singleton
{
    public sealed class Authenticator
    {
        private static readonly Lazy<Authenticator> _instance =
            new Lazy<Authenticator>(() => new Authenticator());

        // Приватний конструктор
        private Authenticator()
        {
            Console.WriteLine("Authenticator created.");
        }

        // Глобальна точка доступу
        public static Authenticator Instance
        {
            get { return _instance.Value; }
        }

        public void Authenticate(string username, string password)
        {
            Console.WriteLine($"Authenticating user: {username}");
            // Тут могла би бути логіка перевірки
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.Unicode;
            Console.InputEncoding = Encoding.Unicode;
            Console.WriteLine("Лабораторна робота 2, Завдання 3\nВиконала Черкавська Д.В., група ВТ-23-2\n");
            var auth1 = Authenticator.Instance;
            auth1.Authenticate("admin", "1234");

            var auth2 = Authenticator.Instance;
            auth2.Authenticate("user", "password");

            Console.WriteLine(object.ReferenceEquals(auth1, auth2)
                ? "auth1 і auth2 — це один і той самий об'єкт"
                : "auth1 і auth2 — різні об'єкти");
        }
    }
}
