using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab_3
{
    class Logger
    {
        public void Log(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public void Error(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public void Warn(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow; // оранжевого немає, жовтий найближчий
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }

    // 2. Клас FileWriter з методами Write і WriteLine
    class FileWriter
    {
        private string _filePath;

        public FileWriter(string filePath)
        {
            _filePath = filePath;
        }

        public void Write(string message)
        {
            File.AppendAllText(_filePath, message);
        }

        public void WriteLine(string message)
        {
            File.AppendAllText(_filePath, message + Environment.NewLine);
        }
    }

    // 3. Адаптер - файловий логер, що використовує FileWriter
    class FileLoggerAdapter
    {
        private FileWriter _fileWriter;

        public FileLoggerAdapter(string filePath)
        {
            _fileWriter = new FileWriter(filePath);
        }

        public void Log(string message)
        {
            _fileWriter.WriteLine("[LOG] " + message);
        }

        public void Error(string message)
        {
            _fileWriter.WriteLine("[ERROR] " + message);
        }

        public void Warn(string message)
        {
            _fileWriter.WriteLine("[WARN] " + message);
        }
    }

    // 4. Головний метод для демонстрації
    class Program
    {
        static void Main()
        {
            Console.WriteLine("=== Console Logger ===");
            Logger consoleLogger = new Logger();
            consoleLogger.Log("This is a log message.");
            consoleLogger.Warn("This is a warning message.");
            consoleLogger.Error("This is an error message.");

            Console.WriteLine("\n=== File Logger (Adapter) ===");
            string filePath = "log.txt";
            FileLoggerAdapter fileLogger = new FileLoggerAdapter(filePath);
            fileLogger.Log("This is a log message.");
            fileLogger.Warn("This is a warning message.");
            fileLogger.Error("This is an error message.");

            Console.WriteLine($"Messages written to file: {Path.GetFullPath(filePath)}");
        }
    }
}
