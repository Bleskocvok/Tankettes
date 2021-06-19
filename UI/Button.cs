using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tankettes.UI
{
    public class Button : AbstractDrawable, IElementUI
    {
        public ButtonState State { get; private set; }

        public override string Texture {
            get
            {
                return State switch
                {
                    ButtonState.Hover => _texture.Hover,
                    ButtonState.Pressed => _texture.Press,
                    _ => _texture.Normal
                };
            }
        }

        public string Text
        {
            get => Label.Text;
            set => Label = new Label(value);
        }

        public event EventHandler EventOnRelease;

        private readonly ButtonTexture _texture;

        private bool _mouseOver = false;

        public Button(string text, Rectangle rectangle, ButtonTexture texture)
        {
            Text = text;
            Rectangle = rectangle;
            _texture = texture;
        }

        public void UpdateMouse(Point pos)
        {
            _mouseOver = Rectangle.Contains(pos);

            if (State == ButtonState.Pressed)
            {
                return;
            }

            SetState();
        }
        
        private void SetState()
        {
            State = _mouseOver ? ButtonState.Hover
                               : ButtonState.Idle;
        }

        public void Click(bool ended = false)
        {
            switch (ended)
            {
                // click pressed
                case false:
                    if (State == ButtonState.Hover)
                    {
                        State = ButtonState.Pressed;
                    }
                    break;

                // click released
                case true:
                    if (State == ButtonState.Pressed && _mouseOver)
                    {
                        EventOnRelease?.Invoke(this, new EventArgs());
                    }
                    SetState();
                    break;
            }
        }
    }
}
