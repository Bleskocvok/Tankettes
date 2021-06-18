using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tankettes.UI;

namespace Tankettes
{
    class GameLoop : AbstractDrawable, UI.IMenuScreen
    {
        private Terrain _terrain = new(
                "terrain",
                new Rectangle(0, 100, 600, 100),
                0, 4, 60, 2);

        public override IEnumerable<IDrawable> Elements => _terrain.Elements;

        public GameLoop()
        {
            // TODO
        }

        public void UpdateControls(KeyboardState keyboard)
        {
        }

        public void Update(GameTime gameTime)
        {
        }

        public void UpdateMouse(Point pos)
        {
        }

        public void Click(bool ended = false)
        {
        }
    }
}
