using System;
using System.Collections;
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
        protected string Text { get; set; }

        public LightTextNode(string text)
        {
            Text = text;
        }

        public override string OuterHTML => Text;
        public override string InnerHTML => Text;
    }

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

    // ==== State Pattern ====
    public interface IRenderState
    {
        string Render(LightElementNode element);
    }

    public class NormalRenderState : IRenderState
    {
        public string Render(LightElementNode element)
        {
            if (element.Closing == ClosingType.Single)
            {
                return $"<{element.TagName}{element.CssClassString}/>";
            }
            return $"<{element.TagName}{element.CssClassString}>{element.InnerHTML}</{element.TagName}>";
        }
    }

    public class MinifiedRenderState : IRenderState
    {
        public string Render(LightElementNode element)
        {
            var innerHtmlMinified = "";
            foreach (var child in element.Children)
            {
                if (child is LightElementNode childElement)
                {
                    innerHtmlMinified += Render(childElement);
                }
                else
                {
                    innerHtmlMinified += child.OuterHTML;
                }
            }

            var classStr = element.CssClasses.Count > 0 ? $" class=\"{string.Join(" ", element.CssClasses)}\"" : "";

            if (element.Closing == ClosingType.Single)
            {
                return $"<{element.TagName}{classStr}/>";
            }
            else
            {
                innerHtmlMinified = System.Text.RegularExpressions.Regex.Replace(innerHtmlMinified, @"\s+", " ");

                return $"<{element.TagName}{classStr}>{innerHtmlMinified}</{element.TagName}>";
            }
        }
    }

    public class PrettyRenderState : IRenderState
    {
        public string Render(LightElementNode element)
        {
            StringBuilder sb = new StringBuilder();
            PrettyRenderRecursive(element, sb, 0);
            return sb.ToString();
        }

        private void PrettyRenderRecursive(LightElementNode element, StringBuilder sb, int indent)
        {
            string indentStr = new string(' ', indent * 2);
            sb.AppendLine($"{indentStr}<{element.TagName}{element.CssClassString}>");
            foreach (var child in element.Children)
            {
                if (child is LightElementNode childElement)
                {
                    PrettyRenderRecursive(childElement, sb, indent + 1);
                }
                else
                {
                    sb.AppendLine($"{new string(' ', (indent + 1) * 2)}{child.OuterHTML}");
                }
            }
            sb.AppendLine($"{indentStr}</{element.TagName}>");
        }
    }

    public class RenderContext
    {
        private IRenderState _state;

        public RenderContext(IRenderState state)
        {
            _state = state;
        }

        public void SetState(IRenderState state)
        {
            _state = state;
        }

        public string Render(LightElementNode element)
        {
            return _state.Render(element);
        }
    }

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

        public string CssClassString => CssClasses.Count > 0 ? $" class=\"{string.Join(" ", CssClasses)}\"" : "";

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
                var context = new RenderContext(new NormalRenderState());
                return context.Render(this);
            }
        }

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

    // ==== Command Pattern ====
    public interface ICommand
    {
        void Execute();
    }

    public class AddChildCommand : ICommand
    {
        private readonly LightElementNode _parent;
        private readonly LightNode _child;

        public AddChildCommand(LightElementNode parent, LightNode child)
        {
            _parent = parent;
            _child = child;
        }

        public void Execute()
        {
            _parent.AddChild(_child);
        }
    }

    public class AddCssClassCommand : ICommand
    {
        private readonly LightElementNode _element;
        private readonly string _cssClass;

        public AddCssClassCommand(LightElementNode element, string cssClass)
        {
            _element = element;
            _cssClass = cssClass;
        }

        public void Execute()
        {
            _element.CssClasses.Add(_cssClass);
        }
    }

    public class CommandManager
    {
        private readonly Queue<ICommand> _commands = new Queue<ICommand>();

        public void AddCommand(ICommand command)
        {
            _commands.Enqueue(command);
        }

        public void ExecuteAll()
        {
            while (_commands.Count > 0)
            {
                var command = _commands.Dequeue();
                command.Execute();
            }
        }
    }

    // ==== Program Entry ====
    class Program
    {
        static void Main(string[] args)
        {
            var commandManager = new CommandManager();

            var ul = new LightElementNode("ul", DisplayType.Block, ClosingType.Pair);
            commandManager.AddCommand(new AddCssClassCommand(ul, "list"));

            var li1 = new LightElementNode("li", DisplayType.Block, ClosingType.Pair);
            var li2 = new LightElementNode("li", DisplayType.Block, ClosingType.Pair);

            commandManager.AddCommand(new AddChildCommand(li1, new LoggingTextNode("Item 1")));
            commandManager.AddCommand(new AddChildCommand(li2, new LoggingTextNode("Item 2")));

            commandManager.AddCommand(new AddChildCommand(ul, li1));
            commandManager.AddCommand(new AddChildCommand(ul, li2));

            commandManager.ExecuteAll();

            var renderContext = new RenderContext(new PrettyRenderState());
            Console.WriteLine("=== Pretty Render ===");
            Console.WriteLine(renderContext.Render(ul));

            renderContext.SetState(new MinifiedRenderState());
            Console.WriteLine("\n=== Minified Render ===");
            Console.WriteLine(renderContext.Render(ul));

            renderContext.SetState(new NormalRenderState());
            Console.WriteLine("\n=== Normal Render ===");
            Console.WriteLine(renderContext.Render(ul));

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