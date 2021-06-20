using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tankettes.UI
{
    class MenuFrame : AbstractDrawable, IMenuScreen
    {
        public override IEnumerable<IDrawable> Elements { get => _elements; }

        private readonly List<IElementUI> _elements = new();

        public void Add(IElementUI elem) => _elements.Add(elem);

        public void UpdateMouse(Point pos)
        {
            foreach (var but in _elements)
            {
                but.UpdateMouse(pos);
            }
        }

        public void Click(bool ended = false)
        {
            foreach (var but in _elements)
            {
                but.Click(ended);
            }
        }

        public void UpdateControls(KeyboardState keyboard) { }

        public void Update(GameTime gameTime)
        {
            foreach (var but in _elements)
            {
                but.Update(gameTime);
            }
        }
    }
}
