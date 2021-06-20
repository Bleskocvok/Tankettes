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

        private const int PhysicsDelta = 20;     // 20ms = 50fps

        private KeyboardState _previous = Keyboard.GetState();

        public override ICollection<IDrawable> Elements
        {
            get => _gameState.Elements.Union(_ui.Elements).ToList();
        }

        private float _accumulated = 0;

        private readonly GameLogic.State _gameState;

        private bool _waitForEquilibrium = false;


        private readonly UI.MenuFrame _ui = new();

        private UI.Button _switchButton;
        private UI.Slider _powerSlider;
        private UI.Slider _angleSlider;

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
            back.EventOnRelease += (s, a) => QuitGame();

            _powerSlider = new Slider(0, GameLogic.Player.MaxPower, 0, new Rectangle(800, 640, 200, 50));
            _angleSlider = new Slider(0, 180, 0, new Rectangle(1030, 640, 200, 50));

            _angleSlider.EventOnChange += (s, a)
                    => SetAngle(180 - _angleSlider.Value);

            _powerSlider.EventOnChange += (s, a)
                    => SetPower(_powerSlider.Value);

            _ui.Add(fire);
            _ui.Add(_switchButton);
            _ui.Add(back);
            _ui.Add(_powerSlider);
            _ui.Add(_angleSlider);

            UpdateAmmoLabel();
            UpdateSliders();
        }

        private void SetAngle(float angle)
        {
            if (_gameState.CurrentPlayer == null
                    || _gameState.CurrentPlayer.Tank == null)
                return;

            _gameState.CurrentPlayer.Tank.CannonAngle = angle;
        }

        private void SetPower(int val)
        {
            if (_gameState.CurrentPlayer == null)
                return;

            _gameState.CurrentPlayer.Power = val;
        }

        public void UpdateControls(KeyboardState keyboard)
        {
            if (keyboard.IsKeyDown(Keys.Escape))
            {
                QuitGame();
            }

            if (!_waitForEquilibrium)
            {
                PlayerControls(keyboard);
            }

            _previous = keyboard;
        }

        private void QuitGame()
        {
            Quit = true;
            Serializer.SaveGame("E:\\skola\\c-sharp\\project\\Tankettes\\ahoj.json", _gameState);
        }

        private void SwitchType()
        {
            if (_gameState.Players.Count == 0)
                return;

            _gameState.CurrentPlayer.NextAmmo();
            UpdateAmmoLabel();
        }

        private void UpdateSliders()
        {
            if (_gameState.Players.Count == 0)
                return;

            if (_gameState.CurrentPlayer.Tank == null)
                return;

            _powerSlider.Value = _gameState.CurrentPlayer.Power;

            var angle = _gameState.CurrentPlayer.Tank.CannonAngle;
            _angleSlider.Value = (int)(180f - angle);
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
            {
                playerTank.CannonAngle -= CannonAngleSpeed;
                UpdateSliders();
            }

            if (keyboard.IsKeyDown(Keys.Left))
            {
                playerTank.CannonAngle += CannonAngleSpeed;
                UpdateSliders();
            }

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
                NextTurn();
            }

            _ui.Update(gameTime);
        }

        private void NextTurn()
        {
            _gameState.NextPlayer();
            UpdateAmmoLabel();
            UpdateSliders();
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
