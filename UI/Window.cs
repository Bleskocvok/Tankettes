using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tankettes.UI
{
    class Window : AbstractDrawable, IMenuScreen
    {
        public IMenuScreen Current
        {
            get => _menuScreens[_current];
        }

        public override IEnumerable<IDrawable> Elements => Current.Elements;

        public bool Quit { get; set; }

        private Dictionary<string, IMenuScreen> _menuScreens = new ();
        private string _current;

        public void Add(string tag, IMenuScreen screen)
        {
            _menuScreens.Add(tag, screen);

            // the first added screen will be made current
            if (_current == null)
            {
                MakeCurrent(tag);
            }
        }

        public void AddReplace(string tag, IMenuScreen screen)
        {
            if (_menuScreens.TryAdd(tag, screen))
            {
                return;
            }

            Remove(tag);
            Add(tag, screen);
        }

        public void Remove(string tag)
                => _menuScreens.Remove(tag);

        public void MakeCurrent(string tag)
                => _current = tag;

        public void Click(bool ended = false)
                => Current.Click(ended);

        public void Update(GameTime gameTime)
                => Current.Update(gameTime);

        public void UpdateControls(KeyboardState keyboard)
                => Current.UpdateControls(keyboard);

        public void UpdateMouse(Point pos)
                => Current.UpdateMouse(pos);
    }
}
