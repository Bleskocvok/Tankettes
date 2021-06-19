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
        public override IEnumerable<IDrawable> Elements
        {
            get
            {
                var tanks = _gameState.Players.Select(p => p.Tank);
                var result = _gameState.Terrain.Elements.Union(tanks);
                return result;
            }
        }

        private const int PhysicsDelta = 20;     // 50fps

        private float _accumulated = 0;

        private GameLogic.State _gameState;

        private UI.MenuFrame _ui;

        public bool Quit { get; private set; }

        public GameLoop(GameLogic.State state)
        {
            _gameState = state;
        }

        public void UpdateControls(KeyboardState keyboard)
        {
            if (keyboard.IsKeyDown(Keys.Escape))
                Quit = true;

            if (keyboard.IsKeyDown(Keys.D))
            {
                _gameState.Players[0].Tank.Move(1, 0);
            }
            if (keyboard.IsKeyDown(Keys.A))
            {
                _gameState.Players[0].Tank.Move(-1, 0);
            }
        }

        public void Update(GameTime gameTime)
        {
            _accumulated += (int)gameTime.ElapsedGameTime.TotalMilliseconds;

            for (; _accumulated >= PhysicsDelta; _accumulated -= PhysicsDelta)
            {
                _gameState.Update(PhysicsDelta);
            }
        }

        public void UpdateMouse(Point pos)
        {
        }

        public void Click(bool ended = false)
        {
        }
    }
}
