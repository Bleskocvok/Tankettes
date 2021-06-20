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
        {
            get => _gameState.Elements.Union(_ui.Elements);
        }

        private const int PhysicsDelta = 20;     // 20ms = 50fps

        private float _accumulated = 0;

        private GameLogic.State _gameState;

        private bool _waitForEquilibrium = false;


        private UI.MenuFrame _ui = new();

        private UI.Button _switchButton;

        public bool Quit { get; private set; }

        public GameLoop(GameLogic.State state)
        {
            _gameState = state;

            InitUI();
        }

        private void InitUI()
        {
            var fire = new Button("", new Rectangle(565, 570, 150, 150),
                            new ButtonTexture(
                                "fire_button_normal",
                                "fire_button_hover",
                                "fire_button_press"));
            fire.EventOnRelease += (s, a) => Shoot();

            _switchButton = new Button("", new Rectangle(200, 640, 200, 50),
                            new ButtonTexture(
                                "button_normal",
                                "button_over",
                                "button_press"));
            _switchButton.EventOnRelease += (s, a) => SwitchType();

            var back = new Button("exit", new Rectangle(30, 640, 100, 50),
                            new ButtonTexture(
                                "button_normal",
                                "button_over",
                                "button_press"));
            back.EventOnRelease += (s, a) => Quit = true;

            var powerSlider = new Slider(0, 100, 50, new Rectangle(800, 640, 200, 50));
            var angleSlider = new Slider(0, 180, 0, new Rectangle(1030, 640, 200, 50));

            _ui.Add(fire);
            _ui.Add(_switchButton);
            _ui.Add(back);
            _ui.Add(powerSlider);
            _ui.Add(angleSlider);

            UpdateAmmoLabel();
        }

        public void UpdateControls(KeyboardState keyboard)
        {
            if (keyboard.IsKeyDown(Keys.Escape))
            {
                Quit = true;
                // TODO save progress
            }

            if (!_waitForEquilibrium)
            {
                PlayerControls(keyboard);
            }

            _previous = keyboard;
        }

        private void SwitchType()
        {
            if (_gameState.Players.Count == 0)
                return;

            _gameState.CurrentPlayer.NextAmmo();
            UpdateAmmoLabel();
        }

        private void UpdateAmmoLabel()
        {
            if (_gameState.Players.Count == 0)
                return;

            var ammo = _gameState.CurrentPlayer.SelectedAmmo;
            _switchButton.Text = $"{ammo.Count}: {ammo.Type.Name}";
        }

        private void PlayerControls(KeyboardState keyboard)
        {
            if (_gameState.Players.Count == 0)
                return;

            var playerTank = _gameState.CurrentPlayer.Tank;

            if (playerTank == null)
                return;

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
                Shoot();
        }

        private void Shoot()
        {
            if (_waitForEquilibrium)
                return;
            
            _gameState.Shoot();
            _waitForEquilibrium = true;
        }

        public void Update(GameTime gameTime)
        {
            _accumulated += (int)gameTime.ElapsedGameTime.TotalMilliseconds;

            for (; _accumulated >= PhysicsDelta; _accumulated -= PhysicsDelta)
            {
                _gameState.Update(PhysicsDelta);
            }

            if (_waitForEquilibrium && _gameState.IsEquilibrium())
            {
                _waitForEquilibrium = false;
                _gameState.NextPlayer();
                UpdateAmmoLabel();
            }

            _ui.Update(gameTime);
        }

        public void UpdateMouse(Point pos)
        {
            _ui.UpdateMouse(pos);
        }

        public void Click(bool ended = false)
        {
            _ui.Click(ended);
        }
    }
}
