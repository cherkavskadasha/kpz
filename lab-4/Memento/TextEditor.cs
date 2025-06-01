using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memento
{
    public class TextEditor
    {
        private readonly TextDocument _document = new TextDocument();
        private readonly Stack<Memento> _history = new Stack<Memento>();

        public void Type(string text)
        {
            _history.Push(_document.Save());
            _document.Write(text);
        }

        public void Undo()
        {
            if (_history.Count > 0)
            {
                var previousState = _history.Pop();
                _document.Restore(previousState);
            }
            else
            {
                Console.WriteLine("No more undos available.");
            }
        }

        public void Show()
        {
            Console.WriteLine($"Current document content: \"{_document.Content}\"");
        }
    }
}
