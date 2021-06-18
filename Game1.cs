﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Tankettes
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Dictionary<string, Texture2D> textures = new ();
        private SpriteFont font;

        private UI.Window _window = new ();
        private readonly UI.ButtonTexture _buttonTexture
                = new("button_normal", "button_over", "button_press");

        private GameLoop _currentGame = null;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        private KeyboardState _prevKeyboard;

        protected override void Initialize()
        {
            var button = new UI.Button("play", new Rectangle(0, 0, 200, 50), _buttonTexture);
            button.EventOnRelease += (s, a) =>
            {
                var terrain = new Terrain("terrain",
                                          new Rectangle(0, 100, 600, 100),
                                          0, 4, 60, 2);
                _currentGame = new GameLoop(terrain);
                _window.AddReplace("game", _currentGame);
                _window.MakeCurrent("game");
            };

            var exit = new UI.Button("Exit", new Rectangle(100, 200, 200, 50), _buttonTexture);
            exit.EventOnRelease += (s, a) => _window.Quit = true;

            var mainMenu = new UI.MenuFrame();
            mainMenu.Add(button);
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

            textures["button_normal"] = Content.Load<Texture2D>("button_normal");
            textures["button_over"] = Content.Load<Texture2D>("button_over");
            textures["button_press"] = Content.Load<Texture2D>("button_press");
            textures["terrain"] = Content.Load<Texture2D>("terrain");

            font = Content.Load<SpriteFont>("font");
        }

        protected override void Update(GameTime gameTime)
        {
            var keyboard = Keyboard.GetState();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
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

        private void Draw(IDrawable obj, Point origin)
        {
            Rectangle rect = new(obj.Rectangle.Location + origin,
                                 obj.Rectangle.Size);

            if (obj.Texture != null)
            {
                float angle = (float)obj.Angle * (float)Math.PI / 180;

                float sin = MathF.Sin(angle);
                float cos = MathF.Cos(angle);

                float x = rect.Width / 2;
                float y = rect.Height / 2;

                var correction = new Vector2(x - (x * cos - y * sin),
                                             y - (x * sin + y * cos));

                rect.Offset(correction);

                _spriteBatch.Draw(
                    textures[obj.Texture],  // 🡸 texture
                    rect,                   // 🡸 dest rectangle
                    null,                   // 🡸 source rectangle
                    obj.Color,              // 🡸 how to color the texture
                    angle,                  // 🡸 rotation angle
                    Vector2.Zero,           // 🡸 rotation origin
                    SpriteEffects.None,     // 🡸 some dumb sprite effects
                    1);                     // 🡸 some dumb layer depth

                rect.Offset(-correction);
            }

            if (obj.Label != null)
            {
                _spriteBatch.DrawString(
                        font,
                        obj.Label.Text,
                        rect.Center.ToVector2(),
                        new Color(112, 133, 53),
                        0,
                        font.MeasureString(obj.Label.Text) / 2,
                        1,
                        SpriteEffects.None,
                        1);
            }

            if (obj.Elements != null)
            {
                var moved = origin + obj.Rectangle.Location;

                foreach (var el in obj.Elements)
                {
                    Draw(el, moved);
                }
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            Draw(_window, Point.Zero);
            
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
