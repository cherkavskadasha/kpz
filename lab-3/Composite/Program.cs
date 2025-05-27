using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Composite
{
    // Типи відображення елементів
    public enum DisplayType { Block, Inline }

    // Типи закриття тегів
    public enum ClosingType { Pair, Single }

    // 2. Базовий клас
    public abstract class LightNode
    {
        public abstract string OuterHTML { get; }
        public abstract string InnerHTML { get; }
    }

    // 3. Текстовий вузол
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

    // 4. Елемент вузол
    public class LightElementNode : LightNode
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
    }

    // 7. Демонстрація
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
        }
    }
}
