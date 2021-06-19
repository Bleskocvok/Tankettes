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
        private GameLogic.Terrain _terrain;

        private UI.MenuFrame _ui;

        public override IEnumerable<IDrawable> Elements => _terrain.Elements;

        public bool Quit { get; private set; }

        public GameLoop(GameLogic.Terrain terrain)
        {
            _terrain = terrain;
        }

        public void UpdateControls(KeyboardState keyboard)
        {
            if (keyboard.IsKeyDown(Keys.Escape))
                Quit = true;
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
