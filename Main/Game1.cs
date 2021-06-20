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

        private readonly UI.Window _window = new();
        private readonly UI.ButtonTexture _buttonTexture
                = new("button_normal", "button_over", "button_press");

        private const int BlockSize = 4;

        private GameLoop _currentGame = null;

        private List<Player> _players;

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

            var button = new UI.Button("play", new Rectangle(540, 300, 200, 50), _buttonTexture);
            button.EventOnRelease += (s, a) =>
            {
                var terrain = new GameLogic.Terrain("terrain",
                                          new Rectangle(0, 100, 1200, 600),
                                          0, 4, 120, 1);

                var shop = new List<IShopItem>
                {
                    new AmmoItem<NormalProjectile>{ Name = "Normal", Price = 500, BuyAmount = 5 },
                };

                var normal = new Shop.NormalProjectile();

                var state = new GameLogic.State(
                        new Random().Next(),
                        new List<GameLogic.Player>
                        {
                        new GameLogic.Player("Green", 1000, Color.LimeGreen, new AmmoCapacity(normal, 99)),
                        new GameLogic.Player("Purple", 1000, Color.LightPink, new AmmoCapacity(normal, 99)),
                        new GameLogic.Player("Red", 1000, Color.Red, new AmmoCapacity(normal, 99)),
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
            };

            var load = new UI.Button("Continue", new Rectangle(540, 100, 200, 50), _buttonTexture);
            load.EventOnRelease += (s, a) =>
            {
                var loaded = Serializer.LoadGame("E:\\skola\\c-sharp\\project\\Tankettes\\ahoj.json");
                _currentGame = new GameLoop(loaded);
                _window.AddReplace("game", _currentGame);
                _window.MakeCurrent("game");
            };

            var exit = new UI.Button("Exit", new Rectangle(540, 500, 200, 50), _buttonTexture);
            exit.EventOnRelease += (s, a) => _window.Quit = true;

            var mainMenu = new UI.MenuFrame();
            mainMenu.Add(button);
            mainMenu.Add(load);
            mainMenu.Add(exit);

            var back = new UI.Button("back", new Rectangle(150, 50, 200, 50), _buttonTexture);
            back.EventOnRelease += (s, a) => _window.MakeCurrent("main");

            var second = new UI.MenuFrame();
            second.Add(back);

            _window.Add("main", mainMenu);
            _window.Add("second", second);

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
                "button_normal", "button_over", "button_press",
                "fire_button_normal", "fire_button_hover", "fire_button_press",
                "slider", "slider_background",
                "terrain", "explosion",
                "tank", "cannon", "ball",
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

            if (_currentGame != null && _currentGame.Quit)
            {
                _window.MakeCurrent("main");
            }

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
    }
}
