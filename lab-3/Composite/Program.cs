using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Composite
{
    // Типи відображення елементів
    public enum DisplayType { Block, Inline }

    // Типи закриття тегів
    public enum ClosingType { Pair, Single }

    // Базовий вузол
    public abstract class LightNode
    {
        public abstract string OuterHTML { get; }
        public abstract string InnerHTML { get; }
    }

    // Текстовий вузол
    public class LightTextNode : LightNode
    {
        protected string Text { get; set; }

        public LightTextNode(string text)
        {
            Text = text;
        }

        public override string OuterHTML => Text;
        public override string InnerHTML => Text;
    }

    // Розширений текстовий вузол з хуком рендерингу
    public class LoggingTextNode : LightTextNode
    {
        public LoggingTextNode(string text) : base(text) { }

        protected virtual void OnTextRendered()
        {
            Console.WriteLine($"[HOOK] Rendering text: '{Text}'");
        }

        public override string OuterHTML
        {
            get
            {
                OnTextRendered();
                return base.OuterHTML;
            }
        }

        public override string InnerHTML => OuterHTML;
    }

    // Елемент вузол
    public class LightElementNode : LightNode, IEnumerable<LightNode>
    {
        public string TagName { get; set; }
        public DisplayType Display { get; set; }
        public ClosingType Closing { get; set; }
        public List<string> CssClasses { get; set; } = new List<string>();
        public List<LightNode> Children { get; set; } = new List<LightNode>();

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

        // DFS-ітерація (глибина)
        public IEnumerator<LightNode> GetEnumerator()
        {
            yield return this;

            foreach (var child in Children)
            {
                yield return child;

                if (child is LightElementNode elementChild)
                {
                    foreach (var descendant in elementChild)
                    {
                        yield return descendant;
                    }
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        // BFS-ітерація (ширина)
        public IEnumerable<LightNode> TraverseBreadthFirst()
        {
            var queue = new Queue<LightNode>();
            queue.Enqueue(this);

            while (queue.Count > 0)
            {
                var node = queue.Dequeue();
                yield return node;

                if (node is LightElementNode element)
                {
                    foreach (var child in element.Children)
                    {
                        queue.Enqueue(child);
                    }
                }
            }
        }
    }

    // Демонстрація
    class Program
    {
        static void Main(string[] args)
        {
            // Створення: <ul class="list"><li>Item 1</li><li>Item 2</li></ul>
            var ul = new LightElementNode("ul", DisplayType.Block, ClosingType.Pair);
            ul.CssClasses.Add("list");

            var li1 = new LightElementNode("li", DisplayType.Block, ClosingType.Pair);
            li1.AddChild(new LoggingTextNode("Item 1"));

            var li2 = new LightElementNode("li", DisplayType.Block, ClosingType.Pair);
            li2.AddChild(new LoggingTextNode("Item 2"));

            ul.AddChild(li1);
            ul.AddChild(li2);

            Console.WriteLine("=== OuterHTML ===");
            Console.WriteLine(ul.OuterHTML);

            Console.WriteLine("\n=== InnerHTML ===");
            Console.WriteLine(ul.InnerHTML);

            Console.WriteLine("\n=== DFS Traversal ===");
            foreach (var node in ul)
            {
                Console.WriteLine($"Node: {node.GetType().Name}");
            }

            Console.WriteLine("\n=== BFS Traversal ===");
            foreach (var node in ul.TraverseBreadthFirst())
            {
                Console.WriteLine($"Node: {node.GetType().Name}");
            }

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}
