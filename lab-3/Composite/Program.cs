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

        // ДОДАНО: Словник для зберігання подій (спостерігачів)
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

        // ДОДАНО: Метод додавання слухача на подію
        public void AddEventListener(string eventType, EventHandler handler)
        {
            if (!_eventListeners.ContainsKey(eventType))
            {
                _eventListeners[eventType] = new List<EventHandler>();
            }
            _eventListeners[eventType].Add(handler);
        }

        // ДОДАНО: Метод виклику (тригера) події
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

    // 4. Демонстрація
    class Program
    {
        static void Main(string[] args)
        {
            // Створення <ul class="list"><li>Item 1</li><li>Item 2</li></ul>
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

            // Підписка на події
            li1.AddEventListener("click", (sender, e) =>
            {
                Console.WriteLine("Клікнули на Item 1!");
            });

            li2.AddEventListener("click", (sender, e) =>
            {
                Console.WriteLine("Клікнули на Item 2!");
            });

            // Симуляція події: викликає обробник
            li1.TriggerEvent("click", EventArgs.Empty);
            li2.TriggerEvent("click", EventArgs.Empty);

            Console.ReadKey();
        }

    }
}