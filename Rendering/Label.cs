using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tankettes
{
    // we might want to add more attributes to this Label class
    // in the future (color, size, etc.)
    public class Label
    {
        public string Text { get; set; }

        public Label() { }

        public Label(string text)
        {
            Text = text;
        }
    }
}
