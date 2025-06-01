using System;
using System.Collections.Generic;
using System.Text;

namespace Composite
{
    // Типи відображення елементів
    public enum DisplayType { Block, Inline }

    // Типи закриття тегів
    public enum ClosingType { Pair, Single }

    // 1. Базовий клас LightNode (без змін)
    public abstract class LightNode
    {
        public abstract string OuterHTML { get; }
        public abstract string InnerHTML { get; }
    }

    // 2. Текстовий вузол (без змін)
    public class LightTextNode : LightNode
    {
        public string Text { get; set; }

        public LightTextNode(string text)
        {
            Text = text;
        }

        public override string OuterHTML => Text;
        public override string InnerHTML => Text;
    }

    // 3. Елемент вузол — основні зміни тут
    public class LightElementNode : LightNode
    {
        public string TagName { get; set; }
        public DisplayType Display { get; set; }
        public ClosingType Closing { get; set; }
        public List<string> CssClasses { get; set; } = new List<string>();
        public List<LightNode> Children { get; set; } = new List<LightNode>();

        private Dictionary<string, List<EventHandler>> _eventListeners = new Dictionary<string, List<EventHandler>>();

        public LightElementNode(string tagName, DisplayType display, ClosingType closing)
        {
            TagName = tagName;
            Display = display;
            Closing = closing;
        }

        public void AddChild(LightNode node)
        {
            Children.Add(node);
        }

        private string CssClassString => CssClasses.Count > 0 ? $" class=\"{string.Join(" ", CssClasses)}\"" : "";

        public override string InnerHTML
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                foreach (var child in Children)
                {
                    sb.Append(child.OuterHTML);
                }
                return sb.ToString();
            }
        }

        public override string OuterHTML
        {
            get
            {
                if (Closing == ClosingType.Single)
                {
                    return $"<{TagName}{CssClassString}/>";
                }

                return $"<{TagName}{CssClassString}>{InnerHTML}</{TagName}>";
            }
        }

        public void AddEventListener(string eventType, EventHandler handler)
        {
            if (!_eventListeners.ContainsKey(eventType))
            {
                _eventListeners[eventType] = new List<EventHandler>();
            }
            _eventListeners[eventType].Add(handler);
        }

        public void TriggerEvent(string eventType, EventArgs args)
        {
            if (_eventListeners.ContainsKey(eventType))
            {
                foreach (var handler in _eventListeners[eventType])
                {
                    handler(this, args);
                }
            }
        }
    }

    // --- НОВЕ --- //
    // 4. Інтерфейс стратегії завантаження зображень
    public interface IImageLoader
    {
        string LoadImage(string href);
    }

    // 5. Стратегія для завантаження з файлу
    public class FileImageLoader : IImageLoader
    {
        public string LoadImage(string href)
        {
            // Симуляція завантаження з файлової системи
            return $"[Image loaded from file: {href}]";
        }
    }

    // 6. Стратегія для завантаження з мережі
    public class NetworkImageLoader : IImageLoader
    {
        public string LoadImage(string href)
        {
            // Симуляція завантаження з мережі
            return $"[Image loaded from network: {href}]";
        }
    }

    // 7. Новий клас для <img> з підтримкою стратегії
    public class LightImageNode : LightNode
    {
        public string Href { get; set; }
        public IImageLoader ImageLoader { get; set; }

        public LightImageNode(string href)
        {
            Href = href;

            // Вибір стратегії в залежності від href
            if (href.StartsWith("http://") || href.StartsWith("https://"))
            {
                ImageLoader = new NetworkImageLoader();
            }
            else
            {
                ImageLoader = new FileImageLoader();
            }
        }

        public override string OuterHTML
        {
            get
            {
                // Вбудовуємо в тег img з href
                return $"<img src=\"{Href}\" alt=\"{ImageLoader.LoadImage(Href)}\"/>";
            }
        }

        public override string InnerHTML => ""; // img — самозакриваючийся тег
    }

    // 8. Демонстрація
    class Program
    {
        static void Main(string[] args)
        {
            var ul = new LightElementNode("ul", DisplayType.Block, ClosingType.Pair);
            ul.CssClasses.Add("list");

            var li1 = new LightElementNode("li", DisplayType.Block, ClosingType.Pair);
            li1.AddChild(new LightTextNode("Item 1"));

            var li2 = new LightElementNode("li", DisplayType.Block, ClosingType.Pair);
            li2.AddChild(new LightTextNode("Item 2"));

            ul.AddChild(li1);
            ul.AddChild(li2);

            Console.WriteLine("=== OuterHTML ===");
            Console.WriteLine(ul.OuterHTML);

            Console.WriteLine("\n=== InnerHTML ===");
            Console.WriteLine(ul.InnerHTML);

            li1.AddEventListener("click", (sender, e) =>
            {
                Console.WriteLine("Клікнули на Item 1!");
            });

            li2.AddEventListener("click", (sender, e) =>
            {
                Console.WriteLine("Клікнули на Item 2!");
            });

            li1.TriggerEvent("click", EventArgs.Empty);
            li2.TriggerEvent("click", EventArgs.Empty);

            // --- Демонстрація нового Image елемента зі стратегією ---
            Console.WriteLine("\n=== Image nodes with Strategy ===");

            var image1 = new LightImageNode("1.png"); // завантаження з файлової системи
            var image2 = new LightImageNode("2.png"); // завантаження з файлової системи

            Console.WriteLine(image1.OuterHTML);
            Console.WriteLine(image2.OuterHTML);

            Console.ReadKey();
        }
    }
}