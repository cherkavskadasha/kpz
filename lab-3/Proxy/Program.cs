using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.IO;

namespace Proxy
{
    // 1. Інтерфейс
    public interface ISmartTextReader
    {
        char[][] ReadText(string filePath);
    }

    // 2. Реальний об'єкт
    public class SmartTextReader : ISmartTextReader
    {
        public char[][] ReadText(string filePath)
        {
            var lines = File.ReadAllLines(filePath);
            char[][] result = new char[lines.Length][];
            for (int i = 0; i < lines.Length; i++)
            {
                result[i] = lines[i].ToCharArray();
            }
            return result;
        }
    }

    // 3. Проксі з логуванням
    public class SmartTextChecker : ISmartTextReader
    {
        private SmartTextReader reader = new SmartTextReader();

        public char[][] ReadText(string filePath)
        {
            Console.WriteLine($"Opening file: {filePath}");
            char[][] content = reader.ReadText(filePath);
            Console.WriteLine($"File read successfully. Lines: {content.Length}");

            int totalChars = 0;
            foreach (var line in content)
                totalChars += line.Length;

            Console.WriteLine($"Total characters: {totalChars}");
            Console.WriteLine($"Closing file: {filePath}");

            return content;
        }
    }

    // 4. Проксі з обмеженням доступу
    public class SmartTextReaderLocker : ISmartTextReader
    {
        private SmartTextReader reader = new SmartTextReader();
        private Regex restrictedPattern;

        public SmartTextReaderLocker(string pattern)
        {
            restrictedPattern = new Regex(pattern);
        }

        public char[][] ReadText(string filePath)
        {
            if (restrictedPattern.IsMatch(filePath))
            {
                Console.WriteLine("Access denied!");
                return null;
            }
            return reader.ReadText(filePath);
        }
    }

    // 5. Головна програма
    class Program
    {
        static void Main(string[] args)
        {
            string allowedFile = "sample.txt";
            string restrictedFile = "secret.txt";

            // Створення тестових файлів
            File.WriteAllLines(allowedFile, new[] {
                "Hello, world!",
                "This is a test."
            });

            File.WriteAllLines(restrictedFile, new[] {
                "Top secret content.",
                "Do not read!"
            });

            Console.WriteLine("=== SmartTextChecker ===");
            ISmartTextReader checker = new SmartTextChecker();
            checker.ReadText(allowedFile);

            Console.WriteLine("\n=== SmartTextReaderLocker ===");
            ISmartTextReader locker = new SmartTextReaderLocker(@"secret\.txt");

            locker.ReadText(restrictedFile); // має вивести "Access denied!"
            locker.ReadText(allowedFile);    // має дозволити читання
        }
    }
}
