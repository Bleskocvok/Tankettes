using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using static Tankettes.GameLogic.Player;
using Tankettes.GameLogic;
using Tankettes.Shop;

namespace Tankettes
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Renderer _renderer;

        private UI.Window _window = new();

        private const int BlockSize = 4;

        private GameLoop _currentGame = null;

        private UI.Image _victoryImage;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        private KeyboardState _prevKeyboard;

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 720;
            _graphics.ApplyChanges();

            InitMenu();

            base.Initialize();

            _prevKeyboard = Keyboard.GetState();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _renderer = new Renderer
            {
                Graphics = _graphics,
                SpriteBatch = _spriteBatch,
            };

            var toLoad = new string[]
            {
                // ui
                "button_normal", "button_over", "button_press",
                "fire_button_normal", "fire_button_hover", "fire_button_press",
                "slider", "slider_background",
                "left_arrow_normal", "left_arrow_hover", "left_arrow_press",
                "right_arrow_normal", "right_arrow_hover", "right_arrow_press",
                
                // gameplay
                "terrain", "explosion",
                "tank", "cannon", "ball",
                "victory",

                // useless
                "icon",
            };

            _renderer.LoadAssets(Content, "font", toLoad);
        }

        protected override void Update(GameTime gameTime)
        {
            var keyboard = Keyboard.GetState();

            if (GamePad.GetState(PlayerIndex.One)
                    .Buttons.Back == ButtonState.Pressed)
                Exit();

            if (_window.Current != _currentGame
                    && keyboard.IsKeyDown(Keys.Escape)
                    && _prevKeyboard.IsKeyUp(Keys.Escape))
                Exit();

            if (_window.Quit)
                Exit();

            CheckGameOver();

            MouseState state = Mouse.GetState();
            _window.UpdateMouse(state.Position);

            if (state.LeftButton == ButtonState.Pressed
                    || state.LeftButton == ButtonState.Released)
            {
                _window.Click(state.LeftButton == ButtonState.Released);
            }

            _window.UpdateControls(keyboard);
            _window.Update(gameTime);

            base.Update(gameTime);

            _prevKeyboard = keyboard;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            _renderer.Draw(_window, Point.Zero);

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private void CheckGameOver()
        {
            if (_currentGame != null && _currentGame.GameEnded)
            {
                _window.MakeCurrent("game_over");
                _victoryImage.Color = _currentGame.Victor()?.TankColor
                                      ?? Color.White;
                _currentGame = null;
            }

            if (_currentGame != null && _currentGame.Quit)
            {
                _window.MakeCurrent("main");
                _currentGame = null;
            }
        }

        private void NewGame()
        {
            // TBD
            var shop = new List<IShopItem>
            {
                new AmmoItem<NormalProjectile>{ Name = "Normal", Price = 500, BuyAmount = 5 },
            };

            var normal = new Shop.NormalProjectile();
            var bigBoom = new Shop.BigBoomProjectile();

            List<AmmoCapacity> GenerateAmmo()
            {
                return new List<AmmoCapacity>
                {
                    new AmmoCapacity(normal, 99),
                    new AmmoCapacity(bigBoom, 3),
                };
            }

            var state = new GameLogic.State(
                    new Random().Next(),
                    new List<Player>
                    {
                        new Player("Green", 1000, Color.LimeGreen, GenerateAmmo()),
                        new Player("Purple", 1000, Color.LightPink, GenerateAmmo()),
                        new Player("Red", 1000, Color.Red, GenerateAmmo()),
                    },
                    new Rectangle(0, 0, 1280, 600),
                    "tank",
                    "cannon",
                    "terrain",
                    BlockSize
                );

            _currentGame = new GameLoop(state);
            _window.AddReplace("game", _currentGame);
            _window.MakeCurrent("game");
        }

        private void LoadGame()
        {
            if (!Serializer.Exists("save"))
            {
                return;
            }

            var loaded = Serializer.LoadGame("save");
            _currentGame = new GameLoop(loaded);
            _window.AddReplace("game", _currentGame);
            _window.MakeCurrent("game");
        }

        private void InitMenu()
        {
            var builder = new UI.GameWindowBuilder
            {
                ButtonTexture = new UI.ButtonTexture("button_normal",
                                                     "button_over",
                                                     "button_press")
            };

            builder.AddMenuFrame("main");
            builder.AddMenuFrame("game_over");


            builder.AddAny("main", new UI.Image("icon", new Rectangle(590, 100, 100, 100)));
            builder.AddButton("main", "Play", 540, 300, NewGame);
            builder.AddButton("main", "Continue", 540, 400, LoadGame);
            builder.AddButton("main", "Exit", 540, 600, Exit);

            _victoryImage = builder.AddAny("game_over", new UI.Image("victory",
                        new Rectangle(490, 100, 300, 300)))
                        as UI.Image;
            builder.AddButtonTransition("game_over", "Back", 540, 600, "main");

            _window = builder.Build();
        }
    }
}
