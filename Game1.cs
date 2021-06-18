using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            var button = new UI.Button("play", new Rectangle(200, 50, 200, 50), _buttonTexture);
            button.EventOnRelease += (s, a) =>
            {
                _window.AddReplace("game", new GameLoop());
                _window.MakeCurrent("game");
            };

            var exit = new UI.Button("Exit", new Rectangle(200, 200, 200, 50), _buttonTexture);
            exit.EventOnRelease += (s, a) => Exit();

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
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            textures["button_normal"] = Content.Load<Texture2D>("button_normal");
            textures["button_over"] = Content.Load<Texture2D>("button_over");
            textures["button_press"] = Content.Load<Texture2D>("button_press");
            textures["terrain"] = Content.Load<Texture2D>("terrain");

            font = Content.Load<SpriteFont>("font");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
                    || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            MouseState state = Mouse.GetState();
            _window.UpdateMouse(state.Position);
            if (state.LeftButton == ButtonState.Pressed)
            {
                _window.Click();
            }
            if (state.LeftButton == ButtonState.Released)
            {
                _window.Click(true);
            }

            _window.Update(gameTime);

            base.Update(gameTime);
        }

        private void Draw(IDrawable obj, Point origin)
        {
            Rectangle rect = new(obj.Rectangle.Location + origin,
                                 obj.Rectangle.Size);

            if (obj.Texture != null)
            {
                _spriteBatch.Draw(textures[obj.Texture], rect, null, obj.Color);
            }

            if (obj.Label != null)
            {
                var point = new Vector2(rect.Center.X, rect.Center.Y);
                _spriteBatch.DrawString(
                        font,
                        obj.Label.Text,
                        point,
                        new Color(112, 133, 53),
                        0,
                        font.MeasureString(obj.Label.Text) / 2,
                        1,
                        new SpriteEffects(),
                        1);
            }

            if (obj.Elements != null)
            {
                var moved = rect.Location;

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
