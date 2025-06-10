using System;
using System.Collections.Generic;
using System.Text;

namespace Composite
{
    public enum DisplayType { Block, Inline }
    public enum ClosingType { Pair, Single }

    public abstract class LightNode
    {
        public abstract string OuterHTML { get; }
        public abstract string InnerHTML { get; }
    }

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

    public class LightElementNode : LightNode
    {
        public string TagName { get; set; }
        public DisplayType Display { get; set; }
        public ClosingType Closing { get; set; }
        public List<string> CssClasses { get; set; }
        public List<LightNode> Children { get; set; }

        public LightElementNode(string tagName, DisplayType display, ClosingType closing)
        {
            TagName = tagName;
            Display = display;
            Closing = closing;
            CssClasses = new List<string>();
            Children = new List<LightNode>();
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
                var sb = new StringBuilder();
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

        // Ітератор для глибинного обходу
        public DepthFirstIterator GetDepthFirstIterator()
        {
            return new DepthFirstIterator(this);
        }

        // Внутрішній клас ітератора
        public class DepthFirstIterator
        {
            private Stack<IEnumerator<LightNode>> stack;
            private LightNode current;

            public DepthFirstIterator(LightElementNode root)
            {
                stack = new Stack<IEnumerator<LightNode>>();
                // Починаємо з кореневого елемента — сам вузол (обернемо в список)
                current = root;
                stack.Push(new List<LightNode> { root }.GetEnumerator());
            }

            public bool HasNext()
            {
                return current != null;
            }

            public LightNode Next()
            {
                if (!HasNext())
                    throw new InvalidOperationException("No more elements");

                var result = current;

                // Якщо це LightElementNode — піднімаємось у глибину через дітей
                if (current is LightElementNode element)
                {
                    var childrenEnum = element.Children.GetEnumerator();
                    if (childrenEnum.MoveNext())
                    {
                        stack.Push(childrenEnum);
                        current = childrenEnum.Current;
                        return result;
                    }
                }

                // Якщо немає дітей або це LightTextNode — шукаємо наступного сусіда
                while (stack.Count > 0)
                {
                    var topEnum = stack.Peek();
                    if (topEnum.MoveNext())
                    {
                        current = topEnum.Current;
                        return result;
                    }
                    else
                    {
                        stack.Pop();
                    }
                }

                current = null; // Ітерація завершена
                return result;
            }
        }
    }

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

            Console.WriteLine("\n=== Depth-First Traversal ===");
            var iterator = ul.GetDepthFirstIterator();
            while (iterator.HasNext())
            {
                var node = iterator.Next();
                Console.WriteLine(node.OuterHTML);
            }

            Console.ReadKey();
        }
    }
}
