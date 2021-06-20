using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tankettes.UI
{
    public class Slider : AbstractDrawable, IElementUI
    {
        private int _value;

        public int Value
        {
            get => _value;
            set
            {
                _value = value;
                Shift();
            }
        }

        private (int min, int max) _limit;

        private readonly Button _button;

        private Point _mouse;

        public override string Texture => "slider_background";

        public void Click(bool ended = false)
        {
            _button.Click(ended);
        }

        public void Update(GameTime gameTime)
        {
            _button.Update(gameTime);
        }

        public void UpdateMouse(Point pos)
        {
            _mouse = pos;
            _button.UpdateMouse(pos);
        }

        private void Holding()
        {
            var r = _button.Rectangle;

            var newX = _mouse.X;
            newX = Math.Clamp(newX, Rectangle.Left, Rectangle.Right - r.Width);
            _button.Rectangle = new Rectangle(newX, Rectangle.Y,
                                              r.Width, r.Height);
            float ratio = ((newX - Rectangle.Left) / (float)(Rectangle.Width - r.Width));
            _value = (int)(ratio * (_limit.max - _limit.min) + _limit.min);

            Label = new(Value.ToString());
        }

        private void Shift()
        {
            _value = Math.Clamp(_value, _limit.max, _limit.max);

            var r = _button.Rectangle;

            var ratio = (_value - _limit.min)
                        / (float)(_limit.max - _limit.min);

            int d = (int)(ratio * (Rectangle.Width - r.Width));

            _button.Rectangle = new Rectangle(Rectangle.X + d, Rectangle.Y,
                                              r.Width, r.Height);
        }

        public Slider(int min, int max, int starting, Rectangle rectangle)
        {
            _limit = (min, max);
            Rectangle = rectangle;

            _button = new Button("",
                    new Rectangle(0,
                                  0,
                                  rectangle.Height / 2,
                                  rectangle.Height),
                    new ButtonTexture("slider", "slider", "slider"));
            _button.EventOnHold += (s, a) => Holding();

            Value = starting;
            Holding();

            ChildrenRelative = false;

            Elements = new List<IDrawable> { _button };
        }
    }
}
