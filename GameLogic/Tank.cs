using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tankettes.GameLogic
{
    public class Tank : AbstractDrawable
    {
        public decimal CannonAngle
        {
            get => _cannon.Angle;
            set => _cannon.Angle = Math.Clamp(value, 0, 180);
        }

        public int Health { get; set; }

        private readonly Sprite _cannon;

        public Tank(string tank,
                    string cannon,
                    Rectangle rectangle,
                    Color color,
                    int startHealth)
        {
            base.Texture = new string(tank);
            base.Rectangle = rectangle;
            base.Color = color;
            Health = startHealth;
            _cannon = new Sprite(cannon,
                    new Rectangle(Point.Zero, Rectangle.Size));
            base.Elements = new List<IDrawable> { _cannon };
            CannonAngle = 0;
        }

        public void Update(State state, decimal delta)
        {
            // TODO
        }

        public bool IsDestroyed() => Health <= 0;
    }
}
