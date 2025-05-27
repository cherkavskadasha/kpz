using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flyweight
{
    public enum DisplayType { Block, Inline }
    public enum ClosingType { Pair, Single }

    // === LightNode (Base) ===
    public abstract class LightNode
    {
        public abstract string OuterHTML { get; }
        public abstract string InnerHTML { get; }
    }

    // === LightTextNode ===
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

    // === LightElementNode ===
    public class LightElementNode : LightNode
    {
        public string TagName { get; }
        public DisplayType Display { get; }
        public ClosingType Closing { get; }
        public List<LightNode> Children { get; } = new List<LightNode>();

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

        public override string InnerHTML
        {
            get
            {
                var sb = new StringBuilder();
                foreach (var child in Children)
                    sb.Append(child.OuterHTML);
                return sb.ToString();
            }
        }

        public override string OuterHTML
        {
            get
            {
                if (Closing == ClosingType.Single)
                    return $"<{TagName}/>";
                return $"<{TagName}>{InnerHTML}</{TagName}>";
            }
        }
    }

    // === Flyweight Factory ===
    public class ElementFlyweightFactory
    {
        private readonly Dictionary<string, LightElementNode> _flyweights = new Dictionary<string, LightElementNode>();

        public LightElementNode GetElement(string tagName, DisplayType display, ClosingType closing)
        {
            string key = $"{tagName}-{display}-{closing}";
            if (!_flyweights.ContainsKey(key))
            {
                var node = new LightElementNode(tagName, display, closing);
                _flyweights[key] = node;
            }

            // Повертаємо нову обгортку над шаблоном, щоб мати окремий список дітей
            var flyweight = _flyweights[key];
            return new LightElementNodeProxy(flyweight);
        }

        // Проксі, що має власні дочірні елементи, але спільну конфігурацію
        private class LightElementNodeProxy : LightElementNode
        {
            private readonly LightElementNode _shared;

            public LightElementNodeProxy(LightElementNode shared)
                : base(shared.TagName, shared.Display, shared.Closing)
            {
                _shared = shared;
            }

            public override string OuterHTML => $"<{_shared.TagName}>{InnerHTML}</{_shared.TagName}>";
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            string[] bookLines = new string[]
            {
                "The Great Adventure",
                "",
                "Chapter 1",
                "  It was a dark and stormy night.",
                "The wind howled through the trees.",
                "Suddenly, a shot rang out!"
            };

            Console.WriteLine("=== Without Flyweight ===");
            var htmlNodes1 = GenerateHtml(bookLines, useFlyweight: false);
            PrintHtml(htmlNodes1);

            long memoryBeforeFlyweight = GC.GetTotalMemory(true);

            Console.WriteLine("\nMemory used (without Flyweight): " + memoryBeforeFlyweight + " bytes");

            Console.WriteLine("\n=== With Flyweight ===");
            var htmlNodes2 = GenerateHtml(bookLines, useFlyweight: true);
            PrintHtml(htmlNodes2);

            long memoryAfterFlyweight = GC.GetTotalMemory(true);

            Console.WriteLine("\nMemory used (with Flyweight): " + memoryAfterFlyweight + " bytes");
        }

        static List<LightElementNode> GenerateHtml(string[] lines, bool useFlyweight)
        {
            var result = new List<LightElementNode>();
            var factory = useFlyweight ? new ElementFlyweightFactory() : null;

            foreach (var line in lines)
            {
                string tag;
                if (string.IsNullOrWhiteSpace(line)) continue;
                else if (result.Count == 0) tag = "h1";
                else if (line.Length < 20) tag = "h2";
                else if (char.IsWhiteSpace(line[0])) tag = "blockquote";
                else tag = "p";

                LightElementNode element;
                if (useFlyweight)
                    element = factory.GetElement(tag, DisplayType.Block, ClosingType.Pair);
                else
                    element = new LightElementNode(tag, DisplayType.Block, ClosingType.Pair);

                element.AddChild(new LightTextNode(line));
                result.Add(element);
            }

            return result;
        }

        static void PrintHtml(List<LightElementNode> elements)
        {
            foreach (var el in elements)
            {
                Console.WriteLine(el.OuterHTML);
            }
        }
    }
}
