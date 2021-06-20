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
        public Dictionary<string, IMenuScreen> MenuScreens
        {
            get; private set;
        } = new();

        public IMenuScreen Current
        {
            get => MenuScreens[_current];
        }

        public override ICollection<IDrawable> Elements => Current.Elements;

        public bool Quit { get; set; }

        private string _current;

        public void Add(string tag, IMenuScreen screen)
        {
            MenuScreens.Add(tag, screen);

            // the first added screen will be made current
            if (_current == null)
            {
                MakeCurrent(tag);
            }
        }

        public void AddReplace(string tag, IMenuScreen screen)
        {
            if (MenuScreens.TryAdd(tag, screen))
            {
                return;
            }

            Remove(tag);
            Add(tag, screen);
        }

        public void Remove(string tag)
                => MenuScreens.Remove(tag);

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
