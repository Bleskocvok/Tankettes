using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;

namespace Tankettes
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Dictionary<string, Texture2D> textures = new ();
        private SpriteFont font;

        private UI.Menu _menu = new ();
        private UI.ButtonTexture _buttonTexture = new("button_normal", "button_over", "button_press");

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            var button = new UI.Button("ahoj", new Rectangle(50, 50, 200, 50), _buttonTexture);
            button.EventOnRelease += (_, _) => System.Diagnostics.Debug.WriteLine("ahoj");
            _menu.Add(button);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            textures["button_normal"] = Content.Load<Texture2D>("button_normal");
            textures["button_over"] = Content.Load<Texture2D>("button_over");
            textures["button_press"] = Content.Load<Texture2D>("button_press");

            font = Content.Load<SpriteFont>("font");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
                    || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            MouseState state = Mouse.GetState();
            _menu.UpdateMouse(state.Position);
            if (state.LeftButton == ButtonState.Pressed)
            {
                _menu.Click();
            }
            if (state.LeftButton == ButtonState.Released)
            {
                _menu.Click(true);
            }

            base.Update(gameTime);
        }

        private void Draw(IDrawable obj)
        {
            if (obj.Texture != null)
            {
                _spriteBatch.Draw(textures[obj.Texture], obj.Rectangle, null, Color.White);
            }

            if (obj.Label != null)
            {
                var point = new Vector2(obj.Rectangle.Center.X, obj.Rectangle.Center.Y);
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
                foreach (var el in obj.Elements)
                {
                    Draw(el);
                }
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            _spriteBatch.Begin();
            Draw(_menu);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
