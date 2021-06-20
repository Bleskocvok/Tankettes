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


        private readonly BackgroundSaver _backgroundSaver = new BackgroundSaver
        {
            Filename = "save",
            IntervalMs = 5000
        };


        private readonly UI.MenuFrame _ui = new();

        private UI.Button _switchButton;
        private UI.Slider _powerSlider;
        private UI.Slider _angleSlider;
        private UI.Image _playerImage;

        public bool Quit { get; private set; }

        public bool GameEnded { get; private set; }

        public GameLogic.Player Victor()
        {
            if (_gameState.Tanks().Count() != 1)
            {
                return null;
            }

            return _gameState.Players
                    .First(p => p.Tank != null && !p.Tank.IsDestroyed());
        }

        public GameLoop(GameLogic.State state)
        {
            _gameState = state;

            _waitForEquilibrium = !_gameState.IsEquilibrium();

            InitUI();
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
            _backgroundSaver.ForceSave(_gameState);
        }

        private void SwitchType()
        {
            if (_gameState.Players.Count == 0)
                return;

            _gameState.CurrentPlayer.NextAmmo();
            UpdateAmmoLabel();
        }

        private void UpdatePlayerImage()
        {
            _playerImage.Color = _gameState.CurrentPlayer.TankColor;
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

        private void MoveTank(int way)
        {
            if (_gameState.Players.Count == 0)
                return;

            var playerTank = _gameState.CurrentPlayer.Tank;
            playerTank.Move(way * TankSpeed, 0);
        }

        private void PlayerControls(KeyboardState keyboard)
        {
            if (_gameState.Players.Count == 0)
                return;

            var playerTank = _gameState.CurrentPlayer.Tank;

            if (playerTank == null)
                return;

            if (keyboard.IsKeyDown(Keys.D))
                MoveTank(1);

            if (keyboard.IsKeyDown(Keys.A))
                MoveTank(-1);

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
            if (_gameState.IsRoundOver())
            {
                QuitGame();

                // to indicate that the game ended by someone winning
                // rather than the game being exited mid-game
                GameEnded = true;
            }

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

            _backgroundSaver.Update(_gameState, gameTime);
        }

        private void NextTurn()
        {
            _gameState.NextPlayer();
            UpdateAmmoLabel();
            UpdateSliders();
            UpdatePlayerImage();
        }

        public void UpdateMouse(Point pos)
        {
            _ui.UpdateMouse(pos);
        }

        public void Click(bool ended = false)
        {
            _ui.Click(ended);
        }

        private void InitUI()
        {
            var fire = new Button("",
                            new Rectangle(565, 570, 150, 150),
                            new ButtonTexture(
                                "fire_button_normal",
                                "fire_button_hover",
                                "fire_button_press"));
            fire.EventOnRelease += (s, a) => Shoot();

            _switchButton = new Button("",
                            new Rectangle(200, 640, 200, 50),
                            new ButtonTexture(
                                "button_normal",
                                "button_over",
                                "button_press"));
            _switchButton.EventOnRelease += (s, a) => SwitchType();

            var back = new Button("exit",
                            new Rectangle(30, 640, 100, 50),
                            new ButtonTexture(
                                "button_normal",
                                "button_over",
                                "button_press"));
            back.EventOnRelease += (s, a) => QuitGame();

            var move_left = new Button("",
                            new Rectangle(430, 640, 50, 50),
                            new ButtonTexture(
                                "left_arrow_normal",
                                "left_arrow_hover",
                                "left_arrow_press"));
            move_left.EventOnHold += (s, a) => MoveTank(-1);

            var move_right = new Button("",
                            new Rectangle(490, 640, 50, 50),
                            new ButtonTexture(
                                "right_arrow_normal",
                                "right_arrow_hover",
                                "right_arrow_press"));
            move_right.EventOnHold += (s, a) => MoveTank(1);

            _playerImage = new Image("tank",
                            new Rectangle(140, 640, 50, 50));

            _powerSlider = new Slider(0, GameLogic.Player.MaxPower, 0,
                                      new Rectangle(1030, 640, 200, 50));
            _angleSlider = new Slider(0, 180, 0,
                                      new Rectangle(800, 640, 200, 50));

            _angleSlider.EventOnChange += (s, a)
                    => SetAngle(180 - _angleSlider.Value);

            _powerSlider.EventOnChange += (s, a)
                    => SetPower(_powerSlider.Value);

            _ui.Add(fire);
            _ui.Add(_switchButton);
            _ui.Add(back);
            _ui.Add(_powerSlider);
            _ui.Add(_angleSlider);
            _ui.Add(_playerImage);
            _ui.Add(move_left);
            _ui.Add(move_right);

            UpdateAmmoLabel();
            UpdateSliders();
            UpdatePlayerImage();
        }
    }
}
