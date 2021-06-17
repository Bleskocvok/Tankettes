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

        private void Draw(IDrawable element)
        {
            if (element.Texture != null)
            {
                _spriteBatch.Draw(textures[element.Texture], element.Rectangle, null, Color.White);
            }
            if (element.Elements != null)
            {
                foreach (var el in element.Elements)
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
            //_spriteBatch.Draw(menuNormal, new Vector2(50, 50), Color.White);
            Draw(_menu);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
