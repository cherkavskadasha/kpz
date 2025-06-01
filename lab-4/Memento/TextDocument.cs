using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memento
{
    public class TextDocument
    {
        public string Content { get; private set; } = "";

        public void Write(string text)
        {
            Content += text;
        }

        public void Erase()
        {
            Content = "";
        }

        public Memento Save()
        {
            return new Memento(Content);
        }

        public void Restore(Memento memento)
        {
            Content = memento.GetSavedState();
        }
    }
}
