using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tankettes.UI
{
    class Menu : AbstractDrawable
    {
        public override IEnumerable<IDrawable> Elements { get => _buttons; }

        private List<Button> _buttons = new();

        public void Add(Button button) => _buttons.Add(button);

        public void UpdateMouse(Point pos)
        {
            foreach (var but in _buttons)
            {
                but.UpdateMouse(pos);
            }
        }

        public void Click(bool ended = false)
        {
            foreach (var but in _buttons)
            {
                but.Click(ended);
            }
        }
    }
}
