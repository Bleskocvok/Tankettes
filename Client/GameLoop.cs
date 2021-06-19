using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tankettes.UI;
using static Tankettes.GameLogic.Player;

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

        private const int BlockSize = 4;
        private const decimal PhysicsDelta = 0.02M;     // 50fps

        private decimal _accumulated = 0;

        private GameLogic.State _gameState;

        private UI.MenuFrame _ui;

        public bool Quit { get; private set; }

        public GameLoop(string tankTexture,
                        string cannonTexture,
                        string terrainTexture)
        {
            var normal = new Shop.NormalProjectile();



            _gameState = new GameLogic.State(
                    0,
                    new List<GameLogic.Player>
                    {
                        new GameLogic.Player("A", 1000, Color.Green, new AmmoCapacity(normal, 99)),
                        new GameLogic.Player("B", 1000, Color.Purple, new AmmoCapacity(normal, 99)),
                        new GameLogic.Player("C", 1000, Color.Red, new AmmoCapacity(normal, 99)),
                    },
                    new Rectangle(0, 0, 1200, 600),
                    tankTexture,
                    cannonTexture,
                    terrainTexture,
                    BlockSize
                );
        }

        public void UpdateControls(KeyboardState keyboard)
        {
            if (keyboard.IsKeyDown(Keys.Escape))
                Quit = true;
        }

        public void Update(GameTime gameTime)
        {
            _accumulated += (decimal)gameTime.ElapsedGameTime.TotalMilliseconds;

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
