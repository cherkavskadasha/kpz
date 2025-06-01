using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memento
{
    class Program
    {
        static void Main(string[] args)
        {
            var editor = new TextEditor();

            editor.Type("Hello, ");
            editor.Type("world!");
            editor.Show(); // Output: Hello, world!

            editor.Undo();
            editor.Show(); // Output: Hello, 

            editor.Undo();
            editor.Show(); // Output: 

            editor.Undo(); // No more undos available.
        }
    }
}
