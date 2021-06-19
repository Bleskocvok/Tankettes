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
        private const int TankSpeed = 1;
        private const float CannonAngleSpeed = 1;


        private KeyboardState _previous = Keyboard.GetState();

        public override IEnumerable<IDrawable> Elements
                => _gameState.Elements;

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

            var playerTank = _gameState.CurrentPlayer.Tank;

            if (keyboard.IsKeyDown(Keys.D))
                playerTank.Move(TankSpeed, 0);

            if (keyboard.IsKeyDown(Keys.A))
                playerTank.Move(-TankSpeed, 0);

            if (keyboard.IsKeyDown(Keys.Right))
                playerTank.CannonAngle -= CannonAngleSpeed;

            if (keyboard.IsKeyDown(Keys.Left))
                playerTank.CannonAngle += CannonAngleSpeed;

            if (_previous.IsKeyDown(Keys.Space)
                    && keyboard.IsKeyUp(Keys.Space))
                _gameState.Shoot();

            _previous = keyboard;
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
